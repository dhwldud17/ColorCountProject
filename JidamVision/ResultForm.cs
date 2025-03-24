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
using BrightIdeasSoftware;
using JidamVision.Inspect;
using JidamVision.Core;
using JidamVision.Teach;

namespace JidamVision
{
    /*
    #RESULT FORM# - <<<검사결과를 보기 위해, 트리 리스트 뷰에 출력하는 기능 개발>>> 
    1) 검사 결과를 출력하기 위한 TreeListView 컨트롤을 사용하기 위해, 
        ExternalLib\Dll\ObjectListView\ObjectListView.dll을 참조에 추가할것
        using BrightIdeasSoftware; 코드 추가

    2) Inspect/InspResult 클래스를 생성하여, 검사 결과로 사용할 프로퍼티 만들기
    3) ResultForm에 TreeListView와 자세한 결과를 보여주는 TextBox, 
        2개 컨트롤을 상하로 분리하는 SplitContainer 추가
    */
    public partial class ResultForm : DockContent
    {
        //#RESULT FORM#2 검사 결과를 보여주기 위한 컨트롤 추가
        private SplitContainer _splitContainer;
        private TreeListView _treeListView;
        private TextBox _txtDetails;

        public ResultForm()
        {
            InitializeComponent();

            //#RESULT FORM#3 컨트롤 초기화, 아래 함수 구현할것
            InitTreeListView();

            //테스트를 위한 임의 데이터 로드, 추후 삭제 할것
            LoadTestData();
        }

        private void InitTreeListView()
        {
            // SplitContainer 사용하여 상하 분할 레이아웃 구성
            _splitContainer = new SplitContainer()
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Horizontal,
                SplitterDistance = 250,
                Panel1MinSize = 100,
                Panel2MinSize = 100
            };

            //TreeListView 검사 결과 트리 생성
            _treeListView = new TreeListView()
            {
                Dock = DockStyle.Fill,
                FullRowSelect = true,
                ShowGroups = false,
                UseFiltering = true,
                OwnerDraw = true,
                MultiSelect = false,
                GridLines = true
            };
            _treeListView.SelectionChanged += TreeListView_SelectionChanged;

            //컬럼 추가
            var colBaseID = new OLVColumn("Base ID", "BaseID")
            {
                Width = 100,
                IsEditable = false
            };

            var colObjectID = new OLVColumn("Object ID", "ObjectID")
            {
                Width = 150,
                IsEditable = false
            };

            var colStatus = new OLVColumn("Status", "IsDefect")
            {
                Width = 80,
                TextAlign = HorizontalAlignment.Center,
                AspectGetter = obj => ((InspResult)obj).IsDefect ? "NG" : "OK"
            };

            var colScore = new OLVColumn("Score", "ResultScore")
            {
                Width = 80,
                TextAlign = HorizontalAlignment.Center,
                AspectToStringFormat = "{0:F2}"
            };

            // 컬럼 추가
            _treeListView.Columns.AddRange(new OLVColumn[] {colBaseID, colObjectID, colStatus, colScore});

            // 검사 상세 정보 텍스트박스 생성
            _txtDetails = new TextBox()
            {
                Dock = DockStyle.Fill,
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                Font = new Font("Arial", 10),
                ReadOnly = true
            };

            // 컨테이너에 컨트롤 추가
            _splitContainer.Panel1.Controls.Add(_treeListView);
            _splitContainer.Panel2.Controls.Add(_txtDetails);
            Controls.Add(_splitContainer);
        }

        //실제 검사가 되었을때, 검사 결과를 추가하는 함수
        public void AddResult(InspResult result)
        {
            if (result == null)
                return;

            // 현재 트리에 있는 객체 리스트 가져오기
            var existingResults = _treeListView.Objects as List<InspResult>;

            if (existingResults == null)
                existingResults = new List<InspResult>();

            // 기존 검사 결과에서 같은 BodyID를 가진 부모 찾기
            var parentResult = existingResults.FirstOrDefault(r => r.BaseID == result.BaseID);

            existingResults.Add(result);
            
            // TreeListView 업데이트
            _treeListView.SetObjects(existingResults);
        }

        private void LoadTestData()
        {
            // 부모 ROI
            var parentRoi = new InspResult(new InspWindow(), "Body-1", "ROI-001", InspWindowType.Global)
            {
                IsDefect = false,
                ResultScore = 95.5f,
                ResultInfo = "정상 검사 완료"
            };

            // 하위 ROI - NG 발생
            var child1 = new InspResult(new InspWindow(), "Body-1", "ROI-001-1", InspWindowType.Sub)
            {
                IsDefect = true,
                ResultScore = 70.2f,
                ResultInfo = "이물 감지됨 (크기: 1.5mm)"
            };

            var child2 = new InspResult(new InspWindow(), "Body-1", "ROI-001-2", InspWindowType.Sub)
            {
                IsDefect = false,
                ResultScore = 89.3f,
                ResultInfo = "치수 정상"
            };

            // ObjectListView에 데이터 설정
            _treeListView.SetObjects(new List<InspResult> { parentRoi });
        }

        //해당 트리 리스트 뷰 선택시, 상세 정보 텍스트 박스에 표시
        private void TreeListView_SelectionChanged(object sender, EventArgs e)
        {
            if (_treeListView.SelectedObject == null)
            {
                _txtDetails.Text = string.Empty;
                return;
            }
            var result = (InspResult)_treeListView.SelectedObject;
            _txtDetails.Text = $"Base ID: {result.BaseID}\r\n" +
                              $"Object ID: {result.ObjectID}\r\n" +
                              $"Result Score: {result.ResultScore:F2}\r\n" +
                              $"Result Value: {result.ResultValue:F2}\r\n" +
                              $"Result Info: {result.ResultInfo}";
        }
    }
}
