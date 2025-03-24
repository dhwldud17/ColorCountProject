using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using JidamVision.Algorithm;
using JidamVision.Core;
using JidamVision.Teach;
using OpenCvSharp;

namespace JidamVision.Property
{
    /*
    #MATCH PROP# - <<<템플릿 매칭 개발>>> 
    설정된 ROI 이미지를 이용해, 유사한 이미지를 대상 이미지에서 찾는다.
    [확장영역]은 현재 구현되지 않았음
    [매칭스코어]는 템플릿 매칭 결과가 입력된 스코어보다 큰것만을 유효한 것으로 판단
    [매칭갯수]는 찾고자 하는 패턴의 갯수를 입력
     */
    public partial class MatchInspProp : UserControl
    {
        public MatchInspProp()
        {
            InitializeComponent();
        }

        //#MATCH PROP#7 템플릿 매칭 속성값을 GUI에 설정
        public void LoadInspParam()
        {
            InspWindow inspWindow = Global.Inst.InspStage.InspWindow;
            if (inspWindow is null)
                return;

            //#INSP WORKER#14 inspWindow에서 매칭 알고리즘 찾는 코드
            MatchAlgorithm matchAlgo = (MatchAlgorithm)inspWindow.FindInspAlgorithm(InspectType.InspMatch);
            if (matchAlgo is null)
                return;

            OpenCvSharp.Size extendSize = matchAlgo.ExtSize;
            int matchScore = matchAlgo.MatchScore;
            int matchCount = matchAlgo.MatchCount;

            txtExtendX.Text = extendSize.Width.ToString();
            txtExtendY.Text = extendSize.Height.ToString();
            txtScore.Text = matchScore.ToString();
            txtMatchCount.Text = matchCount.ToString();
        }

        //#MATCH PROP#10 템플릿 매칭 실행
        private void btnSearch_Click(object sender, EventArgs e)
        {
            InspWindow inspWindow = Global.Inst.InspStage.InspWindow;
            if (inspWindow is null)
                return;

            //#INSP WORKER#11 inspWindow에서 매칭 알고리즘 찾는 코드 추가
            MatchAlgorithm matchAlgo = (MatchAlgorithm)inspWindow.FindInspAlgorithm(InspectType.InspMatch);
            if (matchAlgo is null)
                return;

            //GUI에 설정된 정보를 MatchAlgorithm에 설정
            OpenCvSharp.Size extendSize = new OpenCvSharp.Size();
            extendSize.Width = int.Parse(txtExtendX.Text);
            extendSize.Height = int.Parse(txtExtendY.Text);
            int matchScore = int.Parse(txtScore.Text);
            int matchCount = int.Parse(txtMatchCount.Text);

            matchAlgo.ExtSize = extendSize;
            matchAlgo.MatchScore = matchScore;
            matchAlgo.MatchCount = matchCount;

            //#INSP WORKER#12 매칭 검사시, 해당 InspWindow와 매칭 알고리즘만 실행
            Global.Inst.InspStage.InspWorker.TryInspect(inspWindow, InspectType.InspMatch);
        }

        //#MATCH PROP#9 저장된 ROI이미지 로딩
        private void btnTeach_Click(object sender, EventArgs e)
        {
            InspWindow inspWindow = Global.Inst.InspStage.InspWindow;
            if (inspWindow.PatternLearn())
                MessageBox.Show("티칭 성공");
            else
                MessageBox.Show("티칭 실패");
        }
    }
}
