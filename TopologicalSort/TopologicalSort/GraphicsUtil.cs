using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TopologicalSort
{
    public static class GraphicsUtil
    {
        // 存储要绘制的状态信息
        private static List<Status> Statuses = new List<Status>();
        public static int StatusIndex { get; set; }

        public static void Init()
        {
            Statuses.Add(new Status());
            ResetStatusIndex();
        }

        private static void ResetStatusIndex()
        {
            StatusIndex = 0;
        }

        // 添加一个圆形到绘制列表
        public static void AddCircle(int x, int y, int radius, int statusIndex)
        {
            Statuses[statusIndex].Circles.Add(new Circle(Constants.PEN_BLACK, Constants.BRUSH_BLACK, x, y, radius, Statuses[StatusIndex].Circles.Count));
        }

        private static void AddArrow(Circle circle1, Circle circle2, int statusIndex)
        {
            Statuses[statusIndex].Arrows.Add(new Arrow(circle1, circle2));
        }

        public static void DeleteCircleAndArrow(int x, int y)
        {
            DeleteCircle(x, y);
            DeleteArrow(x, y);
        }

        private static void DeleteCircle(int x, int y)
        {
            bool needToChange = false;
            for (int i = 0; i < Statuses[StatusIndex].Circles.Count; i++)
            {
                if (needToChange)
                {
                    for (int j = 0; j < Statuses[StatusIndex].Arrows.Count; j++)
                    {
                        if (Statuses[StatusIndex].Arrows.ElementAt(j).StartCircle == Statuses[StatusIndex].Circles.ElementAt(i).Index)
                        {
                            Statuses[StatusIndex].Arrows[j].StartCircle--;
                        }
                        if (Statuses[StatusIndex].Arrows.ElementAt(j).EndCircle == Statuses[StatusIndex].Circles.ElementAt(i).Index)
                        {
                            Statuses[StatusIndex].Arrows[j].EndCircle--;
                        }
                    }
                    Statuses[StatusIndex].Circles[i].Index--;
                }
                if (Statuses[StatusIndex].Circles.ElementAt(i).IsInCircle(x, y))
                {
                    for (int j = 0; j < Statuses[StatusIndex].Arrows.Count; j++)
                    {
                        if (Statuses[StatusIndex].Arrows.ElementAt(j).IsExistByCircle(i))
                        {
                            DeleteArrow(j);
                            j--;
                        }
                    }

                    Statuses[StatusIndex].Circles.RemoveAt(i);
                    needToChange = true;
                    i--;
                }
            }
        }

        private static void DeleteArrow(int x, int y)
        {
            for (int i = 0; i < Statuses[StatusIndex].Arrows.Count; i++)
            {
                if (Statuses[StatusIndex].Arrows.ElementAt(i).IsInArrow(x, y))
                {
                    Statuses.ElementAt(StatusIndex).Circles.ElementAt(Statuses[StatusIndex].Arrows.ElementAt(i).EndCircle).EnterDegree--;
                    Statuses[StatusIndex].Arrows.RemoveAt(i);
                    break;
                }
            }
        }

        private static void DeleteArrow(int index)
        {
            Statuses[StatusIndex].Arrows.RemoveAt(index);
        }

        public static void DrawGraphics(Graphics g)
        {
            // 启用高质量绘图
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            DrawCircles(g);
            DrawFrameWork(g);
            DrawArrow(g);
        }

        // 在指定的 Graphics 对象上绘制所有圆形
        private static void DrawCircles(Graphics g)
        {
            foreach (var circle in Statuses[StatusIndex].Circles)
            {
                g.DrawEllipse(circle.Cpen, circle.X - circle.R, circle.Y - circle.R, circle.R * 2, circle.R * 2);
                g.DrawString(circle.Index.ToString(), TopologicalSortForm.FONT, circle.Cbrush, circle.Rect, TopologicalSortForm.STRING_FORMAT);
            }
        }

        private static void DrawFrameWork(Graphics g)
        {
            int x = TopologicalSortForm.GetFormSize().Width;
            int y = TopologicalSortForm.GetFormSize().Height;
            // 按钮间隔
            g.DrawLine(Constants.PEN_BLACK, new Point(0, (int)(y * 0.8)), new Point(2560, (int)(y * 0.8)));
            // 结果间隔
            g.DrawLine(Constants.PEN_BLACK, new Point((int)(x * 0.7)), new Point((int) (x * 0.7), (int) (y * 0.8)));
        }

        private static void DrawArrow(this Graphics g)
        {
            foreach(var arrow in Statuses[StatusIndex].Arrows)
            {
                // 绘制箭头主体
                g.DrawLine(arrow.Cpen, arrow.Start, arrow.End);
                    
                // 绘制箭头头部
                DrawArrowHead(g, arrow, arrow.Cpen);
            }
            
        }

        private static void DrawArrowHead(Graphics g, Arrow arrow, Pen pen)
        {
            float dx = arrow.End.X - arrow.Start.X;
            float dy = arrow.End.Y - arrow.Start.Y;
            float length = (float)Math.Sqrt(dx * dx + dy * dy);
            if (length == 0) return; // 防止除以零错误

            float sin = dy / length;
            float cos = dx / length;

            // 箭头头部两个点的坐标
            Point headPoint1 = new Point(
                (int)(arrow.End.X - arrow.HeadSize * (cos + sin * (float)Math.Tan(arrow.HeadAngle))),
                (int)(arrow.End.Y - arrow.HeadSize * (sin - cos * (float)Math.Tan(arrow.HeadAngle)))
            );
            Point headPoint2 = new Point(
                (int)(arrow.End.X - arrow.HeadSize * (cos - sin * (float)Math.Tan(arrow.HeadAngle))),
                (int)(arrow.End.Y - arrow.HeadSize * (sin + cos * (float)Math.Tan(arrow.HeadAngle)))
            );

            // 使用三角形绘制箭头头部
            PointF[] arrowHeadPoints = { new PointF(arrow.End.X, arrow.End.Y), new PointF(headPoint1.X, headPoint1.Y), new PointF(headPoint2.X, headPoint2.Y) };
            g.FillPolygon(pen.Brush, arrowHeadPoints);
        }


        public static void ReSetInfoToCirclesAsFormReSize(double rating)
        {
            foreach(var circle in Statuses[StatusIndex].Circles)
            {
                circle.R = (int) (circle.R * rating);
                circle.X = (int) (circle.X * rating);
                circle.Y = (int) (circle.Y * rating);
                circle.Rect = new Rectangle((int)(circle.X - circle.R * Math.Cos(45)), (int)(circle.Y - circle.R * Math.Sin(45)),
                (int)(circle.R * Math.Sqrt(2)), (int)(circle.R * Math.Sqrt(2)));
            }
        }

        public static bool CheckItHasIntersectCircle(Circle c, double rating)
        {
            foreach (var circle in Statuses[StatusIndex].Circles)
            {
                if (circle.IsIntersect(c, rating))
                {
                    return true;
                }
            }
            return false;
        }

        public static int GetCirclesLength()
        {
            return Statuses[StatusIndex].Circles.Count;
        }

        public static int GetStatussLength()
        {
            return Statuses.Count;
        }

        public static void ChangeCircleColor(Circle circle)
        {
            if (circle.Cpen.Color == Color.Black)
            {
                circle.Cpen = Constants.PEN_RED;
                circle.Cbrush = Constants.BRUSH_RED;
            }
            else
            {
                circle.Cpen = Constants.PEN_BLACK;
                circle.Cbrush = Constants.BRUSH_BLACK;
            }
        }

        public static void SetArrow(int x, int y)
        {
            foreach(var thisCircle in Statuses[StatusIndex].Circles)
            {
                if (thisCircle.IsInCircle(x, y))
                {
                    foreach(var otherCircle in Statuses[StatusIndex].Circles)
                    {
                        if (otherCircle.Cpen.Color == Color.Red && !thisCircle.isSameCircle(otherCircle))
                        {
                            ChangeCircleColor(otherCircle);
                            thisCircle.AddPointTo(otherCircle);
                            thisCircle.EnterDegree++;
                            AddArrow(otherCircle, thisCircle, StatusIndex);
                            ReSetCirclesColor();
                            return;
                        }
                    }
                    ChangeCircleColor(thisCircle);
                }
            }
        }

        private static void ReSetCirclesColor()
        {  
            foreach(var circle in Statuses[StatusIndex].Circles)
            {
                if (circle.Cpen.Color != Color.Black)
                {
                    ChangeCircleColor(circle);
                }
            }
        }

        public static void ClearStatuses()
        {
            Statuses.Clear();
            Statuses.Add(new Status());
        }

        public static Status GetNowStatus()
        {
            return Statuses.ElementAt(StatusIndex);
        }

        public static void ChangeStatusesReference(List<Status> statuses)
        {
            Statuses = statuses;
        }
    }
}