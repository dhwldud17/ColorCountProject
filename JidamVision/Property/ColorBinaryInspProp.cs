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
        ShowColorBinaryNone = 0,
        ShowColorBinaryHighlight,
        ShowColorBinaryOnly
    }

    public partial class ColorBinaryInspProp : UserControl
    {
        public event EventHandler<RangeChangedEventArgs> ThresholdChanged;
        private ColorBlobAlgorithm colorBlobAlgorithm;

        public int HCenter => hTrackBar.Value;
        public int SMin => sTrackBar.Value;
        public int VMin => vTrackBar.Value;

        public ColorBinaryInspProp()
        {
            InitializeComponent();
        }

        public void SetAlgorithm(ColorBlobAlgorithm algorithm)
        {
            colorBlobAlgorithm = algorithm;
            LoadInspParam();
        }

        public void SetHSVValues(int hue, int saturation, int brightness)
        {
            hTrackBar.Value = hue;
            sTrackBar.Value = saturation;
            vTrackBar.Value = brightness;

            txtH.Text = hue.ToString();
            txtS.Text = saturation.ToString();
            txtV.Text = brightness.ToString();

            UpdateColorBinary();
        }

        public void LoadInspParam()
        {
            if (colorBlobAlgorithm == null)
                return;

            hTrackBar.Value = colorBlobAlgorithm.HCenter;
            sTrackBar.Value = colorBlobAlgorithm.SMin;
            vTrackBar.Value = colorBlobAlgorithm.VMin;

            txtH.Text = HCenter.ToString();
            txtS.Text = SMin.ToString();
            txtV.Text = VMin.ToString();
        }

        private void OnValueChanged(object sender, EventArgs e)
        {
            if (colorBlobAlgorithm != null)
            {
                colorBlobAlgorithm.HCenter = hTrackBar.Value;
                colorBlobAlgorithm.SMin = sTrackBar.Value;
                colorBlobAlgorithm.VMin = vTrackBar.Value;
            }

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

            public Mat ProcessColor(Mat inputImage, Color targetColor, int threshold = 30)
            {
                if (inputImage.Empty())
                    return null;

                Mat hsvImage = new Mat();
                Cv2.CvtColor(inputImage, hsvImage, ColorConversionCodes.BGR2HSV);

                Scalar lowerBound = new Scalar(targetColor.R - threshold, targetColor.G - threshold, targetColor.B - threshold);
                Scalar upperBound = new Scalar(targetColor.R + threshold, targetColor.G + threshold, targetColor.B + threshold);

                Mat binaryMask = new Mat();
                Cv2.InRange(hsvImage, lowerBound, upperBound, binaryMask);

                return binaryMask;
            }

            public Mat ProcessColor(Mat inputImage, Color targetColor)
            {
                // 색상을 기준으로 이진화하는 알고리즘 추가
                Mat binaryImage = new Mat();
                Cv2.InRange(inputImage, new Scalar(targetColor.R - 20, targetColor.G - 20, targetColor.B - 20),
                                       new Scalar(targetColor.R + 20, targetColor.G + 20, targetColor.B + 20),
                                       binaryImage);
                return binaryImage;
            }
        }
    }
}
