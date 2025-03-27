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
using OpenCvSharp;
using OpenCvSharp.Extensions;
using OpenCvSharp.Flann;
using WeifenLuo.WinFormsUI.Docking;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace JidamVision
{
    public partial class InspectionForm : DockContent
    {
        private List<Mat> receivedImages = new List<Mat>(); // 외부 프로그램에서 받은 이미지 목록
        private int currentImageIndex = 0;
        private Timer inspectionTimer;
        private InspAlgorithm inspector;
        private Timer currentTimeTimer; // 현재 시간을 자동 갱신하는 타이머
        private int totalCount = 0;  // 총 검사 개수
        private int goodCount = 0;   // 양품 개수
        private int faultyCount = 0; // 불량 개수
        private string[] imageFiles; // 이미지 파일 목록

        // ✅검사할 색상 정의 (추가된 부분)
        private readonly List<Color> expectedColors = new List<Color>
        {
            Color.Red,   // 빨강
            Color.Yellow, // 노랑
           // Color.Blue,  // 파랑
            Color.Black // 검정
        };
        public InspectionForm()
        {
            InitializeComponent();
            InitializeInspection();
            InitializeTimers();  // 현재 시간 갱신 타이머 초기화
            ConfigureDateTimePickers(); // DateTimePicker 포맷 설정
            ConfigureDateTimePickers(); // DateTimePicker 포맷 설정
            InitializeDataGridView();  // DataGridView 초기화
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

            // 열 크기 자동 조정
            dgvMetric.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // 특정 열이 너무 작아지지 않도록 최소 너비 설정
            dgvMetric.Columns[0].MinimumWidth = 10;  // 이미지 번호
            dgvMetric.Columns[1].MinimumWidth = 10; // 기준 색상
            dgvMetric.Columns[2].MinimumWidth = 10; // 검사 결과

            // 행 높이 자동 조정 (필요한 경우)
            dgvMetric.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
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
                //  처음 시작하는 경우만 초기화 (이미 진행된 경우에는 초기화하지 않음)
                if (!inspectionTimer.Enabled)
                {
                    if (currentImageIndex >= receivedImages.Count)
                    {
                        // 모든 검사가 완료되었으면 처음부터 다시 시작
                        currentImageIndex = 0;
                        totalCount = 0;
                        goodCount = 0;
                        faultyCount = 0;
                        UpdateInspectionResults();
                    }

                    dtpStartTime.Value = DateTime.Now; // 시작 시간 기록
                }

                // 현재 이미지부터 검사 시작
                StartInspection();
                inspectionTimer.Start();
            }
            else
            {
                MessageBox.Show("이미지가 없습니다.");
            }
        }
        //나중에 확인 필요
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
            if (currentImageIndex < receivedImages.Count)
            {
                StartInspection();
                currentImageIndex++; // 다음 이미지로 이동
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

                // ✅ 총 검사 개수 증가
                totalCount++;

                // 검사 결과 출력
                Console.WriteLine($"검사 결과: {(result ? "성공" : "실패")}");

                // ✅ 이미지 색상 검사 실행
                CheckColorsInImage(receivedImages[currentImageIndex], currentImageIndex);

                // ✅ UI 업데이트 (총 개수 표시)
                UpdateInspectionResults();

                // ✅ 이미지 색상 검사 추가
                CheckColorsInImage(receivedImages[currentImageIndex], currentImageIndex);

                // 현재 검사 중인 이미지 UI에 표시
                ShowImage(currentImageIndex);
            }
        }

        // ✅ 추가된 메서드: 이미지에서 색상 확인 후 DataGridView에 추가
        private void CheckColorsInImage(Mat image, int imageIndex)
        {
            Dictionary<Color, bool> colorResults = new Dictionary<Color, bool>();

            // 초기화 (모든 색상을 false로 설정)
            foreach (var color in expectedColors)
            {
                colorResults[color] = false;
            }

            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    Color pixelColor = GetPixelColor(image, x, y);

                    foreach (var expectedColor in expectedColors)
                    {
                        if (IsSimilarColor(pixelColor, expectedColor))
                        {
                            colorResults[expectedColor] = true;
                        }
                    }
                }
            }

            // 검사 결과를 DataGridView에 추가
            foreach (var kvp in colorResults)
            {
                string resultText = kvp.Value ? $"{kvp.Key.Name} OK" : $"{kvp.Key.Name} NOK";
                dgvMetric.Rows.Add(imageIndex + 1, kvp.Key.Name, resultText);
            }
        }

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

            // DataGridView 크기 자동 조정
            dgvMetric.Width = this.Width / 4;  // 폼 너비의 1/3 크기로 설정 (조정 가능)
            dgvMetric.Height = this.Height - 100; // 폼 높이에 맞게 조정

            // DataGridView 위치 조정 (오른쪽 정렬)
            dgvMetric.Location = new System.Drawing.Point(this.Width - dgvMetric.Width - margin, dgvMetric.Location.Y);

            dtpStartTime.Location = new System.Drawing.Point(dtpStartTime.Location.X, dtpStartTime.Location.Y);
            lbStartTime.Location = new System.Drawing.Point(lbStartTime.Location.X, lbStartTime.Location.Y);
            dtpCurrenttime.Location = new System.Drawing.Point(dtpCurrenttime.Location.X, dtpCurrenttime.Location.Y);
            lbCurrenttime.Location = new System.Drawing.Point(lbCurrenttime.Location.X, lbCurrenttime.Location.Y);


            bntStart.Location = new System.Drawing.Point(xPos-bntStart.Width-30, bntStart.Location.Y);
            bntStop.Location = new System.Drawing.Point(xPos- bntStop.Width-30, bntStop.Location.Y);
            rtbTotalnumber.Location = new System.Drawing.Point(xPos- rtbTotalnumber.Width-30, rtbTotalnumber.Location.Y);
            lbTotalnumber.Location = new System.Drawing.Point(xPos - lbTotalnumber.Width - 120, lbTotalnumber.Location.Y);
            rtbGood.Location = new System.Drawing.Point(xPos - lbGood.Width - 130, rtbGood.Location.Y +5);
            lbGood.Location = new System.Drawing.Point(xPos - lbGood.Width - 130, lbGood.Location.Y + 5);
            rtbFaulty.Location = new System.Drawing.Point(xPos - lbFaulty.Width - 60, rtbFaulty.Location.Y +5);
            lbFaulty.Location = new System.Drawing.Point(xPos - lbFaulty.Width - 60, lbFaulty.Location.Y + 5);
            dgvMetric.Location = new System.Drawing.Point(xPos, dgvMetric.Location.Y);
            rtbPercent.Location = new System.Drawing.Point(xPos - rtbPercent.Width - 30, rtbPercent.Location.Y +10);
            lbPercent.Location = new System.Drawing.Point(xPos - lbPercent.Width - 120, lbPercent.Location.Y +10);
            btImageLode.Location = new System.Drawing.Point(xPos - btImageLode.Width -30, bntStop.Location.Y + 40);

            // imageViewCCtrl1 크기 조정 (좌측 상단에 고정)
            imageViewer.Width = xPos - margin * 3; // UI 요소들과 겹치지 않도록 조정
            imageViewer.Height = this.Height - margin * 2;
            imageViewer.Location = new System.Drawing.Point(margin-50, margin);
        }

        private void btImageLode_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "이미지가 있는 폴더를 선택하세요.";

                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedFolder = folderDialog.SelectedPath;

                    // 선택한 폴더 내의 이미지 파일(.jpg, .png, .bmp) 목록 가져오기
                    imageFiles = Directory.GetFiles(selectedFolder, "*.*")
                                          .Where(f => f.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                                                      f.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                                                      f.EndsWith(".bmp", StringComparison.OrdinalIgnoreCase))
                                          .ToArray();

                    receivedImages.Clear(); // 기존 이미지 목록 초기화

                    foreach (var imagePath in imageFiles)
                    {
                        Mat matImage = Cv2.ImRead(imagePath); // OpenCV로 이미지 읽기
                        receivedImages.Add(matImage);
                    }

                    // 첫 번째 이미지 표시
                    if (receivedImages.Count > 0)
                    {
                        currentImageIndex = 0;
                        ShowImage(currentImageIndex);
                    }
                    else
                    {
                        MessageBox.Show("선택한 폴더에 이미지가 없습니다.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }

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

    }
}
