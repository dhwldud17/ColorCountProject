using JidamVision.Grab;
using JidamVision.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JidamVision.Setting
{
    //#SETUP#4 환경설정창에 추가할 경로설정 UserContorl 추가
    //모델에서 사용하는 모든 경로 설정

    public partial class PathSetting : UserControl
    {
        public PathSetting()
        {
            InitializeComponent();

            //최초 로딩시, 환경설정 정보 로딩
            LoadSetting();
        }

        private void LoadSetting()
        {
            //환경설정에서 모델 저장 경로 얻기
            txtModelDir.Text = SettingXml.Inst.ModelDir;
        }

        private void SaveSetting()
        {
            //환경설정에 모델 저장 경로 설정
            SettingXml.Inst.ModelDir = txtModelDir.Text;
            //환경설정 저장
            SettingXml.Save();

            SLogger.Write($"Path 설정 저장");
        }

        //폴더 선택 기능
        private void btnSelModelDir_Click(object sender, EventArgs e)
        {
            using(FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "폴더를 선택하세요.";
                folderDialog.ShowNewFolderButton = true;    //새 폴더 생성 버튼 활성화

                if(folderDialog.ShowDialog() == DialogResult.OK)
                {
                    txtModelDir.Text = folderDialog.SelectedPath;
                }
            }
        }

        //적용 버튼 선택시 저장하기
        private void btnApply_Click(object sender, EventArgs e)
        {
            SaveSetting();
        }
    }
}
