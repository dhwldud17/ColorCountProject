using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using JidamVision.Algorithm;
using JidamVision.Core;
using JidamVision.Teach;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using static System.Windows.Forms.MonthCalendar;

namespace JidamVision.Property
{
    public partial class MatchInspProp : UserControl
    {
        MatchAlgorithm _matchAlgo = null;
        public MatchInspProp()
        {
            InitializeComponent();
        }

        public void SetAlgorithm(MatchAlgorithm matchAlgo)
        {
            _matchAlgo = matchAlgo;
            SetProperty();
        }

        public void SetProperty()
        {
            Mat teachImage = _matchAlgo.GetTemplateImage();
            if (teachImage != null)
            {
                Bitmap bmpImage = BitmapConverter.ToBitmap(teachImage);
                picTeachImage.Image = bmpImage;
            }
        }
    }
}
