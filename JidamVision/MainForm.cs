﻿using JidamVision.Core;
using JidamVision.Setting;
using JidamVision.Util;
using JidamVision.Teach;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace JidamVision
{
    public partial class MainForm : Form
    {
        private static DockPanel _dockPanel;
        private CameraForm _teachWindow;
        private InspectionForm _inspectionWindow; // 기존 검사 실행 창

        private bool _isInspectionUIShown = false;
        private bool _isTeachUIShown = false;
        public MainForm()
        {
            InitializeComponent();

            _dockPanel = new DockPanel
            {
                Dock = DockStyle.Fill
            };
            Controls.Add(_dockPanel);

            // Visual Studio 2015 테마 적용
            _dockPanel.Theme = new VS2015BlueTheme();

               LoadDockingWindows();
       
           
            ShowInspectionUI();

            // 활성화된 도킹 창이 변경될 때 이벤트 핸들러 등록
            _dockPanel.ActiveContentChanged += DockPanel_ActiveContentChanged;

            Global.Inst.Initialize();


        }
        private void DockPanel_ActiveContentChanged(object sender, EventArgs e)
        {
         
            var activeForm = _dockPanel.ActiveContent as DockContent;
            if (activeForm == null)
            {
                // 활성 창이 없으면 기본적으로 _inspectionWindow를 활성화
              //  _dockPanel.ActiveContent = _inspectionWindow;
                activeForm = _inspectionWindow;
                _isInspectionUIShown = true;
            }
            if (activeForm == _inspectionWindow && !_isInspectionUIShown)
            {
                _isInspectionUIShown = true;
                _isTeachUIShown = false;
                Console.WriteLine("검사 실행 창 선택됨");
                ShowInspectionUI();
            }
            else if (activeForm == _teachWindow && !_isTeachUIShown)
            {
                _isInspectionUIShown = false;
                _isTeachUIShown = true;
                Console.WriteLine("Teach 창 선택됨");
                ShowTeachUI();
            }
        }

        // 검사 실행 창 활성화 시 특정 UI 설정
        private void ShowInspectionUI()
        {

            //결과폼
           var resultWindow = GetDockForm<ResultForm>();
            if(resultWindow == null)
            {
                resultWindow = new ResultForm();
                resultWindow.Show(_inspectionWindow.Pane, DockAlignment.Bottom, 0.3);
            }
          

            // 모델 트리 창을 닫기 전에 닫혀있는지 확인하고, 닫기
            var modelTreeWindow = GetDockForm<ModelTreeForm>();
            if (modelTreeWindow != null)
            {
                modelTreeWindow.Close(); // ModelTreeForm을 닫음
            }

            var propWindow = GetDockForm<PropertiesForm>();
            if (propWindow != null)
            {
                propWindow.Close(); //propWindow을 닫음
            }

          
        }

        // Teach 창 활성화 시 특정 UI 설정
        private void ShowTeachUI()
        {
            // 모델 트리 창이 이미 열려 있는지 확인
            var modelTreeWindow = GetDockForm<ModelTreeForm>();
            if (modelTreeWindow == null)
            {
                modelTreeWindow = new ModelTreeForm();
                modelTreeWindow.Show(_teachWindow.Pane, DockAlignment.Bottom, 0.2);
                Console.WriteLine("ModelTreeForm이 이제 표시되었습니다.");
            }
            
            var propWindow = GetDockForm<PropertiesForm>();
            if (propWindow == null)
            {
                propWindow = new PropertiesForm();
                propWindow.Show(_dockPanel, DockState.DockRight);
                Console.WriteLine("propWindows이 이제 표시되었습니다.");
            }




            // 
            var resultWindow = GetDockForm<ResultForm>();
            if (resultWindow != null)
            {
                resultWindow.Close(); //
                Console.WriteLine("ResultForm이 닫혔습니다.");
            }

        }



        private void LoadDockingWindows()
        {
            //도킹해제 금지 설정
            _dockPanel.AllowEndUserDocking = false;
           

            _inspectionWindow = new InspectionForm();
            _inspectionWindow.Text = "검사창";
            _inspectionWindow.CloseButton = false; // 닫기 버튼 제거
            _inspectionWindow.CloseButtonVisible = false;
            _inspectionWindow.Show(_dockPanel, DockState.Document);

            // Teach 창 추가 (처음에는 숨겨둠)

            _teachWindow = new CameraForm();
            _teachWindow.Text = "학습창";
            _teachWindow.CloseButton = false; // 닫기 버튼 제거
            _teachWindow.CloseButtonVisible = false;
            _teachWindow.Show(_dockPanel, DockState.Document);

            ////로그창 50% 비율로 추가
          

            //var cameraWindow = new CameraForm();
            //cameraWindow.Show(_dockPanel, DockState.Document);
            ////검사 결과창 30% 비율로 추가
            //var resultWindow = new ResultForm();
            ////resultWindow.Show(cameraWindow.Pane, DockAlignment.Bottom, 0.3);

            ////# MODEL TREE#3 검사 결과창 우측에 40% 비율로 모델트리 추가
            //var modelTreeWindow = new ModelTreeForm();
            ////modelTreeWindow.Show( _teachWindow.Pane, DockAlignment.Right, 0.4);

            //속성창 추가
            //var propWindow = new PropertiesForm();
            //propWindow.Show(_dockPanel, DockState.DockRight);
            var logWindow = new LogForm();
            logWindow.Show(_teachWindow.Pane, DockAlignment.Bottom, 0.2);

            ////속성창과 같은탭에 추가하기
            //var statisticWindow = new StatisticForm();
            //statisticWindow.Show(_dockPanel, DockState.DockRight);

            //propWindow.Activate();

            
        }



     




        //제네릭 함수 사용를 이용해 입력된 타입의 폼 객체 얻기
        public static T GetDockForm<T>() where T : DockContent
        {
            var findForm = _dockPanel.Contents.OfType<T>().FirstOrDefault();
            return findForm;
        }

        //모든 DockContent 리스트 얻기
        private void GetDockContentState()
        {
            var dockedForms = _dockPanel.Contents.OfType<DockContent>().ToList();

            foreach (var form in dockedForms)
            {
                //MessageBox.Show($"Docked Form: {form.Text}");
                Console.WriteLine($"Docked Form: {form.Text}");
            }
        }

        //#MODEL SAVE#4 아래 메뉴 추가 
        /*
            Model New : 신규 모델 생성
            Model Open : 모델 열기
            Model Save : 모델 저장
            Model Save As : 모델 다른 이름으로 저장
         */

        private void ModelNewMenuItem_Click(object sender, EventArgs e)
        {
            //신규 모델 추가를 위한 모델 정보를 받기 위한 창 띄우기
            NewModel newModel = new NewModel();
            newModel.ShowDialog();
        }

        private void ModelOpenMenuItem_Click(object sender, EventArgs e)
        {
            //모델 파일 열기
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "모델 파일 선택";
                openFileDialog.Filter = "Model Files|*.xml;";
                openFileDialog.Multiselect = false;
                openFileDialog.InitialDirectory = SettingXml.Inst.ModelDir;
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    Global.Inst.InspStage.LoadModel(filePath);
                }
            }
        }
        private void ModelSaveMenuItem_Click(object sender, EventArgs e)
        {
            //모델 파일 저장
            Global.Inst.InspStage.SaveModel("");
        }

        private void ModelSaveAsMenuItem_Click(object sender, EventArgs e)
        {
            //다른이름으로 모델 파일 저장

            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.InitialDirectory = SettingXml.Inst.ModelDir;
                saveFileDialog.Title = "모델 파일 선택";
                saveFileDialog.Filter = "Model Files|*.xml;";
                saveFileDialog.DefaultExt = "xml";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = saveFileDialog.FileName;
                    Global.Inst.InspStage.SaveModel(filePath);
                }
            }
        }

        private void ImageLoadMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "이미지 파일 선택";
                openFileDialog.Filter = "Image Files|*.bmp;*.jpg;*.jpeg;*.png;*.gif";
                openFileDialog.Multiselect = false;
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    Global.Inst.InspStage.SetImageBuffer(filePath);
                    Global.Inst.InspStage.CurModel.InspectImagePath = filePath;
                }
            }
        }

        private void ImageSaveMenuItem_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Title = "이미지 저장";
                saveFileDialog.Filter = "PNG 파일|*.png|JPEG 파일|*.jpg|Bitmap 파일|*.bmp";
                saveFileDialog.DefaultExt = "png";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = saveFileDialog.FileName;
                    Global.Inst.InspStage.SaveCurrentImage(filePath);
                }
            }
        }

        //#SETUP#8 메인메뉴에 Setup 메뉴 추가하고, 아래 함수로 환경설정창 띄우기
        private void SetupMenuItem_Click(object sender, EventArgs e)
        {
            SLogger.Write($"환경설정창 열기");
            SetupForm setupForm = new SetupForm();
            setupForm.ShowDialog();
        }
        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Global.Inst.Dispose();

            this.FormClosed -= MainForm_FormClosed;
        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }

        
    }
}
