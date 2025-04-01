using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
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

    public enum ShowColorBinaryMode
    {
        ShowBinaryNone = 0,             //이진화 하이라이트 끄기
        ShowBinaryHighlight,            //이진화 하이라이트 보기
        ShowBinaryOnly                  //배경 없이 이진화 이미지만 보기
    }


    public partial class ColorBinaryInspProp : UserControl

    {
        private Bitmap _bitmapImage;
        private Rectangle _pickColorRect;
        private bool isSelecting = false;
        private bool _isPickColor = false;

        public event EventHandler<ColorEventArgs> ColorPicked;
        public event EventHandler<EventArgs> TeachingColorClicked;
        public event EventHandler<EventArgs> ColorPickCanceled;

        public event EventHandler<EventArgs> PropertyChanged;
        public event EventHandler<ColorRangeChangedEventArgs> ColorRangeChanged;

        ColorBlobAlgorithm _colorblobAlgo = null;


        public int hLower => hTrackBarLower.Value;
        public int hUpper => hTrackBarUpper.Value;
        public int sLower => sTrackBarLower.Value;
        public int sUpper => sTrackBarUpper.Value;
        public int vLower => vTrackBarLower.Value;
        public int vUpper => vTrackBarUpper.Value;


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

            //기본값 설정
            hTrackBarLower.Value = 10;
            hTrackBarUpper.Value = 100;
            sTrackBarLower.Value = 100;
            sTrackBarUpper.Value = 200;
            vTrackBarLower.Value = 100;
            vTrackBarUpper.Value = 200;

            // 이벤트 핸들러 등록
            this.ColorPicked += ColorPicked;
        }
        public void SetAlgorithm(ColorBlobAlgorithm colorblobAlgo)
        {
            _colorblobAlgo = colorblobAlgo;
            SetProperty();
        }
        public void SetProperty()
        {
            if (_colorblobAlgo is null)
                return;
            HSVThreshold threshold = _colorblobAlgo.HSVThreshold;



            hTrackBarLower.Value = (int)threshold.lower.Val0;
            hTrackBarUpper.Value = (int)threshold.upper.Val0;
            sTrackBarLower.Value = (int)threshold.lower.Val1;
            sTrackBarUpper.Value = (int)threshold.upper.Val1;
            vTrackBarLower.Value = (int)threshold.lower.Val2;
            vTrackBarUpper.Value = (int)threshold.upper.Val2;

           txtMinH.Text = hTrackBarLower.Value.ToString();
           txtMinS.Text = sTrackBarLower.Value.ToString();
            txtMinV.Text = vTrackBarLower.Value.ToString();

           txtMaxH.Text = hTrackBarUpper.Value.ToString();
            txtMaxS.Text = sTrackBarUpper.Value.ToString();
            txtMaxV.Text = vTrackBarUpper.Value.ToString();

   
        }

        public void SetHSV(Vec3b minHSV, Vec3b maxHSV)
        {
            


            hTrackBarLower.Value = (int)minHSV.Item0;
            hTrackBarUpper.Value = (int)maxHSV.Item0;
            sTrackBarLower.Value = (int)minHSV.Item1;
            sTrackBarUpper.Value = (int)maxHSV.Item1;
            vTrackBarLower.Value = (int)minHSV.Item2;
            vTrackBarUpper.Value = (int)maxHSV.Item2;

            txtMinH.Text = minHSV.Item0.ToString();
            txtMinS.Text = minHSV.Item1.ToString();
            txtMinV.Text = minHSV.Item2.ToString();

            txtMaxH.Text = maxHSV.Item0.ToString();
            txtMaxS.Text = maxHSV.Item1.ToString();
            txtMaxV.Text = maxHSV.Item2.ToString();


        }

        public void GetProperty()
        {
            if (_colorblobAlgo is null)
                return;

            HSVThreshold threshold = _colorblobAlgo.HSVThreshold;
            //트랙 바 값 -> 알고리즘으로 보냄 .
            threshold.lower.Val0 = hLower;
            threshold.upper.Val0 = hUpper;
            threshold.lower.Val1 = sLower;
            threshold.upper.Val1 = sUpper;
            threshold.lower.Val2 = vLower;
            threshold.upper.Val2 = vUpper;

            // 필터 적용 버튼 클릭 이벤트
            //   btnApply.Click += btnApply_Click;
            //  btnApplyHSV.Click += btnApplyHSV_Click;

            // 필터 선택 콤보박스 이벤트
            //   select_effect.SelectedIndexChanged += select_effect_SelectedIndexChanged;
            //   select_effect2.SelectedIndexChanged += select_effect_SelectedIndexChanged;


        }


        //#COLOR BINARY FILTER#10 컬러이진화 옵션을 선택할때마다, 컬러이진화 이미지가 갱신되도록 하는 함수
        private void UpdateColorBinary()
        {
            GetProperty();
            bool invert = chkInvert.Checked;
            bool highlight = chkHighlight.Checked;

            ShowColorBinaryMode showBinaryMode = ShowColorBinaryMode.ShowBinaryNone;
            if (highlight)
            {
                showBinaryMode = ShowColorBinaryMode.ShowBinaryHighlight;

                bool showBinary = chkShowColorBinaryOnly.Checked;

                if (showBinary)
                    showBinaryMode = ShowColorBinaryMode.ShowBinaryOnly;
            }

            ColorRangeChanged?.Invoke(this, new ColorRangeChangedEventArgs(hLower, hUpper, sLower, sUpper, vLower, vUpper, invert, showBinaryMode));


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




        //해당 버튼누르면 해당 roi에 현재 설정된 HSV값 저장되게.
        private void btnApplyHSV_Click(object sender, EventArgs e)
        {

        }

        private void btnApply_Click(object sender, EventArgs e)
        {

        }

        private void select_effect_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        //#COLOR BINARY FILTER#9 컬러이진화 관련 이벤트 발생시, 전달할 값 추가
        public class ColorRangeChangedEventArgs : EventArgs
        {
            public int LowerHue { get; }
            public int UpperHue { get; }
            public int LowerSaturation { get; }
            public int UpperSaturation { get; }
            public int LowerValue { get; }
            public int UpperValue { get; }


            public bool Invert { get; }
            public ShowColorBinaryMode ShowColorBinaryMode { get; }


            public ColorRangeChangedEventArgs(int lowerHue, int upperHue, int lowerSaturation, int upperSaturation, int lowerValue, int upperValue, bool invert, ShowColorBinaryMode showBinaryMode)
            {
                this.LowerHue = lowerHue;
                this.UpperHue = upperHue;
                this.LowerSaturation = lowerSaturation;
                this.UpperSaturation = upperSaturation;
                this.LowerValue = lowerValue;
                this.UpperValue = upperValue;
                this.Invert = invert;
                this.ShowColorBinaryMode = showBinaryMode;

            }
        }

        protected virtual void OnColorPicked(Color color)
        {
            if (ColorPicked == null)
            {
                Console.WriteLine("⚠ ColorPicked 이벤트가 구독되지 않음!");
                return;
            }

            Console.WriteLine($"ColorPicked 이벤트 발생: {color}");
            ColorPicked?.Invoke(this, new ColorEventArgs(color)); // 이벤트 발생!
        }

        public void PickColor(Color color)
        {
            OnColorPicked(color); // 이벤트 실행
            SetHSVRangeFromColor(color); // 👈 추가: 자동 HSV 범위 설정
            UpdateColorBinary();   // 👈 추가: 이진화 반영
        }

        private void SetHSVRangeFromColor(Color color)
        {
            // RGB → HSV 변환
            Mat rgbMat = new Mat(1, 1, MatType.CV_8UC3, new Scalar(color.B, color.G, color.R));
            Mat hsvMat = new Mat();
            Cv2.CvtColor(rgbMat, hsvMat, ColorConversionCodes.BGR2HSV);
            Vec3b hsv = hsvMat.At<Vec3b>(0, 0);

            int h = hsv.Item0;
            int s = hsv.Item1;
            int v = hsv.Item2;

            // ±범위로 트랙바 자동 설정 (안정적 범위 설정)
            hTrackBarLower.Value = Math.Max(0, h - 10);
            hTrackBarUpper.Value = Math.Min(180, h + 10);
            sTrackBarLower.Value = Math.Max(0, s - 50);
            sTrackBarUpper.Value = Math.Min(255, s + 50);
            vTrackBarLower.Value = Math.Max(0, v - 50);
            vTrackBarUpper.Value = Math.Min(255, v + 50);
        }

        //색상 선택 이벤트 발생시, 전달할 값 추가
        public class ColorEventArgs : EventArgs
        {
            public Color PickedColor { get; }
            public ColorEventArgs(Color pickedColor)
            {
                this.PickedColor = pickedColor;
            }
        }

        //색상 추출 메서드
        //public void ExtractColorFromSelection()
        //{
        //    if (_bitmapImage == null || _pickColorRect.Width == 0 || _pickColorRect.Height == 0)
        //        return;
        //    Console.WriteLine($"_pickColorRect: {_pickColorRect}, Width: {_pickColorRect.Width}, Height: {_pickColorRect.Height}");

        //    // 선택된 영역에서 색상 추출 (예: 이미지의 픽셀 색상 평균값)
        //    Bitmap bmpImage = new Bitmap(_bitmapImage); // 이미지 복제
        //    Color pixelColor = bmpImage.GetPixel(_pickColorRect.X + _pickColorRect.Width / 2,
        //                                    _pickColorRect.Y + _pickColorRect.Height / 2); // 중간 픽셀 추출

        //    // 추출된 색상을 변수에 저장하거나, UI에 표시
        //    ColorPicked?.Invoke(this, new ColorEventArgs(pixelColor));  // 이벤트로 색상 전달
        //}

        //학습 버튼 클릭 시 색상 추출 호출
        private void btnTeachingColor_Click(object sender, EventArgs e)
        {
            if (!_isPickColor)
            {
                TeachingColorClicked?.Invoke(this, new EventArgs()); // 이벤트 발생
                _isPickColor = true;
                btnTeachingColor.BackColor = Color.LightGreen;
            }
            else
            {
                ColorPickCanceled?.Invoke(this, new EventArgs()); // 이벤트 발생
                _isPickColor = false;
                btnTeachingColor.BackColor = Color.LightGray;
                
            }
         
        }

        private void panelColorPreview_Paint(object sender, PaintEventArgs e)
        {
            if (_isPickColor)
            {
                Graphics g = e.Graphics;
                g.FillRectangle(new SolidBrush(Color.Red), _pickColorRect);
            }
           
        }

    }
}

        
    

   


        //필터 선택시, 적용할 필터 효과를 선택하고, 필터 옵션을 선택할 수 있도록 개선





     

      
    

