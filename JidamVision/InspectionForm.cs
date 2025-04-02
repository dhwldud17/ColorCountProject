using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using JidamVision.Algorithm;
using JidamVision.Core;
using JidamVision.Inspect;
using JidamVision.Teach;
using JidamVision.Util;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using OpenCvSharp.Flann;
using WeifenLuo.WinFormsUI.Docking;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using static JidamVision.Core.ImageSpace;

namespace JidamVision
{
    public partial class InspectionForm : DockContent
    {
        eImageChannel _currentImageChannel = eImageChannel.Color;
        private List<Mat> receivedImages = new List<Mat>(); // 외부 프로그램에서 받은 이미지 목록
        private int currentImageIndex = 0;
        private Timer inspectionTimer;
        private InspAlgorithm inspector;
        private Timer currentTimeTimer; // 현재 시간을 자동 갱신하는 타이머
        private int totalCount = 0;  // 총 검사 개수
        private int goodCount = 0;   // 양품 개수
        private int faultyCount = 0; // 불량 개수
        private string[] imageFiles; // 이미지 파일 목록
        private ColorBlobAlgorithm colorBlobAlgorithm;
        private Rect selectedROI; // ROI 영역 저장
        private InspWindow _inspWindow;
       
        private Model currentModel;
        private Mat inspectedImage;
        private List<InspWindow> currentROIs;
        // ✅검사할 색상 정의 (추가된 부분)
        
        public InspectionForm()
        {
            InitializeComponent();
            InitializeInspection();
            InitializeTimers();  // 현재 시간 갱신 타이머 초기화
            ConfigureDateTimePickers(); // DateTimePicker 포맷 설정
            ConfigureDateTimePickers(); // DateTimePicker 포맷 설정
            InitializeDataGridView();  // DataGridView 초기화
            colorBlobAlgorithm = new ColorBlobAlgorithm();
            // 모델 불러오기 
           

            imageViewer.DiagramEntityEvent += ImageViewer_DiagramEntityEvent;
           
          
            Controls.Add(imageViewer);
        }

        public void UpdateInspectionResults()
        {
            var manager = InspectionManager.Instance;

            if (manager.AllResults.Count > 0)
            {
                var latestResult = manager.AllResults.Last(); // 가장 최신 검사 결과 가져오기
                totalCount++;
                if (rtbTotalnumber.InvokeRequired)
                {
                    rtbTotalnumber.Invoke(new Action(UpdateInspectionResults));
                    return;
                }


                rtbTotalnumber.Text = totalCount.ToString();
                rtbWireCount.Text = latestResult.WireCount.ToString();
                
                rtbCableResults.Text = string.Join(", ", latestResult.CableResults); // 리스트 데이터를 문자열로 변환
                                                                                     // 퍼센트 계산 (불량 개수 / 총 개수 * 100)
                if (latestResult.WireCount != 9 || latestResult.CableResults.Any(result => result == "NG")) 
                {    //카운트개수 안맞거나 케이블에서 NG가 하나라도있으면
                    faultyCount++;

                }
                else
                {
                    goodCount++;
                }
                Show_Result();
            }
        }

        private void Show_Result()
        {
                rtbFaulty.Text=faultyCount.ToString();
                rtbGood.Text=goodCount.ToString();

                double percent = totalCount > 0 ? (faultyCount / (double)totalCount) * 100 : 0;
                rtbPercent.Text = percent.ToString("0.00") + "%";
        }
        private void ImageViewer_DiagramEntityEvent(object sender, DiagramEntityEventArgs e)
        {
            SLogger.Write($"ImageViewer Action {e.ActionType.ToString()}");
            switch (e.ActionType)
            {
                case EntityActionType.Select:
                    Global.Inst.InspStage.SelectInspWindow(e.InspWindow);
                    imageViewer.Focus();
                    break;
                case EntityActionType.Inspect:
                    Global.Inst.InspStage.TryInspection(e.InspWindow);
                    break;
                case EntityActionType.Add:
                    Global.Inst.InspStage.AddInspWindow(e.WindowType, e.Rect);
                    break;
                case EntityActionType.Move:
                    Global.Inst.InspStage.MoveInspWindow(e.InspWindow, e.OffsetMove);
                    break;
                case EntityActionType.Resize:
                    Global.Inst.InspStage.ModifyInspWindow(e.InspWindow, e.Rect);
                    break;
                case EntityActionType.Delete:
                    Global.Inst.InspStage.DelInspWindow(e.InspWindow);
                    break;
                case EntityActionType.DeleteList:
                    Global.Inst.InspStage.DelInspWindow(e.InspWindowList);
                    break;
                case EntityActionType.AddGroup:
                    Global.Inst.InspStage.CreateGroupWindow(e.InspWindowList);
                    break;
                case EntityActionType.Break:
                    Global.Inst.InspStage.BreakGroupWindow(e.InspWindow);
                    break;
                case EntityActionType.UpdateImage:
                    Global.Inst.InspStage.SetTeachingImage(e.InspWindow);
                    break;

                case EntityActionType.PickColor:
                    Rect rect = imageViewer.GetPickColorRect();
                    Global.Inst.InspStage.PickColorWindow(rect);
                    break;

            }
        }


        private eImageChannel GetCurrentChannel()
        {
         

            return eImageChannel.Color;
        }

        public void UpdateDisplay(Bitmap bitmap = null)
        {
            {
                if (bitmap == null)
                {
                    //# SAVE ROI#3 채널 정보 변수에 저장
                    //참고 프로젝트에서 _currentImageChannel를 모두 찾아서, 수정할것
                    _currentImageChannel = GetCurrentChannel();
                    bitmap = Global.Inst.InspStage.GetBitmap(1, _currentImageChannel);
                    if (bitmap == null)
                        return;
                }

                imageViewer.LoadBitmap(bitmap);

                Mat curImage = Global.Inst.InspStage.GetMat(0);
                Global.Inst.InspStage.PreView.SetImage_Inspection(curImage);
            }
        }








        public void UpdateDiagramEntity()
        {
            Model model = Global.Inst.InspStage.CurModel;
            List<DiagramEntity> diagramEntityList = new List<DiagramEntity>();

            foreach (InspWindow window in model.InspWindowList)
            {
                if (window is null)
                    continue;

                if (window is GroupWindow group)
                {
                    foreach (InspWindow member in group.Members)
                    {
                        DiagramEntity entity = new DiagramEntity()
                        {
                            LinkedWindow = member,
                            EntityROI = new Rectangle(
                                member.WindowArea.X, member.WindowArea.Y,
                                member.WindowArea.Width, member.WindowArea.Height),
                            EntityColor = imageViewer.GetWindowColor(member.InspWindowType),
                            IsHold = member.IsTeach,
                        };
                        diagramEntityList.Add(entity);
                    }
                }
                else if (window.Parent == null)
                {
                    DiagramEntity entity = new DiagramEntity()
                    {
                        LinkedWindow = window,
                        EntityROI = new Rectangle(
                            window.WindowArea.X, window.WindowArea.Y,
                                window.WindowArea.Width, window.WindowArea.Height),
                        EntityColor = imageViewer.GetWindowColor(window.InspWindowType),
                        IsHold = window.IsTeach
                    };
                    diagramEntityList.Add(entity);
                }
            }

            imageViewer.SetDiagramEntityList(diagramEntityList);
        }
        public void SelectDiagramEntity(InspWindow window)
        {
            imageViewer.SelectDiagramEntity(window);
        }

        public void UpdateImageViewer()
        {
            imageViewer.Invalidate();
        }




      


        private void InitializeInspection()
        {
            inspectionTimer = new Timer();
            inspectionTimer.Interval = 1000; // 1초마다 검사 실행
            inspectionTimer.Tick += InspectionTimer_Tick;

            // 사용할 검사 알고리즘 인스턴스 생성 (예제: MatchAlgorithm)
            inspector = new MatchAlgorithm();
        }
        private void InitializeDataGridView()
        {
            dgvMetric.ColumnCount = 3;
            dgvMetric.Columns[0].Name = "이미지 번호";
            dgvMetric.Columns[1].Name = "기준 색상";
            dgvMetric.Columns[2].Name = "검사 결과";
        }
      
        private void InitializeTimers()
        {
            // 현재 시간 자동 갱신 타이머 설정
            currentTimeTimer = new Timer();
            currentTimeTimer.Interval = 1000; // 1초마다 현재 시간 갱신
            currentTimeTimer.Tick += (s, e) =>
            {
                dtpCurrenttime.Value = DateTime.Now;
            };
            currentTimeTimer.Start();
        }
        private void ConfigureDateTimePickers()
        {
            // DateTimePicker에 초까지 표시하도록 설정
            dtpStartTime.Format = DateTimePickerFormat.Custom;
            dtpStartTime.CustomFormat = "yyyy-MM-dd HH:mm:ss";

            dtpCurrenttime.Format = DateTimePickerFormat.Custom;
            dtpCurrenttime.CustomFormat = "yyyy-MM-dd HH:mm:ss";
        }
        // 외부 프로그램에서 이미지 리스트를 설정하는 함수
       
        private void bntStart_Click(object sender, EventArgs e)
        {
            if (receivedImages.Count > 0)
            {
                currentImageIndex = 0;

                // 시작할 때 개수 초기화
                totalCount = 0;
                goodCount = 0;
                faultyCount = 0;
                //UpdateInspectionResults();

                StartInspection();
                inspectionTimer.Start();

                dtpStartTime.Value = DateTime.Now; // 시작 버튼을 누른 순간의 시간 기록
            }
            else
            {
                MessageBox.Show("이미지가 없습니다.");
            }
        }
        

        private void bntStop_Click(object sender, EventArgs e)
        {
            inspectionTimer.Stop();
        }
        private void InspectionTimer_Tick(object sender, EventArgs e)
        {
            currentImageIndex++;
            if (currentImageIndex < receivedImages.Count)
            {
                StartInspection();
            }
            else
            {
                inspectionTimer.Stop();
                MessageBox.Show("검사가 완료되었습니다.");
            }
        }

        private void StartInspection()
        {
            if (inspector != null)
            {
                inspector.SetInspData(receivedImages[currentImageIndex]);
                bool result = inspector.DoInspect();

                // 검사 결과 출력
                Console.WriteLine($"검사 결과: {(result ? "성공" : "실패")}");
            }
            // ✅ 이미지 색상 검사 추가
         //   CheckColorsInImage(receivedImages[currentImageIndex], currentImageIndex);
        }

     
        private void InspectionForm_Resize(object sender, EventArgs e)
        {
            int margin = 80;

            // 오른쪽 UI 요소들의 X 위치 조정
            int xPos =  this.Width - dgvMetric.Width - margin;

            dtpStartTime.Location = new System.Drawing.Point(dtpStartTime.Location.X, dtpStartTime.Location.Y);
            lbStartTime.Location = new System.Drawing.Point(lbStartTime.Location.X, lbStartTime.Location.Y);
            dtpCurrenttime.Location = new System.Drawing.Point(dtpCurrenttime.Location.X, dtpCurrenttime.Location.Y);
            lbCurrenttime.Location = new System.Drawing.Point(lbCurrenttime.Location.X, lbCurrenttime.Location.Y);


            bntStart.Location = new System.Drawing.Point(xPos-bntStart.Width-30, bntStart.Location.Y);
            bntStop.Location = new System.Drawing.Point(xPos- bntStop.Width-30, bntStop.Location.Y);
            rtbTotalnumber.Location = new System.Drawing.Point(xPos- rtbTotalnumber.Width-30, rtbTotalnumber.Location.Y);
            lbTotalnumber.Location = new System.Drawing.Point(xPos - lbTotalnumber.Width - 120, lbTotalnumber.Location.Y);
            rtbGood.Location = new System.Drawing.Point(xPos - lbGood.Width - 130, rtbGood.Location.Y);
            lbGood.Location = new System.Drawing.Point(xPos - lbGood.Width - 130, lbGood.Location.Y);
            rtbFaulty.Location = new System.Drawing.Point(xPos - lbFaulty.Width - 60, rtbFaulty.Location.Y);
            lbFaulty.Location = new System.Drawing.Point(xPos - lbFaulty.Width - 60, lbFaulty.Location.Y);
            dgvMetric.Location = new System.Drawing.Point(xPos, dgvMetric.Location.Y);
            rtbPercent.Location = new System.Drawing.Point(xPos, rtbPercent.Location.Y);
            lbPercent.Location = new System.Drawing.Point(xPos, lbPercent.Location.Y);
            btImageLode.Location = new System.Drawing.Point(xPos - bntStop.Width -30, bntStop.Location.Y + 40);

            // imageViewCCtrl1 크기 조정 (좌측 상단에 고정)
            imageViewer.Width = xPos - margin * 3; // UI 요소들과 겹치지 않도록 조정
            imageViewer.Height = this.Height - margin * 2;
            imageViewer.Location = new System.Drawing.Point(margin-50, margin);
        }

      
            
        

        private void ShowImage(int index)
        {
            if (imageFiles != null && imageFiles.Length > 0 && index >= 0 && index < imageFiles.Length)
            {
                string imagePath = imageFiles[index];

                // 파일 경로에서 Bitmap을 생성
                Bitmap bitmap = new Bitmap(imagePath);

                // imageViewCCtrl에 표시 (imageViewer와 같은 방식 적용)
                imageViewer.LoadBitmap(bitmap);
            }
        }

        private void imageViewer_Load(object sender, EventArgs e)
        {

        }

        private void btImageLode_Click_1(object sender, EventArgs e)
        {
           
            UpdateDisplay();
        }

        private void rtbTotalnumber_TextChanged(object sender, EventArgs e)
        {

        }

        private void imageViewer_Load_1(object sender, EventArgs e)
        {

        }

        private void rtbGood_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
