using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tao.DevIl;
using Tao.FreeGlut;
using Tao.OpenGl;

namespace furniture
{
    public partial class Form1 : Form
    {
        float global_time = 0;
        double translateX = -9, translateY = -60, translateZ = -10;
        double bookTranslateX = 40, bookTranslateY = 145, bookTranslateZ = 42;
        double chairTranslateX, chairTranslateY;
        double angleX = -70, angleY = 0, angleZ = 0;
        float deltaColor = 0;
        double deltaRotate, bulbRotate, chairRotate;
        double chairZ;
        bool chairFall, boom;

        // эккземпляра класса Explosion
        private Explosion BOOOOM_1 = new Explosion(1, 10, 1, 300, 500);


        public Form1()
        {
            InitializeComponent();
            AnT.InitializeContexts();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (deltaColor == 0)
            {
                deltaColor = 0.2f;
                button1.Text = "Включить свет";
            } else
            {
                deltaColor = 0;
                button1.Text = "Выключить свет";
            }
                
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (chairFall)
            {
                chairFall = false;
                chairRotate = 0;
                chairZ = 0;
                button2.Text = "Уронить стул";
            }
            else
            {
                chairFall = true;
                boom = true;
                button2.Text = "Поднять стул";
            }
        }

        private void AnT_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.X)
            {
                translateZ -= 1;
            }
            if (e.KeyCode == Keys.E)
            {
                translateZ += 1;
            }
            if (e.KeyCode == Keys.A)
            {
                translateX += 1;
            }
            if (e.KeyCode == Keys.D)
            {
                translateX -= 1;
            }
            if (e.KeyCode == Keys.W)
            {
                translateY -= 1;
            }
            if (e.KeyCode == Keys.S)
            {
                translateY += 1;
            }
            if (e.KeyCode == Keys.Q)
            {
                angleZ += 5;
            }
            if (e.KeyCode == Keys.Z)
            {
                angleZ -= 5;
            }
            if (e.KeyCode == Keys.NumPad4 && bookTranslateX >= -32)
            {
                bookTranslateX -= 1;
            }
            if (e.KeyCode == Keys.NumPad8 && bookTranslateY <= 155)
            {
                bookTranslateY += 1;
            }
            if (e.KeyCode == Keys.NumPad6 && bookTranslateX <= 51)
            {
                bookTranslateX += 1;
            }
            if (e.KeyCode == Keys.NumPad2 && bookTranslateY >= 134)
            {
                bookTranslateY -= 1;
            }
            if (e.KeyCode == Keys.G && chairTranslateY >= -95)
            {
                chairTranslateY -= 1;
                if (chairTranslateY == 61 && chairTranslateX <= 111 && chairTranslateX >= 40 ||
                    chairTranslateY == 75 && (chairTranslateX <= -27 && chairTranslateX >= -51 || chairTranslateX <= 39 && chairTranslateX >= 27)||
                    chairTranslateY == 74 && chairTranslateX <= -52 && chairTranslateX >= -79)
                {
                    chairTranslateY += 5;
                }
            }
            if (e.KeyCode == Keys.T && chairTranslateY <= 86)
            {
                chairTranslateY += 1;
                if (chairTranslateY == 18 && chairTranslateX <= 111 && chairTranslateX >= 40 ||
                    chairTranslateY == 25 && (chairTranslateX <= -27 && chairTranslateX >= -51 || chairTranslateX <= 39 && chairTranslateX >= 27)||
                    chairTranslateY == 38 && chairTranslateX <= -52 && chairTranslateX >= -79)
                {
                    chairTranslateY -= 5;
                }
            }
            if (e.KeyCode == Keys.F && chairTranslateX >= -199)
            {
                chairTranslateX -= 1;
                if (chairTranslateY >= 26 && chairTranslateY <= 74 && chairTranslateX == -27 ||
                    chairTranslateY >= 19 && chairTranslateY <= 62 && chairTranslateX == 110)
                {
                    chairTranslateX += 5;
                }
            }
            if (e.KeyCode == Keys.H && chairTranslateX <= 179)
            {
                chairTranslateX += 1;
                if ((chairTranslateX == -51 || chairTranslateX == 27) && (chairTranslateY >= 26 && chairTranslateY <= 74) ||
                    chairTranslateX == 40 && chairTranslateY <= 24 && chairTranslateY >= 18 ||
                    chairTranslateX == -81 && chairTranslateY <= 72 && chairTranslateY >= 36)
                {
                    chairTranslateX -= 5;
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // инициализация Glut
            Glut.glutInit();
            Glut.glutInitDisplayMode(Glut.GLUT_RGB | Glut.GLUT_DOUBLE);
            Il.ilInit();
            Il.ilEnable(Il.IL_ORIGIN_SET);
            // отчитка окна
            Gl.glClearColor(255, 255, 255, 1);
            // установка порта вывода в соответствии с размерами элемента anT
            Gl.glViewport(0, 0, AnT.Width, AnT.Height);
            // настройка проекции
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glLoadIdentity();
            Glu.gluPerspective(60, (float)AnT.Width / (float)AnT.Height, 0.1, 800);
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();
            // настройка параметров OpenGL для визуализации
            Gl.glEnable(Gl.GL_DEPTH_TEST);

            comboBox1.SelectedIndex = 0;
            deltaRotate = 1;

            RenderTimer.Start();
        }

        private void RenderTimer_Tick(object sender, EventArgs e)
        {
            global_time += (float)RenderTimer.Interval / 1000;
            Draw();
        }

        private void Draw()
        {
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT); 
            Gl.glLoadIdentity(); 
            Gl.glColor3f(1.0f, 0, 0); 
            Gl.glPushMatrix();
            Gl.glTranslated(translateX, translateY, translateZ);
            Gl.glRotated(angleX, 1, 0, 0);
            Gl.glRotated(angleY, 0, 1, 0);
            Gl.glRotated(angleZ, 0, 0, 1);
            BOOOOM_1.Calculate(global_time);

            //пол
            Gl.glColor3f(0.2f - deltaColor, 0.1f - deltaColor, 0);
            Gl.glBegin(Gl.GL_TRIANGLE_FAN);
            Gl.glVertex3d(200, 200, 0);
            Gl.glVertex3d(-200, 200, 0);
            Gl.glVertex3d(-200, -10, 0);
            Gl.glVertex3d(200, -10, 0);
            Gl.glEnd();
            double line = 0;
            Gl.glColor3f(0, 0, 0);
            Gl.glLineWidth(3f);
            do
            {
                Gl.glBegin(Gl.GL_LINE_STRIP);
                Gl.glVertex3d(200 - line, 195, 1);
                Gl.glVertex3d(200 - line, -10, 1);
                Gl.glEnd();
                line += 10;
            }
            while (line < 400);

            //потолок
            Gl.glColor3f(0.6823529f - deltaColor, 0.7803921f - deltaColor, 0.96078f - deltaColor);
            Gl.glBegin(Gl.GL_TRIANGLE_FAN);
            Gl.glVertex3d(200, 200, 100);
            Gl.glVertex3d(-200, 200, 100);
            Gl.glVertex3d(-200, -10, 100);
            Gl.glVertex3d(200, -10, 100);
            Gl.glEnd();

            //Стол
            Gl.glPushMatrix();
            Gl.glTranslated(10, 145, 39);
            Gl.glScaled(9, 3.5, 0.2);
            Gl.glColor3f(0.2f - deltaColor, 0.2f - deltaColor, 0.2f - deltaColor);
            Glut.glutSolidCube(10);
            Gl.glColor3f(0, 0, 0);
            Gl.glLineWidth(2f);
            Glut.glutWireCube(10);
            Gl.glPopMatrix();

            Gl.glPushMatrix();
            Gl.glTranslated(-29, 145, 20);
            Gl.glScaled(0.6, 7, 9);
            Gl.glColor3f(0.2f - deltaColor, 0.2f - deltaColor, 0.2f - deltaColor);
            Glut.glutSolidCube(4);
            Gl.glColor3f(0, 0, 0);
            Gl.glLineWidth(5f);
            Glut.glutWireCube(4);
            Gl.glPopMatrix();

            Gl.glPushMatrix();
            Gl.glTranslated(49, 145, 20);
            Gl.glScaled(0.6, 7, 9);
            Gl.glColor3f(0.2f - deltaColor, 0.2f - deltaColor, 0.2f - deltaColor);
            Glut.glutSolidCube(4);
            Gl.glColor3f(0, 0, 0);
            Gl.glLineWidth(5f);
            Glut.glutWireCube(4);
            Gl.glPopMatrix();

            //табуретка
            Gl.glPushMatrix();
            Gl.glTranslated(0 + chairTranslateX, 105 + chairTranslateY, 14);
            Gl.glScaled(0.15, 0.15, 2.5);
            Gl.glColor3f(0.4f - deltaColor, 0.4f - deltaColor, 0.4f - deltaColor);
            Glut.glutSolidCube(10);
            Gl.glColor3f(0, 0, 0);
            Gl.glLineWidth(2f);
            Glut.glutWireCube(10);
            Gl.glPopMatrix();

            Gl.glPushMatrix();
            Gl.glTranslated(20 + chairTranslateX, 105 + chairTranslateY, 14);
            Gl.glScaled(0.15, 0.15, 2.5);
            Gl.glColor3f(0.4f - deltaColor, 0.4f - deltaColor, 0.4f - deltaColor);
            Glut.glutSolidCube(10);
            Gl.glColor3f(0, 0, 0);
            Gl.glLineWidth(2f);
            Glut.glutWireCube(10);
            Gl.glPopMatrix();

            Gl.glPushMatrix();
            Gl.glTranslated(20 + chairTranslateX, 85 + chairTranslateY, 14);
            Gl.glScaled(0.15, 0.15, 2.5);
            Gl.glColor3f(0.4f - deltaColor, 0.4f - deltaColor, 0.4f - deltaColor);
            Glut.glutSolidCube(10);
            Gl.glColor3f(0, 0, 0);
            Gl.glLineWidth(2f);
            Glut.glutWireCube(10);
            Gl.glPopMatrix();

            Gl.glPushMatrix();
            Gl.glTranslated(0 + chairTranslateX, 85 + chairTranslateY, 14);
            Gl.glScaled(0.15, 0.15, 2.5);
            Gl.glColor3f(0.4f - deltaColor, 0.4f - deltaColor, 0.4f - deltaColor);
            Glut.glutSolidCube(10);
            Gl.glColor3f(0, 0, 0);
            Gl.glLineWidth(2f);
            Glut.glutWireCube(10);
            Gl.glPopMatrix();

            Gl.glPushMatrix();
            Gl.glTranslated(10 + chairTranslateX, 95 + chairTranslateY, 27);
            Gl.glScaled(2.2, 2.2, 0.15);
            Gl.glColor3f(0.4f - deltaColor, 0.4f - deltaColor, 0.4f - deltaColor);
            Glut.glutSolidCube(10);
            Gl.glColor3f(0, 0, 0);
            Gl.glLineWidth(2f);
            Glut.glutWireCube(10);
            Gl.glPopMatrix();

            //шкаф
            Gl.glPushMatrix();
            Gl.glTranslated(85, 135, 40);
            Gl.glScaled(5, 2.2, 7);
            Gl.glColor3f(0.7882f - deltaColor, 0.7098f - deltaColor, 0.6078f - deltaColor);
            Glut.glutSolidCube(10);
            Gl.glColor3f(0, 0, 0);
            Gl.glLineWidth(2f);
            Glut.glutWireCube(10);
            Gl.glPopMatrix();

            Gl.glPushMatrix();
            Gl.glTranslated(85, 123.4, 5);
            Gl.glLineWidth(1);
            Gl.glBegin(Gl.GL_LINES);
            Gl.glColor3d(0, 0, 0);
            Gl.glVertex3d(0, 0, 0);
            Gl.glVertex3d(0, 0, 70);
            Gl.glEnd();
            Gl.glPopMatrix();

            Gl.glPushMatrix();
            Gl.glTranslated(75, 124, 43);
            Gl.glRotatef(90, 1, 0, 0);
            Gl.glColor3f(0.1f - deltaColor, 0.1f - deltaColor, 0.1f - deltaColor);
            Glut.glutSolidCylinder(1.5, 3, 15, 15);
            Gl.glColor3f(0, 0, 0);
            Gl.glLineWidth(2f);
            Glut.glutSolidCylinder(1, 3, 15, 15);
            Gl.glPopMatrix();

            Gl.glPushMatrix();
            Gl.glTranslated(95, 124, 43);
            Gl.glRotatef(90, 1, 0, 0);
            Gl.glColor3f(0.1f - deltaColor, 0.1f - deltaColor, 0.1f - deltaColor);
            Glut.glutSolidCylinder(1.5, 3, 15, 15);
            Gl.glColor3f(0, 0, 0);
            Gl.glLineWidth(2f);
            Glut.glutSolidCylinder(1, 3, 15, 15);
            Gl.glPopMatrix();

            //книга
            Gl.glPushMatrix();
            Gl.glTranslated(bookTranslateX, bookTranslateY, bookTranslateZ);
            Gl.glRotatef(-19, 0, 0, 1);
            Gl.glScaled(0.9, 1.7, 0.3);
            Gl.glColor3f(0.8588235294f - deltaColor, 0.5333333333f - deltaColor, 0.9607843137f - deltaColor);
            Glut.glutSolidCube(10);
            Gl.glColor3f(0, 0, 0);
            Gl.glLineWidth(2f);
            Glut.glutWireCube(10);
            Gl.glPopMatrix();

            //лампочка
            Gl.glPushMatrix();
            Gl.glTranslated(10, 150, 100);
            if (bulbRotate > 45)
                deltaRotate = -1;
            if (bulbRotate < -45)
                deltaRotate = 1;
            bulbRotate += deltaRotate;
            Gl.glRotated(bulbRotate + 180, 0, 1, 0);
            Gl.glColor3f(0.843137f - deltaColor, 0.843137f - deltaColor, 0.843137f - deltaColor);
            Glut.glutSolidCylinder(0.3, 15, 10, 10);
            Gl.glTranslated(0, 0, 15);
            Gl.glColor3f(1f - deltaColor, 0.8980392157f - deltaColor, 0);
            Glut.glutWireSphere(2.5, 32, 32);
            Gl.glPopMatrix();

            //стул
            if (chairRotate < 85 && chairFall)
            {
                chairRotate += 5;
                chairZ += 0.4;
            }

            if (boom)
            {
                // устанавливаем новые координаты взрыва
                BOOOOM_1.SetNewPosition(-50, 150, 1);
                // случайную силу
                BOOOOM_1.SetNewPower(70);
                // и активируем сам взрыв
                BOOOOM_1.Boooom(global_time);
                boom = false;
            }

            Gl.glPushMatrix();
            
            Gl.glTranslated(-50, 150, 1.2 + chairZ);
            Gl.glRotated(chairRotate, 1, 0, 0);
            Gl.glColor3f(0.9f - deltaColor, 0.9f - deltaColor, 0.9f - deltaColor);
            Glut.glutSolidCylinder(7, 2, 10, 10);
            Gl.glLineWidth(2f);
            Gl.glColor3f(0, 0, 0);
            Glut.glutWireCylinder(7, 2, 10, 10);

            Gl.glTranslated(0, 0, 1.2);
            Gl.glColor3f(0.9f - deltaColor, 0.9f - deltaColor, 0.9f - deltaColor);
            Glut.glutSolidCylinder(1, 25, 10, 10);
            Gl.glLineWidth(2f);
            Gl.glColor3f(0, 0, 0);
            Glut.glutWireCylinder(1, 25, 10, 10);

            Gl.glTranslated(0, 0, 25);
            Gl.glColor3f(0.4705882f - deltaColor, 0.074509803f - deltaColor, 0.980392156f - deltaColor);
            Glut.glutSolidCylinder(9, 4, 10, 10);
            Gl.glLineWidth(2f);
            Gl.glColor3f(0, 0, 0);
            Glut.glutWireCylinder(9, 4, 10, 10);
            Gl.glPopMatrix();

            /*label2.Text = Convert.ToString(chairTranslateX);
            label3.Text = Convert.ToString(chairTranslateY);
            label4.Text = Convert.ToString(translateY);
            label5.Text = Convert.ToString(translateZ);*/

            Gl.glPopMatrix(); 
            Gl.glFlush(); 
            AnT.Invalidate();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                angleX = -70;
                angleY = 0;
                angleZ = 25;
                translateX = 40;
                translateY = -66;
                translateZ = -61;
            }
            if (comboBox1.SelectedIndex == 1)
            {
                angleX = -70;
                angleY = 0;
                angleZ = 0;
                translateX = -11;
                translateY = -96;
                translateZ = 27;
            }
        }
    }
}
