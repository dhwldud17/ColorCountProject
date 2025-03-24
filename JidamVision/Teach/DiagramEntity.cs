using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JidamVision.Teach
{
    //#MULTI ROI#1 ImageViewer에 표시되는 ROI정보를 관리하는 클래스
    public class DiagramEntity
    {
        //ROI 연결된 InspWindow
        public InspWindow LinkedWindow { get; set; }
        //ROI 영역정보
        public Rectangle EntityROI { get; set; }
        //ROI 표시 칼라
        public Color EntityColor { get; set; }
        //ROI 위치 이동을 하지 못하게 할지 여부
        public bool IsHold { get; set; }

        public DiagramEntity()
        {
            LinkedWindow = null;
            EntityROI = new Rectangle(0, 0, 0, 0);
            EntityColor = Color.White;
            IsHold = false;
        }
        public DiagramEntity(Rectangle rect, Color entityColor, bool hold = false)
        {
            LinkedWindow = null;
            EntityROI = rect;
            EntityColor = entityColor;
            IsHold = hold;
        }
    }

    public static class DiagramEntityExtensions
    {
        /// <summary>
        /// 해당 DiagramEntity가 어떤 GroupWindow에 속한 멤버인지 찾아 반환
        /// </summary>
        public static GroupWindow GetParentGroup(this DiagramEntity entity)
        {
            if (entity?.LinkedWindow?.Parent is GroupWindow group)
                return group;

            return null;
        }

        /// <summary>
        /// 동일 그룹의 모든 자식 DiagramEntity 목록을 가져오기
        /// </summary>
        public static List<DiagramEntity> GetGroupMembers(this GroupWindow group, List<DiagramEntity> allEntities)
        {
            return allEntities
                .Where(e => group.Members.Contains(e.LinkedWindow))
                .ToList();
        }
    }
}
