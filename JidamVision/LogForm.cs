using JidamVision.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.Linq;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using static JidamVision.Util.SLogger;

namespace JidamVision
{
    /*
    #LOGFORM# - <<<로그 저장하는 기능 개발>>> 
    프로그램에서 일어나는 이벤트를 로그로 저장하는 기능
    리스트 박스에 로그로 출력하면서, 로그 파일로 저장함
    1) log4net 라이브러리 참조 추가하기 : ExternalLib\Dll\Log4Net\log4net.dll
    2) log4net.config 파일을 프로젝트 내에 추가하기 : ExternalLib\Dll\Log4Net\log4net.config
    3) Util/SLogger.cs : 로그를 저장하는 클래스 생성
    4) LogForm 클래스에 listBox 컨트롤 추가
    */

    //#LOGFORM#3 로그폼 클래스 생성
    //1) listbox 컨트롤을 추가하여 로그를 출력
    public partial class LogForm : DockContent
    {
        public LogForm()
        {
            InitializeComponent();

            //#LOGFORM#4 이벤트 추가
            //폼이 닫힐 때 이벤트 제거를 위해 이벤트 추가
            this.FormClosed += LogForm_FormClosed;
            //로그가 추가될 때 이벤트 추가
            SLogger.LogUpdated += OnLogUpdated;
        }

        //#LOGFORM#6 로그 이벤트 발생시, 리스트박스에 로그 추가 함수 호출
        private void OnLogUpdated(string logMessage)
        {
            //UI 스레드가 아닐 경우, Invoke()를 호출하여 UI 스레드에서 실행되도록 강제함
            if (listBoxLogs.InvokeRequired)
            {
                listBoxLogs.Invoke(new Action(() => AddLog(logMessage)));
            }
            else
            {
                AddLog(logMessage);
            }
        }

        //#LOGFORM#5 리스트박스에 로그 추가
        private void AddLog(string logMessage)
        {
            //로그가 1000개 이상이면, 가장 오래된 로그를 삭제
            if (listBoxLogs.Items.Count > 1000)
            {
                listBoxLogs.Items.RemoveAt(0);
            }
            listBoxLogs.Items.Add(logMessage);
            
            //자동 스크롤
            listBoxLogs.TopIndex = listBoxLogs.Items.Count - 1;     
        }

        //#LOGFORM#7 폼이 닫힐 때 이벤트 제거
        private void LogForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            SLogger.LogUpdated -= OnLogUpdated;
            this.FormClosed -= LogForm_FormClosed;
        }
    }
}
