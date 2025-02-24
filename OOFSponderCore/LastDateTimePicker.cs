using System;
using System.Drawing;
using System.Windows.Forms;

namespace OOFScheduling
{
    //extend existing class so we can store the old value
    internal class LastDateTimePicker : DateTimePicker
    {
        public LastDateTimePicker()
        {
            //this.SetStyle(ControlStyles.UserPaint, true);
        }

        //hooking into this allows us to preserve the old value
        protected override void OnValueChanged(EventArgs eventargs)
        {
            base.OnValueChanged(eventargs);

            LastValue = Value;

        }

        public DateTime LastValue { get; private set; }

        public new DateTime Value
        {
            get { return base.Value; }
            set
            {
                base.Value = value;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {

            //int udWidth;
            //// Find the up-down control within the DateTimePicker
            //foreach (Control control in this.Controls)
            //{
            //    if (control is UpDownBase upDown)
            //    {
            //        // Adjust the width of the up-down control
            //        udWidth = upDown.Width; // Set your desired width here
            //        break;
            //    }
            //}

            Graphics g = e.Graphics;
            Rectangle rect = this.ClientRectangle;

            // Draw the background
            //g.FillRectangle(new SolidBrush(this.BackColor), rect);
            //g.FillRectangle(new SolidBrush(System.Drawing.ColorTranslator.FromHtml("#F0F0F0")), rect);

            // Draw the text
            string text = this.Text;
            // Create a StringFormat object to specify text alignment
            StringFormat stringFormat = new StringFormat
            {
                Alignment = StringAlignment.Center, // Center horizontally
                LineAlignment = StringAlignment.Center // Center vertically
            };
            using (Font font = new Font(this.Font.FontFamily, this.Font.Size, this.Font.Style))
            {
                SizeF textSize = g.MeasureString(text, font);
                PointF location = new PointF((float)(rect.Width - textSize.Width * 1.35) / 2, (rect.Height - textSize.Height) / 2);
                g.DrawString(text, font, new SolidBrush(this.ForeColor), location);
                //g.DrawString(text, font, new SolidBrush(this.ForeColor), rect, stringFormat);
            }


            // Draw the drop-down button
            Rectangle buttonRect = new Rectangle(rect.Width - 17, 0, 17, rect.Height);
            ControlPaint.DrawComboButton(g, buttonRect, ButtonState.Normal);
        }
    }
}
