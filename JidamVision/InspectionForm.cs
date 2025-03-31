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

                Mat curImage = Global.Inst.InspStage.GetMat(1);
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


        public void AddRect(List<Rect> rects)
        {
            //#BINARY FILTER#18 imageViewer는 Rectangle 타입으로 그래픽을 그리므로, 
            //아래 코드를 이용해, Rect -> Rectangle로 변환하는 람다식
            var rectangles = rects.Select(r => new Rectangle(r.X, r.Y, r.Width, r.Height)).ToList();
            imageViewer.AddRect(rectangles);

        }

        public void AddRoi(InspWindowType inspWindowType)
        {
            imageViewer.NewRoi(inspWindowType);
        }




      
        // 검사 이미지 불러오기
        public void LoadImage(string imagePath)
        {
            inspectedImage = Cv2.ImRead(imagePath);
            // 검사 이미지 로드 후 화면에 표시하는 코드 추가
          
        }
        public void CompareROIWithInspection()
        {
            foreach (var roi in currentROIs)
            {// ROI 위치 정보와 비교할 이미지에서 해당 영역 추출
                var roiRect = roi.WindowArea;  // ROI의 위치 및 크기 정보
          //      var roiImage = inspectedImage[roiRect];

                // ROI 이미지와 검사가 올바른지 비교 (여기서 컬러 이진화 알고리즘을 사용할 수도 있음)
       //         bool isMatch = CompareROI(roiImage, roi);

                // 결과에 따라 표시 (초록색/빨간색)
                //if (isMatch)
                //{
                //    DrawResult(roiRect, Color.Green);  // 초록색 표시
                //}
                //else
                //{
                //    DrawResult(roiRect, Color.Red);    // 빨간색 표시
                //}
            }
        }

        // ROI 비교 함수 (간단한 예시로, 실제 비교 로직은 컬러 이진화 등으로 확장 가능)
        private bool CompareROI(Mat roiImage, InspWindow roi)
        {
            // 예시: 단순히 색상값 비교 또는 이진화 알고리즘을 통한 비교
            return ColorMatch(roiImage, roi); // ColorMatch는 예시 함수
        }

        // 검사 결과 그리기
        private void DrawResult(Rect rect, Color color)
        {
            // 이미지를 그리기 위해서는 OpenCV의 그리기 함수를 사용할 수 있음
            Scalar colorScalar = new Scalar(color.B, color.G, color.R); // OpenCV에서 색상은 BGR 순서
            Cv2.Rectangle(inspectedImage, rect, colorScalar, 2);
            // 그린 이미지를 화면에 표시
         
        }
        // 색상 매칭 함수 (컬러 이진화 알고리즘을 이용한 예시)
        private bool ColorMatch(Mat roiImage, InspWindow roi)
        {
            // 예시로 ColorBlobAlgorithm을 사용하여 색상 비교
            // 실제로는 ROI에 대한 색상 비교 후, 매칭 여부 반환
            colorBlobAlgorithm.SetSourceImage(roiImage);
            return colorBlobAlgorithm.DoInspect();  // 컬러 이진화 알고리즘을 통해 색상 매칭
        }


    
        private void CheckInspectionImage(Mat inspectionImg)
        {
           

            // 검사 이미지에서 ROI 영역 추출
            Mat roiInspectionImage = new Mat(inspectionImg, selectedROI);

            // 검사 이미지에서 해당 ROI 영역을 검사
            colorBlobAlgorithm.SetInspData(roiInspectionImage);

            // 검사 실행
            bool result = colorBlobAlgorithm.DoInspect();

            // 결과 출력
            if (colorBlobAlgorithm.IsDefect)
            {
                lblResult.Text = "NG"; // 불량
            }
            else
            {
                lblResult.Text = "OK"; // 정상
            }

            // 검사 이미지 화면에 표시
            Bitmap bmp = BitmapConverter.ToBitmap(inspectionImg);
           
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
        public void SetImages(List<Mat> images)
        {
            receivedImages = images;
            currentImageIndex = 0;

            // 검사 시작 전 개수 초기화
            totalCount = 0;
            goodCount = 0;
            faultyCount = 0;
            UpdateInspectionResults(); // UI 업데이트
        }
        private void bntStart_Click(object sender, EventArgs e)
        {
            if (receivedImages.Count > 0)
            {
                currentImageIndex = 0;

                // 시작할 때 개수 초기화
                totalCount = 0;
                goodCount = 0;
                faultyCount = 0;
                UpdateInspectionResults();

                StartInspection();
                inspectionTimer.Start();

                dtpStartTime.Value = DateTime.Now; // 시작 버튼을 누른 순간의 시간 기록
            }
            else
            {
                MessageBox.Show("이미지가 없습니다.");
            }
        }
        private void UpdateInspectionResults()
        {
            rtbTotalnumber.Text = totalCount.ToString();
            rtbGood.Text = goodCount.ToString();
            rtbFaulty.Text = faultyCount.ToString();

            // 퍼센트 계산 (불량 개수 / 총 개수 * 100)
            double percent = totalCount > 0 ? (faultyCount / (double)totalCount) * 100 : 0;
            rtbPercent.Text = percent.ToString("0.00") + "%";
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

        // ✅ 추가된 메서드: 이미지에서 색상 확인 후 DataGridView에 추가
        //private void CheckColorsInImage(Mat image, int imageIndex)
        //{
        //    Dictionary<Color, bool> colorResults = new Dictionary<Color, bool>();

        //    // 초기화 (모든 색상을 false로 설정)
        //    foreach (var color in expectedColors)
        //    {
        //        colorResults[color] = false;
        //    }

        //    for (int x = 0; x < image.Width; x++)
        //    {
        //        for (int y = 0; y < image.Height; y++)
        //        {
        //            Color pixelColor = GetPixelColor(image, x, y);

        //            foreach (var expectedColor in expectedColors)
        //            {
        //                if (IsSimilarColor(pixelColor, expectedColor))
        //                {
        //                    colorResults[expectedColor] = true;
        //                }
        //            }
        //        }
        //    }

        //    // 검사 결과를 DataGridView에 추가
        //    foreach (var kvp in colorResults)
        //    {
        //        string resultText = kvp.Value ? $"{kvp.Key.Name} OK" : $"{kvp.Key.Name} NOK";
        //        dgvMetric.Rows.Add(imageIndex + 1, kvp.Key.Name, resultText);
        //    }
        //}

        // ✅ OpenCV Mat에서 특정 좌표의 픽셀 색상을 가져오는 메서드
        private Color GetPixelColor(Mat image, int x, int y)
        {
            Vec3b pixel = image.At<Vec3b>(y, x);
            return Color.FromArgb(pixel[2], pixel[1], pixel[0]); // OpenCV는 BGR 순서이므로 RGB로 변환
        }

        // ✅ 색상 유사성 검사 (약간의 오차 허용)
        private bool IsSimilarColor(Color color1, Color color2, int tolerance = 30)
        {
            return Math.Abs(color1.R - color2.R) <= tolerance &&
                   Math.Abs(color1.G - color2.G) <= tolerance &&
                   Math.Abs(color1.B - color2.B) <= tolerance;
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
    }
}
