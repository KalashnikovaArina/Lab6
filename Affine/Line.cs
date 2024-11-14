using System;
using System.Collections.Generic;
using System.Drawing;

namespace Lab6
{
    public class Line
    {
        public XYZPoint First { get; set; }
        public XYZPoint Second { get; set; }

        public Line(XYZPoint p1, XYZPoint p2)
        {
            First = new XYZPoint(p1);
            Second = new XYZPoint(p2);
        }

        private List<PointF> make_perspective(int k = 1000)
        {
            List<PointF> res = new List<PointF>
            {
                First.make_perspective(k),
                Second.make_perspective(k)
            };

            return res;
        }

        private List<PointF> make_isometric()
        {
            List<PointF> res = new List<PointF>
            {
                First.make_axonometric(),
                Second.make_axonometric()
            };
            return res;
        }

        public void Draw(Graphics g, Projection pr = 0, Pen pen = null)
        {
            if (pen == null)
                pen = Pens.Black;

            List<PointF> pts;
            if (pr == Projection.AXONOMETRIC)
                pts = make_isometric();
            else
                pts = make_perspective();

            g.DrawLine(pen, pts[0], pts[pts.Count - 1]);
        }

        public void translate(float x, float y, float z)
        {
            First.translate(x, y, z);
            Second.translate(x, y, z);
        }

        public void rotate(double angle, Axis a, Line line = null)
        {
            First.rotate(angle, a, line);
            Second.rotate(angle, a, line);
        }

        public void scale(float kx, float ky, float kz)
        {
            First.scale(kx, ky, kz);
            Second.scale(kx, ky, kz);
        }
    }
}
