using JidamVision.Grab;
using JidamVision.Util;
using System;
using System.Collections;
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
    //#SETUP#3 환경설정창에 추가할 카메라설정 UserContorl 추가
    //카메라 타입 설정
    //콤보박스 속성 DropDownStyle : DropDownList로 설정

    public partial class CameraSetting : UserControl
    {
        public CameraSetting()
        {
            InitializeComponent();

            //최초 로딩시, 환경설정 정보 로딩
            LoadSetting();
        }

        private void LoadSetting()
        {
            //카메라 타입을 콤보박스에 추가
            
            //방법1
            //List<CameraType> cameras = new List<CameraType>();
            //cameras.Add(CameraType.None);
            //cameras.Add(CameraType.WebCam);
            //cameras.Add(CameraType.HikRobotCam);

            //cbCameraType.DataSource = cameras;

            //방법2
            //Enum.GetValues(typeof(CameraType)) → CameraType의 모든 값을 배열로 가져옴
            //.Cast<CameraType>() → 배열을 IEnumerable<CameraType>으로 변환
            //.ToList() → List<CameraType> 으로 변환
            cbCameraType.DataSource = Enum.GetValues(typeof(CameraType)).Cast<CameraType>().ToList();

            //환경설정에서 현재 카메라 타입 얻기
            cbCameraType.SelectedIndex = (int)SettingXml.Inst.CamType;
        }

        private void SaveSetting()
        {
            //환경설정에 카메라 타입 설정
            SettingXml.Inst.CamType = (CameraType)cbCameraType.SelectedIndex;
            //환경설정 저장
            SettingXml.Save();

            SLogger.Write($"카메라 설정 저장");
        }

        //적용 버튼 선택시 저장하기
        private void btnApply_Click(object sender, EventArgs e)
        {
            SaveSetting();
        }
    }
}
