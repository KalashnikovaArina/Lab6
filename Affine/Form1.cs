using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Lab6
{

    public enum Axis { AXIS_X, AXIS_Y, AXIS_Z, OTHER };
    public enum Projection { PERSPECTIVE = 0, AXONOMETRIC};
    public partial class Form1 : Form
    {
        Graphics g;
        Projection projection = 0;
        Axis rotateLineMode = 0;
        Polyhedron figure = null;
        int revertId = -1;

        public Form1()
        {
            InitializeComponent();
            g = pictureBox1.CreateGraphics();
            g.TranslateTransform(pictureBox1.ClientSize.Width / 2, pictureBox1.ClientSize.Height / 2);
            g.ScaleTransform(1, -1);
        }

        //TRANSLATION ROTATION SCALE
        private void button2_Click(object sender, EventArgs e)
        {
            if (figure == null)
            {
                MessageBox.Show("Неодбходимо выбрать фигуру!", "Ошибка!", MessageBoxButtons.OK);
            }
            else
            {
                //TRANSLATE
                int offsetX = (int)numericUpDown1.Value, offsetY = (int)numericUpDown2.Value, offsetZ = (int)numericUpDown3.Value;
                figure.translate(offsetX, offsetY, offsetZ);
                
                //ROTATE
                int rotateAngleX = (int)numericUpDown4.Value;
                figure.rotate(rotateAngleX, 0);

                int rotateAngleY = (int)numericUpDown5.Value;
                figure.rotate(rotateAngleY, Axis.AXIS_Y);

                int rotateAngleZ = (int)numericUpDown6.Value;
                figure.rotate(rotateAngleZ, Axis.AXIS_Z);

                //SCALE
                if (checkBox1.Checked)
                {
                    float old_x = figure.Center.X, old_y = figure.Center.Y, old_z = figure.Center.Z;
                    figure.translate(-old_x, -old_y, -old_z);

                    float kx = (float)numericUpDown9.Value, ky = (float)numericUpDown8.Value, kz = (float)numericUpDown7.Value;
                    figure.scale(kx, ky, kz);

                    figure.translate(old_x, old_y, old_z);
                }
                else
                {
                    float kx = (float)numericUpDown9.Value, ky = (float)numericUpDown8.Value, kz = (float)numericUpDown7.Value;
                    figure.scale(kx, ky, kz);
                }
            }

            g.Clear(pictureBox1.BackColor);
            figure.Draw(g, projection);
        }
        private void DrawAx(Graphics g, Projection pr = 0)
        {
            List<Line> p = new List<Line>();
            XYZPoint a = new XYZPoint(0, 0, 0);
            XYZPoint b = new XYZPoint(1, 0, 0);
            XYZPoint c = new XYZPoint(0, 1, 0);
            XYZPoint d = new XYZPoint(0, 0, 1);

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
        }
        //DRAW FIGURE
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    //Hexahedron
                   g.Clear(pictureBox1.BackColor);
                    figure = new Polyhedron();
                    figure.make_hexahedron();
                    figure.Draw(g, projection);
                    DrawAx(g, projection);
                    break;
                case 1:
                    //Icosahedron
                    g.Clear(pictureBox1.BackColor);
                    figure = new Polyhedron();
                    figure.make_icosahedron();
                    figure.Draw(g, projection);
                    break;
                case 2:
                    //Dodecahedron
                    g.Clear(pictureBox1.BackColor);
                    figure = new Polyhedron();
                    figure.make_dodecahedron();
                    figure.Draw(g, projection);
                    break;
                case 3:
                    //Tetrahedron
                    g.Clear(pictureBox1.BackColor);
                    figure = new Polyhedron();
                    figure.make_tetrahedron();
                    figure.Draw(g, projection);
                    DrawAx(g, projection);
                    break;
                default:
                    break;
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e) => projection = (Projection)comboBox2.SelectedIndex;
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e) => revertId = comboBox3.SelectedIndex;
        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            rotateLineMode = (Axis)comboBox4.SelectedIndex;
            switch (comboBox4.SelectedIndex) 
            {
                case 0:
                    checkNumeric(false);
                    break;
                case 1:
                    checkNumeric(false);
                    break;
                case 2:
                    checkNumeric(false);
                    break;
                case 3:
                    checkNumeric(true); //разрешить вводить значения произвольной прямой
                    break;
                default:
                    break;
            }

            void checkNumeric(bool flag)
            {
                numericUpDown10.Enabled = flag;
                numericUpDown11.Enabled = flag;
                numericUpDown12.Enabled = flag;
                numericUpDown13.Enabled = flag;
                numericUpDown14.Enabled = flag;
                numericUpDown15.Enabled = flag;
            }
        }

        private void RotateAroundLine() //вращение вокруг произвольной прямой
        {
            Line rotateLine = new Line(
                                new XYZPoint(
                                    (float)numericUpDown12.Value,
                                    (float)numericUpDown11.Value,
                                    (float)numericUpDown10.Value),
                                new XYZPoint(
                                    (float)numericUpDown15.Value,
                                    (float)numericUpDown14.Value,
                                    (float)numericUpDown13.Value));

            float Ax = rotateLine.First.X, Ay = rotateLine.First.Y, Az = rotateLine.First.Z;
            //figure.translate(-Ax, -Ay, -Az);

            double angle = (double)numericUpDown16.Value;
            figure.rotate(angle, rotateLineMode, rotateLine);

            //figure.translate(Ax, Ay, Az);

            g.Clear(pictureBox1.BackColor);
            figure.Draw(g, projection);
        }

        //CAMERA PROJECTION
        private void button1_Click_1(object sender, EventArgs e)
        {
            g.Clear(pictureBox1.BackColor);
            if (figure != null)
                figure.Draw(g, projection);
        }

        //REVERT FUNCTIONS
        private void button3_Click(object sender, EventArgs e)
        {
            if (revertId == 0)
            {
                figure.reflectX();
                g.Clear(pictureBox1.BackColor);
                figure.Draw(g, projection);
            }
            else if (revertId == 1)
            {
                figure.reflectY();
                g.Clear(pictureBox1.BackColor);
                figure.Draw(g, projection);
            }
            else if (revertId == 2)
            {
                figure.reflectZ();
                g.Clear(pictureBox1.BackColor);
                figure.Draw(g, projection);
            }
        }

        //ROTATE AROUND LINE
        private void button4_Click(object sender, EventArgs e) => RotateAroundLine();

        /*----------------------------- save and load -----------------------------*/
        private void button5_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Object Files(*.obj)| *.obj | Text files(*.txt) | *.txt | All files(*.*) | *.* ";
            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string info = "";
                    List<XYZPoint> points = figure.GetUniquePoints();
                    foreach (XYZPoint point in points)
                    {
                        info += "v ";
                        info += point.X + " ";
                        info += point.Y + " ";
                        info += point.Z;
                        info += "\r\n";
                    }

                    foreach (Verge f in figure.Verges)
                    {
                        info += "f ";
                        foreach (var point in f.Points)
                        {
                            var index = points.IndexOf(point) + 1;
                            info += index + "/" + index + "/" + index + " ";
                        }
                        info += "\r\n";
                    }

                    System.IO.File.WriteAllText(saveDialog.FileName, info);
                }
                catch
                {
                    DialogResult rezult = MessageBox.Show("Невозможно сохранить файл",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }

        private void button6_Click(object sender, EventArgs e)
        {
            OpenFileDialog loadDialog = new OpenFileDialog();
            loadDialog.Filter = "Object Files(*.obj)|*.obj|Text files (*.txt)|*.txt|All files (*.*)|*.*";
            if (loadDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    List<Verge> verges = new List<Verge>();
                    List<XYZPoint> points = new List<XYZPoint>();
                    string str = System.IO.File.ReadAllText(loadDialog.FileName).Replace("\r\n", "!");
                    string[] info = str.Split('!');

                    List<Verge> Verges = new List<Verge>();

                    foreach (var verge in info)
                    {
                        if (string.IsNullOrWhiteSpace(verge)) continue; // Пропуск пустых строк

                        string[] pIndex = verge.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        if (pIndex.Length == 0) continue;

                        if (pIndex[0] == "v") // Обработка точки
                        {
                            if (pIndex.Length < 4) continue; // недостаточно координат
                            if (float.TryParse(pIndex[1], out float x) &&
                                float.TryParse(pIndex[2], out float y) &&
                                float.TryParse(pIndex[3], out float z))
                            {
                                points.Add(new XYZPoint(x, y, z));
                            }
                        }
                        else if (pIndex[0] == "f") // Обработка грани
                        {
                            List<XYZPoint> pts = new List<XYZPoint>();
                            for (int i = 1; i < pIndex.Length; i++) // пропускаем f
                            {
                                string[] nums = pIndex[i].Split('/');
                                if (nums.Length > 0 && int.TryParse(nums[0], out int ind1))
                                {
                                    if (ind1 > 0 && ind1 <= points.Count)
                                    {
                                        pts.Add(points[ind1 - 1]); // Индексы начинаются с 1
                                    }
                                }
                            }
                            if (pts.Count > 0)
                            {
                                Verges.Add(new Verge(pts));
                            }
                        }
                    }

                    // Создание и отрисовка фигуры
                    g.Clear(pictureBox1.BackColor);
                    figure = new Polyhedron(Verges);
                    figure.Draw(g, projection);
                }
                catch
                {
                    DialogResult rezult = MessageBox.Show("Невозможно отобразить файл",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        /*----------------------------------------------------------*/
    }
}