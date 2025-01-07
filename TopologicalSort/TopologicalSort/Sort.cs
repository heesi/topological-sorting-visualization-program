using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopologicalSort
{
    public static class Sort
    {
        public static bool TopologicalSort(Status nowStatus)
        {
            List<Status> res = new List<Status>();
            res.Add(nowStatus.Clone());
            Status status = nowStatus.Clone();


            // 排序
            while (getNumOfZeroCircle(status) > 0)
            {
                // 把入度为 0 的圆标蓝
                foreach (var circle in status.Circles)
                {
                    if (circle.EnterDegree == 0)
                    {
                        circle.ChangeColor(Color.Blue);
                        break;
                    }
                }
                res.Add(status.Clone());

                // 将蓝色圆移动到排序List
                Status tempStatus = status.Clone();
                foreach (var circle in tempStatus.Circles)
                {
                    if (circle.GetColor() == Color.Blue)
                    {
                        status.GetCircleAt(circle.Index).ChangeColor(Color.Black);
                        status.MoveCircleFromCirlesToSortedCircles(circle.Index);
                    }
                }
                res.Add(status.Clone());
            }
            
            if (status.Circles.Count > 0)
            {
                return false;
            }

            GraphicsUtil.ChangeStatusesReference(res);
            return true;
        }

        private static int getNumOfZeroCircle(Status status)
        {
            int res = 0;
            foreach (var circle in status.Circles)
            {
                if (circle.EnterDegree == 0)
                {
                    res++;
                }
            }
            return res;
        }
    }
}
