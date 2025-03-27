using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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

    //컬러이진화 하이라이트, 이외에, 이진화 이미지를 보기 위한 옵션
    public enum ShowColorBinaryMode
    {
        ShowColorBinaryNone = 0, //이진화 컬러 하이라이트 끄기
        ShowColorBinaryHighlight, //이진화 컬러 하이라이트 보기
        ShowColorBinary, //이진화 컬러 이미지만 보기
        ShowColorBinaryOnly //배경 없이 이진화 컬러 이미지만 보기
    }

    public partial class ColorBinaryInspProp : UserControl

    {
        public event EventHandler<FilterSelectedEventArgs> FilterSelected;
        private String _selected_effect;
        private int _selected_effect2 = -1;

        public event EventHandler<RangeChangedEventArgs> ThresholdChanged; //이벤트 추가
       

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

        public void LoadInspParam()
        {
           
            // TrackBar 초기 설정
            hTrackBar.ValueChanged += OnValueChanged;
            sTrackBar.ValueChanged += OnValueChanged;
            vTrackBar.ValueChanged += OnValueChanged;

            //초기설정 
            hTrackBar.Value = 0;
            sTrackBar.Value = 0;
            vTrackBar.Value = 0;
            InspWindow inspWindow = Global.Inst.InspStage.InspWindow;
           
                if (inspWindow is null)
                    return;
            //#INSP WORKER#13 inspWindow에서 이진화 알고리즘 찾는 코드
            ColorBlobAlgorithm colorblobAlgo = (ColorBlobAlgorithm)inspWindow.FindInspAlgorithm(InspectType.InspColorBinary);
            if (colorblobAlgo != null)
            {
            txtH.Text = hTrackBar.Value.ToString();
            txtS.Text = sTrackBar.Value.ToString();
            txtV.Text = vTrackBar.Value.ToString();
            }
                


        }
        // 트랙바 값에 맞는 TextBox를 업데이트하는 메서드 추가
        public void UpdateColorValues(int hue, int sat, int val)
        {
            
        }
        private void OnValueChanged(object sender, EventArgs e)
        {
            //if (colorBlobAlgorithm != null)
            //{
            //    colorBlobAlgorithm.Hue = hTrackBar.Value;
            //    colorBlobAlgorithm.Sat = sTrackBar.Value;
            //    colorBlobAlgorithm.Val = vTrackBar.Value;
            //}


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

        private void UpdateColorBinary()
        {
            bool invert = chkInvert.Checked;
            ShowColorBinaryMode showMode = chkHighlight.Checked ? ShowColorBinaryMode.ShowColorBinaryHighlight : ShowColorBinaryMode.ShowColorBinaryNone;

            ThresholdChanged?.Invoke(this, new RangeChangedEventArgs(HueValue, SatValue, ValValue, invert, showMode));
        }

        public class RangeChangedEventArgs : EventArgs
        {
            //private Scalar hue;
            //private Scalar sat;
            //private Scalar val;
            //private ShowColorBinaryMode showMode;

            public int Hue { get; }
            public int Sat { get; }
            public int Val { get; }
            public bool Invert { get; }
            public ShowColorBinaryMode ShowColorBinMode { get; }

            public RangeChangedEventArgs(int hue, int sat, int val, bool invert, ShowColorBinaryMode showColorBinaryMode)
            {
                Hue = hue;
                Sat = sat;
                Val = val;
                Invert = invert;
                ShowColorBinMode = showColorBinaryMode;
            }

            //public RangeChangedEventArgs(Scalar hue, Scalar sat, Scalar val, ShowColorBinaryMode showMode)
            //{
            //    this.hue = hue;
            //    this.sat = sat;
            //    this.val = val;
            //    this.showMode = showMode;
            //}

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
            if (inspWindow == null)
            {
                MessageBox.Show("Inspection window is not initialized.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            ColorBlobAlgorithm ColorblobAlgo = (ColorBlobAlgorithm)inspWindow.FindInspAlgorithm(InspectType.InspColorBinary);
            if (ColorblobAlgo is null)
                return;
            // lower 값은 0으로 고정
            int lowerH = 0; // H 값 (lower는 0)
            int lowerS = 0; // S 값 (lower는 0)
            int lowerV = 0; // V 값 (lower는 0)

            // upper 값은 TrackBar에서 가져오기
            int upperH = hTrackBar.Value; // H의 upper 값
            int upperS = sTrackBar.Value; // S의 upper 값
            int upperV = vTrackBar.Value; // V의 upper 값
            bool invert = chkInvert.Checked;


            //inspWindow에서 컬러이진화 알고리즘 찾는 코드 추가


            // ColorThreshold threshold = new ColorThreshold();
            //threshold.upper = HueValue;
            //threshold.upper = SatValue;
            //threshold.upper = ValValue;

            //threshold.lower = HueValue;
            //threshold.lower = SatValue;
            //threshold.lower = ValValue;

            //구조체에 값 설정
            ColorThreshold colorThreshold = new ColorThreshold
            {
                lower = new Scalar(lowerH, lowerS, lowerV), // Scalar에 HSV 값 할당
                upper = new Scalar(upperH, upperS, upperV), // Scalar에 HSV 값 할당
                invert = invert
            };




            //ColorblobAlgo.ColorBinThreshold = threshold;         
            //int hueArea = int.Parse(txtH.Text);
            //int satArea = int.Parse(txtS.Text);
            //int valArea = int.Parse(txtV.Text);
            //ColorblobAlgo.AreaFilter = hueArea;
            //ColorblobAlgo.AreaFilter = satArea;
            //ColorblobAlgo.AreaFilter = valArea;

            //#INSP WORKER#10 이진화 검사시, 해당 InspWindow와 이진화 알고리즘만 실행
            Global.Inst.InspStage.InspWorker.TryInspect(inspWindow, InspectType.InspColorBinary);
        }

        private void txtH_TextChanged(object sender, EventArgs e)
        {

        }
    }

}
