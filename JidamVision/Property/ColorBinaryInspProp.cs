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
      
        ColorBlobAlgorithm _colorblobAlgo = null;
        // 속성값을 이용하여 이진화 임계값 설정
        public int HueValue => hTrackBar.Value; // Hue 값만 설정
        public int SatValue => sTrackBar.Value; // Saturation 값만 설정
        public int ValValue => vTrackBar.Value; // Value 값만 설정
        public ColorBinaryInspProp()
        {
            InitializeComponent();
        }

        public void SetAlgorithm(ColorBlobAlgorithm colorblobAlgo)
        {
            _colorblobAlgo = colorblobAlgo;
            SetProperty();
        }
        public void SetProperty()
        {
           


            UpdateColorBinary();
        }
        //#BIN PROP# 이진화 검사 속성값을 GUI에 설정
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
    
          
        }

        // 트랙바 값이 바뀔 때 텍스트박스도 업데이트
        private void OnValueChanged(object sender, EventArgs e)
        {
            UpdateColorBinary();
           
        }
        private void chkHighlight_CheckedChanged(object sender, EventArgs e)
        {
            UpdateColorBinary();
        }

        private void UpdateColorBinary()
        {
            bool highlight = chkHighlight.Checked;
            ShowColorBinaryMode showMode = highlight ? ShowColorBinaryMode.ShowColorBinaryHighlight : ShowColorBinaryMode.ShowColorBinaryNone;
 
        }

        //버튼 클릭 시 색 선택하는 코드
        private void btnTeachinColor_Click(object sender, EventArgs e)
        {
            using (ColorDialog colorDialog = new ColorDialog())
            {
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    panelColorPreview.BackColor = colorDialog.Color; // 선택한 색상 미리보기
                    ConvertRGBtoHSV(colorDialog.Color);
                }
            }
        }

        // 컬러 선택 시 HSV 값 변환하여 반영
        private void ConvertRGBtoHSV(Color color)
        {
            float hue = color.GetHue();
            float saturation = color.GetSaturation() * 255;
            float brightness = color.GetBrightness() * 255;

            hTrackBar.Value = (int)hue;
            sTrackBar.Value = (int)saturation;
            vTrackBar.Value = (int)brightness;

            // HSV 값을 텍스트박스에 표시
            txtH.Text = hTrackBar.Value.ToString();
            txtS.Text = sTrackBar.Value.ToString();
            txtV.Text = vTrackBar.Value.ToString();

            UpdateColorBinary();
        }


        //컬러 이진화 관련 이벤트 발생시, 전달할 값 추가
        public class RangeChangedEventArgs : EventArgs
        {
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
        }

       
    }
}
