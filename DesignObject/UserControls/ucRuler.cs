using DesignObject;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace DesignObject.Controls
{
    public partial class Ruler : UserControl
    {
        const float _inch = 25.4f; //mm
        RulerType _type = RulerType.V;
        Paper _paper;
        Paper Paper
        {
            get
            {
                if (_paper == null)
                    _paper = new Paper(enPaperSizeType.A4);
                return _paper;
            }
            set
            {
                _paper = value;
            }
        }
        public float MinWidthScale(Graphics graphics)
        {
            switch (Paper.Unit)
            {
                case Unit.inch:
                    return (graphics.DpiX) * Paper.ZoomRatio;
                    break;
                case Unit.mm:
                default:
                    return (graphics.DpiX / _inch) * Paper.ZoomRatio;
                    break;
            }
        }
        public float MinHeightScale(Graphics graphics)
        {
            switch (Paper.Unit)
            {
                case Unit.inch:
                    return (graphics.DpiY) * Paper.ZoomRatio;
                    break;
                case Unit.mm:
                default:
                    return (graphics.DpiY / _inch) * Paper.ZoomRatio;
                    break;
            }
        }
        public Ruler()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            this.UpdateStyles();

            Load += Ruler_Load;
        }
        public void InitializeRuler(Paper paper)
        {
            Paper = paper;
        }
        public void InvalidateRuler(Paper paper)
        {
            Paper = paper;
            Invalidate();
        }
        private void Ruler_Load(object sender, EventArgs e)
        {
            if (this.Size.Width > this.Size.Height)
            {
                _type = RulerType.H;
            }
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            using (SolidBrush brush = new SolidBrush(Color.Black))
            using (Pen p = new Pen(Color.Black, 1f))
            {
                if (_type == RulerType.H)
                {
                    float point = Paper.Location.X * Paper.ZoomRatio; //dot per mm
                    float minWidthScale = MinWidthScale(e.Graphics);

                    var paperWidth = (Paper.PaperRect.Width * Paper.ZoomRatio) / minWidthScale;
                    float dot = 0;
                    while (dot < paperWidth + (5 * Paper.ZoomRatio))
                    {
                        int i = (int)dot;
                        if (dot % 1 == 0)
                        {
                            if (i % 5 != 0)
                                e.Graphics.DrawLine(p, new PointF(point, this.Height), new PointF(point, this.Height - 10));
                            else
                                e.Graphics.DrawLine(p, new PointF(point, this.Height), new PointF(point, this.Height - 20));

                            if (Paper.Unit == Unit.mm)
                            {
                                if (i % 10 == 0)
                                {
                                    e.Graphics.DrawString(i.ToString(), new Font("Arial", 8), brush, new PointF(point, this.Height - 35));
                                }
                            }
                            else
                            {
                                e.Graphics.DrawString(i.ToString(), new Font("Arial", 8), brush, new PointF(point, this.Height - 35));
                            }
                            point += minWidthScale;
                        }
                        dot = Convert.ToSingle(Math.Round(dot + 0.01f, 3));
                    }
                }
                else
                {
                    float point = (Paper.Location.Y) * Paper.ZoomRatio; //dot per mm
                    float minHeightScale = MinHeightScale(e.Graphics);

                    var paperHeight = (Paper.PaperRect.Height * Paper.ZoomRatio) / minHeightScale;
                    float dot = 0;
                    while (dot < paperHeight + (5 * Paper.ZoomRatio))
                    {
                        int i = (int)dot;
                        if (dot % 1 == 0)
                        {
                            if (i % 5 != 0)
                                e.Graphics.DrawLine(p, new PointF(this.Width - 10, point), new PointF(this.Height, point));
                            else
                                e.Graphics.DrawLine(p, new PointF(this.Width - 20, point), new PointF(this.Height, point));

                            if (Paper.Unit == Unit.mm)
                            {
                                if (i % 10 == 0)
                                {
                                    e.Graphics.DrawString(i.ToString(), new Font("Arial", 8), brush, new PointF(this.Width - 35, point));
                                }
                            }
                            else
                            {
                                e.Graphics.DrawString(i.ToString(), new Font("Arial", 8), brush, new PointF(this.Width - 35, point));
                            }

                            point += minHeightScale;
                        }
                        dot = Convert.ToSingle(Math.Round(dot + 0.01f, 3));
                    }
                }
            }
        }

        enum RulerType
        {
            H,
            V
        }
    }
}
