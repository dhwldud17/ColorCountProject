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

    public enum ShowColorBinaryMode
    {
        ShowBinaryNone = 0,             //이진화 하이라이트 끄기
        ShowBinaryHighlight,            //이진화 하이라이트 보기
        ShowBinaryOnly                  //배경 없이 이진화 이미지만 보기
    }


    public partial class ColorBinaryInspProp : UserControl

    {
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
            txtH.Text = hUpper.ToString();
            txtS.Text = sUpper.ToString();
            txtV.Text = vUpper.ToString();

            //txtAreaMax.Text = _blobAlgo.AreaMax.ToString();
            //txtWidthMin.Text = _blobAlgo.WidthMin.ToString();
            //txtWidthMax.Text = _blobAlgo.WidthMax.ToString();
            //txtHeightMin.Text = _blobAlgo.HeightMin.ToString();
            //txtHeightMax.Text = _blobAlgo.HeightMax.ToString();
            //txtCount.Text = _blobAlgo.BlobCount.ToString();
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
            //   Global.Inst.InspStage.InspWorker.TryInspect(inspWindow, InspectType.InspColorBinary);
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

        private void btnTeachinColor_Click(object sender, EventArgs e)
        {
          
        }


    }


}



     

      
    

