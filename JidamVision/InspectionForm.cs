﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using JidamVision.Algorithm;
using OpenCvSharp;
using WeifenLuo.WinFormsUI.Docking;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace JidamVision
{
    public partial class InspectionForm : DockContent
    {
        private List<Mat> receivedImages = new List<Mat>(); // 외부 프로그램에서 받은 이미지 목록
        private int currentImageIndex = 0;
        private Timer inspectionTimer;
        private InspAlgorithm inspector;
        private Timer currentTimeTimer; // 현재 시간을 자동 갱신하는 타이머
        private int totalCount = 0;  // 총 검사 개수
        private int goodCount = 0;   // 양품 개수
        private int faultyCount = 0; // 불량 개수
        public InspectionForm()
        {
            InitializeComponent();
            InitializeInspection();
            InitializeTimers();  // 현재 시간 갱신 타이머 초기화
            ConfigureDateTimePickers(); // DateTimePicker 포맷 설정
        }
        private void InitializeInspection()
        {
            inspectionTimer = new Timer();
            inspectionTimer.Interval = 1000; // 1초마다 검사 실행
            inspectionTimer.Tick += InspectionTimer_Tick;

            // 사용할 검사 알고리즘 인스턴스 생성 (예제: MatchAlgorithm)
            inspector = new MatchAlgorithm();
        }
        private void InitializeTimers()
        {
            // 현재 시간 자동 갱신 타이머 설정
            currentTimeTimer = new Timer();
            currentTimeTimer.Interval = 1000; // 1초마다 현재 시간 갱신
            currentTimeTimer.Tick += (s, e) =>
            {
                dtpCurrenttime.Value = DateTime.Now;
            };
            currentTimeTimer.Start();
        }
        private void ConfigureDateTimePickers()
        {
            // DateTimePicker에 초까지 표시하도록 설정
            dtpStartTime.Format = DateTimePickerFormat.Custom;
            dtpStartTime.CustomFormat = "yyyy-MM-dd HH:mm:ss";

            dtpCurrenttime.Format = DateTimePickerFormat.Custom;
            dtpCurrenttime.CustomFormat = "yyyy-MM-dd HH:mm:ss";
        }
        // 외부 프로그램에서 이미지 리스트를 설정하는 함수
        public void SetImages(List<Mat> images)
        {
            receivedImages = images;
            currentImageIndex = 0;

            // 검사 시작 전 개수 초기화
            totalCount = 0;
            goodCount = 0;
            faultyCount = 0;
            UpdateInspectionResults(); // UI 업데이트
        }
        private void bntStart_Click(object sender, EventArgs e)
        {
            if (receivedImages.Count > 0)
            {
                currentImageIndex = 0;

                // 시작할 때 개수 초기화
                totalCount = 0;
                goodCount = 0;
                faultyCount = 0;
                UpdateInspectionResults();

                StartInspection();
                inspectionTimer.Start();

                dtpStartTime.Value = DateTime.Now; // 시작 버튼을 누른 순간의 시간 기록
            }
            else
            {
                MessageBox.Show("이미지가 없습니다.");
            }
        }
        private void UpdateInspectionResults()
        {
            rtbTotalnumber.Text = totalCount.ToString();
            rtbGood.Text = goodCount.ToString();
            rtbFaulty.Text = faultyCount.ToString();

            // 퍼센트 계산 (불량 개수 / 총 개수 * 100)
            double percent = totalCount > 0 ? (faultyCount / (double)totalCount) * 100 : 0;
            rtbPercent.Text = percent.ToString("0.00") + "%";
        }

        private void bntStop_Click(object sender, EventArgs e)
        {
            inspectionTimer.Stop();
        }
        private void InspectionTimer_Tick(object sender, EventArgs e)
        {
            currentImageIndex++;
            if (currentImageIndex < receivedImages.Count)
            {
                StartInspection();
            }
            else
            {
                inspectionTimer.Stop();
                MessageBox.Show("검사가 완료되었습니다.");
            }
        }

        private void StartInspection()
        {
            if (inspector != null)
            {
                inspector.SetInspData(receivedImages[currentImageIndex]);
                bool result = inspector.DoInspect();

                // 검사 결과 출력
                Console.WriteLine($"검사 결과: {(result ? "성공" : "실패")}");
            }
        }

        private void InspectionForm_Resize(object sender, EventArgs e)
        {
            int margin = 80;

            // 오른쪽 UI 요소들의 X 위치 조정
            int xPos =  this.Width - dgvMetric.Width - margin;

            dtpStartTime.Location = new System.Drawing.Point(dtpStartTime.Location.X, dtpStartTime.Location.Y);
            lbStartTime.Location = new System.Drawing.Point(lbStartTime.Location.X, lbStartTime.Location.Y);
            dtpCurrenttime.Location = new System.Drawing.Point(dtpCurrenttime.Location.X, dtpCurrenttime.Location.Y);
            lbCurrenttime.Location = new System.Drawing.Point(lbCurrenttime.Location.X, lbCurrenttime.Location.Y);


            bntStart.Location = new System.Drawing.Point(xPos-bntStart.Width-30, bntStart.Location.Y);
            bntStop.Location = new System.Drawing.Point(xPos- bntStop.Width-30, bntStop.Location.Y);
            rtbTotalnumber.Location = new System.Drawing.Point(xPos- rtbTotalnumber.Width-30, rtbTotalnumber.Location.Y);
            lbTotalnumber.Location = new System.Drawing.Point(xPos - lbTotalnumber.Width - 120, lbTotalnumber.Location.Y);
            rtbGood.Location = new System.Drawing.Point(xPos, rtbGood.Location.Y);
            lbGood.Location = new System.Drawing.Point(xPos - lbGood.Width - 5, lbGood.Location.Y);
            rtbFaulty.Location = new System.Drawing.Point(xPos, rtbFaulty.Location.Y);
            lbFaulty.Location = new System.Drawing.Point(xPos - lbFaulty.Width - 5, lbFaulty.Location.Y);
            dgvMetric.Location = new System.Drawing.Point(xPos, dgvMetric.Location.Y);
            rtbPercent.Location = new System.Drawing.Point(xPos, rtbPercent.Location.Y);
            lbPercent.Location = new System.Drawing.Point(xPos - lbPercent.Width - 5, lbPercent.Location.Y);

            // pictureBox1 크기 조정 (좌측 상단에 고정)
            pictureBox1.Width = xPos - margin * 2; // UI 요소들과 겹치지 않도록 조정
            pictureBox1.Height = this.Height - margin * 2;
            pictureBox1.Location = new System.Drawing.Point(margin, margin);
        }
    }
}
