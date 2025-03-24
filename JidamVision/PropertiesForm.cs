using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using JidamVision.Property;
using JidamVision.Core;

namespace JidamVision
{
    /*
    #PANEL TO TAB# - <<<패널 방식을 모든 속성을 한번에 볼 수 있는 탭 방식으로 변경>>> 
    디자인 창에서 [PANEL]을 삭제하고 [TabControl]을 추가할것
     */


    //#PANEL TO TAB#1 enum 타입 이름 변경
    //InspPropType => InspectType
    //Ctrl + R 키를 이용해 변경해보기
    public enum InspectType
    {
        InspNone = -1,
        InspBinary,
        InspMatch,
        InspFilter,
        InspCount
    }

    public partial class PropertiesForm : DockContent
    {
        public PropertiesForm()
        {
            InitializeComponent();
        }

        //#PANEL TO TAB#3 속성탭이 있다면 그것을 반환하고, 없다면 생성
        private void LoadOptionControl(InspectType inspType)
        {
            string tabName = inspType.ToString();

            // 이미 있는 TabPage인지 확인
            foreach (TabPage tabPage in tabPropControl.TabPages)
            {
                if (tabPage.Text == tabName)
                {
                    tabPropControl.SelectedTab = tabPage;
                    return;
                }
            }

            // 새로운 UserControl 생성
            UserControl _inspProp = CreateUserControl(inspType);
            if (_inspProp == null) 
                return;

            // 새 탭 추가
            TabPage newTab = new TabPage(tabName)
            {
                Dock = DockStyle.Fill
            };
            _inspProp.Dock = DockStyle.Fill;
            newTab.Controls.Add(_inspProp);
            tabPropControl.TabPages.Add(newTab);
            tabPropControl.SelectedTab = newTab; // 새 탭 선택
        }

        //#PANEL TO TAB#2 속성탭 타입에 맞게 UseControl 생성하여 반환
        private UserControl CreateUserControl(InspectType inspPropType)
        {
            UserControl _inspProp = null;
            switch (inspPropType)
            {
                case InspectType.InspBinary:
                    BinaryInspProp blobProp = new BinaryInspProp();
                    blobProp.LoadInspParam();
                    blobProp.RangeChanged += RangeSlider_RangeChanged;
                    _inspProp = blobProp;
                    break;
                case InspectType.InspMatch:
                    MatchInspProp matchProp = new MatchInspProp();
                    matchProp.LoadInspParam();
                    _inspProp = matchProp;
                    break;
                case InspectType.InspFilter:
                    FilterInspProp filterProp = new FilterInspProp();
                    //filterProp.LoadInspParam();
                    filterProp.FilterSelected += FilterSelect_FilterChanged;
                    _inspProp = filterProp;
                    break;
                default:
                    MessageBox.Show("유효하지 않은 옵션입니다.");
                    return null;
            }
            return _inspProp;
        }

        public void AddInspType(InspectType inspPropType)
        {
            LoadOptionControl(inspPropType);
        }


        //#BINARY FILTER#16 이진화 속성 변경시 발생하는 이벤트 수정
        private void RangeSlider_RangeChanged(object sender, RangeChangedEventArgs e)
        {
            // 속성값을 이용하여 이진화 임계값 설정
            int lowerValue = e.LowerValue;
            int upperValue = e.UpperValue;
            bool invert = e.Invert;
            ShowBinaryMode showBinMode = e.ShowBinMode;
            Global.Inst.InspStage.PreView?.SetBinary(lowerValue, upperValue, invert, showBinMode);
        }

        private void FilterSelect_FilterChanged(object sender, FilterSelectedEventArgs e)
        {
            //선택된 필터값 PrieviewImage의 ApplyFilter로 보냄
            string filter1 = e.FilterSelected1;
            int filter2 = e.FilterSelected2;
            Global.Inst.InspStage.PreView?.ApplyFilter(filter1, filter2);

        }
    }
}
