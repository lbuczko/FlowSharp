﻿/* 
* Copyright (c) Marc Clifton
* The Code Project Open License (CPOL) 1.02
* http://www.codeproject.com/info/cpol10.aspx
*/

using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using Clifton.Core.ExtensionMethods;

using FlowSharpLib;

namespace FlowSharpWindowsControlShapes
{
    [ExcludeFromToolbox]
    public class TextboxShape : ControlShape
    {
        public bool Multiline { get; set; }

        public TextboxShape(Canvas canvas) : base(canvas)
        {
            control = new TextBox();
            canvas.Controls.Add(control);
        }

        public override ElementProperties CreateProperties()
        {
            return new TextboxShapeProperties(this);
        }

        public override void Serialize(ElementPropertyBag epb, IEnumerable<GraphicElement> elementsBeingSerialized)
        {
            Json["Multiline"] = Multiline.ToString();
            base.Serialize(epb, elementsBeingSerialized);
        }

        public override void Deserialize(ElementPropertyBag epb)
        {
            base.Deserialize(epb);
            string multiline;

            if (Json.TryGetValue("Multiline", out multiline))
            {
                Multiline = multiline.to_b();
            }
        }

        public override void Draw(Graphics gr)
        {
            base.Draw(gr);
            Rectangle r = DisplayRectangle.Grow(-4);
            control.Location = r.Location;
            control.Size = r.Size;
            control.Text = Text;
            control.Font = TextFont;
            ((TextBox)control).Multiline = Multiline;
        }
    }

    [ToolboxShape]
    public class ToolboxTextboxShape : GraphicElement
    {
        public const string TOOLBOX_TEXT = "txtbox";

        protected Brush brush = new SolidBrush(Color.Black);

        public ToolboxTextboxShape(Canvas canvas) : base(canvas)
        {
            TextFont.Dispose();
            TextFont = new Font(FontFamily.GenericSansSerif, 8);
        }

        public override GraphicElement CloneDefault(Canvas canvas)
        {
            return CloneDefault(canvas, Point.Empty);
        }

        public override GraphicElement CloneDefault(Canvas canvas, Point offset)
        {
            TextboxShape shape = new TextboxShape(canvas);
            shape.DisplayRectangle = shape.DefaultRectangle().Move(offset);
            shape.UpdateProperties();
            shape.UpdatePath();

            return shape;
        }

        public override void Draw(Graphics gr)
        {
            SizeF size = gr.MeasureString(TOOLBOX_TEXT, TextFont);
            Point textpos = DisplayRectangle.Center().Move((int)(-size.Width / 2), (int)(-size.Height / 2));
            gr.DrawString(TOOLBOX_TEXT, TextFont, brush, textpos);
            base.Draw(gr);
        }
    }
}
