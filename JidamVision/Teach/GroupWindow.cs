using JidamVision.Algorithm;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace JidamVision.Teach
{
    /*
    #GROUP ROI# - <<<계층 구조를 가지는 ROI 그룹을 관리하는 클래스 만들기>>> 
    검사를 위해서는 여러개의 ROI가 각각의 역할을 해야하고, 이 들의 관계를 묶어서, 검사함.
    이를 위해 InspWindow를 상속받은 GroupWindow를 만들어 사용함    
     */

    public class GroupWindow : InspWindow
    {
        //#MODEL#1 InspStage에 있던 InspWindowList 위치를 이곳으로 변경
        [XmlElement("InspWindow")]
        public List<InspWindow> Members { get; private set; } = new List<InspWindow>();

        public GroupWindow(string groupName)
            : base(Core.InspWindowType.Group, groupName)
        {
            Name = groupName;
        }

        public void AddMember(InspWindow window)
        {
            if (window != null && !Members.Contains(window))
                Members.Add(window);
        }

        public void RemoveMember(InspWindow window)
        {
            if (window != null)
                Members.Remove(window);
        }

        public bool Contains(InspWindow window)
        {
            return Members.Contains(window);
        }

        //그룹내 모든 ROI 전체의 바운딩 박스를 구함
        public Rectangle GetBoundingRect()
        {
            if (Members.Count == 0)
                return Rectangle.Empty;

            var rects = Members.Select(w => new Rectangle(w.WindowArea.X, w.WindowArea.Y, w.WindowArea.Width, w.WindowArea.Height));
            int minX = rects.Min(r => r.Left);
            int minY = rects.Min(r => r.Top);
            int maxX = rects.Max(r => r.Right);
            int maxY = rects.Max(r => r.Bottom);

            return new Rectangle(minX, minY, maxX - minX, maxY - minY);
        }

        //그룹내 모든 윈도우 검사
        public override bool DoInpsect(InspectType inspType)
        {
            foreach (var window in Members)
            {
                window.DoInpsect(inspType);
            }
            return true;
        }

        //그룹내의 모든 윈도우 이동
        public override bool OffsetMove(OpenCvSharp.Point offset)
        {
            foreach (var window in Members)
            {
                window.OffsetMove(offset);
            }
            return true;
        }

        public override bool SaveInspWindow(Model curModel)
        {
            if (curModel is null)
                return false;

            foreach(var window in Members)
            {
                window.SaveInspWindow(curModel);
            }

            return true;
        }

        public override bool LoadInspWindow(Model curModel)
        {
            if (curModel is null)
                return false;

            foreach (var window in Members)
            {
                window.LoadInspWindow(curModel);
            }

            return true;
        }
    }
}
