using JidamVision.Core;
using JidamVision.Teach;
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
using WeifenLuo.WinFormsUI.Docking;

namespace JidamVision
{
    /*
    #MODEL TREE# - <<<ROI 티칭을 위한 모델트리 만들기>>> 
    다양한 타입의 ROI를 입력하고, 관리하기 위해, 계층 구조를 나타낼 수 있는
    TreeView 컨트롤을 이용해, ROI를 입력하는 기능 개발
    1) ModelTreeForm WindowForm 생성
    2) TreeView Control 추가
    3) name을 tvModelTree로 설정
    */

    //# MODEL TREE#1 디자인창에서 모델 생성 후 아래 코드 구현
    public partial class ModelTreeForm : DockContent
    {
        //개별 트리 노트에서 팝업 메뉴 보이기를 위한 메뉴
        private ContextMenuStrip _contextMenu;

        private Button btnCountWires;  // 전역으로 선언

        public ModelTreeForm()
        {
            InitializeComponent();

            //초기 트리 노트의 기본값은 "Root"
            tvModelTree.Nodes.Add("Root");

            // ROI 리셋 버튼 생성
            Button btnResetROI = new Button();
            btnResetROI.Text = "ROI 리셋";  // 버튼에 표시될 텍스트
            btnResetROI.Location = new System.Drawing.Point(370, 10);
            btnResetROI.Click += BtnResetROI_Click;  // 클릭 이벤트 추가

            this.Controls.Add(btnResetROI); // 폼에 버튼 추가

            // 전선 카운트 버튼 생성
            btnCountWires = new Button();
            btnCountWires.Text = "전선 카운트";  // 버튼 텍스트
            btnCountWires.Font = new System.Drawing.Font("맑은 고딕", 8);
            btnCountWires.Location = new System.Drawing.Point(370, 50);  // 위치 조정
            btnCountWires.Enabled = false;
            btnCountWires.Click += BtnCountWires_Click;  // 클릭 이벤트 추가

            // 처음엔 비활성화 상태로 시작
            btnCountWires.Enabled = false;

            this.Controls.Add(btnCountWires);  // 폼에 버튼 추가

            // 컨텍스트 메뉴 초기화
            _contextMenu = new ContextMenuStrip();
            ToolStripMenuItem addBaseRoiItem = new ToolStripMenuItem("Base", null, AddNode_Click) { Tag = "Base" };
            ToolStripMenuItem addCabelRoiItem = new ToolStripMenuItem("Cabel", null, AddNode_Click) { Tag = "Cabel" };
           

            _contextMenu.Items.Add(addBaseRoiItem);
            _contextMenu.Items.Add(addCabelRoiItem);

        }

        private void tvModelTree_MouseDown(object sender, MouseEventArgs e)
        {
            //Root 노드에서 마우스 오른쪽 버튼 클릭 시에, 팝업 메뉴 생성
            if (e.Button == MouseButtons.Right)
            {
                TreeNode clickedNode = tvModelTree.GetNodeAt(e.X, e.Y);
                if (clickedNode != null && clickedNode.Text == "Root")
                {
                    tvModelTree.SelectedNode = clickedNode;
                    _contextMenu.Show(tvModelTree, e.Location);
                }
            }
        }

        //팝업 메뉴에서, 메뉴 선택시 실행되는 함수
        private void AddNode_Click(object sender, EventArgs e)
        {
            if (tvModelTree.SelectedNode != null & sender is ToolStripMenuItem)
            {
                ToolStripMenuItem menuItem = (ToolStripMenuItem)sender;
                string nodeType = menuItem.Tag?.ToString();
                if (nodeType == "Base")
                {
                    AddNewROI(InspWindowType.Base);
                }
                else if (nodeType == "Cabel")
                {
                    AddNewROI(InspWindowType.Cabel);
                }
                
            }
        }

        //imageViewer에 ROI 추가 기능 실행
        private void AddNewROI(InspWindowType inspWindowType)
        {
            CameraForm cameraForm = MainForm.GetDockForm<CameraForm>();
            if (cameraForm != null)
            {
                cameraForm.AddRoi(inspWindowType);

                if (inspWindowType == InspWindowType.Base)
                {
                    btnCountWires.Enabled = true;
                }
            }

            
        }

        //#MODEL#14 현재 모델 전체의 ROI를 트리 모델에 업데이트
        public void UpdateDiagramEntity()
        {
            tvModelTree.Nodes.Clear();
            TreeNode rootNode = tvModelTree.Nodes.Add("Root");

            Model model = Global.Inst.InspStage.CurModel;
            List<InspWindow> windowList = model.InspWindowList;
            if (windowList.Count <= 0)
                return;

            foreach (InspWindow window in model.InspWindowList)
            {
                if (window is null)
                    continue;

                string uid = window.UID;

                TreeNode node = new TreeNode(uid);
                rootNode.Nodes.Add(node);
            }

            tvModelTree.ExpandAll();
        }

        // ROI 초기화 기능 추가
        public void ResetROI()
        {
            // 트리뷰 초기화
            tvModelTree.Nodes.Clear();
            tvModelTree.Nodes.Add("Root");

            // 현재 모델의 ROI 리스트 초기화
            Model model = Global.Inst.InspStage.CurModel;
            model.InspWindowList.Clear();

            // UI 갱신
            CameraForm cameraForm = MainForm.GetDockForm<CameraForm>();
            if (cameraForm != null)
            {
                cameraForm.UpdateDiagramEntity();
                cameraForm.UpdateImageViewer();
            }
        }

        private void BtnResetROI_Click(object sender, EventArgs e)
        {
            ResetROI();  // ROI 초기화 실행
            MessageBox.Show("ROI가 초기화되었습니다!", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private int CountWiresInBaseROI()
        {
            // 현재 활성화된 카메라 폼 가져오기
            CameraForm cameraForm = MainForm.GetDockForm<CameraForm>();
            if (cameraForm == null)
            {
                MessageBox.Show("카메라 화면을 찾을 수 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0;
            }

            // 현재 이미지 가져오기
            Mat image = cameraForm.GetCurrentImage();
            if (image == null || image.Empty())
            {
                MessageBox.Show("이미지를 가져올 수 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0;
            }

            // Base ROI 가져오기
            Model model = Global.Inst.InspStage.CurModel;
            InspWindow baseROI = model.InspWindowList.FirstOrDefault(w => w.InspWindowType == InspWindowType.Base);

            if (baseROI == null)
            {
                MessageBox.Show("Base ROI가 설정되지 않았습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return 0;
            }

            // ROI 영역 추출
            Rect roiRect = new Rect(baseROI.WindowArea.X, baseROI.WindowArea.Y, baseROI.WindowArea.Width, baseROI.WindowArea.Height);
            Mat roiImage = new Mat(image, roiRect); // ROI 영역만 추출

            // 전선 감지를 위한 전처리 수행
            Mat gray = new Mat();
            Mat binary = new Mat();
            Cv2.CvtColor(roiImage, gray, ColorConversionCodes.BGR2GRAY); // 그레이스케일 변환
            Cv2.Threshold(gray, binary, 100, 255, ThresholdTypes.BinaryInv); // 이진화

            // Blob Analysis 수행
            Mat labels = new Mat();
            Mat stats = new Mat();
            Mat centroids = new Mat();
            int wireCount = Cv2.ConnectedComponentsWithStats(binary, labels, stats, centroids) - 1; // 배경 제외

            return wireCount;
        }

        private void BtnCountWires_Click(object sender, EventArgs e)
        {
            int wireCount = CountWiresInBaseROI();
            MessageBox.Show($"감지된 전선 개수: {wireCount}", "전선 카운트 결과", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

    }
}
