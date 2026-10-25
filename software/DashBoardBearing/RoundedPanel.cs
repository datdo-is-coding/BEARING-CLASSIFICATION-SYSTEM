

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace DashBoardBearing
{
    public class RoundedPanel : Panel
    {
        // =========================================================
        // FIELDS
        // =========================================================

        private int borderSize = 0;
        private int borderRadius = 10;
        private Color borderColor = Color.PaleVioletRed;


        // =========================================================
        // PROPERTIES
        // =========================================================

        [Category("RJ Code Advance")]
        [DefaultValue(0)]
        public int BorderSize
        {
            get
            {
                return borderSize;
            }
            set
            {
                borderSize = Math.Max(0, value);

                if (borderRadius < borderSize)
                    borderRadius = borderSize;

                Invalidate();
            }
        }


        [Category("RJ Code Advance")]
        [DefaultValue(10)]
        public int BorderRadius
        {
            get
            {
                return borderRadius;
            }
            set
            {
                borderRadius = Math.Max(0, value);

                UpdateRegion();
                Invalidate();
            }
        }


        [Category("RJ Code Advance")]
        public Color BorderColor
        {
            get
            {
                return borderColor;
            }
            set
            {
                borderColor = value;
                Invalidate();
            }
        }


        [Category("RJ Code Advance")]
        public Color BackgroundColor
        {
            get
            {
                return BackColor;
            }
            set
            {
                BackColor = value;
                Invalidate();
            }
        }


        // =========================================================
        // CONSTRUCTOR
        // =========================================================

        public RoundedPanel()
        {
            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw,
                true
            );

            DoubleBuffered = true;

            BackColor = Color.White;

            Resize += RJPanel_Resize;
        }


        // =========================================================
        // CREATE ROUNDED PATH
        // =========================================================

        private GraphicsPath GetFigurePath(
            Rectangle rect,
            int radius)
        {
            GraphicsPath path = new GraphicsPath();

            if (radius <= 0)
            {
                path.AddRectangle(rect);
                return path;
            }

            float curveSize = radius * 2F;

            path.StartFigure();

            // TOP LEFT
            path.AddArc(
                rect.X,
                rect.Y,
                curveSize,
                curveSize,
                180,
                90
            );

            // TOP RIGHT
            path.AddArc(
                rect.Right - curveSize,
                rect.Y,
                curveSize,
                curveSize,
                270,
                90
            );

            // BOTTOM RIGHT
            path.AddArc(
                rect.Right - curveSize,
                rect.Bottom - curveSize,
                curveSize,
                curveSize,
                0,
                90
            );

            // BOTTOM LEFT
            path.AddArc(
                rect.X,
                rect.Bottom - curveSize,
                curveSize,
                curveSize,
                90,
                90
            );

            path.CloseFigure();

            return path;
        }


        // =========================================================
        // PAINT
        // =========================================================

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics graphics = e.Graphics;

            graphics.SmoothingMode =
                SmoothingMode.AntiAlias;

            graphics.PixelOffsetMode =
                PixelOffsetMode.HighQuality;


            // =====================================================
            // SURFACE
            // =====================================================

            Rectangle rectSurface =
                ClientRectangle;


            // =====================================================
            // BORDER RECTANGLE
            // =====================================================

            Rectangle rectBorder =
                Rectangle.Inflate(
                    rectSurface,
                    -borderSize,
                    -borderSize
                );


            // =====================================================
            // SMOOTH SIZE
            // =====================================================

            int smoothSize = 2;

            if (borderSize > 0)
                smoothSize = borderSize;


            // =====================================================
            // ROUNDED PANEL
            // =====================================================

            if (borderRadius > 2)
            {
                int surfaceRadius =
                    borderRadius;

                int borderRadiusInside =
                    borderRadius - borderSize;


                if (borderRadiusInside < 1)
                    borderRadiusInside = 1;


                using (
                    GraphicsPath pathSurface =
                        GetFigurePath(
                            rectSurface,
                            surfaceRadius))
                using (
                    GraphicsPath pathBorder =
                        GetFigurePath(
                            rectBorder,
                            borderRadiusInside))
                using (
                    Pen penSurface =
                        new Pen(
                            Parent != null
                                ? Parent.BackColor
                                : BackColor,
                            smoothSize))
                using (
                    Pen penBorder =
                        new Pen(
                            borderColor,
                            borderSize))
                {
                    // =================================================
                    // CLIP ROUNDED REGION
                    // =================================================

                    Region = new Region(pathSurface);


                    // =================================================
                    // SURFACE SMOOTHING
                    // =================================================

                    graphics.DrawPath(
                        penSurface,
                        pathSurface
                    );


                    // =================================================
                    // BORDER
                    // =================================================

                    if (borderSize >= 1)
                    {
                        graphics.DrawPath(
                            penBorder,
                            pathBorder
                        );
                    }
                }
            }
            else
            {
                // =====================================================
                // NORMAL RECTANGLE
                // =====================================================

                graphics.SmoothingMode =
                    SmoothingMode.None;

                Region = new Region(
                    rectSurface
                );


                if (borderSize >= 1)
                {
                    using (
                        Pen penBorder =
                            new Pen(
                                borderColor,
                                borderSize))
                    {
                        penBorder.Alignment =
                            PenAlignment.Inset;

                        graphics.DrawRectangle(
                            penBorder,
                            0,
                            0,
                            Width - 1,
                            Height - 1
                        );
                    }
                }
            }
        }


        // =========================================================
        // HANDLE CREATED
        // =========================================================

        protected override void OnHandleCreated(
            EventArgs e)
        {
            base.OnHandleCreated(e);

            if (Parent != null)
            {
                Parent.BackColorChanged +=
                    Container_BackColorChanged;
            }

            UpdateRegion();
        }


        // =========================================================
        // PARENT BACKGROUND CHANGED
        // =========================================================

        private void Container_BackColorChanged(
            object sender,
            EventArgs e)
        {
            Invalidate();
        }


        // =========================================================
        // RESIZE
        // =========================================================

        private void RJPanel_Resize(
            object sender,
            EventArgs e)
        {
            if (Height <= 0 || Width <= 0)
                return;


            // Không cho radius lớn hơn panel
            int maxRadius =
                Math.Min(
                    Width,
                    Height
                ) / 2;


            if (borderRadius > maxRadius)
            {
                borderRadius =
                    maxRadius;
            }


            UpdateRegion();

            Invalidate();
        }


        // =========================================================
        // UPDATE REGION
        // =========================================================

        private void UpdateRegion()
        {
            if (Width <= 0 || Height <= 0)
                return;


            int radius =
                Math.Min(
                    borderRadius,
                    Math.Min(
                        Width,
                        Height
                    ) / 2
                );


            using (
                GraphicsPath path =
                    GetFigurePath(
                        ClientRectangle,
                        radius))
            {
                Region newRegion =
                    new Region(path);

                Region oldRegion =
                    Region;

                Region =
                    newRegion;

                if (oldRegion != null)
                {
                    oldRegion.Dispose();
                }
            }
        }


        // =========================================================
        // HANDLE DESTROY
        // =========================================================

        protected override void Dispose(
            bool disposing)
        {
            if (disposing)
            {
                if (Parent != null)
                {
                    Parent.BackColorChanged -=
                        Container_BackColorChanged;
                }
            }

            base.Dispose(disposing);
        }
    }
}
