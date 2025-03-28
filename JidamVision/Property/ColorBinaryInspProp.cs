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
using ColorMine.ColorSpaces;
using ColorMine.ColorSpaces.Comparisons;
using static System.Windows.Forms.MonthCalendar;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Point = System.Drawing.Point;
using System.Diagnostics.Eventing.Reader;

namespace JidamVision.Property
{
    /*
    #COLOR BINARY FILTER# - <<<컬러이진화 검사 개발>>> 
    입력된 H, S, V 임계값을 이용해, 레퍼런스이미지를 컬러이진화한 후, 
    Filter(area)등을 이용해, 원하는 영역을 찾는다.
     */


    public partial class ColorBinaryInspProp : UserControl

    {   //콤보박스 필터   효과1,2번 선택 이벤트 추가
        public event EventHandler<FilterSelectedEventArgs> FilterSelected;
        private String _selected_effect;
        private int _selected_effect2 = -1;



        // 사용자가 HSV 범위를 변경할 때 발생하는 이벤트
        // 변경된 범위 정보는 RangeChangedEventArgs를 통해 전달됨
        public event EventHandler<RangeChangedEventArgs> RangeChanged;
        private ColorBlobAlgorithm colorBlobAlgorithm;
        // HSV 색상 범위 내에서 특정 색상의 Blob(객체)을 감지하는 알고리즘 객체
        private Panel colorPanel; // 컬러 선택을 위한 패널
        private System.Drawing.Color selectedColor; // 선택된 색상


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

        Vec3b _hsvMin = new Vec3b(0, 0, 0); // HSV 최소 범위 (Hue, Saturation, Value) 초기값 : 0, 0, 0

        Vec3b _hsvMax = new Vec3b(0, 0, 0); // HSV 최대 범위 (Hue, Saturation, Value) 초기값 : 0, 0, 0


        public ColorBinaryInspProp()
        {
            InitializeComponent();

        }

        //컬러 이진화 검사 속성값을 GUI에 설정
        public void LoadInspParam() //검사 설정 값을 불러오는 메서드
        {


            // HSV 범위 슬라이더 값 변경 시 OnValueChanged 메서드를 호출하도록 이벤트 핸들러 연결
            hTrackBarLower.ValueChanged += OnValueChanged;
            hTrackBarUpper.ValueChanged += OnValueChanged;
            sTrackBarLower.ValueChanged += OnValueChanged;
            sTrackBarUpper.ValueChanged += OnValueChanged;
            vTrackBarLower.ValueChanged += OnValueChanged;
            vTrackBarUpper.ValueChanged += OnValueChanged;

            //HSV TrackBar의 최소/최대 범위 설정
            hTrackBarLower.Minimum = 0;
            hTrackBarLower.Maximum = 128;
            sTrackBarLower.Minimum = 0;
            sTrackBarLower.Maximum = 255;
            vTrackBarLower.Minimum = 0;
            vTrackBarLower.Maximum = 255;

            //초기값 설정
            hTrackBarLower.Value = 0;
            hTrackBarUpper.Value = 128;
            sTrackBarLower.Value = 0;
            sTrackBarUpper.Value = 255;
            vTrackBarLower.Value = 0;
            vTrackBarUpper.Value = 255;

            // 필터 적용 버튼 클릭 이벤트
            btnApply.Click += btnApplyFilter_Click;
            btnApplyHSV.Click += btnApplyHSV_Click;

            // 필터 선택 콤보박스 이벤트
            select_effect.SelectedIndexChanged += select_effect_SelectedIndexChanged;
            select_effect2.SelectedIndexChanged += select_effect_SelectedIndexChanged;


            //#COLOR BINARY FILTER#8 컬러이진화 필터값을 GUI에 로딩
            InspWindow inspWindow = Global.Inst.InspStage.InspWindow;

            if (inspWindow != null)
            {
                //#INSP WORKER#13 inspWindow에서 컬러이진화 알고리즘 찾는 코드
                ColorBlobAlgorithm colorBlobAlgo = (ColorBlobAlgorithm)inspWindow.FindInspAlgorithm(InspectType.InspColorBinary);
                if (colorBlobAlgo != null)
                {
                    int filterArea = colorBlobAlgo.AreaFilter;


                    hTrackBarLower.Value = (int)colorBlobAlgo.HsvMin.Item0;
                    hTrackBarUpper.Value = (int)colorBlobAlgo.HsvMax.Item0;
                    sTrackBarLower.Value = (int)colorBlobAlgo.HsvMin.Item1;
                    sTrackBarUpper.Value = (int)colorBlobAlgo.HsvMax.Item1;
                    vTrackBarLower.Value = (int)colorBlobAlgo.HsvMin.Item2;
                    vTrackBarUpper.Value = (int)colorBlobAlgo.HsvMax.Item2;

                    // 범위 텍스트 박스에 최소, 최대값 표시
                    txtH.Text = $"{hTrackBarLower.Value} ~ {hTrackBarUpper.Value}";
                    txtS.Text = $"{sTrackBarLower.Value} ~ {sTrackBarUpper.Value}";
                    txtV.Text = $"{vTrackBarLower.Value} ~ {vTrackBarUpper.Value}";
                }
            }
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


            _hsvMin.Item0 = (byte)hTrackBarLower.Value;
            _hsvMax.Item0 = (byte)hTrackBarUpper.Value;
            _hsvMin.Item1 = (byte)sTrackBarLower.Value;
            _hsvMax.Item1 = (byte)sTrackBarUpper.Value;
            _hsvMin.Item2 = (byte)vTrackBarLower.Value;
            _hsvMax.Item2 = (byte)vTrackBarUpper.Value;

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


        //#COLOR BINARY FILTER#9 컬러이진화 관련 이벤트 발생시, 전달할 값 추가
        public class RangeChangedEventArgs : EventArgs
        {

            //HSV 각각 최소, 최대값
            public Vec3b HsvMin { get; } //HSV 각각 최소값
            public Vec3b HsvMax { get; } //HSV 각각 최대값
            public bool Invert { get; } //반전 여부
            public ShowBinaryMode ShowBinaryMode { get; } //이진화된 이미지 표시 모드


            public RangeChangedEventArgs(Vec3b hsvMin, Vec3b hsvMax, bool invert, ShowBinaryMode showBinaryMode)
            {
                HsvMin = hsvMin;
                HsvMax = hsvMax;
                Invert = invert;
                ShowBinaryMode = ShowBinaryMode;
            }


        }

        //필터 선택시, 적용할 필터 효과를 선택하고, 필터 옵션을 선택할 수 있도록 개선
        private void select_effect_SelectedIndexChanged(object sender, EventArgs e)
        {
            //만약 이 콤보박스를 눌러서 적용할 효과를 선택하면 각 효과에 따라 밑에 뜨는 콤보박스목록이 달라야함.
            _selected_effect = Convert.ToString(select_effect.SelectedItem);
            select_effect2.Items.Clear(); // 기존 아이템 제거 후 새로 추가

            if (_selected_effect == "연산")
            {
                select_effect2.Items.Add("빼기");
                select_effect2.Items.Add("절대값 차이 계산");
                select_effect2.Show();

            }
            else if (_selected_effect == "비트연산(Bitwise)")
            {
                select_effect2.Items.Add("NOT 연산");
                select_effect2.Show();

            }
            else
            {
                //select_effect2.Hide();

            }
        }

        //필터 적용 버튼 클릭 시
        private void btnApplyFilter_Click(object sender, EventArgs e)
        {
            if (_selected_effect == null || _selected_effect2 == -1)
            {
                MessageBox.Show("효과를 선택해주세요.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            FilterSelected?.Invoke(this, new FilterSelectedEventArgs(_selected_effect, _selected_effect2));

        }

        private void select_effect2_SelectedIndexChanged(object sender, EventArgs e)
        {
            _selected_effect2 = Convert.ToInt32(select_effect2.SelectedIndex);// 선택된 인덱스를 저장
        }



        private void btnApplyHSV_Click(object sender, EventArgs e)
        {
            InspWindow inspWindow = Global.Inst.InspStage.InspWindow;
            if (inspWindow is null)
                return;

            //inspWindow에서 컬러이진화 알고리즘 찾는 코드 추가
            ColorBlobAlgorithm colorBlobAlgo = (ColorBlobAlgorithm)inspWindow.FindInspAlgorithm(InspectType.InspColorBinary);
            if (colorBlobAlgo is null)
                return;

            HsvRange threshold = new HsvRange();
            threshold.HueLower = _hsvMin.Item0;
            threshold.HueUpper = _hsvMax.Item0;
            threshold.SaturationLower = _hsvMin.Item1;
            threshold.SaturationUpper = _hsvMax.Item1;
            threshold.ValueLower = _hsvMin.Item2;
            threshold.ValueUpper = _hsvMax.Item2;
            threshold.Invert = chkInvert.Checked;


            colorBlobAlgo.ColorRange = threshold;

            int hueArea = int.Parse(txtH.Text);
            int satArea = int.Parse(txtS.Text);
            int valArea = int.Parse(txtV.Text);
            colorBlobAlgo.AreaFilter = hueArea;
            colorBlobAlgo.AreaFilter = satArea;
            colorBlobAlgo.AreaFilter = valArea;

            //이진화 검사시, 해당 InspWindow와 이진화 알고리즘만 실행
            Global.Inst.InspStage.InspWorker.TryInspect(inspWindow, InspectType.InspColorBinary);
        }



        private void panelColor_Click(object sender, EventArgs e)
        {
            // 컬러 다이얼로그를 열어 색을 선택하도록 함
            ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                selectedColor = colorDialog.Color;
                panelColor.BackColor = selectedColor;  // 선택된 색상을 패널 배경으로 설정
            }
        }



        private void btnTeachinColor_Click(object sender, EventArgs e)
        {
            isSelecting = true;
            this.Cursor = Cursors.Cross; // 펜 모양으로 변경

            // imageViewer가 null이면 새로 생성
            if (imageViewer == null)
            {
                imageViewer = new ImageViewCCtrl();
                this.Controls.Add(imageViewer);
                imageViewer.Dock = DockStyle.Fill;
            }

            // 색상 선택 기능 활성화
            imageViewer._isPickColor = !imageViewer._isPickColor;

            imageViewer.Invalidate(); // 화면 갱신



            // 선택된 색상에 맞는 HSV 범위를 추출하여 이진화
            if (selectedColor != null)
            {
                // 선택된 색상을 HSV로 변환
                ColorToHSV(selectedColor, out int h, out int s, out int v);

                // 추출된 HSV 값을 트랙바 값에 맞게 설정
                hTrackBarLower.Value = h; // Hue 범위 설정
                hTrackBarUpper.Value = h; // Hue 범위 설정
                sTrackBarLower.Value = s; // Saturation 범위 설정
                sTrackBarUpper.Value = s;// Saturation 범위 설정
                vTrackBarLower.Value = v; // Value 범위 설정
                vTrackBarUpper.Value = v;// Value 범위 설정

                // 범위 텍스트 박스에 최소, 최대값 표시
                txtH.Text = $"{hTrackBarLower.Value} ~ {hTrackBarUpper.Value}";
                txtS.Text = $"{sTrackBarLower.Value} ~ {sTrackBarUpper.Value}";
                txtV.Text = $"{vTrackBarLower.Value} ~ {vTrackBarUpper.Value}";

                // 범위가 설정되면 자동으로 이진화 업데이트
                UpdateColorBinary();

            }
        }

        // 색상을 HSV로 변환하는 함수
        private void ColorToHSV(System.Drawing.Color color, out int hue, out int saturation, out int value)
        {
            ColorMine.ColorSpaces.Rgb rgb = new ColorMine.ColorSpaces.Rgb
            {
                R = color.R / 255.0,
                G = color.G / 255.0,
                B = color.B / 255.0
            };

            ColorMine.ColorSpaces.Hsv hsv = rgb.To<Hsv>(); // RGB -> HSV로 변환




            hue = (int)(hsv.H * 360);  // Hue 범위는 0~360
            saturation = (int)(hsv.S * 255);  // Saturation 범위는 0~255
            value = (int)(hsv.V * 255);  // Value 범위는 0~255
        }

        private void select_effect2_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            _selected_effect2 = Convert.ToInt32(select_effect2.SelectedIndex);// 선택된 인덱스를 저장
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
    }
}



