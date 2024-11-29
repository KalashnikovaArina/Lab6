using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Lab6
{
    class Polyhedron
    {
        public List<Verge> Verges { get; set; } = null;
        public XYZPoint Center { get; set; } = new XYZPoint(0, 0, 0);

        public string cur_fig = "";
        public Polyhedron(List<Verge> fs = null)
        {
            if (fs != null)
            {
                Verges = fs.Select(face => new Verge(face)).ToList();
                find_center();
            }
        }

        public Polyhedron(Polyhedron polyhedron)
        {
            Verges = polyhedron.Verges.Select(face => new Verge(face)).ToList();
            Center = new XYZPoint(polyhedron.Center);
        }

        private void find_center()
        {
            Center.X = 0;
            Center.Y = 0;
            Center.Z = 0;
            foreach (Verge f in Verges)
            {
                Center.X += f.Center.X;
                Center.Y += f.Center.Y;
                Center.Z += f.Center.Z;
            }
            Center.X /= Verges.Count;
            Center.Y /= Verges.Count;
            Center.Z /= Verges.Count;
        }
                
        public void Draw(Graphics g, Projection pr = 0, Pen pen = null)
        {
            //коорд оси
            List<Line> p = new List<Line>();
            XYZPoint a = new XYZPoint(0, 0, 0);
            XYZPoint b = new XYZPoint(100, 0, 0);
            b.Draw(g, pr);
            XYZPoint c = new XYZPoint(0, 100, 0);
            c.Draw(g, pr);
            XYZPoint d = new XYZPoint(0, 0, 100);
            d.Draw(g, pr);

            p.Add(new Line(a, b));
            p.Add(new Line(a, c));
            p.Add(new Line(a, d)); 


            foreach (Line x in p)
            {
                x.Draw(g, pr);
            }

            //многоугольник
            int count = -1;
            foreach (Verge f in Verges)
            {
                count++;
                f.Draw(g, count, pr, pen);
            }
        }

        public void translate(float x, float y, float z)
        {
            foreach (Verge f in Verges)
                f.translate(x, y, z);
            find_center();
        }

        public void rotate(double angle, Axis a, Line line = null)
        {
            foreach (Verge f in Verges)
                f.rotate(angle, a, line);
            find_center();
        }

        public void scale(float kx, float ky, float kz)
        {
            foreach (Verge f in Verges)
                f.scale(kx, ky, kz);
            find_center();
        }

        public void reflectX()
        {
            if (Verges != null)
                foreach (var f in Verges)
                    f.reflectX();
            find_center();
        }

        public void reflectY()
        {
            if (Verges != null)
                foreach (var f in Verges)
                    f.reflectY();
            find_center();
        }

        public void reflectZ()
        {
            if (Verges != null)
                foreach (var f in Verges)
                    f.reflectZ();
            find_center();
        }

        public void make_hexahedron(float cube_half_size = 50)
        {
            Verge f = new Verge(
                new List<XYZPoint>
                {
                    new XYZPoint(-cube_half_size, cube_half_size, cube_half_size),
                    new XYZPoint(cube_half_size, cube_half_size, cube_half_size),
                    new XYZPoint(cube_half_size, -cube_half_size, cube_half_size),
                    new XYZPoint(-cube_half_size, -cube_half_size, cube_half_size)
                }
            );


            Verges = new List<Verge> { f };

            List<XYZPoint> l1 = new List<XYZPoint>();
            foreach (var point in f.Points)
            {
                l1.Add(new XYZPoint(point.X, point.Y, point.Z - 2 * cube_half_size));
            }
            Verge f1 = new Verge(
                    new List<XYZPoint>
                    {
                        new XYZPoint(-cube_half_size, cube_half_size, -cube_half_size),
                        new XYZPoint(-cube_half_size, -cube_half_size, -cube_half_size),
                        new XYZPoint(cube_half_size, -cube_half_size, -cube_half_size),
                        new XYZPoint(cube_half_size, cube_half_size, -cube_half_size)
                    });

            Verges.Add(f1);

            List<XYZPoint> l2 = new List<XYZPoint>
            {
                new XYZPoint(f.Points[2]),
                new XYZPoint(f1.Points[2]),
                new XYZPoint(f1.Points[1]),
                new XYZPoint(f.Points[3]),
            };
            Verge f2 = new Verge(l2);
            Verges.Add(f2);

            List<XYZPoint> l3 = new List<XYZPoint>
            {
                new XYZPoint(f1.Points[0]),
                new XYZPoint(f1.Points[3]),
                new XYZPoint(f.Points[1]),
                new XYZPoint(f.Points[0]),
            };
            Verge f3 = new Verge(l3);
            Verges.Add(f3);

            List<XYZPoint> l4 = new List<XYZPoint>
            {
                new XYZPoint(f1.Points[0]),
                new XYZPoint(f.Points[0]),
                new XYZPoint(f.Points[3]),
                new XYZPoint(f1.Points[1])
            };
            Verge f4 = new Verge(l4);
            Verges.Add(f4);

            List<XYZPoint> l5 = new List<XYZPoint>
            {
                new XYZPoint(f1.Points[3]),
                new XYZPoint(f1.Points[2]),
                new XYZPoint(f.Points[2]),
                new XYZPoint(f.Points[1])
            };
            Verge f5 = new Verge(l5);
            Verges.Add(f5);
            cur_fig = "Hexahedron";
            find_center();
        }
        public void make_tetrahedron(float size = 50)
        {
            // Задаём вершины тетраэдра
            XYZPoint p1 = new XYZPoint(0, size, 0);                      // Верхняя вершина
            XYZPoint p2 = new XYZPoint(-size, -size / 2, -size * (float)Math.Sqrt(3) / 2); // Левая вершина
            XYZPoint p3 = new XYZPoint(size, -size / 2, -size * (float)Math.Sqrt(3) / 2);  // Правая вершина
            XYZPoint p4 = new XYZPoint(0, -size / 2, size * (float)Math.Sqrt(3));          // Нижняя вершина

            Verges = new List<Verge>
            {
            new Verge(new List<XYZPoint> { p1, p2, p3 }), // Верхняя грань
            new Verge(new List<XYZPoint> { p1, p3, p4 }), // Правая грань
            new Verge(new List<XYZPoint> { p1, p4, p2 }), // Левая грань
            new Verge(new List<XYZPoint> { p2, p4, p3 })  // Нижняя грань
            };
            cur_fig = "Tetrahedron";
            find_center();
        }

        public void make_icosahedron()
        {
            Verges = new List<Verge>();

            float size = 100;

            float r1 = size * (float)Math.Sqrt(3) / 4;   // половина высоты правильного треугольника - для высоты цилиндра
            float r = size * (3 + (float)Math.Sqrt(5)) / (4 * (float)Math.Sqrt(3)); // радиус вписанной сферы - для правильных пятиугольников

            XYZPoint up_center = new XYZPoint(0, -r1, 0);  // центр верхней окружности
            XYZPoint down_center = new XYZPoint(0, r1, 0); // центр нижней окружности

            double a = Math.PI / 2;
            List<XYZPoint> up_points = new List<XYZPoint>();
            for (int i = 0; i < 5; ++i)
            {
                up_points.Add(new XYZPoint(up_center.X + r * (float)Math.Cos(a), up_center.Y, up_center.Z - r * (float)Math.Sin(a)));
                a += 2 * Math.PI / 5;
            }

            a = Math.PI / 2 - Math.PI / 5;
            List<XYZPoint> down_points = new List<XYZPoint>();
            for (int i = 0; i < 5; ++i)
            {
                down_points.Add(new XYZPoint(down_center.X + r * (float)Math.Cos(a), down_center.Y, down_center.Z - r * (float)Math.Sin(a)));
                a += 2 * Math.PI / 5;
            }

            var R = Math.Sqrt(2 * (5 + Math.Sqrt(5))) * size / 4;

            XYZPoint p_up = new XYZPoint(up_center.X, (float)(-R), up_center.Z);
            XYZPoint p_down = new XYZPoint(down_center.X, (float)R, down_center.Z);

            for (int i = 0; i < 5; ++i)
            {
                Verges.Add(
                    new Verge(new List<XYZPoint>
                    {
                        new XYZPoint(p_up),
                        new XYZPoint(up_points[i]),
                        new XYZPoint(up_points[(i+1) % 5]),
                    })
                    );
            }

            for (int i = 0; i < 5; ++i)
            {
                Verges.Add(
                    new Verge(new List<XYZPoint>
                    {
                        new XYZPoint(p_down),
                        new XYZPoint(down_points[i]),
                        new XYZPoint(down_points[(i+1) % 5]),
                    })
                    );
            }

            for (int i = 0; i < 5; ++i)
            {
                Verges.Add(
                    new Verge(new List<XYZPoint>
                    {
                        new XYZPoint(up_points[i]),
                        new XYZPoint(up_points[(i+1) % 5]),
                        new XYZPoint(down_points[(i+1) % 5])
                    })
                    );

                Verges.Add(
                    new Verge(new List<XYZPoint>
                    {
                        new XYZPoint(up_points[i]),
                        new XYZPoint(down_points[i]),
                        new XYZPoint(down_points[(i+1) % 5])
                    })
                    );
            }
            cur_fig = "Icosahedron";
            find_center();
        }

        public void make_dodecahedron()
        {
            Verges = new List<Verge>();
            Polyhedron ik = new Polyhedron();
            ik.make_icosahedron();

            List<XYZPoint> pts = new List<XYZPoint>();
            foreach (Verge f in ik.Verges)
            {
                pts.Add(f.Center);
            }

            Verges.Add(new Verge(new List<XYZPoint>
            {
                new XYZPoint(pts[0]),
                new XYZPoint(pts[1]),
                new XYZPoint(pts[2]),
                new XYZPoint(pts[3]),
                new XYZPoint(pts[4])
            }));

            Verges.Add(new Verge(new List<XYZPoint>
            {
                new XYZPoint(pts[5]),
                new XYZPoint(pts[6]),
                new XYZPoint(pts[7]),
                new XYZPoint(pts[8]),
                new XYZPoint(pts[9])
            }));

            for (int i = 0; i < 5; ++i)
            {
                Verges.Add(new Verge(new List<XYZPoint>
                {
                    new XYZPoint(pts[i]),
                    new XYZPoint(pts[(i + 1) % 5]),
                    new XYZPoint(pts[(i == 4) ? 10 : 2*i + 12]),
                    new XYZPoint(pts[(i == 4) ? 11 : 2*i + 13]),
                    new XYZPoint(pts[2*i + 10])
                }));
            }

            Verges.Add(new Verge(new List<XYZPoint>
            {
                new XYZPoint(pts[5]),
                new XYZPoint(pts[6]),
                new XYZPoint(pts[13]),
                new XYZPoint(pts[10]),
                new XYZPoint(pts[11])
            }));
            Verges.Add(new Verge(new List<XYZPoint>
            {
                new XYZPoint(pts[6]),
                new XYZPoint(pts[7]),
                new XYZPoint(pts[15]),
                new XYZPoint(pts[12]),
                new XYZPoint(pts[13])
            }));
            Verges.Add(new Verge(new List<XYZPoint>
            {
                new XYZPoint(pts[7]),
                new XYZPoint(pts[8]),
                new XYZPoint(pts[17]),
                new XYZPoint(pts[14]),
                new XYZPoint(pts[15])
            }));
            Verges.Add(new Verge(new List<XYZPoint>
            {
                new XYZPoint(pts[8]),
                new XYZPoint(pts[9]),
                new XYZPoint(pts[19]),
                new XYZPoint(pts[16]),
                new XYZPoint(pts[17])
            }));
            Verges.Add(new Verge(new List<XYZPoint>
            {
                new XYZPoint(pts[9]),
                new XYZPoint(pts[5]),
                new XYZPoint(pts[11]),
                new XYZPoint(pts[18]),
                new XYZPoint(pts[19])
            }));
            cur_fig = "Dodecahedron";
            find_center();
        }
        public List<XYZPoint> GetUniquePoints()
        {
            HashSet<XYZPoint> uniquePoints = new HashSet<XYZPoint>(new XYZPointComparer());

            foreach (var verge in Verges)
            {
                foreach (var point in verge.Points)
                {
                    uniquePoints.Add(point);
                }
            }

            return uniquePoints.ToList();
        }

        // Компаратор для сравнения XYZPoint
        public class XYZPointComparer : IEqualityComparer<XYZPoint>
        {
            public bool Equals(XYZPoint p1, XYZPoint p2)
            {
                if (p1 == null || p2 == null) return false;
                return Math.Abs(p1.X - p2.X) < 1e-6 &&
                       Math.Abs(p1.Y - p2.Y) < 1e-6 &&
                       Math.Abs(p1.Z - p2.Z) < 1e-6;
            }

            public int GetHashCode(XYZPoint point)
            {
                if (point == null) return 0;
                return point.X.GetHashCode() ^ point.Y.GetHashCode() ^ point.Z.GetHashCode();
            }
        }
    }
}
