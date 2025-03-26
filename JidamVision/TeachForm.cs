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
    public partial class TeachForm : DockContent
    {
        public TeachForm()
        {
            InitializeComponent();
            imageViewer.DiagramEntityEvent += ImageViewer_DiagramEntityEvent;
        }

        private void ImageViewer_Load(object sender, EventArgs e)
        {

        }


        private void ImageViewer_DiagramEntityEvent(object sender, DiagramEntityEventArgs e)
        {
            switch (e.ActionType)
            {
                case EntityActionType.Select:
                    Global.Inst.InspStage.SelectInspWindow(e.InspWindow);
                    break;
                case EntityActionType.Inspect:
                    Global.Inst.InspStage.TryInspection(e.InspWindow);
                    break;
                case EntityActionType.Add:
                    Global.Inst.InspStage.AddInspWindow(e.WindowType, e.Rect);
                    break;
                case EntityActionType.Move:
                    Global.Inst.InspStage.MoveInspWindow(e.InspWindow, e.OffsetMove);
                    break;
                case EntityActionType.Resize:
                    Global.Inst.InspStage.ModifyInspWindow(e.InspWindow, e.Rect);
                    break;
                case EntityActionType.Delete:
                    Global.Inst.InspStage.DelInspWindow(e.InspWindow);
                    break;
                case EntityActionType.DeleteList:
                    Global.Inst.InspStage.DelInspWindow(e.InspWindowList);
                    break;
                case EntityActionType.AddGroup:
                    Global.Inst.InspStage.CreateGroupWindow(e.InspWindowList);
                    break;
                case EntityActionType.Break:
                    Global.Inst.InspStage.BreakGroupWindow(e.InspWindow);
                    break;
            }
        }



        
        public void AddRect(List<Rect> rects)
        {
            //#BINARY FILTER#18 imageViewer는 Rectangle 타입으로 그래픽을 그리므로, 
            //아래 코드를 이용해, Rect -> Rectangle로 변환하는 람다식
            var rectangles = rects.Select(r => new Rectangle(r.X, r.Y, r.Width, r.Height)).ToList();
            imageViewer.AddRect(rectangles);
        }

        public void AddRoi(InspWindowType inspWindowType)
        {
            imageViewer.NewRoi(inspWindowType);
        }
        public void UpdateDiagramEntity()
        {
            Model model = Global.Inst.InspStage.CurModel;
            List<DiagramEntity> diagramEntityList = new List<DiagramEntity>();

            foreach (InspWindow window in model.InspWindowList)
            {
                if (window is null)
                    continue;

                if (window is GroupWindow group)
                {
                    foreach (InspWindow member in group.Members)
                    {
                        DiagramEntity entity = new DiagramEntity()
                        {
                            LinkedWindow = member,
                            EntityROI = new Rectangle(
                                member.WindowArea.X, member.WindowArea.Y,
                                member.WindowArea.Width, member.WindowArea.Height),
                            EntityColor = imageViewer.GetWindowColor(member.InspWindowType)
                        };
                        diagramEntityList.Add(entity);
                    }
                }
                else if (window.Parent == null)
                {
                    DiagramEntity entity = new DiagramEntity()
                    {
                        LinkedWindow = window,
                        EntityROI = new Rectangle(
                            window.WindowArea.X, window.WindowArea.Y,
                                window.WindowArea.Width, window.WindowArea.Height),
                        EntityColor = imageViewer.GetWindowColor(window.InspWindowType)
                    };
                    diagramEntityList.Add(entity);
                }
            }

            imageViewer.SetDiagramEntityList(diagramEntityList);
        }

        public void SelectDiagramEntity(InspWindow window)
        {
            imageViewer.SelectDiagramEntity(window);
        }

        public void UpdateImageViewer()
        {
            imageViewer.Invalidate();
        }
       

        private void TeachForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            imageViewer.DiagramEntityEvent -= ImageViewer_DiagramEntityEvent;
            this.FormClosed -= TeachForm_FormClosed;
        }
    }
}
