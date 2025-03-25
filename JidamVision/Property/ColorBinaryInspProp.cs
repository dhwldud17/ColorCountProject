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
        ShowColorBinaryNone = 0,             //컬러이진화 하이라이트 끄기
        ShowColorBinaryHighlight,            //컬러이진화 하이라이트 보기
        ShowColorBinaryOnly                  //배경 없이 컬러이진화 이미지만 보기
    }
    public partial class ColorBinaryInspProp : UserControl
    {
        public event EventHandler<RangeChangedEventArgs> ThresholdChanged;

        public int HCenter => hTrackBar.Value;
        public int SMin => sTrackBar.Value;
        public int VMin => vTrackBar.Value;
        public ColorBinaryInspProp()
        {
            InitializeComponent();
        }

        //#BIN PROP# 이진화 검사 속성값을 GUI에 설정
        public void LoadInspParam()
        {
            // TrackBar 초기 설정
            hTrackBar.ValueChanged += OnValueChanged;
            sTrackBar.ValueChanged += OnValueChanged;
            vTrackBar.ValueChanged += OnValueChanged;

            hTrackBar.Value = 0;
            sTrackBar.Value = 0;
            vTrackBar.Value = 0;


            //#BINARY FILTER#8 이진화 필터값을 GUI에 로딩
            InspWindow inspWindow = Global.Inst.InspStage.InspWindow;
            if (inspWindow != null)
            {
                //#INSP WORKER#13 inspWindow에서 이진화 알고리즘 찾는 코드
                BlobAlgorithm blobAlgo = (BlobAlgorithm)inspWindow.FindInspAlgorithm(InspectType.InspBinary);
                if (blobAlgo != null)
                {
                    int filterArea = blobAlgo.AreaFilter;
                    // HSV 값에 대응하는 텍스트박스 추가
                    txtH.Text = HCenter.ToString();
                    txtS.Text = SMin.ToString();
                    txtV.Text = VMin.ToString();
                }
            }
        }

        // 트랙바 값이 바뀔 때 텍스트박스도 업데이트
        private void OnValueChanged(object sender, EventArgs e)
        {
            txtH.Text = hTrackBar.Value.ToString();
            txtS.Text = sTrackBar.Value.ToString();
            txtV.Text = vTrackBar.Value.ToString();

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

            ThresholdChanged?.Invoke(this, new RangeChangedEventArgs(HCenter, SMin, VMin, showMode));
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
            public int HCenter { get; }
            public int SMin { get; }
            public int VMin { get; }

            public ShowColorBinaryMode ShowColorBinMode { get; }

            public RangeChangedEventArgs(int hCenter, int sMin, int vMin, ShowColorBinaryMode showColorBinaryMode)
            {
                HCenter = hCenter;
                SMin = sMin;
                VMin = vMin;
                ShowColorBinMode = showColorBinaryMode;
            }
        }

       
    }
}
