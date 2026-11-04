using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using FontAwesome.Sharp;

namespace DashBoardBearing
{
    public static class UITheme
    {
        // ==========================================
        // THEME MODE (Default: Light Mode as requested)
        // ==========================================
        public static bool IsDarkMode { get; set; } = false;

        public static event Action? ThemeChanged;

        public static void ToggleTheme()
        {
            IsDarkMode = !IsDarkMode;
            ThemeChanged?.Invoke();
        }

        public static void SetTheme(bool darkMode)
        {
            if (IsDarkMode != darkMode)
            {
                IsDarkMode = darkMode;
                ThemeChanged?.Invoke();
            }
        }

        // ==========================================
        // DYNAMIC COLOR PROPERTIES
        // ==========================================
        // Backgrounds
        public static Color BgDark => IsDarkMode ? Color.FromArgb(15, 23, 42) : Color.FromArgb(241, 245, 249);       // Dark: Slate 900 | Light: Slate 100
        public static Color BgSidebar => IsDarkMode ? Color.FromArgb(11, 15, 25) : Color.FromArgb(255, 255, 255);    // Dark: Deep Slate | Light: Pure White
        public static Color BgCard => IsDarkMode ? Color.FromArgb(24, 34, 53) : Color.FromArgb(255, 255, 255);       // Dark: Slate 850 | Light: White Card
        public static Color BgCardAlt => IsDarkMode ? Color.FromArgb(30, 41, 59) : Color.FromArgb(248, 250, 252);    // Dark: Slate 800 | Light: Slate 50
        public static Color BgInput => IsDarkMode ? Color.FromArgb(15, 23, 42) : Color.FromArgb(255, 255, 255);      // Input fields
        public static Color BgViewport => IsDarkMode ? Color.FromArgb(10, 15, 26) : Color.FromArgb(15, 23, 42);      // Camera / 3D viewport

        // Borders
        public static Color BorderCard => IsDarkMode ? Color.FromArgb(41, 53, 76) : Color.FromArgb(226, 232, 240);   // Dark: Slate border | Light: Slate 200
        public static Color BorderHighlight => Color.FromArgb(59, 130, 246);

        // Accents
        public static Color Primary => Color.FromArgb(37, 99, 235);         // Royal Blue
        public static Color PrimaryHover => Color.FromArgb(29, 78, 216);
        public static Color PrimaryLight => IsDarkMode ? Color.FromArgb(59, 130, 246) : Color.FromArgb(37, 99, 235);
        public static Color Success => Color.FromArgb(16, 185, 129);        // Emerald Green
        public static Color SuccessBg => IsDarkMode ? Color.FromArgb(6, 78, 59) : Color.FromArgb(220, 252, 231);    // Dark green / Light green
        public static Color Danger => Color.FromArgb(239, 68, 68);          // Vibrant Red
        public static Color DangerBg => IsDarkMode ? Color.FromArgb(127, 29, 29) : Color.FromArgb(254, 226, 226);    // Dark red / Light red
        public static Color Warning => IsDarkMode ? Color.FromArgb(245, 158, 11) : Color.FromArgb(217, 119, 6);     // Amber
        public static Color Info => IsDarkMode ? Color.FromArgb(6, 182, 212) : Color.FromArgb(2, 132, 199);         // Cyan / Sky
        public static Color Purple => Color.FromArgb(139, 92, 246);

        // Typography Colors
        public static Color TextPrimary => IsDarkMode ? Color.FromArgb(248, 250, 252) : Color.FromArgb(15, 23, 42); // Dark: White | Light: Deep Slate
        public static Color TextSecondary => IsDarkMode ? Color.FromArgb(148, 163, 184) : Color.FromArgb(71, 85, 105);// Dark: Muted | Light: Slate 600
        public static Color TextMuted => IsDarkMode ? Color.FromArgb(100, 116, 139) : Color.FromArgb(148, 163, 184); // Slate 400

        // ==========================================
        // FONTS
        // ==========================================
        public static readonly Font FontAppTitle = new Font("Segoe UI", 12F, FontStyle.Bold);
        public static readonly Font FontAppSub = new Font("Segoe UI", 8F, FontStyle.Bold);
        public static readonly Font FontHeader = new Font("Segoe UI", 13F, FontStyle.Bold);
        public static readonly Font FontCardTitle = new Font("Segoe UI", 10.5F, FontStyle.Bold);
        public static readonly Font FontBody = new Font("Segoe UI", 9.5F, FontStyle.Regular);
        public static readonly Font FontBodyBold = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        public static readonly Font FontSmall = new Font("Segoe UI", 8.25F, FontStyle.Regular);
        public static readonly Font FontSmallBold = new Font("Segoe UI", 8.25F, FontStyle.Bold);
        public static readonly Font FontStatNum = new Font("Segoe UI", 20F, FontStyle.Bold);
        public static readonly Font FontStatusHuge = new Font("Segoe UI", 22F, FontStyle.Bold);
        public static readonly Font FontConsole = new Font("Consolas", 9.25F, FontStyle.Regular);

        // ==========================================
        // STYLING HELPERS
        // ==========================================
        public static void StylePrimaryButton(IconButton btn, string text, IconChar icon, int iconSize = 18)
        {
            btn.Text = "  " + text;
            btn.IconChar = icon;
            btn.IconColor = Color.White;
            btn.IconSize = iconSize;
            btn.Font = FontBodyBold;
            btn.BackColor = Primary;
            btn.ForeColor = Color.White;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Cursor = Cursors.Hand;
            btn.TextImageRelation = TextImageRelation.ImageBeforeText;
            btn.ImageAlign = ContentAlignment.MiddleCenter;
            btn.TextAlign = ContentAlignment.MiddleCenter;
        }

        public static void StyleSecondaryButton(IconButton btn, string text, IconChar icon, int iconSize = 18)
        {
            btn.Text = "  " + text;
            btn.IconChar = icon;
            btn.IconColor = TextPrimary;
            btn.IconSize = iconSize;
            btn.Font = FontBody;
            btn.BackColor = BgCardAlt;
            btn.ForeColor = TextPrimary;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderColor = BorderCard;
            btn.FlatAppearance.BorderSize = 1;
            btn.Cursor = Cursors.Hand;
            btn.TextImageRelation = TextImageRelation.ImageBeforeText;
            btn.ImageAlign = ContentAlignment.MiddleCenter;
            btn.TextAlign = ContentAlignment.MiddleCenter;
        }

        public static void StyleDangerButton(IconButton btn, string text, IconChar icon, int iconSize = 18)
        {
            btn.Text = "  " + text;
            btn.IconChar = icon;
            btn.IconColor = Color.White;
            btn.IconSize = iconSize;
            btn.Font = FontBodyBold;
            btn.BackColor = Danger;
            btn.ForeColor = Color.White;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Cursor = Cursors.Hand;
            btn.TextImageRelation = TextImageRelation.ImageBeforeText;
            btn.ImageAlign = ContentAlignment.MiddleCenter;
            btn.TextAlign = ContentAlignment.MiddleCenter;
        }

        public static void StyleStandardButton(Button btn, Color bg, Color fg)
        {
            btn.BackColor = bg;
            btn.ForeColor = fg;
            btn.Font = FontBodyBold;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Cursor = Cursors.Hand;
        }

        public static void StyleComboBox(ComboBox cb)
        {
            cb.BackColor = BgInput;
            cb.ForeColor = TextPrimary;
            cb.Font = FontBody;
            cb.FlatStyle = FlatStyle.Flat;
        }

        public static void StyleTextBox(TextBox tb)
        {
            tb.BackColor = BgInput;
            tb.ForeColor = TextPrimary;
            tb.Font = FontBody;
            tb.BorderStyle = BorderStyle.FixedSingle;
        }

        public static GraphicsPath GetRoundedRectangle(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            if (radius <= 0)
            {
                path.AddRectangle(rect);
                return path;
            }

            int d = radius * 2;
            path.AddArc(rect.X, rect.Y, d, d, 180, 90);
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}
