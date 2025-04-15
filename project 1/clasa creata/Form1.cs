using System;
using System.Diagnostics.Metrics;
using System.Drawing.Drawing2D;
using static clasa_creata.Form1;

namespace clasa_creata
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();


          this.DoubleBuffered = true; 
          masinaOrizontala = new Masina(50, 300, vitezaOrizontala, 0); 
          masinaVerticala = new Masina(300, 50, vitezaVerticala, 1); 
          semaforVertical = new Semafor();
          semaforOrizontal = new Semafor();
        }
        Graphics desen;
        Semafor semaforVertical, semaforOrizontal;
        Masina masinaOrizontala, masinaVerticala;
        int stareVertical = 2; 
        int stareOrizontal = 0; 
        int vitezaOrizontala = 2;
        int vitezaVerticala = 2;
        bool accelereazaOrizontala = false;
        bool accelereazaVerticala = false;
        private int contorSemafor = 0;
        private const int INTERVAL_SCHIMBARE = 50;

          
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            desen =e.Graphics;
            

          

            DeseneazaIntersectia();
            DeseneazaSemafoarele();

            masinaOrizontala.Desenare(desen);
            masinaVerticala.Desenare(desen);
        }
        private void DeseneazaIntersectia()
        {
            
            desen.FillRectangle(Brushes.Gray, 0, 290, 600, 20);
           
            desen.FillRectangle(Brushes.Gray, 290, 0, 20, 600);

            
            using (Pen linieAlb = new Pen(Brushes.White, 2))
            {
                for (int i = 0; i < 600; i += 40)
                {
                    desen.DrawLine(linieAlb, i, 300, i + 20, 300); 
                    desen.DrawLine(linieAlb, 300, i, 300, i + 20); 
                }
            }
        }

        private void DeseneazaSemafoarele()
        {
           
            semaforVertical.DeseneazaSemafor(desen, 400, 100, 30, 90);
            semaforVertical.Set_val(stareVertical);

            
            semaforOrizontal.DeseneazaSemafor(desen, 100, 400, 30, 90);
            semaforOrizontal.Set_val(stareOrizontal);
        }





        private void timer1_Tick(object sender, EventArgs e)
        {
            
            if (accelereazaOrizontala && vitezaOrizontala < 5)
                vitezaOrizontala++;
            else if (!accelereazaOrizontala && vitezaOrizontala > 2)
                vitezaOrizontala--;

            if (accelereazaVerticala && vitezaVerticala < 5)
                vitezaVerticala++;
            else if (!accelereazaVerticala && vitezaVerticala > 2)
                vitezaVerticala--;

            masinaOrizontala.viteza = vitezaOrizontala;
            masinaVerticala.viteza = vitezaVerticala;

            
            if (masinaOrizontala.EsteInApropiereaIntersectiei() && !masinaVerticala.EsteInApropiereaIntersectiei())
            {
                stareVertical = 0; 
                stareOrizontal = 2; 
                contorSemafor = 0; 
            }
            else if (masinaVerticala.EsteInApropiereaIntersectiei() && !masinaOrizontala.EsteInApropiereaIntersectiei())
            {
                stareVertical = 2; 
                stareOrizontal = 0; 
                contorSemafor = 0; 
            }
            else if (!masinaOrizontala.EsteInApropiereaIntersectiei() && !masinaVerticala.EsteInApropiereaIntersectiei())
            {
                
                contorSemafor++;
                if (contorSemafor >= INTERVAL_SCHIMBARE)
                {
                    contorSemafor = 0;
                    if (stareVertical == 2)
                    {
                        stareVertical = 1; 
                        stareOrizontal = 0; 
                    }
                    else if (stareVertical == 1)
                    {
                        stareVertical = 0; 
                        stareOrizontal = 2; 
                    }
                    else
                    {
                        stareVertical = 2; 
                        stareOrizontal = 0; 
                    }
                }
            }

            
            if (stareOrizontal == 2 || !masinaOrizontala.EsteInApropiereaIntersectiei())
            {
                masinaOrizontala.Misca();
            }

            if (stareVertical == 2 || !masinaVerticala.EsteInApropiereaIntersectiei())
            {
                masinaVerticala.Misca();
            }

            
            if (masinaOrizontala.x > 600)
            {
                masinaOrizontala.x = -50;
            }

            
            if (masinaVerticala.y > 600)
            {
                masinaVerticala.y = -50;
            }

            this.Invalidate();
        }

        
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W) accelereazaOrizontala = true;
            if (e.KeyCode == Keys.Up) accelereazaVerticala = true;
        }
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W) accelereazaOrizontala = false;
            if (e.KeyCode == Keys.Up) accelereazaVerticala = false;
        }


        public class Masina
        {
            public float x;
            public float y;
            public int viteza;
            public int directie; 

            public Masina(float x, float y, int viteza, int directie)
            {
                this.x = x;
                this.y = y;
                this.viteza = viteza;
                this.directie = directie;
            }

            public void Misca()
            {
                if (directie == 0)
                {
                    x += viteza;
                }
                else 
                {
                    y += viteza;
                }
            }

            public bool EsteInApropiereaIntersectiei()
            {
                if (directie == 0) 
                {
                    return x > 250 && x < 350;
                }
                else 
                {
                    return y > 250 && y < 350;
                }
            }

            public void Desenare(Graphics g)
            {
                if (directie == 0) 
                {
                    g.FillRectangle(Brushes.Blue, x, y, 50, 20);
                }
                else 
                {
                    g.FillRectangle(Brushes.Red, x, y, 20, 50);
                }
            }
        }

        public class Semafor
        {
            private int stare;

            public void Set_val(int stare)
            {
                this.stare = stare;
            }

            public void DeseneazaSemafor(Graphics g, int x, int y, int width, int height)
            {
                g.FillRectangle(Brushes.Black, x, y, width, height);

                int pozY = y + 10;

                g.FillEllipse(stare == 0 ? Brushes.Red : Brushes.DarkGray, x + 5, pozY, 20, 20);
                g.FillEllipse(stare == 1 ? Brushes.Yellow : Brushes.DarkGray, x + 5, pozY + 30, 20, 20);
                g.FillEllipse(stare == 2 ? Brushes.Green : Brushes.DarkGray, x + 5, pozY + 60, 20, 20);
            }
        }
    }
}




