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
using Point = System.Drawing.Point;

namespace JidamVision.Property
{
    /*
    #COLOR BINARY FILTER# - <<<컬러이진화 검사 개발>>> 
    입력된 H, S, V 임계값을 이용해, 레퍼런스이미지를 컬러이진화한 후, Filter(area)등을 이용해, 원하는 영역을 찾는다.
     */

    public struct HsvRange
    {
        public int HueLower, HueUpper; // Hue 범위
        public int SaturationLower, SaturationUpper; // Saturation 범위
        public int ValueLower, ValueUpper; // Value 범위
        
        //public HsvRange(int hueLower, int hueUpper, int saturationLower, int saturationUpper, int valueLower, int valueUpper)
        //{
        //    int HueLower = hueLower;
        //    int HueUpper = hueUpper;
        //    int SaturationLower = saturationLower;
        //    int SaturationUpper = saturationUpper;
        //    int ValueLower = valueLower;
        //    int ValueUpper = valueUpper;
        //}
    }


    public partial class ColorBinaryInspProp : UserControl

    {   //콤보박스 필터   효과1,2번 선택 이벤트 추가
        public event EventHandler<FilterSelectedEventArgs> FilterSelected;
        private String _selected_effect;
        private int _selected_effect2 = -1;
        ColorBlobAlgorithm _colorBlobAlgo = null;
        public event EventHandler<RangeChangedEventArgs> RangeChanged; //HSV임계값 이벤트 추가
        private ColorBlobAlgorithm colorBlobAlgorithm;

        private System.Drawing.Point startPoint;
        private Rectangle selectedArea;
        private bool isSelecting = false;

        /* NOTE
        public int LowerValue
        {
            get { return trackBarLower.Value; }
        }
        C# 6부터는 위 코드를 더 간결하게 람다(=>) 문법을 사용하여 표현
        */

        Vec3b _hsvMin = new Vec3b(0, 0, 0);

        Vec3b _hsvMax = new Vec3b(0, 0, 0);


        public ColorBinaryInspProp()
        {
            InitializeComponent();
            // TrackBar 초기 설정
            hTrackBarLower.ValueChanged += OnValueChanged;
            hTrackBarUpper.ValueChanged += OnValueChanged;
            sTrackBarLower.ValueChanged += OnValueChanged;
            sTrackBarUpper.ValueChanged += OnValueChanged;
            vTrackBarLower.ValueChanged += OnValueChanged;
            vTrackBarUpper.ValueChanged += OnValueChanged;

            // HSV 범위 초기화
            hTrackBarLower.Minimum = 0;
            hTrackBarLower.Maximum = 255;
            sTrackBarLower.Minimum = 0;
            sTrackBarLower.Maximum = 255;
            vTrackBarLower.Minimum = 0;
            vTrackBarLower.Maximum = 255;

            hTrackBarLower.Value = 0;
            sTrackBarUpper.Value = 0;
            vTrackBarLower.Value = 0;
        
                //int filterArea = colorBlobAlgo.AreaFilter    ;

                   
        }

        public void SetProperty()
        {
            if (_colorBlobAlgo is null)
                return;
       
                    hTrackBarLower.Value = (int)_colorBlobAlgo.HsvMin.Item0;
                    hTrackBarUpper.Value = (int)_colorBlobAlgo.HsvMax.Item0;
                    sTrackBarLower.Value = (int)_colorBlobAlgo.HsvMin.Item1;
                    sTrackBarUpper.Value = (int)_colorBlobAlgo.HsvMax.Item1;
                    vTrackBarLower.Value = (int)_colorBlobAlgo.HsvMin.Item2;
                    vTrackBarUpper.Value = (int)_colorBlobAlgo.HsvMax.Item2;
            //txtAreaMin.Text = _colorBlobAlgo.AreaMin.ToString();
            //txtAreaMax.Text = _blobAlgo.AreaMax.ToString();
            //txtWidthMin.Text = _blobAlgo.WidthMin.ToString();
            //txtWidthMax.Text = _blobAlgo.WidthMax.ToString();
            //txtHeightMin.Text = _blobAlgo.HeightMin.ToString();
            //txtHeightMax.Text = _blobAlgo.HeightMax.ToString();
            //txtCount.Text = _blobAlgo.BlobCount.ToString();
        }

        public void GetProperty()
        {
            if (_colorBlobAlgo is null)
                return;


            // 필터 적용 버튼 클릭 이벤트
            btnApply.Click += btnApply_Click;
            btnApplyHSV.Click += btnApplyHSV_Click;

            // 필터 선택 콤보박스 이벤트
            select_effect.SelectedIndexChanged += select_effect_SelectedIndexChanged;
            select_effect2.SelectedIndexChanged += select_effect_SelectedIndexChanged;

            _hsvMin.Item0 = (byte)hTrackBarLower.Value;
            _hsvMax.Item0 = (byte)hTrackBarUpper.Value;
            _hsvMin.Item1 = (byte)sTrackBarLower.Value;
            _hsvMax.Item1 = (byte)sTrackBarUpper.Value;
            _hsvMin.Item2 = (byte)vTrackBarLower.Value;
            _hsvMax.Item2 = (byte)vTrackBarUpper.Value;

            int hueArea = int.Parse(txtH.Text);
            int satArea = int.Parse(txtS.Text);
            int valArea = int.Parse(txtV.Text);
            _colorBlobAlgo.AreaFilter = hueArea;
            _colorBlobAlgo.AreaFilter = satArea;
            _colorBlobAlgo.AreaFilter = valArea;
        }


        //#COLOR BINARY FILTER#10 컬러이진화 옵션을 선택할때마다, 컬러이진화 이미지가 갱신되도록 하는 함수
        private void UpdateColorBinary()
        {
            bool invert = chkInvert.Checked;
            bool highlight = chkHighlight.Checked;

            ShowBinaryMode showBinaryMode = ShowBinaryMode.ShowBinaryNone;
            if (highlight)
            {
                showBinaryMode = ShowBinaryMode.ShowBinaryHighlight;

                bool showBinary = chkShowColorBinaryOnly.Checked;

                if (showBinary)
                    showBinaryMode = ShowBinaryMode.ShowBinaryOnly;
            }

 RangeChanged?.Invoke(this, new RangeChangedEventArgs(_hsvMin, _hsvMax, invert, showBinaryMode));
                   
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

        //필터 적용 버튼 클릭 시
        private void btnApplyHSV_Click(object sender, EventArgs e)
        {
            
            

            //inspWindow에서 컬러이진화 알고리즘 찾는 코드 추가
           
           

            //HsvRange threshold = new HsvRange();
            //threshold.HueUpper = hTrackBarUpper;
            //threshold.SaturationUpper = sTrackBarUpper;

            //threshold.ValueUpper = ValValue;

            //threshold.HueLower = HueValue;
            //threshold.SaturationLower = SatValue;
            //threshold.ValueLower = ValValue;


            //threshold.Invert = chkInvert.Checked;

            //ColorblobAlgo.ColorRange = threshold;

           

            //이진화 검사시, 해당 InspWindow와 이진화 알고리즘만 실행
            Global.Inst.InspStage.InspWorker.TryInspect(inspWindow, InspectType.InspColorBinary);   
        }

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

       
        //#COLOR BINARY FILTER#9 컬러이진화 관련 이벤트 발생시, 전달할 값 추가
        public class RangeChangedEventArgs : EventArgs
        {
            //public int LowerHue { get; }
            //public int UpperHue { get; }
            //public int LowerSaturation { get; }
            //public int UpperSaturation { get; }
            //public int LowerValue { get; }
            //public int UpperValue { get; }
            //
            public Vec3b HsvMin { get;  }
            public Vec3b HsvMax { get;  }

            public bool Invert { get; }
            public ShowBinaryMode ShowBinaryMode { get; }


            public RangeChangedEventArgs(Vec3b hsvMin, Vec3b hsvMax, bool invert, ShowBinaryMode showBinaryMode)
            {
                HsvMin = hsvMin;
                HsvMax = hsvMax;
                Invert = invert;
                ShowBinaryMode = ShowBinaryMode;
            }

            //public RangeChangedEventArgs(int hueValue, int satValue, int valValue, bool invert, ShowColorBinaryMode showColorBinaryMode)
            //{
            //    LowerHue = hueValue;
            //    UpperHue = hueValue;
            //    LowerSaturation = satValue;
            //    UpperSaturation = satValue;
            //    LowerValue = valValue;
            //    UpperValue = valValue;
            //    Invert = invert;
            //    ShowColorBinMode = showColorBinaryMode;
            //}


            //public Mat ProcessColor(Mat inputImage, int hue, int sat, int val, ShowColorBinaryMode mode, int thresholdH = 10, int thresholdS = 50, int thresholdV = 50)
            //{
            //    if (inputImage.Empty())
            //        return null;

            //    Mat hsvImage = new Mat();
            //    Cv2.CvtColor(inputImage, hsvImage, ColorConversionCodes.BGR2HSV);

            //    // HSV 범위 설정
            //    Scalar lowerBound = new Scalar(hue - thresholdH, Math.Max(sat - thresholdS, 0), Math.Max(val - thresholdV, 0));
            //    Scalar upperBound = new Scalar(hue + thresholdH, Math.Min(sat + thresholdS, 255), Math.Min(val + thresholdV, 255));

            //    Mat binaryMask = new Mat();
            //    Cv2.InRange(hsvImage, lowerBound, upperBound, binaryMask);

            //    if (mode == ShowColorBinaryMode.ShowColorBinaryHighlight)
            //    {
            //        Mat highlightedImage = new Mat();
            //        inputImage.CopyTo(highlightedImage); // 원본 이미지 복사

            //        // 빨간색(또는 원하는 색)으로 강조 (bitwise 연산 활용)
            //        Mat redHighlight = new Mat(inputImage.Size(), inputImage.Type(), new Scalar(0, 0, 255));
            //        redHighlight.CopyTo(highlightedImage, binaryMask);

            //        return highlightedImage;
            //    }
            //    else if (mode == ShowColorBinaryMode.ShowColorBinaryOnly)
            //    {
            //        return binaryMask; // 컬러 이진화된 이미지 반환
            //    }

            //    return inputImage; // 기본적으로 원본 유지
            //}
        }

        //필터 선택시, 적용할 필터 효과를 선택하고, 필터 옵션을 선택할 수 있도록 개선
        

        
       

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
        

       

        private void btnTeachinColor_Click(object sender, EventArgs e)
        {
            isSelecting = true;
            this.Cursor = Cursors.Cross; // 펜 모양으로 변경
        }

        

    }

        private void hTrackBarLower_Scroll(object sender, EventArgs e)
        {

        }
    }
