using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JidamVision.Property
{
    /*
    #BINARY FILTER# - <<<이진화 검사 개발>>> 
    입력된 lower, upper 임계값을 이용해, 영상을 이진화한 후, Filter(area)등을 이용해, 원하는 영역을 찾는다.
     */

    //#BINARY FILTER#7 이진화 하이라이트, 이외에, 이진화 이미지를 보기 위한 옵션
    public enum ShowColorBinaryMode
    {
        ShowColorBinaryNone = 0,             //컬러이진화 하이라이트 끄기
        ShowColorBinaryHighlight,            //컬러이진화 하이라이트 보기
        ShowColorBinaryOnly                  //배경 없이 컬러이진화 이미지만 보기
    }
    public partial class ColorBinaryInspProp : UserControl
    {
        public event EventHandler ThresholdChanged;

        public int HCenter => hTrackBar.Value;
        public int SMin => sTrackBar.Value;
        public int VMin => vTrackBar.Value;

        
        //컬러이진화초기화
        public ColorBinaryInspProp()
        {
            InitializeComponent();
        }

        //컬러 이진화 관련 이벤트 발생시, 전달할 값 추가
        public class RangeChangedEventArgs : EventArgs
        {
            public int HCenter { get; }
            public int SMin { get; }
            public int VMin { get; }

            public ShowColorBinaryMode ShowColorBinMode { get; }

            public RangeChangedEventArgs(int lowerValue, int upperValue, ShowColorBinaryMode showColorBinaryMode)
            {
                
                ShowColorBinMode = showColorBinaryMode;
            }
        }

        //버튼 클릭 시 색 선택하는 코드
        private void btnTeachinColor_Click(object sender, EventArgs e)
        {
            using (ColorDialog colorDialog = new ColorDialog())
            {
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    panelColorPreview.BackColor = colorDialog.Color; // 선택한 색상 미리보기
                }
            }
        }
    }
}
