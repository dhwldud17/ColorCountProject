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
        private ColorBlobAlgorithm colorBlobAlgorithm;

        /* NOTE
        public int LowerValue
        {
            get { return trackBarLower.Value; }
        }
        C# 6부터는 위 코드를 더 간결하게 람다(=>) 문법을 사용하여 표현
        */

        // 속성값을 이용하여 이진화 임계값 설정
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

        //private void UpdateColorBinary()
        //{
        //    if (colorBlobAlgorithm == null)
        //        return;

        //    ShowColorBinaryMode showMode = chkHighlight.Checked ? ShowColorBinaryMode.ShowColorBinaryHighlight : ShowColorBinaryMode.ShowColorBinaryNone;

        //    Mat processedImage = colorBlobAlgorithm.ProcessColor(colorBlobAlgorithm.SourceImage, HCenter, SMin, VMin, showMode);

        //    if (processedImage != null)
        //    {
        //        imageViewer.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(processedImage);  // imageViewer로 변경
        //    }

        //    ThresholdChanged?.Invoke(this, new RangeChangedEventArgs(HCenter, SMin, VMin, showMode));
        //}

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

            /*ProcessColor작업 : 
              ThresholdChanged 이벤트가 발생할 때 ProcessColor를 호출하여 필터링 적용

              colorBlobAlgorithm을 통해 HCenter, SMin, VMin 값을 동적으로 업데이트

              ShowColorBinaryMode 옵션을 적용하여 이진화된 이미지를 UI에서 확인할 수 있도록 개선*/
            public Mat ProcessColor(Mat inputImage, int hCenter, int sMin, int vMin, ShowColorBinaryMode mode, int thresholdH = 10, int thresholdS = 50, int thresholdV = 50)
            {
                if (inputImage.Empty())
                    return null;

                Mat hsvImage = new Mat();
                Cv2.CvtColor(inputImage, hsvImage, ColorConversionCodes.BGR2HSV);

                // HSV 범위 설정
                Scalar lowerBound = new Scalar(hCenter - thresholdH, Math.Max(sMin - thresholdS, 0), Math.Max(vMin - thresholdV, 0));
                Scalar upperBound = new Scalar(hCenter + thresholdH, Math.Min(sMin + thresholdS, 255), Math.Min(vMin + thresholdV, 255));

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

        private void hTrackBar_Scroll(object sender, EventArgs e)
        {

        }
    }
    
}
