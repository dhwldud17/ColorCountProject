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

        private PictureBox pictureBoxBinary;

        private Label lblWireCount;

        private Button btnCountWires;

        private ContextMenuStrip _contextMenuRoot;
        
        private ContextMenuStrip _contextMenuRoi;

        // #우클릭 시 삭제 ContextMenu초기화#[17] 생성자에서 메뉴 구성
        public ModelTreeForm()
        {

            CameraForm cameraForm = MainForm.GetDockForm<CameraForm>();
            if (cameraForm != null)
            {
                cameraForm.RoiAdded += CameraForm_RoiAdded; // ROI 이벤트 연결
            }

            InitializeComponent();

            tvModelTree.AfterSelect += tvModelTree_AfterSelect;

            //초기 트리 노트의 기본값은 "Root"
            tvModelTree.Nodes.Add("Root");

            // ROI 리셋 버튼 생성
            Button btnResetROI = new Button();
            btnResetROI.Text = "ROI 리셋";  // 버튼에 표시될 텍스트
            btnResetROI.Location = new System.Drawing.Point(370, 0);
            btnResetROI.Click += BtnResetROI_Click;  // 클릭 이벤트 추가

            this.Controls.Add(btnResetROI); // 폼에 버튼 추가

            // 전선 카운트 버튼 생성
            btnCountWires = new Button();
            btnCountWires.Text = "전선 카운트";
            btnCountWires.Font = new System.Drawing.Font("맑은 고딕", 8);
            btnCountWires.Location = new System.Drawing.Point(370, 30);
            btnCountWires.Enabled = false;  // 초기엔 비활성화
            btnCountWires.Click += BtnCountWires_Click;
            this.Controls.Add(btnCountWires);

            // 전선 개수 표시용 라벨
            lblWireCount = new Label();
            lblWireCount.Text = "전선 개수: 0";
            lblWireCount.Location = new System.Drawing.Point(372, 60);
            lblWireCount.AutoSize = true;
            this.Controls.Add(lblWireCount);

            // ROI 영역 이미지 창 UI로 띄우기
            pictureBoxBinary = new PictureBox();
            pictureBoxBinary.Name = "pictureBoxBinary";
            pictureBoxBinary.Location = new System.Drawing.Point(500, -10);
            pictureBoxBinary.Size = new System.Drawing.Size(320, 100);
            pictureBoxBinary.SizeMode = PictureBoxSizeMode.Zoom;
            this.Controls.Add(pictureBoxBinary);

            //// 컨텍스트 메뉴 초기화
            //_contextMenu = new ContextMenuStrip();
            //ToolStripMenuItem addBaseRoiItem = new ToolStripMenuItem("Base", null, AddNode_Click) { Tag = "Base" };
            //ToolStripMenuItem addCabelRoiItem = new ToolStripMenuItem("Cabel", null, AddNode_Click) { Tag = "Cabel" };

            //_contextMenu.Items.Add(addBaseRoiItem);
            //_contextMenu.Items.Add(addCabelRoiItem);

            //_contextMenuRoi = new ContextMenuStrip();
            //ToolStripMenuItem deleteRoiItem = new ToolStripMenuItem("삭제", null, DeleteNode_Click) { Tag = "Delete" };

            // #우클릭 시 삭제 ContextMenu초기화#[17-1] Root 전용 메뉴 (Base, Cabel 추가)  // Root 노드 전용 컨텍스트 메뉴 초기화
            _contextMenuRoot = new ContextMenuStrip();
            _contextMenuRoot.Items.Add(new ToolStripMenuItem("Base", null, AddNode_Click) { Tag = "Base" });
            _contextMenuRoot.Items.Add(new ToolStripMenuItem("Cabel", null, AddNode_Click) { Tag = "Cabel" });

            // #우클릭 시 삭제 ContextMenu초기화#[17-2] ROI 노드 전용 메뉴 (삭제)  // ROI 노드 전용 컨텍스트 메뉴 초기화
            _contextMenuRoi = new ContextMenuStrip();
            _contextMenuRoi.Items.Add(new ToolStripMenuItem("삭제", null, DeleteNode_Click) { Tag = "Delete" });
        }

        // #우클릭 시 삭제 우클릭Context#[1] 트리뷰에서 마우스 누를 때 발생하는 이벤트
        private void tvModelTree_MouseDown(object sender, MouseEventArgs e)
        {
            // #우클릭 시 삭제 우클릭Context#[1-1] 마우스 우클릭인지 확인  //Root 노드에서 마우스 오른쪽 버튼 클릭 시에, 팝업 메뉴 생성
            if (e.Button == MouseButtons.Right)
            {
                // #우클릭 시 삭제 우클릭Context#[2] 클릭된 위치의 노드 가져오기
                TreeNode clickedNode = tvModelTree.GetNodeAt(e.X, e.Y);
                if (clickedNode != null)
                {
                    // #우클릭 시 삭제 우클릭Context#[3] 트리뷰에서 해당 노드를 선택 상태로 설정
                    tvModelTree.SelectedNode = clickedNode;

                    // #우클릭 시 삭제 우클릭Context#[4] "Root" 노드일 경우 → Base/Cabel 추가 메뉴 표시
                    if (clickedNode.Text == "Root")
                    {
                        _contextMenuRoot.Show(tvModelTree, e.Location);  // Base, Cabel 메뉴
                    }
                    else
                    {
                        // #우클릭 시 삭제 우클릭Context#[5] 그 외의 노드일 경우 → 삭제 메뉴 표시
                        _contextMenuRoi?.Show(tvModelTree, e.Location);
                    }
                }
            }
        }

        // #우클릭 시 삭제 ROI추가#[6] 메뉴에서 Base 또는 Cabel 클릭 시 실행  //팝업 메뉴에서, 메뉴 선택시 실행되는 함수
        private void AddNode_Click(object sender, EventArgs e)
        {
            if (tvModelTree.SelectedNode != null & sender is ToolStripMenuItem)
            {
                ToolStripMenuItem menuItem = (ToolStripMenuItem)sender;

                // #우클릭 시 삭제 ROI추가#[7] 메뉴 항목의 Tag 값으로 ROI 타입 구분
                string nodeType = menuItem.Tag?.ToString();

                // #우클릭 시 삭제 ROI추가#[8] Base 추가
                if (nodeType == "Base")
                {
                    AddNewROI(InspWindowType.Base);
                }

                // #우클릭 시 삭제 ROI추가#[9] Cabel 추가
                else if (nodeType == "Cabel")
                {
                    AddNewROI(InspWindowType.Cabel);
                }

            }
        }

        // #우클릭 시 삭제 ROI추가#[10] 실제로 ImageViewer에 ROI를 추가하는 함수  //imageViewer에 ROI 추가 기능 실행
        private void AddNewROI(InspWindowType inspWindowType)
        {
            CameraForm cameraForm = MainForm.GetDockForm<CameraForm>();
            if (cameraForm != null)
            {
                cameraForm.AddRoi(inspWindowType); // 실제 ROI 객체 생성 요청

                // #우클릭 시 삭제 ROI추가#[11] Base ROI가 추가되면 전선 카운트 버튼 활성화  // Base ROI를 설정한 경우에만 카운트 버튼 활성화
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

        // // #ROI초기화#[1~4] ROI 초기화 기능 정의  // ROI 초기화 기능 추가
        public void ResetROI()
        {
            // #ROI초기화#[1-1] TreeView 전체 노드 제거  // 트리뷰 초기화
            tvModelTree.Nodes.Clear();
            // #ROI초기화#[1-2] 기본 루트 노드 "Root"만 다시 추가
            tvModelTree.Nodes.Add("Root");

            // #ROI초기화#[2] 현재 모델의 ROI 리스트 초기화 (즉, 내부 ROI 데이터 삭제)
            Model model = Global.Inst.InspStage.CurModel;
            model.InspWindowList.Clear();  // 실제 데이터 제거

            // #ROI초기화#[3] 전선 카운트 버튼 비활성화 (Base ROI가 없기 때문에)
            btnCountWires.Enabled = false;

            // #ROI초기화#[4-1] CameraForm 화면 및 이미지 갱신  //UI 갱신
            CameraForm cameraForm = MainForm.GetDockForm<CameraForm>();
            if (cameraForm != null)
            {
                cameraForm.UpdateDiagramEntity();  // ROI 리스트 UI 갱신
                cameraForm.UpdateImageViewer();  // 이미지 위 ROI 제거 후 다시 그림
            }

            // #ROI초기화#[4-2] InspectionForm 화면 및 이미지 갱신 (있을 경우)
            InspectionForm inspectionForm = MainForm.GetDockForm<InspectionForm>();
            if(inspectionForm != null)
            {
                inspectionForm.UpdateDiagramEntity(); // 동일하게 갱신
                inspectionForm.UpdateImageViewer();
            }
        }

        // #ROI초기화#[5] "ROI 리셋" 버튼을 클릭했을 때 실행되는 이벤트 핸들러
        private void BtnResetROI_Click(object sender, EventArgs e)
        {
            ResetROI();  // ROI 초기화 실행
            MessageBox.Show("ROI가 초기화되었습니다!", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // #CountWire# [1] 전선 카운트 버튼 클릭 시 동작
        private void BtnCountWires_Click(object sender, EventArgs e)
        {
            // #CountWire# [2] 전선 개수 계산 함수 호출
            int count = GetBaseRoi();

            // #CountWire# [3] 계산된 전선 개수 라벨에 표시
            lblWireCount.Text = $"전선 개수: {count}";

            //// [1] 현재 모델 정보를 가져온다 (ROI 정보들이 들어 있음)
            //var model = Global.Inst.InspStage.CurModel;

            //// [2] 모델 안의 ROI 리스트에서 Base 타입의 ROI를 하나 찾는다
            //var baseRoi = model.InspWindowList.FirstOrDefault(w => w.InspWindowType == InspWindowType.Base);

            //// [3] Base ROI가 있다면 아래 처리 진행
            //if (baseRoi != null)
            //{
            //    // [4] GetBaseRoi() 함수를 호출하여 전선 개수를 계산
            //    int count = GetBaseRoi(baseRoi);  //  여기서 핵심 처리

            //    // [5] 계산된 개수를 UI 라벨에 표시
            //    lblWireCount.Text = $"전선 개수: {count}";
            //}
        }

        // #CountWire# [4] 실제 전선 개수 세는 로직
        private int GetBaseRoi()  //CountWiresInBaseROI()
        {
            // #CountWire# [5] 카메라에서 현재 이미지 가져오기
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

            // #CountWire# [6] Base ROI 정보 가져오기
            Model model = Global.Inst.InspStage.CurModel;
            InspWindow baseROI = model.InspWindowList.FirstOrDefault(w => w.InspWindowType == InspWindowType.Base);
            if (baseROI == null)
            {
                MessageBox.Show("Base ROI가 설정되지 않았습니다.");
                return 0;
            }

            // #CountWire# [7] ROI 범위가 이미지 내부에 있는지 확인
            Rect imageRect = new Rect(0, 0, image.Width, image.Height);
            Rect roiRect = new Rect(baseROI.WindowArea.X, baseROI.WindowArea.Y, baseROI.WindowArea.Width, baseROI.WindowArea.Height);
            Rect validROI = roiRect & imageRect;
            if (validROI.Width <= 0 || validROI.Height <= 0)
            {
                MessageBox.Show("유효하지 않은 ROI 영역입니다.");
                return 0;
            }

            // #CountWire# [8] ROI 영역 자르기
            Mat roiImage = new Mat(image, validROI);

            // #CountWire# [9] 흑백 이미지로 변환
            Mat gray = new Mat();
            Cv2.CvtColor(roiImage, gray, ColorConversionCodes.BGR2GRAY);

            // #CountWire# [10] 이진화 (Thresholding)
            Mat binary = new Mat();
            Cv2.Threshold(gray, binary, 60, 255, ThresholdTypes.Binary);

            // #CountWire# [11] 노이즈 제거를 위한 Morphology 연산 적용, //전선 사이 분리 보장 (Morphology)
            Mat kernel = Cv2.GetStructuringElement(MorphShapes.Rect, new OpenCvSharp.Size(3, 3));
            Cv2.MorphologyEx(binary, binary, MorphTypes.Open, kernel);

            // #CountWire# [12] 이진화 결과를 PictureBox에 출력
            Bitmap bitmap = BitmapConverter.ToBitmap(binary);
            pictureBoxBinary.Image = bitmap;

            //Cv2.ImShow("binary", binary); Cv2.WaitKey();

            // #CountWire# [13] 외곽선 검출 (윤곽선 기반 Blob 분석)
            OpenCvSharp.Point[][] contours;
            HierarchyIndex[] hierarchy;
            Cv2.FindContours(binary, out contours, out hierarchy, RetrievalModes.External, ContourApproximationModes.ApproxSimple);

            // #CountWire# [14] 일정 면적 이상인 윤곽선만 카운트하여 전선 개수 계산
            int count = 0;
            foreach (var contour in contours)
            {
                double area = Cv2.ContourArea(contour);
                if (area >= 30) // 너무 작은 노이즈 제거 필터링
                    count++;
            }
            
            return count; // 최종 전선 개수 반환

        }

        // #ROI강조#[1] TreeView에서 노드 선택 시 호출
        private void tvModelTree_AfterSelect(object sender, TreeViewEventArgs e)
        {

            // #ROI강조#[2] 선택한 노드의 UID 가져오기
            string selectedUID = e.Node.Text;

            // #ROI강조#[2] 현재 모델에서 UID 일치하는 ROI(InspWindow) 찾기
            Model model = Global.Inst.InspStage.CurModel;
            InspWindow selectedWindow = model.InspWindowList.FirstOrDefault(w => w.UID == selectedUID);

            if (selectedWindow != null)
            {
                // #ROI강조#[3] 카메라 폼의 SelectDiagramEntity() 호출
                CameraForm cameraForm = MainForm.GetDockForm<CameraForm>();
                if (cameraForm != null)
                {
                    // 선택된 ROI를 화면에서 강조
                    cameraForm.SelectDiagramEntity(selectedWindow);
                }
            }

            //MessageBox.Show("선택된 UID: " + selectedUID);
        }

        // #CountWire# [15] ROI가 새로 추가될 때 자동 처리되는 이벤트 핸들러
        private void CameraForm_RoiAdded(object sender, InspWindow e)
        {
            if (!this.IsHandleCreated) return;

            this.Invoke(new Action(() =>
            {
                // #CountWire# [16] 만약 추가된 ROI가 Base라면 전선 자동 카운트 + 이미지 표시
                if (e.InspWindowType == InspWindowType.Base)
                {
                    int count = GetBaseRoi();
                    lblWireCount.Text = $"전선 개수: {count}";
                }

                // #CountWire# [17] 트리뷰에 새 ROI 노드 추가
                TreeNode rootNode = tvModelTree.Nodes[0]; // Root 노드
                TreeNode node = new TreeNode(e.UID);      // UID를 노드 이름으로
                rootNode.Nodes.Add(node);

                tvModelTree.ExpandAll(); // 자동 펼치기
            }));
        }

        // #우클릭 시 삭제 RO삭제#[12] "삭제" 메뉴 클릭 시 실행
        private void DeleteNode_Click(object sender, EventArgs e)
        {
            TreeNode selectedNode = tvModelTree.SelectedNode;

            // #우클릭 시 삭제 RO삭제#[13] "Root"는 삭제 금지, 나머지만 삭제 가능
            if (selectedNode != null && selectedNode.Text != "Root")
            {
                string uid = selectedNode.Text;

                // #우클릭 시 삭제 RO삭제#[14] 모델에서 해당 UID의 ROI 찾기  // 모델에서도 삭제
                Model model = Global.Inst.InspStage.CurModel;
                var target = model.InspWindowList.FirstOrDefault(w => w.UID == uid);
                if (target != null)
                {
                    model.InspWindowList.Remove(target);
                }

                // #우클릭 시 삭제 RO삭제#[15] 트리뷰 노드 삭제  // 트리뷰에서 삭제
                selectedNode.Remove();

                // #우클릭 시 삭제 RO삭제#[16] UI 갱신 (화면 다시 그림)  // 화면 갱신
                CameraForm cam = MainForm.GetDockForm<CameraForm>();
                if (cam != null)
                {
                    cam.UpdateDiagramEntity();
                    cam.UpdateImageViewer();
                }
            }
        }

        //private int GetBaseRoi(InspWindow roi)
        //{
        //    // [1] 현재 카메라 화면에서 이미지(Mat 객체)를 가져온다
        //    var image = MainForm.GetDockForm<CameraForm>()?.GetCurrentImage();

        //    // [2] ROI나 이미지가 null이거나 비어 있다면 0 반환
        //    if (roi == null || image == null || image.Empty()) return 0;

        //    // [3] ROI 영역만큼 이미지를 잘라낸다 (Mat 객체 생성)
        //    var roiImage = new Mat(image, roi.WindowArea);

        //    // [4] ROI 이미지를 흑백(Grayscale)으로 변환
        //    Cv2.CvtColor(roiImage, roiImage, ColorConversionCodes.BGR2GRAY);

        //    // [5] 흑백 이미지를 이진화 (흰색/검은색만 남기는 처리)
        //    Cv2.Threshold(roiImage, roiImage, 60, 255, ThresholdTypes.Binary);

        //    // [6] 전선끼리 붙은 부분을 분리하기 위해 Morphology 연산 (노이즈 제거용)
        //    Cv2.MorphologyEx(
        //        roiImage, roiImage,
        //        MorphTypes.Open,
        //        Cv2.GetStructuringElement(MorphShapes.Rect, new OpenCvSharp.Size(3, 3))
        //    );

        //    // [7] 외곽선(Contour)들을 찾아낸다 (전선 등 밝은 영역 경계 추출)
        //    Cv2.FindContours(
        //        roiImage, out var contours, out _,
        //        RetrievalModes.External,
        //        ContourApproximationModes.ApproxSimple
        //    );

        //    // [8] 일정 면적 이상(30픽셀 이상)의 윤곽선만 전선으로 간주하여 카운트
        //    return contours.Count(c => Cv2.ContourArea(c) >= 30);
        //}



    }
}
