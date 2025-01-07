using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace TopologicalSort
{
    public partial class TopologicalSortForm : Form
    {
        public TopologicalSortForm()
        {
            InitializeComponent();
            initCustomComponent();
            // 双缓冲，防止闪烁
            this.DoubleBuffered = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            // 更新比例
            Rating = this.Size.Height * 1.0 / TopologicalSortForm.Csize.Height;
            // 更新csize
           TopologicalSortForm.Csize.Width = (int) (TopologicalSortForm.Csize.Width * Rating);
            TopologicalSortForm.Csize.Height = (int) (TopologicalSortForm.Csize.Height * Rating);
            // 更新圆
            GraphicsUtil.ReSetInfoToCirclesAsFormReSize(Rating);
            // 更新按钮
            ReSizeButton(Button_create);
            ReSizeButton(Button_select);
            ReSizeButton(Button_delete);
            // 重新绘制
            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            GraphicsUtil.DrawGraphics(e.Graphics);
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            // 新建状态
            if (Status == STATUS_CREATE &&
                !GraphicsUtil.CheckItHasIntersectCircle(new Circle(Constants.PEN_BLACK, Constants.BRUSH_BLACK,
                    e.X, e.Y, (int)(R_CIRCLE * Rating), -1), this.Rating) &&
                GraphicsUtil.GetCirclesLength() < 10 &&
                !IsOverFrameWork(e.X, e.Y))
            {
                GraphicsUtil.AddCircle(e.X, e.Y, (int) (R_CIRCLE * Rating), GraphicsUtil.StatusIndex);
            }
            // 编辑状态
            else if (Status == STATUS_SELECT)
            {
                GraphicsUtil.SetArrow(e.X, e.Y);
            }
            // 删除状态
            else if(Status == STATUS_DELETE)
            {
                GraphicsUtil.DeleteCircleAndArrow(e.X, e.Y);
            }
            else
            {
                ;
            }
            this.Invalidate();
        }

        // 获取csise
        public static Size GetFormSize() 
        {
            return Csize;
        }

        // 检查圆是否与框架边界重合
        public static bool IsOverFrameWork(int x, int y)
        {
            return x + 40 > GetFormSize().Width * 0.7 ||
                y + 40 > GetFormSize().Height * 0.8;
        }

        private void ReSizeButton(Button button)
        {
            int button_w = button.Size.Width;
            int button_h = button.Size.Height;
            int button_x = button.Location.X;
            int button_y = button.Location.Y;
            button.Size = new Size((int)(button_w * Rating), (int)(button_h * Rating));
            button.Location = new Point((int)(button_x * Rating), (int)(button_y * Rating));
        }

        private void button_create_MouseClick(object sender, MouseEventArgs e)
        {
            Status = STATUS_CREATE;
        }

        private void button_select_MouseClick(object sender, MouseEventArgs e)
        {
            Status = STATUS_SELECT;
        }

        private void button_delete_MouseClick(object sender, MouseEventArgs e)
        {
            Status = STATUS_DELETE;
        }

        private void button_lastStep_MouseClick(object sender, MouseEventArgs e)
        {
            Status = STATUS_LAST_STEP;
            if (GraphicsUtil.StatusIndex - 1 >= 0)
            {
                GraphicsUtil.StatusIndex--;
            }
            else
            {
                DialogResult tip = MessageBox.Show("当前状态已经是初始状态！");
            }
            ResetLabelText(GraphicsUtil.GetNowStatus(), GraphicsUtil.StatusIndex);
            this.Invalidate();
        }

        private void button_nextStep_MouseClick(object sender, MouseEventArgs e)
        {
            Status = STATUS_NEXT_STEP;
            if (GraphicsUtil.StatusIndex + 1 < GraphicsUtil.GetStatussLength())
            {
                GraphicsUtil.StatusIndex++;
            }
            else
            {
                DialogResult tip = MessageBox.Show("当前状态已经是最终状态！");
            }
            ResetLabelText(GraphicsUtil.GetNowStatus(), GraphicsUtil.StatusIndex);
            this.Invalidate();
        }

        private void button_complete_MouseClick(object sender, MouseEventArgs e)
        {
            Status = STATUS_COMPLETE;
            if (!Sort.TopologicalSort(GraphicsUtil.GetNowStatus()))
            {
                DialogResult res = MessageBox.Show("该图不能进行拓扑排序，是否清空？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (res == DialogResult.Yes)
                {
                    GraphicsUtil.ClearStatuses();
                }
                else
                {
                    return;
                }
            }
            GraphicsUtil.StatusIndex = 0;
            this.Invalidate();
        }

        private void ResetLabelText(Status status, int statusIndex)
        {
            this.label_res.Text = "";
            foreach (Circle circle in status.SortedCircles)
            {
                this.label_res.Text += "节点" + circle.Index + "入度为0\n";
                this.label_res.Text += "删除节点" + circle.Index + "\n";
            }

            if (statusIndex % 2 == 1)
            {
                this.label_res.Text += "节点" + status.GetBlueCircleIndex() + "入度为0\n";
            }
        }
    }
}
