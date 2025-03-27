using JidamVision.Algorithm;
using JidamVision.Core;
using JidamVision.Teach;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.MonthCalendar;

namespace JidamVision.Property
{
    /*
    #BINARY FILTER# - <<<이진화 검사 개발>>> 
    입력된 lower, upper 임계값을 이용해, 영상을 이진화한 후, Filter(area)등을 이용해, 원하는 영역을 찾는다.
     */

    //#BINARY FILTER#7 이진화 하이라이트, 이외에, 이진화 이미지를 보기 위한 옵션
    public enum ShowBinaryMode
    {
        ShowBinaryNone = 0,             //이진화 하이라이트 끄기
        ShowBinaryHighlight,            //이진화 하이라이트 보기
        ShowBinaryOnly                  //배경 없이 이진화 이미지만 보기
    }

    public partial class BinaryInspProp : UserControl
    {
        public event EventHandler<EventArgs> PropertyChanged;
        public event EventHandler<RangeChangedEventArgs> RangeChanged;

        BlobAlgorithm _blobAlgo = null;

        /* NOTE
        public int LowerValue
        {
            get { return trackBarLower.Value; }
        }
        C# 6부터는 위 코드를 더 간결하게 람다(=>) 문법을 사용하여 표현
        */

        // 속성값을 이용하여 이진화 임계값 설정
        public int LowerValue => trackBarLower.Value;
        public int UpperValue => trackBarUpper.Value;

        public BinaryInspProp()
        {
            InitializeComponent();

            // TrackBar 초기 설정
            trackBarLower.ValueChanged += OnValueChanged;
            trackBarUpper.ValueChanged += OnValueChanged;

            txtArea.Leave += OnAreaLeave;

            trackBarLower.Value = 0;
            trackBarUpper.Value = 128;

        }

        public void SetAlgorithm(BlobAlgorithm blobAlgo)
        {
            _blobAlgo = blobAlgo;
            SetProperty();
        }

        public void SetProperty()
        {
            if (_blobAlgo is null)
                return;

            BinaryThreshold threshold = _blobAlgo.BinThreshold;
            trackBarLower.Value = threshold.lower;
            trackBarUpper.Value = threshold.upper;
            chkInvert.Checked = threshold.invert;

            int filterArea = _blobAlgo.AreaFilter;
            txtArea.Text = filterArea.ToString();

            Mat teachImage = _blobAlgo.GetTemplateImage();
            if (teachImage != null)
            {
                Bitmap bmpImage = BitmapConverter.ToBitmap(teachImage);
                picTeachImage.Image = bmpImage;
            }
        }

        public void GetProperty()
        {
            if (_blobAlgo is null)
                return;

            BinaryThreshold threshold = new BinaryThreshold();
            threshold.upper = UpperValue;
            threshold.lower = LowerValue;
            threshold.invert = chkInvert.Checked;

            _blobAlgo.BinThreshold = threshold;

            if (txtArea.Text != "")
            {
                int filterArea = int.Parse(txtArea.Text);
                _blobAlgo.AreaFilter = filterArea;
            }
        }

        //#BINARY FILTER#10 이진화 옵션을 선택할때마다, 이진화 이미지가 갱신되도록 하는 함수
        private void UpdateBinary()
        {
            GetProperty();

            bool invert = chkInvert.Checked;
            bool highlight = chkHighlight.Checked;

            ShowBinaryMode showBinaryMode = ShowBinaryMode.ShowBinaryNone;
            if (highlight)
            {
                showBinaryMode = ShowBinaryMode.ShowBinaryHighlight;

                bool showBinary = chkShowBinary.Checked;

                if (showBinary)
                    showBinaryMode = ShowBinaryMode.ShowBinaryOnly;
            }

            RangeChanged?.Invoke(this, new RangeChangedEventArgs(LowerValue, UpperValue, invert, showBinaryMode));
        }

        //#BINARY FILTER#11 GUI 이벤트와 UpdateBinary함수 연동
        private void OnValueChanged(object sender, EventArgs e)
        {
            UpdateBinary();
        }

        private void chkInvert_CheckedChanged(object sender, EventArgs e)
        {
            UpdateBinary();
        }

        private void chkBinaryOnly_CheckedChanged(object sender, EventArgs e)
        {
            UpdateBinary();
        }

        private void chkHighlight_CheckedChanged(object sender, EventArgs e)
        {
            UpdateBinary();
        }

        private void OnAreaLeave(object sender, EventArgs e)
        {
            if (_blobAlgo == null)
                return;

            if (int.TryParse(txtArea.Text, out int area))
            {
                _blobAlgo.AreaFilter = area;
                PropertyChanged?.Invoke(this, null);
            }
            else
            {
                MessageBox.Show("숫자만 입력 가능합니다.");
                txtArea.Text = _blobAlgo.AreaFilter.ToString(); // 기존 값 복원
            }
        }
    }

    //#BINARY FILTER#9 이진화 관련 이벤트 발생시, 전달할 값 추가
    public class RangeChangedEventArgs : EventArgs
    {
        public int LowerValue { get; }
        public int UpperValue { get; }
        public bool Invert { get; }
        public ShowBinaryMode ShowBinMode { get; }

        public RangeChangedEventArgs(int lowerValue, int upperValue, bool invert, ShowBinaryMode showBinaryMode)
        {
            LowerValue = lowerValue;
            UpperValue = upperValue;
            Invert = invert;
            ShowBinMode = showBinaryMode;
        }
    }
}
