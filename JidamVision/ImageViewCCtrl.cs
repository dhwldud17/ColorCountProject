using JidamVision.Algorithm;
using JidamVision.Core;
using JidamVision.Teach;
using OpenCvSharp.Dnn;
using OpenCvSharp.Internal.Vectors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Linq;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using static System.Windows.Forms.MonthCalendar;

namespace JidamVision
{
    /*
    #MULTI ROI# - <<<검사에 필요한 다양한 ROI를 추가하고 수정할 수 있도록 기능 수정>>> 
    //여러타입의 ROI를 여러개 입력하도록 기능 수정
    //개별로 선택된 ROI를 수정할 수 있음
    */


    //Virtual좌표계 : 이미지 크기 기준 좌표, 예> 640X480 이미지 상의 좌표(InspWindow상의 좌표)
    //Screen좌표계 : UserControl 크기 기준으로 좌표(마우스 좌표, 커서 위치 등)

    //#MULTI ROI#2 ROI를 추가,수정,삭제하는 액션을 타입으로 설정
    public enum EntityActionType
    {
        None = 0,
        Select,
        Inspect,
        Add,
        Move,
        Resize,
        Delete,
        DeleteList,
        AddGroup,
        Break,
        UpdateImage
    }

    public partial class ImageViewCCtrl : UserControl
    {
        //#MULTI ROI#2 ROI를 추가,수정,삭제 등으로 변경 시, 이벤트 발생
        public event EventHandler<DiagramEntityEventArgs> DiagramEntityEvent;

        private Point _roiStart = Point.Empty;
        private Rectangle _roiRect = Rectangle.Empty;
        private bool _isSelectingRoi = false;
        private bool _isResizingRoi = false;
        private bool _isMovingRoi = false;
        private bool _isInitialized = false;
        private Point _resizeStart = Point.Empty;
        private Point _moveStart = Point.Empty;
        private int _resizeDirection = -1;
        private const int _ResizeHandleSize = 10;

        // 현재 로드된 이미지
        private Bitmap _bitmapImage = null;

        // 더블 버퍼링을 위한 캔버스
        // 더블버퍼링 : 화면 깜빡임을 방지하고 부드러운 펜더링위해 사용
        // Onpaint() 메서드는 화면을 즉시 그리는 방식 사용
        // 사용자가 빠르게 작동할 경우 Flickering 발생
        private Bitmap Canvas = null;

        // 화면에 표시될 이미지의 크기 및 위치
        // 부동 소수점(float) 좌표를 사용하는 사각형 구조체
        private RectangleF ImageRect = new RectangleF(0, 0, 0, 0);

        // 현재 줌 배율
        private float _curZoom = 1.0f;
        // 줌 배율 변경 시, 확대/축소 단위
        private float _zoomFactor = 1.1f;

        // 최소 및 최대 줌 제한 값
        private float MinZoom = 1.0f;
        private const float MaxZoom = 100.0f;

        //#MATCH PROP#11 템플릿 매칭 결과 출력을 위해 Rectangle 리스트 변수 설정
        private List<Rectangle> _rectangles = new List<Rectangle>();

        //#MULTI ROI#5 수정에 필요한 타입 추가

        //새로 추가할 ROI 타입
        private InspWindowType _newRoiType = InspWindowType.None;

        //여러개 ROI를 관리하기 위한 리스트
        private List<DiagramEntity> _diagramEntityList = new List<DiagramEntity>();

        //현재 선택된 ROI 리스트
        private List<DiagramEntity> _multiSelectedEntities = new List<DiagramEntity>();
        private DiagramEntity _selEntity;
        private Color _selColor = Color.White;

        private Rectangle _selectionBox = Rectangle.Empty;
        private bool _isBoxSelecting = false;
        private bool _isCtrlPressed = false;
        private Rectangle _screenSelectedRect = Rectangle.Empty;

        //팝업 메뉴
        private ContextMenuStrip _contextMenu;

        public ImageViewCCtrl()
        {
            InitializeComponent();
            InitializeCanvas();

            //#GROUP ROI#3 화면상에서, 팝업 메뉴 띄우기
            _contextMenu = new ContextMenuStrip();
            _contextMenu.Items.Add("Create Group", null, OnCreateGroupClicked);
            _contextMenu.Items.Add("Break Group", null, OnBreakGroupClicked);
            _contextMenu.Items.Add("Delete", null, OnDeleteClicked);
            _contextMenu.Items.Add(new ToolStripSeparator());
            _contextMenu.Items.Add("Teaching", null, OnTeachingClicked);
            _contextMenu.Items.Add("Update Image", null, OnUpdateImageClicked);
            _contextMenu.Items.Add("Unlock", null, OnUnlockClicked);

            // 마우스 휠 이벤트를 등록하여 줌 기능 추가
            // MouseWheel+= : 같은 이벤트에 여러 개의 핸들러 등록가능. 이전 핸들러 삭제않고 추가됨
            // UserControl1_MouseWheel 메서드를 MouseEventHandler 델리게이트(delegate) 형식으로 변환
            // MouseWheel += UserControl1_MouseWheel; 델리게이트 직접 지정없이해도 자동변환됨
            MouseWheel += new MouseEventHandler(ImageViewCCtrl_MouseWheel);
        }

        private void InitializeCanvas()
        {
            // 캔버스를 UserControl 크기만큼 생성
            ResizeCanvas();

            // 화면 깜빡임을 방지하기 위한 더블 버퍼링 설정
            DoubleBuffered = true;
        }

        //#MULTI ROI#6 InspWindow 타입에 따른, 칼라 정보 얻는 함수
        public Color GetWindowColor(InspWindowType inspWindowType)
        {
            Color color = Color.LightBlue;

            switch (inspWindowType)
            {
                case InspWindowType.Base:
                    color = Color.BurlyWood;
                    break;
                case InspWindowType.Sub:
                    color = Color.Brown;
                    break;
                case InspWindowType.ID:
                    color = Color.Cyan;
                    break;
                case InspWindowType.Package:
                    color = Color.LightBlue;
                    break;
                case InspWindowType.Body:
                    color = Color.Chartreuse;
                    break;
                case InspWindowType.Chip:
                    color = Color.Orange;
                    break;
                case InspWindowType.Pad:
                    color = Color.Yellow;
                    break;
            }

            return color;
        }

        //#MULTI ROI#7 모델트리로 부터 호출되어, 신규 ROI를 추가하도록 하는 기능 시작점
        public void NewRoi(InspWindowType inspWindowType)
        {
            _newRoiType = inspWindowType;
            _selColor = GetWindowColor(inspWindowType);
        }


        //#GROUP ROI#5 줌에 따른 좌표 계산 기능 수정 
        private void ResizeCanvas()
        {
            if (Width <= 0 || Height <= 0 || _bitmapImage == null)
                return;

            // 캔버스를 UserControl 크기만큼 생성
            Canvas = new Bitmap(Width, Height);
            if (Canvas == null)
                return;

            // 이미지 원본 크기 기준으로 확대/축소 (ZoomFactor 유지)
            float virtualWidth = _bitmapImage.Width * _curZoom;
            float virtualHeight = _bitmapImage.Height * _curZoom;

            float offsetX = virtualWidth < Width ? (Width - virtualWidth) / 2f : 0f;
            float offsetY = virtualHeight < Height ? (Height - virtualHeight) / 2f : 0f;

            ImageRect = new RectangleF(offsetX, offsetY, virtualWidth, virtualHeight);
        }

        //#GROUP ROI#6 이미지 로딩 함수
        public void LoadBitmap(Bitmap bitmap)
        {
            // 기존에 로드된 이미지가 있다면 해제 후 초기화, 메모리누수 방지
            if (_bitmapImage != null)
            {
                //이미지 크기가 같다면, 이미지 변경 후, 화면 갱신
                if (_bitmapImage.Width == bitmap.Width && _bitmapImage.Height == bitmap.Height)
                {
                    _bitmapImage = bitmap;
                    Invalidate();
                    return;
                }

                _bitmapImage.Dispose(); // Bitmap 객체가 사용하던 메모리 리소스를 해제
                _bitmapImage = null;  //객체를 해제하여 가비지 컬렉션(GC)이 수집할 수 있도록 설정
            }

            // 새로운 이미지 로드
            _bitmapImage = bitmap;

            ////bitmap==null 예외처리도 초기화되지않은 변수들 초기화
            if (_isInitialized == false)
            {
                _isInitialized = true;
                ResizeCanvas();
            }

            FitImageToScreen();
        }

        private void FitImageToScreen()
        {
            RecalcZoomRatio();

            float NewWidth = _bitmapImage.Width * _curZoom;
            float NewHeight = _bitmapImage.Height * _curZoom;

            // 이미지가 UserControl 중앙에 배치되도록 정렬
            ImageRect = new RectangleF(
                (Width - NewWidth) / 2, // UserControl 너비에서 이미지 너비를 뺀 후, 절반을 왼쪽 여백으로 설정하여 중앙 정렬
                (Height - NewHeight) / 2,
                NewWidth,
                NewHeight
            );

            Invalidate();
        }

        public void LoadImage(string path)
        {
            using (Bitmap image = (Bitmap)Image.FromFile(path))
            {
                LoadBitmap(image);
            }
        }

        //#GROUP ROI#7 현재 이미지를 기준으로 줌 비율 재계산
        private void RecalcZoomRatio()
        {
            if (_bitmapImage == null || Width <= 0 || Height <= 0)
                return;

            Size imageSize = new Size(_bitmapImage.Width, _bitmapImage.Height);

            float aspectRatio = (float)imageSize.Height / (float)imageSize.Width;
            float clientAspect = (float)Height / (float)Width;

            //UserControl과 이미지의 비율의 관계를 통해, 이미지가 UserControl안에 들어가도록 Zoom비율 설정
            float ratio;
            if (aspectRatio <= clientAspect)
                ratio = (float)Width / (float)imageSize.Width;
            else
                ratio = (float)Height / (float)imageSize.Height;

            //최소 줌 비율은 이미지가 UserControl에 꽉차게 들어가는 것으로 설정
            float minZoom = ratio;

            // MinZoom 및 줌 적용
            MinZoom = minZoom;

            _curZoom = Math.Max(MinZoom, Math.Min(MaxZoom, ratio));

            Invalidate();
        }

        #region 나중에 사용할 함수로 활요
        //public Bitmap GetRoiImage(DiagramEntity entity)
        //{
        //    Rectangle rect = entity.EntityROI;

        //    if (Bitmap == null || rect.IsEmpty)
        //        return null;

        //    // 원본 이미지에서 ROI 크롭
        //    Bitmap roiBitmap = new Bitmap(rect.Width, rect.Height);
        //    using (Graphics g = Graphics.FromImage(roiBitmap))
        //    {
        //        g.DrawImage(Bitmap, new Rectangle(0, 0, rect.Width, rect.Height), rect, GraphicsUnit.Pixel);
        //    }

        //    return roiBitmap;
        //}

        //public void SaveROI(DiagramEntity entity, string savePath)
        //{
        //    Rectangle rect = entity.EntityROI;

        //    if (Bitmap == null || rect.IsEmpty)
        //        return;

        //    // 원본 이미지에서 ROI 크롭
        //    using (Bitmap roiBitmap = new Bitmap(rect.Width, rect.Height))
        //    {
        //        using (Graphics g = Graphics.FromImage(roiBitmap))
        //        {
        //            g.DrawImage(Bitmap, new Rectangle(0, 0, rect.Width, rect.Height), rect, GraphicsUnit.Pixel);
        //        }
        //        roiBitmap.Save(savePath, ImageFormat.Png);
        //    }
        //}
        #endregion

        // Windows Forms에서 컨트롤이 다시 그려질 때 자동으로 호출되는 메서드
        // 화면새로고침(Invalidate()), 창 크기변경, 컨트롤이 숨겨졌다가 나타날때 실행
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (_bitmapImage != null && Canvas != null)
            {
                // 캔버스를 초기화하고 이미지 그리기
                using (Graphics g = Graphics.FromImage(Canvas))  // 메모리누수방지
                {
                    g.Clear(Color.Transparent); // 배경을 투명하게 설정

                    //이미지 확대or축소때 화질 최적화 방식(Interpolation Mode) 설정                    
                    g.InterpolationMode = InterpolationMode.NearestNeighbor;
                    g.DrawImage(_bitmapImage, ImageRect);

                    /* Interpolation Mode********************************************
                     * NearestNeighbor	빠르지만 품질이 낮음 (픽셀이 깨질 수 있음)
                     * Bicubic	Bilinear보다 더 부드러움, 그러나 속도가 느릴 수 있음
                     * HighQualityBicubic	가장 부드럽고 고품질, 그러나 가장 느림
                     * HighQualityBilinear	Bilinear보다 품질이 높고 Bicubic보다 빠름
                     ****************************************************************/

                    //#MATCH PROP#12 템플릿 매칭 위치 그리기

                    // 이미지 좌표 → 화면 좌표 변환 후 사각형 그리기
                    if (_rectangles != null)
                    {
                        using (Pen pen = new Pen(Color.LightCoral, 2))
                        {
                            foreach (var rect in _rectangles)
                            {
                                Rectangle screenRect = VirtualToScreen(rect);
                                g.DrawRectangle(pen, screenRect);
                            }
                        }
                    }

                    //#MULTI ROI#8 여러개 ROI를 그려주는 코드
                    //#GROUP ROI#8 멀티ROI 처리
                    _screenSelectedRect = new Rectangle(0, 0, 0, 0);
                    foreach (DiagramEntity entity in _diagramEntityList)
                    {
                        Rectangle screenRect = VirtualToScreen(entity.EntityROI);
                        using (Pen pen = new Pen(entity.EntityColor, 2))
                        {
                            if (_multiSelectedEntities.Contains(entity))
                            {
                                pen.DashStyle = DashStyle.Dash;
                                pen.Width = 2;

                                if (_screenSelectedRect.IsEmpty)
                                {
                                    _screenSelectedRect = screenRect;
                                }
                                else
                                {
                                    //선택된 roi가 여러개 일때, 전체 roi 영역 계산
                                    //선택된 roi 영역 합치기
                                    _screenSelectedRect = Rectangle.Union(_screenSelectedRect, screenRect);
                                }
                            }

                            g.DrawRectangle(pen, screenRect);
                        }

                        //선택된 ROI가 있다면, 리사이즈 핸들 그리기
                        if (_multiSelectedEntities.Count <= 1 && entity == _selEntity)
                        {
                            // 리사이즈 핸들 그리기 (8개 포인트: 4 모서리 + 4 변 중간)
                            using (Brush brush = new SolidBrush(Color.LightBlue))
                            {
                                Point[] resizeHandles = GetResizeHandles(screenRect);
                                foreach (Point handle in resizeHandles)
                                {
                                    g.FillRectangle(brush, handle.X - _ResizeHandleSize / 2, handle.Y - _ResizeHandleSize / 2, _ResizeHandleSize, _ResizeHandleSize);
                                }
                            }
                        }
                    }

                    //#GROUP ROI#9 선택된 개별 roi가 없고, 여러개가 선택되었다면
                    if (_multiSelectedEntities.Count > 1 && !_screenSelectedRect.IsEmpty)
                    {
                        using (Pen pen = new Pen(Color.White, 2))
                        {
                            g.DrawRectangle(pen, _screenSelectedRect);
                        }

                        // 리사이즈 핸들 그리기 (8개 포인트: 4 모서리 + 4 변 중간)
                        using (Brush brush = new SolidBrush(Color.LightBlue))
                        {
                            Point[] resizeHandles = GetResizeHandles(_screenSelectedRect);
                            foreach (Point handle in resizeHandles)
                            {
                                g.FillRectangle(brush, handle.X - _ResizeHandleSize / 2, handle.Y - _ResizeHandleSize / 2, _ResizeHandleSize, _ResizeHandleSize);
                            }
                        }
                    }

                    //#MULTI ROI#9 신규 ROI 추가할때, 해당 ROI 그리기
                    if (_isSelectingRoi && !_roiRect.IsEmpty)
                    {
                        Rectangle rect = VirtualToScreen(_roiRect);
                        using (Pen pen = new Pen(_selColor, 2))
                        {
                            g.DrawRectangle(pen, rect);
                        }
                    }

                    if (_multiSelectedEntities.Count <= 1 && _selEntity != null)
                    {
                        //확장영역이 있다면 표시
                        DrawInspParam(g, _selEntity.LinkedWindow);
                    }

                    //#GROUP ROI#10 선택 영역 박스 그리기
                    if (_isBoxSelecting && !_selectionBox.IsEmpty)
                    {
                        using (Pen pen = new Pen(Color.LightSkyBlue, 3))
                        {
                            pen.DashStyle = DashStyle.Dash;
                            pen.Width = 2;
                            g.DrawRectangle(pen, _selectionBox);
                        }
                    }

                    // 캔버스를 UserControl 화면에 표시
                    e.Graphics.DrawImage(Canvas, 0, 0);
                }
            }
        }

        private void DrawInspParam(Graphics g, InspWindow window)
        {
            if (window is null)
                return;

            MatchAlgorithm matchAlgo = (MatchAlgorithm)window.FindInspAlgorithm(InspectType.InspMatch);
            if (matchAlgo != null)
            {
                Rectangle extArea = new Rectangle(window.WindowArea.X - matchAlgo.ExtSize.Width,
                    window.WindowArea.Y - matchAlgo.ExtSize.Height,
                    window.WindowArea.Width + matchAlgo.ExtSize.Width * 2,
                    window.WindowArea.Height + matchAlgo.ExtSize.Height * 2);
                Rectangle screenRect = VirtualToScreen(extArea);

                using (Pen pen = new Pen(Color.White, 2))
                {
                    pen.DashStyle = DashStyle.Dot;
                    pen.Width = 2;
                    g.DrawRectangle(pen, screenRect);
                }
            }
        }

        private void ImageViewCCtrl_MouseDown(object sender, MouseEventArgs e)
        {
            //#MULTI ROI#10 여러개 ROI 기능에 맞게 코드 수정
            if (e.Button == MouseButtons.Left)
            {
                if (_newRoiType != InspWindowType.None)
                {
                    //새로운 ROI 그리기 시작 위치 설저어
                    _roiStart = e.Location;
                    _isSelectingRoi = true;
                    _selEntity = null;
                }
                else
                {
                    if (_multiSelectedEntities.Count > 1)
                    {
                        if (_screenSelectedRect.Contains(e.Location))
                        {
                            _selEntity = _multiSelectedEntities[0];
                            _isMovingRoi = true;
                            _moveStart = e.Location;
                            _roiRect = _multiSelectedEntities[0].EntityROI;
                            Invalidate();
                            return;
                        }
                    }

                    if (_selEntity != null && !_selEntity.IsHold && _selEntity.GetParentGroup() == null)
                    {
                        Rectangle screenRect = VirtualToScreen(_selEntity.EntityROI);
                        //마우스 클릭 위치가 ROI 크기 변경을 하기 위한 위치(모서리,엣지)인지 여부 판단
                        _resizeDirection = GetResizeHandleIndex(screenRect, e.Location);
                        if (_resizeDirection != -1)
                        {
                            _isResizingRoi = true;
                            _resizeStart = e.Location;
                            Invalidate();
                            return;
                        }
                    }

                    _selEntity = null;
                    foreach (DiagramEntity entity in _diagramEntityList)
                    {
                        Rectangle screenRect = VirtualToScreen(entity.EntityROI);
                        if (screenRect.Contains(e.Location))
                        {
                            //#GROUP ROI#11 컨트롤키를 이용해, 개별 ROI 추가/제거
                            if (_isCtrlPressed)
                            {
                                if (_multiSelectedEntities.Contains(entity))
                                {
                                    _multiSelectedEntities.Remove(entity);
                                }
                                else
                                {
                                    AddSelectedROI(entity);
                                }
                            }
                            else
                            {
                                _multiSelectedEntities.Clear();
                                AddSelectedROI(entity);
                            }

                            _selEntity = entity;
                            _roiRect = entity.EntityROI;
                            _isMovingRoi = true;
                            _moveStart = e.Location;
                            break;
                        }
                    }

                    if (_selEntity == null && !_isCtrlPressed)
                    {
                        _isBoxSelecting = true;
                        _roiStart = e.Location;
                        _selectionBox = new Rectangle();
                    }

                    Invalidate();
                }
            }
            // 마우스 오른쪽 버튼이 눌렸을 때 클릭 위치 저장
            else if (e.Button == MouseButtons.Right)
            {
                //#MULTI ROI#11 같은 타입의 ROI추가가 더이상 없다면 초기화하여, ROI가 추가되지 않도록 함
                _newRoiType = InspWindowType.None;

                // UserControl이 포커스를 받아야 마우스 휠이 정상적으로 동작함
                Focus();
            }
        }

        private void ImageViewCCtrl_MouseMove(object sender, MouseEventArgs e)
        {
            //#MULTI ROI#12 마우스 이동시, 구현 코드
            if (e.Button == MouseButtons.Left)
            {
                //최초 ROI 생성하여 그리기
                if (_isSelectingRoi)
                {
                    int x = Math.Min(_roiStart.X, e.X);
                    int y = Math.Min(_roiStart.Y, e.Y);
                    int width = Math.Abs(e.X - _roiStart.X);
                    int height = Math.Abs(e.Y - _roiStart.Y);
                    _roiRect = ScreenToVirtual(new Rectangle(x, y, width, height));
                    Invalidate();
                }
                //기존 ROI 크기 변경
                else if (_isResizingRoi)
                {
                    ResizeROI(e.Location);
                    if (_selEntity != null)
                        _selEntity.EntityROI = _roiRect;
                    _resizeStart = e.Location;
                    Invalidate();
                }
                //ROI 위치 이동
                else if (_isMovingRoi)
                {
                    int dx = e.X - _moveStart.X;
                    int dy = e.Y - _moveStart.Y;

                    int dxVirtual = (int)((float)dx / _curZoom + 0.5f);
                    int dyVirtual = (int)((float)dy / _curZoom + 0.5f);

                    //#GROUP ROI#12 여러개 선택된 roi 이동
                    if (_multiSelectedEntities.Count > 1)
                    {
                        foreach (var entity in _multiSelectedEntities)
                        {
                            if (entity is null || entity.IsHold)
                                continue;

                            Rectangle rect = entity.EntityROI;
                            rect.X += dxVirtual;
                            rect.Y += dyVirtual;
                            entity.EntityROI = rect;
                        }
                    }
                    else if (_selEntity != null && !_selEntity.IsHold)
                    {
                        _roiRect.X += dxVirtual;
                        _roiRect.Y += dyVirtual;
                        _selEntity.EntityROI = _roiRect;
                    }

                    _moveStart = e.Location;
                    Invalidate();
                }
                //ROI 선택 박스 그리기
                else if (_isBoxSelecting)
                {
                    int x = Math.Min(_roiStart.X, e.X);
                    int y = Math.Min(_roiStart.Y, e.Y);
                    int w = Math.Abs(e.X - _roiStart.X);
                    int h = Math.Abs(e.Y - _roiStart.Y);
                    _selectionBox = new Rectangle(x, y, w, h);
                    Invalidate();

                }
            }
            //마우스 클릭없이, 위치만 이동시에, 커서의 위치가 크기변경또는 이동 위치일때, 커서 변경
            else
            {
                if (_selEntity != null)
                {
                    Rectangle screenRoi = VirtualToScreen(_roiRect);
                    Rectangle screenRect = VirtualToScreen(_selEntity.EntityROI);
                    int index = GetResizeHandleIndex(screenRect, e.Location);
                    if (index != -1)
                    {
                        Cursor = GetCursorForHandle(index);
                    }
                    else if (screenRoi.Contains(e.Location))
                    {
                        Cursor = Cursors.SizeAll; // ROI 내부 이동
                    }
                    else
                    {
                        Cursor = Cursors.Default;
                    }
                }
            }
        }

        private void ImageViewCCtrl_MouseUp(object sender, MouseEventArgs e)
        {
            //#SETROI#5 ROI 크기 변경 또는 이동 완료
            //#MULTI ROI#13 마우스 업일때, 구현 코드
            if (e.Button == MouseButtons.Left)
            {
                if (_isSelectingRoi)
                {
                    _isSelectingRoi = false;

                    if (_bitmapImage is null)
                        return;

                    //ROI 크기가 10보다 작으면, 추가하지 않음
                    if (_roiRect.Width < 10 ||
                        _roiRect.Height < 10 ||
                        _roiRect.X < 0 ||
                        _roiRect.Y < 0 ||
                        _roiRect.Right > _bitmapImage.Width ||
                        _roiRect.Bottom > _bitmapImage.Height)
                        return;

                    _selEntity = new DiagramEntity(_roiRect, _selColor);

                    //모델에 InspWindow 추가하는 이벤트 발생
                    DiagramEntityEvent?.Invoke(this, new DiagramEntityEventArgs(EntityActionType.Add, null, _newRoiType, _roiRect, new Point()));


                }
                else if (_isResizingRoi)
                {
                    _selEntity.EntityROI = _roiRect;
                    _isResizingRoi = false;

                    //모델에 InspWindow 크기 변경 이벤트 발생
                    DiagramEntityEvent?.Invoke(this, new DiagramEntityEventArgs(EntityActionType.Resize, _selEntity.LinkedWindow, _newRoiType, _roiRect, new Point()));
                }
                else if (_isMovingRoi)
                {
                    _isMovingRoi = false;

                    if (_selEntity != null)
                    {
                        InspWindow linkedWindow = _selEntity.LinkedWindow;

                        Point offsetMove = new Point(0, 0);
                        if (linkedWindow != null)
                        {
                            offsetMove.X = _selEntity.EntityROI.X - linkedWindow.WindowArea.X;
                            offsetMove.Y = _selEntity.EntityROI.Y - linkedWindow.WindowArea.Y;
                        }

                        //모델에 InspWindow 이동 이벤트 발생
                        if (offsetMove.X != 0 || offsetMove.Y != 0)
                            DiagramEntityEvent?.Invoke(this, new DiagramEntityEventArgs(EntityActionType.Move, linkedWindow, _newRoiType, _roiRect, offsetMove));
                        else
                            //모델에 InspWindow 선택 변경 이벤트 발생
                            DiagramEntityEvent?.Invoke(this, new DiagramEntityEventArgs(EntityActionType.Select, _selEntity.LinkedWindow));

                    }
                }
                // ROI 선택 완료
                if (_isBoxSelecting)
                {
                    _isBoxSelecting = false;
                    _multiSelectedEntities.Clear();

                    Rectangle selectionVirtual = ScreenToVirtual(_selectionBox);

                    foreach (DiagramEntity entity in _diagramEntityList)
                    {
                        if (selectionVirtual.IntersectsWith(entity.EntityROI))
                        {
                            _multiSelectedEntities.Add(entity);
                        }
                    }

                    if (_multiSelectedEntities.Any())
                        _selEntity = _multiSelectedEntities[0];

                    _selectionBox = Rectangle.Empty;

                    //선택해제
                    DiagramEntityEvent?.Invoke(this, new DiagramEntityEventArgs(EntityActionType.Select, null));

                    Invalidate();

                    return;
                }
            }

            // 마우스를 떼면 마지막 오프셋 값을 저장하여 이후 이동을 연속적으로 처리
            if (e.Button == MouseButtons.Right)
            {
                if (_selEntity != null)
                {
                    //팝업메뉴 표시
                    _contextMenu.Show(this, e.Location);
                }
            }
        }

        private void AddSelectedROI(DiagramEntity entity)
        {
            if (entity is null)
                return;

            GroupWindow group = entity.GetParentGroup();
            if (group != null)
            {
                var entityList = _diagramEntityList
                    .Where(m => group.Members.Contains(m.LinkedWindow))
                    .Except(_multiSelectedEntities)
                    .ToList();
                _multiSelectedEntities.AddRange(entityList);
            }
            else
            {
                if (!_multiSelectedEntities.Contains(entity))
                    _multiSelectedEntities.Add(entity);
            }
        }

        #region ROI Handle
        //마우스 위치가 ROI 크기 변경을 위한 여부를 확인하기 위해, 4개 모서리와 사각형 라인의 중간 위치 반환
        private Point[] GetResizeHandles(Rectangle rect)
        {
            return new Point[]
            {
                new Point(rect.Left, rect.Top), // 좌상
                new Point(rect.Right, rect.Top), // 우상
                new Point(rect.Left, rect.Bottom), // 좌하
                new Point(rect.Right, rect.Bottom), // 우하
                new Point(rect.Left + rect.Width / 2, rect.Top), // 상 중간
                new Point(rect.Left + rect.Width / 2, rect.Bottom), // 하 중간
                new Point(rect.Left, rect.Top + rect.Height / 2), // 좌 중간
                new Point(rect.Right, rect.Top + rect.Height / 2) // 우 중간
            };
        }

        //마우스 위치가 크기 변경 위치에 해당하는 지를, 위치 인덱스로 반환
        private int GetResizeHandleIndex(Rectangle screenRect, Point mousePos)
        {
            Point[] handles = GetResizeHandles(screenRect);
            for (int i = 0; i < handles.Length; i++)
            {
                Rectangle handleRect = new Rectangle(handles[i].X - _ResizeHandleSize / 2, handles[i].Y - _ResizeHandleSize / 2, _ResizeHandleSize, _ResizeHandleSize);
                if (handleRect.Contains(mousePos)) return i;
            }
            return -1;
        }

        //사각 모서리와 중간 지점을 인덱스로 설정하여, 해당 위치에 따른 커서 타입 반환
        private Cursor GetCursorForHandle(int handleIndex)
        {
            switch (handleIndex)
            {
                case 0: case 3: return Cursors.SizeNWSE;
                case 1: case 2: return Cursors.SizeNESW;
                case 4: case 5: return Cursors.SizeNS;
                case 6: case 7: return Cursors.SizeWE;
                default: return Cursors.Default;
            }
        }
        #endregion

        //ROI 크기 변경시, 마우스 위치를 입력받아, ROI 크기 변경
        private void ResizeROI(Point mousePos)
        {
            Rectangle roi = VirtualToScreen(_roiRect);
            switch (_resizeDirection)
            {
                case 0:
                    roi.X = mousePos.X;
                    roi.Y = mousePos.Y;
                    roi.Width -= (mousePos.X - _resizeStart.X);
                    roi.Height -= (mousePos.Y - _resizeStart.Y);
                    break;
                case 1:
                    roi.Width = mousePos.X - roi.X;
                    roi.Y = mousePos.Y;
                    roi.Height -= (mousePos.Y - _resizeStart.Y);
                    break;
                case 2:
                    roi.X = mousePos.X;
                    roi.Width -= (mousePos.X - _resizeStart.X);
                    roi.Height = mousePos.Y - roi.Y;
                    break;
                case 3:
                    roi.Width = mousePos.X - roi.X;
                    roi.Height = mousePos.Y - roi.Y;
                    break;
                case 4:
                    roi.Y = mousePos.Y;
                    roi.Height -= (mousePos.Y - _resizeStart.Y);
                    break;
                case 5:
                    roi.Height = mousePos.Y - roi.Y;
                    break;
                case 6:
                    roi.X = mousePos.X;
                    roi.Width -= (mousePos.X - _resizeStart.X);
                    break;
                case 7:
                    roi.Width = mousePos.X - roi.X;
                    break;
            }

            _roiRect = ScreenToVirtual(roi);
        }

        private void ImageViewCCtrl_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta < 0)
                ZoomMove(_curZoom / _zoomFactor, e.Location);
            else
                ZoomMove(_curZoom * _zoomFactor, e.Location);

            // 새로운 이미지 위치 반영 (점진적으로 초기 상태로 회귀)
            if (_bitmapImage != null)
            {
                ImageRect.Width = _bitmapImage.Width * _curZoom;
                ImageRect.Height = _bitmapImage.Height * _curZoom;
            }

            // 다시 그리기 요청
            Invalidate();
        }

        //#GROUP ROI#13 휠에 의해, Zoom 확대/축소 값 계산
        private void ZoomMove(float zoom, Point zoomOrigin)
        {
            PointF virtualOrigin = ScreenToVirtual(new PointF(zoomOrigin.X, zoomOrigin.Y));

            _curZoom = Math.Max(MinZoom, Math.Min(MaxZoom, zoom));
            if (_curZoom <= MinZoom)
                return;

            PointF zoomedOrigin = VirtualToScreen(virtualOrigin);

            float dx = zoomedOrigin.X - zoomOrigin.X;
            float dy = zoomedOrigin.Y - zoomOrigin.Y;

            ImageRect.X -= dx;
            ImageRect.Y -= dy;
        }

        private void ImageViewCCtrl_Resize(object sender, EventArgs e)
        {
            ResizeCanvas();
            Invalidate();
        }

        public Rectangle GetRoiRect()
        {
            if (_bitmapImage == null || _roiRect.IsEmpty)
                return new Rectangle();

            return _roiRect;
        }

        //#MATCH PROP#13 템플릿 매칭 위치 입력 받는 함수
        public void AddRect(List<Rectangle> rectangles)
        {
            _rectangles = rectangles;
            Invalidate();
        }

        //#GROUP ROI#14 키보드 이벤트 받기 
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            _isCtrlPressed = keyData == Keys.Control;

            switch (keyData)
            {
                case Keys.Delete:
                    {
                        if (_selEntity != null)
                        {
                            DeleteSelEntity();
                        }
                    }
                    break;
                case Keys.Enter:
                    {
                        InspWindow selWindow = null;
                        if (_selEntity != null)
                            selWindow = _selEntity.LinkedWindow;

                        DiagramEntityEvent?.Invoke(this, new DiagramEntityEventArgs(EntityActionType.Inspect, selWindow));
                    }
                    break;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Control)
                _isCtrlPressed = false;

            base.OnKeyUp(e);
        }

        public bool SetDiagramEntityList(List<DiagramEntity> diagramEntityList)
        {
            //작은 roi가 먼저 선택되도록, 소팅
            _diagramEntityList = diagramEntityList
                                .OrderBy(r => r.EntityROI.Width * r.EntityROI.Height)
                                .ToList();

            _selEntity = null;
            Invalidate();
            return true;
        }

        public void SelectDiagramEntity(InspWindow window)
        {
            DiagramEntity entity = _diagramEntityList.Find(e => e.LinkedWindow == window);
            if (entity != null)
            {
                _multiSelectedEntities.Clear();
                AddSelectedROI(entity);

                _selEntity = entity;
                _roiRect = entity.EntityROI;
            }
        }

        //#GROUP ROI#4 팝업 메뉴 함수 
        #region Group Create and Break
        private void OnCreateGroupClicked(object sender, EventArgs e)
        {
            List<InspWindow> selected = _multiSelectedEntities
                .Where(d => d.LinkedWindow != null)
                .Select(d => d.LinkedWindow)
                .ToList();

            if (selected.Count == 0)
                return;

            DiagramEntityEvent?.Invoke(this, new DiagramEntityEventArgs(EntityActionType.AddGroup, selected, InspWindowType.Group));

            // 선택 해제
            _multiSelectedEntities.Clear();
            _selEntity = null;
        }

        private void OnBreakGroupClicked(object sender, EventArgs e)
        {
            if (_selEntity == null)
                return;
            DiagramEntityEvent?.Invoke(this, new DiagramEntityEventArgs(EntityActionType.Break, _selEntity.LinkedWindow));
        }

        private void OnDeleteClicked(object sender, EventArgs e)
        {
            DeleteSelEntity();
        }

        private void OnTeachingClicked(object sender, EventArgs e)
        {
            if (_selEntity is null)
                return;

            InspWindow window = _selEntity.LinkedWindow;

            if (window is null)
                return;

            window.IsTeach = true;
            _selEntity.IsHold = true;
        }

        private void OnUpdateImageClicked(object sender, EventArgs e)
        {
            if (_selEntity is null)
                return;

            InspWindow window = _selEntity.LinkedWindow;

            if (window is null)
                return;

            DiagramEntityEvent?.Invoke(this, new DiagramEntityEventArgs(EntityActionType.UpdateImage, _selEntity.LinkedWindow));
        }

        private void OnUnlockClicked(object sender, EventArgs e)
        {
            if (_selEntity is null)
                return;

            InspWindow window = _selEntity.LinkedWindow;

            if (window is null)
                return;

            _selEntity.IsHold = false;
        }

        private void DeleteSelEntity()
        {
            List<InspWindow> selected = _multiSelectedEntities
                .Where(d => d.LinkedWindow != null)
                .Select(d => d.LinkedWindow)
                .ToList();

            if (selected.Count > 0)
            {
                DiagramEntityEvent?.Invoke(this, new DiagramEntityEventArgs(EntityActionType.DeleteList, selected));
                return;
            }

            if (_selEntity != null)
            {
                InspWindow linkedWindow = _selEntity.LinkedWindow;
                if (linkedWindow is null)
                    return;

                InspWindow group = linkedWindow.Parent;
                if (group != null)
                    linkedWindow = group;

                DiagramEntityEvent?.Invoke(this, new DiagramEntityEventArgs(EntityActionType.Delete, linkedWindow));
            }
        }

        #endregion


        //#GROUP ROI#3-1 Virtual <-> Screen 좌표계 변환
        #region 좌표계 변환
        private PointF GetScreenOffset()
        {
            return new PointF(ImageRect.X, ImageRect.Y);
        }

        private Rectangle ScreenToVirtual(Rectangle screenRect)
        {
            PointF offset = GetScreenOffset();
            return new Rectangle(
                (int)((screenRect.X - offset.X) / _curZoom + 0.5f),
                (int)((screenRect.Y - offset.Y) / _curZoom + 0.5f),
                (int)(screenRect.Width / _curZoom + 0.5f),
                (int)(screenRect.Height / _curZoom + 0.5f));
        }

        private Rectangle VirtualToScreen(Rectangle virtualRect)
        {
            PointF offset = GetScreenOffset();
            return new Rectangle(
                (int)(virtualRect.X * _curZoom + offset.X + 0.5f),
                (int)(virtualRect.Y * _curZoom + offset.Y + 0.5f),
                (int)(virtualRect.Width * _curZoom + 0.5f),
                (int)(virtualRect.Height * _curZoom + 0.5f));
        }

        private PointF ScreenToVirtual(PointF screenPos)
        {
            PointF offset = GetScreenOffset();
            return new PointF(
                (screenPos.X - offset.X) / _curZoom,
                (screenPos.Y - offset.Y) / _curZoom);
        }

        private PointF VirtualToScreen(PointF virtualPos)
        {
            PointF offset = GetScreenOffset();
            return new PointF(
                virtualPos.X * _curZoom + offset.X,
                virtualPos.Y * _curZoom + offset.Y);
        }
        #endregion

        private void ImageViewCCtrl_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            FitImageToScreen();
        }

        public void ResetEntity()
        {
            _diagramEntityList.Clear();
            _rectangles.Clear();
            _selEntity = null;
            Invalidate();
        }
    }

    #region EventArgs
    public class DiagramEntityEventArgs : EventArgs
    {
        public EntityActionType ActionType { get; private set; }
        public InspWindow InspWindow { get; private set; }
        public InspWindowType WindowType { get; private set; }
        public List<InspWindow> InspWindowList { get; private set; }

        public OpenCvSharp.Rect Rect { get; private set; }

        public OpenCvSharp.Point OffsetMove { get; private set; }
        public DiagramEntityEventArgs(EntityActionType actionType, InspWindow inspWindow)
        {
            ActionType = actionType;
            InspWindow = inspWindow;
        }

        public DiagramEntityEventArgs(EntityActionType actionType, InspWindow inspWindow, InspWindowType windowType, Rectangle rect, Point offsetMove)
        {
            ActionType = actionType;
            InspWindow = inspWindow;
            WindowType = windowType;
            Rect = new OpenCvSharp.Rect(rect.X, rect.Y, rect.Width, rect.Height);
            OffsetMove = new OpenCvSharp.Point(offsetMove.X, offsetMove.Y);
        }

        public DiagramEntityEventArgs(EntityActionType actionType, List<InspWindow> inspWindowList, InspWindowType windowType = InspWindowType.None)
        {
            ActionType = actionType;
            InspWindow = null;
            InspWindowList = inspWindowList;
            WindowType = windowType;
        }
    }

    #endregion

}
