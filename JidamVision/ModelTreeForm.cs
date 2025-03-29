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

        private Button btnCountWires;
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
            btnCountWires.Text = "전선 카운트";
            btnCountWires.Font = new System.Drawing.Font("맑은 고딕", 8);
            btnCountWires.Location = new System.Drawing.Point(370, 50);
            btnCountWires.Enabled = false;  // 초기엔 비활성화
            btnCountWires.Click += BtnCountWires_Click;
            this.Controls.Add(btnCountWires);

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

                // Base ROI를 설정한 경우에만 카운트 버튼 활성화
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

            // 버튼 비활성화
            btnCountWires.Enabled = false;

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

        private void BtnCountWires_Click(object sender, EventArgs e)
        {
            int count = CountWiresInBaseROI();
            MessageBox.Show($"감지된 전선 개수: {count}개", "전선 카운트 결과", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private int CountWiresInBaseROI()
        {
            // 카메라에서 현재 이미지 가져오기
            CameraForm cameraForm = MainForm.GetDockForm<CameraForm>();
            if (cameraForm == null)
            {
                MessageBox.Show("카메라 화면을 찾을 수 없습니다.");
                return 0;
            }

            Mat image = cameraForm.GetCurrentImage();
            if (image == null || image.Empty())
            {
                MessageBox.Show("이미지가 없습니다.");
                return 0;
            }

            // Base ROI 가져오기
            Model model = Global.Inst.InspStage.CurModel;
            InspWindow baseROI = model.InspWindowList.FirstOrDefault(w => w.InspWindowType == InspWindowType.Base);
            if (baseROI == null)
            {
                MessageBox.Show("Base ROI가 설정되지 않았습니다.");
                return 0;
            }

            Rect imageRect = new Rect(0, 0, image.Width, image.Height);
            Rect roiRect = new Rect(baseROI.WindowArea.X, baseROI.WindowArea.Y, baseROI.WindowArea.Width, baseROI.WindowArea.Height);
            Rect validROI = roiRect & imageRect;
            if (validROI.Width <= 0 || validROI.Height <= 0)
            {
                MessageBox.Show("유효하지 않은 ROI 영역입니다.");
                return 0;
            }

            // 잘라낸 ROI 이미지
            Mat roiImage = new Mat(image, validROI);

            // 흑백 변환 및 이진화
            Mat gray = new Mat();
            Cv2.CvtColor(roiImage, gray, ColorConversionCodes.BGR2GRAY);

            Mat binary = new Mat();
            Cv2.Threshold(gray, binary, 70, 255, ThresholdTypes.Binary);

            // 전선 사이 분리 보장 (Morphology)
            Mat kernel = Cv2.GetStructuringElement(MorphShapes.Rect, new OpenCvSharp.Size(3, 3));
            Cv2.MorphologyEx(binary, binary, MorphTypes.Open, kernel);

            Cv2.ImShow("binary", binary); Cv2.WaitKey();

            // Blob 분석
            OpenCvSharp.Point[][] contours;
            HierarchyIndex[] hierarchy;
            Cv2.FindContours(binary, out contours, out hierarchy, RetrievalModes.External, ContourApproximationModes.ApproxSimple);

            int count = 0;
            foreach (var contour in contours)
            {
                double area = Cv2.ContourArea(contour);
                if (area >= 30) // 너무 작은 노이즈 제외
                    count++;
            }

            return count;
        }

        private void tvModelTree_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }
    }
}
