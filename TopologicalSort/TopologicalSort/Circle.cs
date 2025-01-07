using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace TopologicalSort
{
    [Serializable]
    public class Circle
    {
        public Pen Cpen { get; set; }
        public Brush Cbrush { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int R { get; set; }
        public int Index { get; set; }
        public int EnterDegree { get; set; }

        public Rectangle Rect { get; set; }
        private int DISTANCE = 60;
        private List<int> PointTo = new List<int>();

        private Circle() { }

        public Circle(Pen pen, Brush brush,int x, int y, int r, int index) 
        {
            Cpen = pen;
            Cbrush = brush;
            X = x;
            Y = y;
            R = r;
            Index = index;
            EnterDegree = 0;
            this.Rect = new Rectangle((int) (X - R * Math.Cos(45)), (int) (Y - R * Math.Sin(45)),
                (int) (R * Math.Sqrt(2)), (int) (R * Math.Sqrt(2)));
        }

        public Circle(Circle other)
        {
            this.Cpen = new Pen(other.Cpen.Color, other.Cpen.Width);
            this.Cbrush = (Brush)other.Cbrush.Clone();
            this.X = other.X;
            this.Y = other.Y;
            this.R = other.R;
            this.Index = other.Index;
            this.EnterDegree = other.EnterDegree;
            this.Rect = new Rectangle(other.Rect.X, other.Rect.Y, other.Rect.Width, other.Rect.Height);
            this.PointTo = new List<int>(other.PointTo);
        }

        public bool IsInCircle(int x, int y) { return Math.Pow((x - this.X), 2) + Math.Pow((y - this.Y), 2) <= Math.Pow(this.R, 2); }

        public bool IsIntersect(Circle other, double rating)
        {
            double R2 = 2 * R;
            return 2 * this.R + DISTANCE * rating >= 2 * Math.Sqrt(Math.Pow(this.X - other.X, 2) + Math.Pow(this.Y - other.Y, 2));
        }

        public bool isSameCircle(Circle other) { return this.X == other.X && this.Y == other.Y; }

        public void AddPointTo(Circle circle) { PointTo.Add(circle.Index); }

        public void ChangeColor(Color color)
        {
            this.Cpen.Color = color;
            this.Cbrush = new SolidBrush(color);
        }

        public Color GetColor() { return this.Cpen.Color; }

        public Circle Clone() { return new Circle(this); }
    }
}