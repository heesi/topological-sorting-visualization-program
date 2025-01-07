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
    public class Arrow
    {
        public Point Start { get; set; } // 箭头的起始点
        public Point End { get; set; }   // 箭头的结束点
        public int HeadSize { get; set; } = 10; // 箭头头部大小
        public float HeadAngle { get; set; } = (float)Math.PI / 6; // 箭头头部角度
        public Pen Cpen { get; set; }
        public int StartCircle {  get; set; }
        public int EndCircle { get; set; }

        private Arrow() { }

        public Arrow(Circle startCircle, Circle endCircle)
        {
            this.Start = CalculateIntersectionPoint(startCircle, endCircle);
            this.End = CalculateIntersectionPoint(endCircle, startCircle);
            this.StartCircle = startCircle.Index;
            this.EndCircle = endCircle.Index;
            Cpen = Constants.PEN_BLACK;
        }

        public Arrow(Arrow other)
        {
            this.Start = new Point(other.Start.X, other.Start.Y);
            this.End = new Point(other.End.X, other.End.Y);
            this.HeadSize = other.HeadSize;
            this.HeadAngle = other.HeadAngle;
            this.Cpen = new Pen(other.Cpen.Color, other.Cpen.Width);
            this.StartCircle = other.StartCircle;
            this.EndCircle = other.EndCircle;
        }

        private Point CalculateIntersectionPoint(Circle circle1, Circle circle2)
        {
            float dx = circle2.X - circle1.X;
            float dy = circle2.Y - circle1.Y;
            float length = (float)Math.Sqrt(dx * dx + dy * dy);
            float unitDx = dx / length;
            float unitDy = dy / length;

            Point intersectionPoint = new Point(
                (int)(circle1.X + unitDx * circle1.R),
                (int)(circle1.Y + unitDy * circle1.R)
            );

            return intersectionPoint;
        }

        public bool IsInArrow(int x, int y)
        {
            bool isOnLine = IsOnLineSegment(x, y, Start, End);
            if (isOnLine) return true;

            PointF arrowHead1, arrowHead2;
            CalculateArrowHeadPoints(out arrowHead1, out arrowHead2);

            bool isInTriangle = IsInTriangle(x, y, Start, End, arrowHead1) ||
                                IsInTriangle(x, y, Start, End, arrowHead2);
            return isInTriangle;
        }

        private bool IsOnLineSegment(int x, int y, Point start, Point end)
        {
            int dx = end.X - start.X;
            int dy = end.Y - start.Y;

            if (dx == 0) { return x == start.X && y >= Math.Min(start.Y, end.Y) && y <= Math.Max(start.Y, end.Y); }
            if (dy == 0) { return y == start.Y && x >= Math.Min(start.X, end.X) && x <= Math.Max(start.X, end.X); }

            float slope = (float)dy / dx;
            float intercept = start.Y - slope * start.X;
            float calculatedY = slope * x + intercept;

            return Math.Abs(y - calculatedY) < 1 && x >= Math.Min(start.X, end.X) && x <= Math.Max(start.X, end.X);
        }

        private void CalculateArrowHeadPoints(out PointF head1, out PointF head2)
        {
            float dx = End.X - Start.X;
            float dy = End.Y - Start.Y;
            float length = (float)Math.Sqrt(dx * dx + dy * dy);
            float unitDx = dx / length;
            float unitDy = dy / length;

            float headLength = HeadSize / (float)Math.Cos(HeadAngle);
            float headDx = headLength * unitDx;
            float headDy = headLength * unitDy;

            head1 = new PointF(End.X - headDx - headDy * (float)Math.Tan(HeadAngle), End.Y - headDy + headDx * (float)Math.Tan(HeadAngle));
            head2 = new PointF(End.X - headDx + headDy * (float)Math.Tan(HeadAngle), End.Y - headDy - headDx * (float)Math.Tan(HeadAngle));
        }

        private bool IsInTriangle(int x, int y, Point p1, Point p2, PointF p3)
        {
            float denominator = ((p2.Y - p3.Y) * (p1.X - p3.X) + (p3.X - p2.X) * (p1.Y - p3.Y));
            float a = ((p2.Y - p3.Y) * (x - p3.X) + (p3.X - p2.X) * (y - p3.Y)) / denominator;
            float b = ((p3.Y - p1.Y) * (x - p3.X) + (p1.X - p3.X) * (y - p3.Y)) / denominator;
            float c = 1 - a - b;
            return a >= 0 && a <= 1 && b >= 0 && b <= 1 && c >= 0 && c <= 1;
        }

        public bool IsExistByCircle(int Index) { return EndCircle == Index || StartCircle == Index; }

        public Arrow Clone() { return new Arrow(this); }
    }
}