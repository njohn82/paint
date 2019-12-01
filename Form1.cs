﻿using System;
using System.Drawing;
using System.Windows.Forms;

namespace Paint1
{
    public partial class Form1 : Form
    {
        IFigure figure123 = new Line();

        Bitmap bitmapImage, memoryBitmap;
        bool mouseDown = false, shift = false;
        int firstPointX, firstPointY, prevPointX = -1, prevPointY = -1, memoryFirstPointX, memoryFirstPointY;
        
        CustomColorDialog colorDialog = new CustomColorDialog();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            bitmapImage = new Bitmap(canvas.Width, canvas.Height);
            canvas.Image = bitmapImage;
        }

        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Location.X >= 0 && e.Location.X < canvas.Width && e.Location.Y >= 0 && e.Location.Y < canvas.Height)
            {
                if(mouseDown)
                {
                    if (figure123 is Line)
                    {
                        DrawBrush(e);
                    }
                    else
                    {
                        DrawFigure(firstPointX, firstPointY, e.Location.X, e.Location.Y);
                    }                    
                }
                else
                {
                    if (figure123 is Polygon && prevPointX != -1 && prevPointY != -1) DrawFigureByPoint(e);
                } 
            }
        }

        private void DrawBrush(MouseEventArgs e)
        {
            figure123.Draw(firstPointX, firstPointY, e.Location.X, e.Location.Y);

            firstPointX = e.Location.X;
            firstPointY = e.Location.Y;

            canvas.Image = bitmapImage;

        }

        private void DrawFigureByPoint(MouseEventArgs e)
        {
            DrawFigure(prevPointX, prevPointY, e.Location.X, e.Location.Y);

            prevPointX = firstPointX;
            prevPointY = firstPointY;
        }


        //Добавлен булевый атрибут для метода DrawFigureSquare
        private void DrawFigure(int x1, int y1, int x2, int y2) 
        {
            CloneBitmap(out memoryBitmap);

            if (figure123 is Square)
            {
                ((Square)figure123).IsRectrangle = !shift;
            } 
           
            if (figure123 is Circle)
            {
                ((Circle)figure123).IsEllipse = !shift;
            }

            figure123.Draw(x1, y1, x2, y2);

            canvas.Image = memoryBitmap;
        }

        private void CloneBitmap(out Bitmap btm)
        {
            Rectangle r = new Rectangle(0, 0, canvas.Width-1 , canvas.Height-1 );
            btm =  bitmapImage.Clone(r, System.Drawing.Imaging.PixelFormat.DontCare);
            Brush.BitmapImage = btm;
        }

        private void canvas_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            firstPointX = e.Location.X;
            firstPointY = e.Location.Y;

            if (figure123 is Polygon && prevPointX == -1 && prevPointY == -1)
            {
                CloneBitmap(out memoryBitmap);
                prevPointX = firstPointX;
                prevPointY = firstPointY;
                memoryFirstPointX = firstPointX;
                memoryFirstPointY = firstPointY;
            }
        }

        private void canvas_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
            if (memoryBitmap != null)
            {
                bitmapImage = memoryBitmap;
            }

            Brush.BrushColor = colorDialog.Color;
        }

        private void colorBt_Click(object sender, EventArgs e)
        {           
            DialogResult dr = colorDialog.ShowDialog();
            if (dr == DialogResult.OK)
            {
                colorBt.BackColor = colorDialog.Color;
                Brush.BrushColor = colorDialog.Color;
            }
        }

        private void canvas_DoubleClick(object sender, EventArgs e)
        {
            if (figure123  is Polygon)
            {
                figure123.Draw(prevPointX, prevPointY, memoryFirstPointX, memoryFirstPointY);
                bitmapImage = memoryBitmap;
                canvas.Image = bitmapImage;
                prevPointX = -1;
                prevPointY = -1;
            }
        }
       
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            shift = false;
        }
  
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (Control.ModifierKeys == Keys.Shift)
            {
                shift = true;
            }
        }

        private void trackBrush_ValueChanged(object sender, EventArgs e)
        {
            Brush.BrushThickness = trackBrush.Value + 1;
        }

        private void trackBrush_Scroll(object sender, EventArgs e)
        {

        }

        private void Form1_SizeChanged(object sender, EventArgs e )
        {
            //   //Color lastColorBrush = brush.BrushColor;
            //   // int lastThicknessBrush = brush.BrushThickness;
            //   // Rectangle r = new Rectangle(0, 0, canvas.Width - 1, canvas.Height - 1);
            //   // btm = bitmapImage.Clone(r, System.Drawing.Imaging.PixelFormat.DontCare);
            //   // bitmapImage = btm(canvas.Width, canvas.Height);
            //   // canvas.Image = bitmapImage; 
            //   // this.brush.BitmapImage = btm;
            //   // this.figure.Brush = brush;
            //   //  brush.BrushColor = lastColorBrush;
            //   // brush.BrushThickness = lastThicknessBrush;
            ////canvas.Image = bitmapImage;
            ////brush.BitmapImage = bitmapImage;
            //CloneBitmap(out memoryBitmap);

            //bitmapImage = new Bitmap(canvas.Width, canvas.Height);
        }

        private void figureSquare_BackColorChanged(object sender, EventArgs e)
        {

        }

        private void figurePolygon_Click(object sender, EventArgs e)
        {
            figure123 = new Polygon();
        }

        private void figureTriangle_Click(object sender, EventArgs e)
        {
            figure123= new Triangle();
        }
        private void figureCircle_Click(object sender, EventArgs e)
        {
            figure123 = new Circle();
        }

        private void figureSquare_Click(object sender, EventArgs e)
        {
            figure123 = new Square();
        }

        private void pointPolygon_Click(object sender, EventArgs e)
        {
            figure123 = new Polygon();
        }

        private void brushTool_Click(object sender, EventArgs e)
        {
            figure123 = new Line();
        }

        private void cleanBt_Click(object sender, EventArgs e)
        {
            Color lastColorBrush = Brush.BrushColor;
            int lastThicknessBrush = Brush.BrushThickness;

            bitmapImage = new Bitmap(canvas.Width, canvas.Height);
            canvas.Image = bitmapImage;
            Brush.BitmapImage = bitmapImage;
            Brush.BrushColor = lastColorBrush;
            Brush.BrushThickness = lastThicknessBrush;
        }    
    }
}
