using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using JidamVision.Algorithm;
using JidamVision.Core;
using JidamVision.Teach;
using log4net.Repository.Hierarchy;
using OpenCvSharp;
using static System.Windows.Forms.MonthCalendar;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace JidamVision.Property
{
    /*
    #COLOR BINARY FILTER# - <<<컬러이진화 검사 개발>>> 
    입력된 H, S, V 임계값을 이용해, 레퍼런스이미지를 컬러이진화한 후, Filter(area)등을 이용해, 원하는 영역을 찾는다.
     */


    //#COLOR BINARY FILTER#7 이진화 하이라이트, 이외에, 이진화 이미지를 보기 위한 옵션
    //컬러이진화 하이라이트, 이외에, 이진화 이미지를 보기 위한 옵션
    public enum ShowColorBinaryMode
    {
        ShowColorBinaryNone = 0, //이진화 컬러 하이라이트 끄기
        ShowColorBinaryHighlight, //이진화 컬러 하이라이트 보기
        ShowColorBinary, //이진화 컬러 이미지만 보기
        ShowColorBinaryOnly //배경 없이 이진화 컬러 이미지만 보기
    }

    public partial class ColorBinaryInspProp : UserControl

    {   //콤보박스 필터   효과1,2번 선택 이벤트 추가
        public event EventHandler<FilterSelectedEventArgs> FilterSelected;
        private String _selected_effect;
        private int _selected_effect2 = -1;

        public event EventHandler<RangeChangedEventArgs> RangeChanged; //HSV임계값 이벤트 추가
        private ColorBlobAlgorithm colorBlobAlgorithm;

        private Point startPoint;
        private Rectangle selectedArea;
        private bool isSelecting = false;

        /* NOTE
        public int LowerValue
        {
            get { return trackBarLower.Value; }
        }
        C# 6부터는 위 코드를 더 간결하게 람다(=>) 문법을 사용하여 표현
        */

        // 속성값을 이용하여 이진화 임계값 설정
        public int HueValue => hTrackBar.Value; // Hue 값만 설정
        public int SatValue => sTrackBar.Value; // Saturation 값만 설정
        public int ValValue => vTrackBar.Value; // Value 값만 설정

        public ColorBinaryInspProp()
        {
            InitializeComponent();
        }

        //컬러 이진화 검사 속성값을 GUI에 설정
        public void LoadInspParam()
        {
            if (colorBlobAlgorithm == null)
                return;


            // TrackBar 초기 설정
            hTrackBar.ValueChanged += OnValueChanged;
            sTrackBar.ValueChanged += OnValueChanged;
            vTrackBar.ValueChanged += OnValueChanged;

            hTrackBar.Value = 0;
            sTrackBar.Value = 0;
            vTrackBar.Value = 0;

            // 필터 적용 버튼 클릭 이벤트
            btnApply.Click += btnApply_Click;
            btnApplyHSV.Click += btnApplyHSV_Click;

            // 필터 선택 콤보박스 이벤트
            select_effect.SelectedIndexChanged += select_effect_SelectedIndexChanged;



            //#COLOR BINARY FILTER#8 컬러이진화 필터값을 GUI에 로딩
            InspWindow inspWindow = Global.Inst.InspStage.InspWindow;
            if (inspWindow != null)
            {
                //#INSP WORKER#13 inspWindow에서 컬러이진화 알고리즘 찾는 코드
                ColorBlobAlgorithm colorBlobAlgo = (ColorBlobAlgorithm)inspWindow.FindInspAlgorithm(InspectType.InspColorBinary);
                if (colorBlobAlgo != null)
                {
                    int filterArea = colorBlobAlgo.AreaFilter;
                    txtH.Text = HueValue.ToString();
                    txtS.Text = SatValue.ToString();
                    txtV.Text = ValValue.ToString();
                }
            }
        }


        //#COLOR BINARY FILTER#10 컬러이진화 옵션을 선택할때마다, 컬러이진화 이미지가 갱신되도록 하는 함수
        private void UpdateColorBinary()
        {
            bool invert = chkInvert.Checked;
            bool highlight = chkHighlight.Checked;


            ShowColorBinaryMode showColorBinaryMode = ShowColorBinaryMode.ShowColorBinaryNone;
            if (highlight)
            {
                showColorBinaryMode = ShowColorBinaryMode.ShowColorBinaryHighlight;

                bool showColorBinary = chkShowColorBinaryOnly.Checked;

                if (showColorBinary)
                    showColorBinaryMode = ShowColorBinaryMode.ShowColorBinaryOnly;
            }

            RangeChanged?.Invoke(this, new RangeChangedEventArgs(HueValue, SatValue, ValValue, invert, showColorBinaryMode));
        }


        //#COLOR BINARY FILTER#11 GUI 이벤트와 UpdateColorBinary함수 연동
        private void OnValueChanged(object sender, EventArgs e)
        {
            UpdateColorBinary();
        }

        //하이라이트 체크박스    선택 시, 이진화된 이미지를 하이라이트로 표시할지 여부 결정
        private void chkHighlight_CheckedChanged_1(object sender, EventArgs e)
        {
            UpdateColorBinary();
        }

        //반전    체크박스 선택 시, 이진화된 이미지를 반전하여 표시할지 여부 결정
        private void chkInvert_CheckedChanged(object sender, EventArgs e)
        {
            UpdateColorBinary();
        }

        private void chkShowColorBinaryOnly_CheckedChanged(object sender, EventArgs e)
        {
            UpdateColorBinary();
        }


        //#COLOR BINARY FILTER#9 컬러이진화 관련 이벤트 발생시, 전달할 값 추가
        public class RangeChangedEventArgs : EventArgs
        {
            public int LowerHue { get; }
            public int UpperHue { get; }
            public int LowerSaturation { get; }
            public int UpperSaturation { get; }
            public int LowerValue { get; }
            public int UpperValue { get; }

            public int HueValue { get; }
            public int SatValue { get; }
            public int ValValue { get; }
            public bool Invert { get; }
            public ShowColorBinaryMode ShowColorBinMode { get; }

            public RangeChangedEventArgs(int hueValue, int satValue, int valValue, bool invert, ShowColorBinaryMode showColorBinaryMode)
            {
                HueValue = hueValue;
                SatValue = satValue;
                ValValue = valValue;
                Invert = invert;
                ShowColorBinMode = showColorBinaryMode;
            }


            public Mat ProcessColor(Mat inputImage, int hue, int sat, int val, ShowColorBinaryMode mode, int thresholdH = 10, int thresholdS = 50, int thresholdV = 50)
            {
                if (inputImage.Empty())
                    return null;

                Mat hsvImage = new Mat();
                Cv2.CvtColor(inputImage, hsvImage, ColorConversionCodes.BGR2HSV);

                // HSV 범위 설정
                Scalar lowerBound = new Scalar(hue - thresholdH, Math.Max(sat - thresholdS, 0), Math.Max(val - thresholdV, 0));
                Scalar upperBound = new Scalar(hue + thresholdH, Math.Min(sat + thresholdS, 255), Math.Min(val + thresholdV, 255));

                Mat binaryMask = new Mat();
                Cv2.InRange(hsvImage, lowerBound, upperBound, binaryMask);

                if (mode == ShowColorBinaryMode.ShowColorBinaryHighlight)
                {
                    Mat highlightedImage = new Mat();
                    inputImage.CopyTo(highlightedImage); // 원본 이미지 복사

                    // 빨간색(또는 원하는 색)으로 강조 (bitwise 연산 활용)
                    Mat redHighlight = new Mat(inputImage.Size(), inputImage.Type(), new Scalar(0, 0, 255));
                    redHighlight.CopyTo(highlightedImage, binaryMask);

                    return highlightedImage;
                }
                else if (mode == ShowColorBinaryMode.ShowColorBinaryOnly)
                {
                    return binaryMask; // 컬러 이진화된 이미지 반환
                }

                return inputImage; // 기본적으로 원본 유지
            }
        }

        //필터 선택시, 적용할 필터 효과를 선택하고, 필터 옵션을 선택할 수 있도록 개선
        private void select_effect_SelectedIndexChanged(object sender, EventArgs e)
        {
            _selected_effect = Convert.ToString(select_effect.SelectedItem);
            select_effect2.Items.Clear(); // 기존 아이템 제거 후 새로 추가

            if (_selected_effect == "비트연산(Bitwise)")
            {
                select_effect2.Items.Add("NOT 연산"); // NOT 연산만 추가
                select_effect2.Show();
            }
            else if (_selected_effect == "연산")
            {
                select_effect2.Items.Add("빼기");
                select_effect2.Items.Add("절대값 차이 계산");
                select_effect2.Show();
            }
            else
            {
                select_effect2.Hide();
            }
        }

        //필터 적용 버튼 클릭 시
        private void btnApply_Click(object sender, EventArgs e)
        {
            if (_selected_effect == null || _selected_effect2 == -1)
            {
                MessageBox.Show("효과를 선택해주세요.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ColorBinaryInspProp.cs에서 필요한 필터만 전달
            if ((_selected_effect == "비트연산(Bitwise)" && _selected_effect2 == 0) ||  // NOT 연산
                (_selected_effect == "연산" && (_selected_effect2 == 0 || _selected_effect2 == 1)))  // 빼기 연산(0), 절대값 차이(1)
            {
                FilterSelected?.Invoke(this, new FilterSelectedEventArgs(_selected_effect, _selected_effect2));
            }
        }

        //필터 콤보박스 선택 시
        public class FilterSelectedEventArgs : EventArgs
        {
            public string FilterSelected1 { get; }  //적용할 필터효과
            public int FilterSelected2 { get; }  //필터 옵션들 중 선택한것

            public FilterSelectedEventArgs(string filterSelected, int filterSelected2)
            {
                FilterSelected1 = filterSelected;
                FilterSelected2 = filterSelected2;

            }
        }

        private void btnApplyHSV_Click(object sender, EventArgs e)
        {
            InspWindow inspWindow = Global.Inst.InspStage.InspWindow;
            if (inspWindow is null)
                return;

            //inspWindow에서 컬러이진화 알고리즘 찾는 코드 추가
            ColorBlobAlgorithm ColorblobAlgo = (ColorBlobAlgorithm)inspWindow.FindInspAlgorithm(InspectType.InspColorBinary);
            if (ColorblobAlgo is null)
                return;

            HsvRange threshold = new HsvRange();
            threshold.HueUpper = HueValue;
            threshold.SaturationUpper = SatValue;
            threshold.ValueUpper = ValValue;

            threshold.HueLower = HueValue;
            threshold.SaturationLower = SatValue;
            threshold.ValueLower = ValValue;


            threshold.Invert = chkInvert.Checked;

            ColorblobAlgo.ColorRange = threshold;

            int hueArea = int.Parse(txtH.Text);
            int satArea = int.Parse(txtS.Text);
            int valArea = int.Parse(txtV.Text);
            ColorblobAlgo.AreaFilter = hueArea;
            ColorblobAlgo.AreaFilter = satArea;
            ColorblobAlgo.AreaFilter = valArea;

            //이진화 검사시, 해당 InspWindow와 이진화 알고리즘만 실행
            Global.Inst.InspStage.InspWorker.TryInspect(inspWindow, InspectType.InspColorBinary);
        }

        private void btnTeachinColor_Click(object sender, EventArgs e)
        {
            isSelecting = true;
            this.Cursor = Cursors.Cross; // 펜 모양으로 변경
        }

        private void ColorBinaryInspProp_MouseClick(object sender, MouseEventArgs e)
        {
            if (isSelecting)
            {
                // 마우스 클릭 위치 저장
                Point point = new Point(e.X, e.Y);
                // 마우스 클릭 위치에 대한 색상값 저장
                System.Drawing.Color color = System.Drawing.Color.FromArgb(255, point.X, point.Y);
                // 색상값을 HSV로 변환
                Hsv hsv = new Hsv(color.R, color.G, color.B);
                // HSV값을 텍스트박스에 출력
                txtH.Text = hsv.H.ToString();
                txtS.Text = hsv.S.ToString();
                txtV.Text = hsv.V.ToString();
                isSelecting = false;
                this.Cursor = Cursors.Default; // 기본 커서로 변경
            }
        }

        //imageViewer에 ROI 추가 기능 실행
        private void ColorBinaryInspProp_MouseClick(InspWindowType inspWindowType)
        {
            CameraForm cameraForm = MainForm.GetDockForm<CameraForm>();
            if (cameraForm != null)
            {
                cameraForm.AddRoi(inspWindowType);
            }
        }

    }
}
