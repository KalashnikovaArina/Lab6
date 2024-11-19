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
    }
}