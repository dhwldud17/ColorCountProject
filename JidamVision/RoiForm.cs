using JidamVision.Core;
using JidamVision.Teach;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace JidamVision
{
    /*
    # 커넥터 전선 색상 검사기 - ROI 티칭 기능 #
    이 클래스는 검사할 전선의 ROI(관심 영역)를 설정하는 기능을 제공합니다.

    주요 기능:
    1) 트리 뷰(tvModelTree)에 기본 노드 "Root" 생성
    2) 사용자가 "노란색", "빨간색" 중 하나를 선택하여 ROI 추가 가능
    3) 추가된 ROI는 실제 이미지 화면에도 반영됨 (CameraForm 사용)
    4) 현재 모델의 ROI 정보를 트리 뷰에 업데이트 가능
    */

    public partial class ModelTreeForm : DockContent
    {
        // 개별 트리 노드에서 팝업 메뉴를 표시하기 위한 컨텍스트 메뉴
        private ContextMenuStrip _contextMenu;

        public ModelTreeForm()
        {
            InitializeComponent(); // Form 초기화

            // 1. 트리 뷰(tvModelTree)의 기본값을 "Root"로 설정
            tvModelTree.Nodes.Add("Root");

            // 2. 컨텍스트 메뉴(마우스 우클릭 시 표시될 메뉴) 초기화
            _contextMenu = new ContextMenuStrip();

            // 3. ROI 추가 메뉴 (노란색, 검은색, 빨간색)
            ToolStripMenuItem addYellowRoiItem = new ToolStripMenuItem("노란색 ROI", null, AddNode_Click) { Tag = "Yellow" };
            ToolStripMenuItem addRedRoiItem = new ToolStripMenuItem("빨간색 ROI", null, AddNode_Click) { Tag = "Red" };

            // 4. 컨텍스트 메뉴에 항목 추가
            _contextMenu.Items.Add(addYellowRoiItem);
            _contextMenu.Items.Add(addRedRoiItem);
        }

        /// <summary>
        /// 사용자가 트리 뷰(tvModelTree)에서 마우스 오른쪽 버튼을 클릭했을 때 실행되는 이벤트 핸들러
        /// </summary>
        private void tvModelTree_MouseDown(object sender, MouseEventArgs e)
        {
            // 마우스 오른쪽 버튼을 클릭했을 때만 실행
            if (e.Button == MouseButtons.Right)
            {
                // 사용자가 클릭한 위치에 있는 노드를 가져옴
                TreeNode clickedNode = tvModelTree.GetNodeAt(e.X, e.Y);

                // 클릭한 노드가 "Root"라면 컨텍스트 메뉴를 표시
                if (clickedNode != null && clickedNode.Text == "Root")
                {
                    tvModelTree.SelectedNode = clickedNode; // 선택한 노드 설정
                    _contextMenu.Show(tvModelTree, e.Location); // 마우스 위치에 팝업 메뉴 표시
                }
            }
        }

        /// <summary>
        /// 사용자가 컨텍스트 메뉴에서 "노란색", "검은색", "빨간색" ROI 중 하나를 선택하면 실행되는 함수
        /// </summary>
        private void AddNode_Click(object sender, EventArgs e)
        {
            // 현재 선택된 트리 노드가 있고, 이벤트를 발생시킨 객체가 메뉴 아이템일 경우 실행
            if (tvModelTree.SelectedNode != null && sender is ToolStripMenuItem)
            {
                ToolStripMenuItem menuItem = (ToolStripMenuItem)sender; // 선택된 메뉴 아이템 가져오기
                string nodeType = menuItem.Tag?.ToString(); // 메뉴 아이템의 태그 값 가져오기 ("Yellow", "Black", "Red")

                // 선택된 메뉴에 따라 다른 ROI를 추가
                if (nodeType == "Yellow")
                {
                    AddNewROI("노란색");
                }
                else if (nodeType == "Red")
                {
                    AddNewROI("빨간색");
                }
            }
        }

        /// <summary>
        /// 사용자가 선택한 ROI 타입(노란색, 검은색, 빨간색)에 따라 새로운 ROI를 추가하는 함수
        /// </summary>
        private void AddNewROI(string roiType)
        {
            // CameraForm을 가져와서 ROI 추가 기능을 실행
            CameraForm cameraForm = MainForm.GetDockForm<CameraForm>();
            if (cameraForm != null)
            {
                cameraForm.AddRoi(roiType); // 선택된 ROI 타입을 CameraForm에 전달하여 추가
            }

            // 트리 뷰에 ROI 정보 추가
            if (tvModelTree.SelectedNode != null)
            {
                TreeNode newNode = new TreeNode(roiType + " ROI"); // 예: "노란색 ROI"
                tvModelTree.SelectedNode.Nodes.Add(newNode);
                tvModelTree.ExpandAll();
            }
        }

        /// <summary>
        /// 현재 모델의 모든 ROI 정보를 트리 뷰(tvModelTree)에 업데이트하는 함수
        /// </summary>
        public void UpdateDiagramEntity()
        {
            // 1. 트리 뷰 초기화 (모든 노드 삭제)
            tvModelTree.Nodes.Clear();

            // 2. 기본 노드 "Root" 추가
            TreeNode rootNode = tvModelTree.Nodes.Add("Root");

            // 3. 현재 모델 정보 가져오기
            Model model = Global.Inst.InspStage.CurModel;
            List<InspWindow> windowList = model.InspWindowList;

            // 4. 모델에 ROI가 없으면 종료
            if (windowList.Count <= 0)
                return;

            // 5. 모든 ROI 정보를 트리 뷰에 추가
            foreach (InspWindow window in model.InspWindowList)
            {
                if (window is null)
                    continue; // ROI가 없으면 건너뜀

                string uid = window.UID; // ROI의 고유 ID 가져오기
                TreeNode node = new TreeNode(uid); // 새 트리 노드 생성
                rootNode.Nodes.Add(node); // "Root" 아래에 추가
            }

            // 6. 트리 뷰 전체 확장하여 보기 쉽게 설정
            tvModelTree.ExpandAll();
        }
    }
}