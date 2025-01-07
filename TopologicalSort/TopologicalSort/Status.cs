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
    public class Status
    {
        public List<Circle> Circles { get; set; }
        public List<Arrow> Arrows { get; set; }
        public List<Circle> SortedCircles { get; set; }
        public Status()
        {
            Circles = new List<Circle>();
            Arrows = new List<Arrow>();
            SortedCircles = new List<Circle>();
        }
        public Status(Status other)
        {
            // 初始化 Circles 列表
            this.Circles = new List<Circle>();
            foreach (var circle in other.Circles) { this.Circles.Add(circle.Clone()); }

            // 初始化 Arrows 列表
            this.Arrows = new List<Arrow>();
            foreach (var arrow in other.Arrows) { this.Arrows.Add(arrow.Clone()); }

            // 初始化 SortedCircles 列表
            this.SortedCircles = new List<Circle>();
            foreach (var circle in other.SortedCircles) { this.SortedCircles.Add(circle.Clone()); }
        }
        public Circle GetCircleByIndex(int index)
        {
            foreach (var circle in this.Circles) { if (circle.Index == index) { return circle; }}
            return null;
        }
        private void RemoveArrowsWithCircle(int index)
        {
            for (int i = this.Arrows.Count - 1; i >= 0; i--)
            {
                if (this.Arrows.ElementAt(i).EndCircle == index) { this.Arrows.RemoveAt(i); }
                if (this.Arrows.ElementAt(i).StartCircle == index)
                {
                    this.Circles.ElementAt(this.GetCircleLocationByIndex(this.Arrows.ElementAt(i).EndCircle)).EnterDegree--;
                    this.Arrows.RemoveAt(i);
                }
            }
        }
        public void MoveCircleFromCirlesToSortedCircles(int index)
        {
            this.SortedCircles.Add(this.GetCircleByIndex(index).Clone());
            RemoveArrowsWithCircle(index);
            this.Circles.RemoveAt(GetCircleLocationByIndex(index));
        }
        public Status Clone() { return new Status(this); }
        public Circle GetCircleAt(int index)
        {
            foreach (var circle in Circles) { if (circle.Index == index) { return circle; }}
            return null;
        }
        public int GetCircleLocationByIndex(int index)
        {
            for (int i = 0; i < this.Circles.Count; i++) { if (this.Circles.ElementAt(i).Index == index) { return i; }}
            return -1;
        }
        public int GetBlueCircleIndex()
        {
            foreach (var circle in Circles) { if (circle.GetColor() == Color.Blue) { return circle.Index; }}
            return -1;
        }
    }
}