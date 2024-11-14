using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab6
{
    public class Verge
    {
        public List<XYZPoint> Points { get; }
        public XYZPoint Center { get; set; } = new XYZPoint(0, 0, 0);
        public List<float> Normal { get; set; }
        public bool IsVisible { get; set; }
        public Verge(Verge face)
        {
            Points = face.Points.Select(pt => new XYZPoint(pt.X, pt.Y, pt.Z)).ToList();
            Center = new XYZPoint(face.Center);
            if (Normal != null)
                Normal = new List<float>(face.Normal);
            IsVisible = face.IsVisible;
        }

        public Verge(List<XYZPoint> pts = null)
        {
            if (pts != null)
            {
                Points = new List<XYZPoint>(pts);
                find_center();
            }
        }

        private void find_center()
        {
            Center.X = 0;
            Center.Y = 0;
            Center.Z = 0;
            foreach (XYZPoint p in Points)
            {
                Center.X += p.X;
                Center.Y += p.Y;
                Center.Z += p.Z;
            }
            Center.X /= Points.Count;
            Center.Y /= Points.Count;
            Center.Z /= Points.Count;
        }

        public void find_normal(XYZPoint p_center, Line camera)
        {
            XYZPoint Q = Points[1], R = Points[2], S = Points[0];
            List<float> QR = new List<float> { R.X - Q.X, R.Y - Q.Y, R.Z - Q.Z };
            List<float> QS = new List<float> { S.X - Q.X, S.Y - Q.Y, S.Z - Q.Z };


            Normal = new List<float> { QR[1] * QS[2] - QR[2] * QS[1],
                                       -(QR[0] * QS[2] - QR[2] * QS[0]),
                                       QR[0] * QS[1] - QR[1] * QS[0] };

            List<float> CQ = new List<float> { Q.X - p_center.X, Q.Y - p_center.Y, Q.Z - p_center.Z };
            if (XYZPoint.mul_matrix(Normal, 1, 3, CQ, 3, 1)[0] > 1E-6)
            {
                Normal[0] *= -1;
                Normal[1] *= -1;
                Normal[2] *= -1;
            }

            XYZPoint E = camera.First;
            List<float> CE = new List<float> { E.X - Center.X, E.Y - Center.Y, E.Z - Center.Z };
            float dot_product = XYZPoint.mul_matrix(Normal, 1, 3, CE, 3, 1)[0];
            IsVisible = Math.Abs(dot_product) < 1E-6 || dot_product < 0;
        }

        public void reflectX()
        {
            Center.X = -Center.X;
            if (Points != null)
                foreach (var p in Points)
                    p.reflectX();
        }
        public void reflectY()
        {
            Center.Y = -Center.Y;
            if (Points != null)
                foreach (var p in Points)
                    p.reflectY();
        }
        public void reflectZ()
        {
            Center.Z = -Center.Z;
            if (Points != null)
                foreach (var p in Points)
                    p.reflectZ();
        }

        public List<PointF> make_perspective(float k = 1000, float z_camera = 1000)
        {
            List<PointF> res = new List<PointF>();

            foreach (XYZPoint p in Points)
            {
                res.Add(p.make_perspective(k));
            }
            return res;
        }

        public List<PointF> make_isometric()
        {
            List<PointF> res = new List<PointF>();

            foreach (XYZPoint p in Points)
                res.Add(p.make_axonometric());

            return res;
        }
        List<Color> colors = new List<Color>() { Color.Brown, Color.DarkSeaGreen, Color.Green, Color.Coral, Color.DarkOrange, Color.DarkGray, Color.Purple, Color.IndianRed, Color.YellowGreen, Color.Violet, Color.HotPink, Color.DarkOrchid, Color.OrangeRed, Color.DarkViolet, Color.Pink, Color.MediumVioletRed, Color.DarkKhaki, Color.Red, Color.Plum, Color.DarkOrange, Color.DarkSalmon };
        public void Draw(Graphics g, int count, Projection pr = 0, Pen pen = null, Line camera = null, float k = 1000)
        {
            if (pen == null)
                pen = Pens.Black;
            Pen br = new Pen(colors[count]);
            List<PointF> pts;

            if (pr == Projection.AXONOMETRIC)
                pts = make_isometric();
            else if (camera != null)
                pts = make_perspective(k, camera.First.Z);
            else
                pts = make_perspective(k);

            if (pts.Count > 1)
            {
                g.DrawLines(br, pts.ToArray());
                g.DrawLine(br, pts[0], pts[pts.Count - 1]);
            }
            else if (pts.Count == 1)
                g.DrawRectangle(br, pts[0].X, pts[0].Y, 1, 1);
        }

        public void translate(float x, float y, float z)
        {
            foreach (XYZPoint p in Points)
                p.translate(x, y, z);
            find_center();
        }

        public void rotate(double angle, Axis a, Line line = null)
        {
            foreach (XYZPoint p in Points)
                p.rotate(angle, a, line);
            find_center();
        }

        public void scale(float kx, float ky, float kz)
        {
            foreach (XYZPoint p in Points)
                p.scale(kx, ky, kz);
            find_center();
        }
    }
}
