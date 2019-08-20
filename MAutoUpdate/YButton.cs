using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace MAutoUpdate.Control
{
    public partial class YButton : Label
    {
        private Boolean isColorChange = true;
        private Boolean isFontChange = false;

        public Boolean IsFontChange
        {
            get { return isFontChange; }
            set { isFontChange = value; }
        }

        public Boolean IsColorChange
        {
            get { return isColorChange; }
            set { isColorChange = value; }
        }
        private Color normalColor = Color.FromArgb(56, 95, 170);

        public Color NormalColor
        {
            get { return normalColor; }
            set { normalColor = value; }
        }
        private Color moveColor = Color.FromArgb(128, 156, 211);

        public Color MoveColor
        {
            get { return moveColor; }
            set { moveColor = value; }
        }
        private Color moveFontColor = Color.FromArgb(128, 156, 211);

        public Color MoveFontColor
        {
            get { return moveFontColor; }
            set { moveFontColor = value; }
        }
        private Color normalFontColor = Color.FromArgb(128, 156, 211);

        public Color NormalFontColor
        {
            get { return normalFontColor; }
            set { normalFontColor = value; }
        }
        public Image EnterImage { get; set; }

        public YButton()
            : base()
        {
            this.Size = new Size(61, 23);
            this.ForeColor = Color.FromArgb(240, 240, 240);
            this.BackColor = normalColor;
            this.TextAlign = ContentAlignment.MiddleCenter;
            this.AutoSize = false;
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            if (isColorChange)
            {
                this.BackColor = moveColor;
            }
            if (isFontChange)
            {
                this.ForeColor =this.moveFontColor;
            }

            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            if (isColorChange)
            {
                this.BackColor = normalColor;
            }
            if (isFontChange)
            {
                this.ForeColor = this.normalFontColor;
            }
            base.OnMouseLeave(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

        }

    }
}
