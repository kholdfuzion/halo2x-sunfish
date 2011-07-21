using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Sunfish.MetaEditor
{
    class ReflexiveValue
    {
        public Rectangle ControlRectangle;
        Color Border = SystemColors.ActiveBorder;
        Color SelectedBorder = SystemColors.Highlight;
        public string ValueName { get { return name; } set { name = value; } }
        public string Text { get { return text; } set { text = value; OnUpdate(ControlRectangle); } }
        Type ValueType;

        string text;
        string name = "Int Value Title";
        Timer blink;
        bool DrawCaret;
        public bool IsSelected;

        public ReflexiveValue(Reflexive rr)
        {
            blink = new Timer();
            blink.Tick += new EventHandler(blink_Tick);
            blink.Interval = 400;
        }

        public void PaintControl(Graphics g, Rectangle r)
        {
            using (Pen p = new Pen(IsSelected == true ? SelectedBorder : Border))
            {
                ControlRectangle = r;
                Point origin = r.Location;
                g.DrawRectangle(SystemPens.ActiveBorder, r);
                SizeF nameRec = g.MeasureString(ValueName, SystemFonts.DefaultFont);
                g.DrawString(ValueName, SystemFonts.DefaultFont, SystemBrushes.ActiveCaptionText, origin);
                Rectangle Input =  new Rectangle(new Point((int)nameRec.Width + 2, origin.Y + 2),new Size(96,  SystemFonts.DefaultFont.Height));
                //g.DrawRectangle(p, Input);
                g.DrawString(Text, SystemFonts.DefaultFont, SystemBrushes.WindowText, Input);
                if (DrawCaret)
                    g.DrawLine(SystemPens.ActiveBorder, new Point((int)nameRec.Width + 4, origin.Y + 4), new Point((int)nameRec.Width + 4, origin.Y + 19));
            }
        }

        public bool HitTest(Point p)
        {
            if (ControlRectangle.Contains(p)) return true;
            else return false;
        }

        public delegate void Update(Rectangle r);

        public event Update OnUpdate;

        internal void Select()
        {
            blink.Start();
            IsSelected = true;
            DrawCaret = true;
        }

        void blink_Tick(object sender, EventArgs e)
        {
            DrawCaret = !DrawCaret;
            OnUpdate(ControlRectangle);
        }
        internal void Unselect()
        {
            blink.Stop();
            IsSelected = false; 
            DrawCaret = false;
        }
    }

    class Reflexive : Control
    {
        public string ReflexiveName { get { return name; } set { name = value; Invalidate(); } }
        string name = "Reflexive Title";
        public string Description = @"This reflexive does some really cool things. 
            In fact you should probably know this information regarding the reflexive before you edit the values in it.";
        ReflexiveValue[] ints;

        public Reflexive()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.ContainerControl, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.Selectable, true);
            SetStyle(ControlStyles.UserPaint, true);
            ints = new ReflexiveValue[] { new ReflexiveValue(this), new ReflexiveValue(this), new ReflexiveValue(this), new ReflexiveValue(this) };
            foreach (ReflexiveValue rv in ints)
            {
                rv.OnUpdate += new ReflexiveValue.Update(rv_OnUpdate);
            }
        
        }

        void rv_OnUpdate(Rectangle r)
        {
            this.Invalidate(r);
            this.Update();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle titleRect = new Rectangle(new Point(0, 0), e.Graphics.MeasureString(ReflexiveName, SystemFonts.DefaultFont).ToSize());
            if (e.ClipRectangle.Contains(titleRect))
                e.Graphics.DrawString(ReflexiveName, SystemFonts.DefaultFont, SystemBrushes.ActiveCaptionText, new PointF(0, 0));
            SizeF size = e.Graphics.MeasureString(Description, SystemFonts.DefaultFont, Width);
            e.Graphics.DrawString(Description, SystemFonts.DefaultFont, SystemBrushes.ActiveCaptionText, new Rectangle(new Point(0, 12), size.ToSize()));
            int y = (int)size.Height + 12;
            e.Graphics.DrawRectangle(SystemPens.ActiveBorder, new Rectangle(0, 0, Width - 1, Height - 1));
            foreach (ReflexiveValue I in ints)
            {
                I.PaintControl(e.Graphics, new Rectangle(0, y, Width, 24));
                y += 24;
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            foreach (ReflexiveValue I in ints)
                if (I.IsSelected) { I.Text += (char)e.KeyValue; }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            foreach (ReflexiveValue I in ints)
                if (I.HitTest(e.Location)) { I.Select(); this.Invalidate(I.ControlRectangle); }
                else if (I.IsSelected) { I.Unselect(); this.Invalidate(I.ControlRectangle); }
            this.Update();
        }
    }
}
