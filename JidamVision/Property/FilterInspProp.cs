using OpenCvSharp.Extensions;
using OpenCvSharp;
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

    public partial class FilterInspProp : UserControl
    {
        public event EventHandler<FilterSelectedEventArgs> FilterSelected;
        private String _selected_effect;
        private int _selected_effect2 = -1;
        //private string op_values = "0 0 0";

        public FilterInspProp()
        {
            InitializeComponent();
        }

        private void select_effect_SelectedIndexChanged(object sender, EventArgs e)
        {
            //만약 이 콤보박스를 눌러서 적용할 효과를 선택하면 각 효과에 따라 밑에 뜨는 콤보박스목록이 달라야함.
            _selected_effect = Convert.ToString(select_effect.SelectedItem); //선택한 효과 적용
            select_effect2.Items.Clear(); // 이전 항목들을 지우고 새 항목을 추가
            if (_selected_effect == "연산")
            {
                select_effect2.Items.Add("더하기");
                select_effect2.Items.Add("빼기");
                select_effect2.Items.Add("곱하기");
                select_effect2.Items.Add("나누기");
                select_effect2.Items.Add("최대값 비교");
                select_effect2.Items.Add("최소값 비교");
                select_effect2.Items.Add("절대값 계산");
                select_effect2.Items.Add("절대값 차이 계산");
                select_effect2.Show();

            }
            else if (_selected_effect == "비트연산(Bitwise)")
            {
                select_effect2.Items.Add("AND 연산");
                select_effect2.Items.Add("OR 연산");
                select_effect2.Items.Add("XOR 연산");
                select_effect2.Items.Add("NOT 연산");
                select_effect2.Show();

            }
            else if (_selected_effect == "블러링")
            {
                select_effect2.Items.Add("블러 필터");
                select_effect2.Items.Add("박스 필터");
                select_effect2.Items.Add("미디안 블러");
                select_effect2.Items.Add("가우시안 블러");
                select_effect2.Items.Add("양방향 필터");
                select_effect2.Show();

            }
            else if (_selected_effect == "Edge")
            {
                select_effect2.Items.Add("Sobel 필터");
                select_effect2.Items.Add("Scharr 필터");
                select_effect2.Items.Add("Laplacian 필터");
                select_effect2.Items.Add("Canny 엣지");
                select_effect2.Show();

            }
            else
            {
                select_effect2.Hide();

            }



        }

        private void apply_Click(object sender, EventArgs e)
        {
            if (_selected_effect == null || _selected_effect2 == -1) // 두 번째 효과가 선택되지 않은 경우
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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

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



}
