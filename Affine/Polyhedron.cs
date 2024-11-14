﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Lab6
{
    class Polyhedron
    {
        public List<Verge> Verges { get; set; } = null;
        public XYZPoint Center { get; set; } = new XYZPoint(0, 0, 0);
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
            List<Line> p = new List<Line>();
            XYZPoint a = new XYZPoint(0, 0, 0);
            XYZPoint b = new XYZPoint(100, 0, 0);
            b.Draw(g, pr);
            XYZPoint c = new XYZPoint(0, 100, 0);
            c.Draw(g, pr);
            XYZPoint d = new XYZPoint(0, 0, 100);
            d.Draw(g, pr);


            //p.Add(a);
            //p.Add(b);
            //p.Add(c);
            //p.Add(d);

            p.Add(new Line(a, b));
            p.Add(new Line(a, c));
            p.Add(new Line(a, d));


            foreach (Line x in p)
            {
                x.Draw(g, pr);
            }

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

            find_center();
        }
    }
}