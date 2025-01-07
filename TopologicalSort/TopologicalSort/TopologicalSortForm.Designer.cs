using System.Drawing;
using System.Windows.Forms;

namespace TopologicalSort
{
    partial class TopologicalSortForm
    {
        // 运行状态
        private const int STATUS_CREATE = 0;
        private const int STATUS_SELECT = 1;
        private const int STATUS_DELETE = 2;
        private const int STATUS_LAST_STEP = 3;
        private const int STATUS_NEXT_STEP = 4;
        private const int STATUS_COMPLETE = 5;
        private int Status = STATUS_CREATE;
        
        // size
        private static int SIZE_PEN = 2;
        private static int SIZE_INDEX = 20;
        private static Size Csize = new Size(855, 482);
        
        // StringFormat
        public static StringFormat STRING_FORMAT { get; set; }
        // Font
        public static Font FONT {  get; set; }
        
        // 圆半径
        private int R_CIRCLE = 20;
        // 方法比例
        private double Rating = 1f;
        

        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.Button_create = new System.Windows.Forms.Button();
            this.Button_select = new System.Windows.Forms.Button();
            this.Button_delete = new System.Windows.Forms.Button();
            this.button_complete = new System.Windows.Forms.Button();
            this.button_lastStep = new System.Windows.Forms.Button();
            this.button_nextStep = new System.Windows.Forms.Button();
            this.label_res_top = new System.Windows.Forms.Label();
            this.label_res = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Button_create
            // 
            this.Button_create.Location = new System.Drawing.Point(50, 604);
            this.Button_create.Name = "Button_create";
            this.Button_create.Size = new System.Drawing.Size(156, 48);
            this.Button_create.TabIndex = 0;
            this.Button_create.Text = "新建";
            this.Button_create.UseVisualStyleBackColor = true;
            this.Button_create.MouseClick += new System.Windows.Forms.MouseEventHandler(this.button_create_MouseClick);
            // 
            // Button_select
            // 
            this.Button_select.Location = new System.Drawing.Point(250, 604);
            this.Button_select.Name = "Button_select";
            this.Button_select.Size = new System.Drawing.Size(156, 48);
            this.Button_select.TabIndex = 0;
            this.Button_select.Text = "选择";
            this.Button_select.UseVisualStyleBackColor = true;
            this.Button_select.MouseClick += new System.Windows.Forms.MouseEventHandler(this.button_select_MouseClick);
            // 
            // Button_delete
            // 
            this.Button_delete.Location = new System.Drawing.Point(450, 604);
            this.Button_delete.Name = "Button_delete";
            this.Button_delete.Size = new System.Drawing.Size(156, 48);
            this.Button_delete.TabIndex = 0;
            this.Button_delete.Text = "删除";
            this.Button_delete.UseVisualStyleBackColor = true;
            this.Button_delete.MouseClick += new System.Windows.Forms.MouseEventHandler(this.button_delete_MouseClick);
            // 
            // button_complete
            // 
            this.button_complete.Location = new System.Drawing.Point(650, 604);
            this.button_complete.Name = "button_complete";
            this.button_complete.Size = new System.Drawing.Size(156, 48);
            this.button_complete.TabIndex = 1;
            this.button_complete.Text = "排序";
            this.button_complete.UseVisualStyleBackColor = true;
            this.button_complete.MouseClick += new System.Windows.Forms.MouseEventHandler(this.button_complete_MouseClick);
            // 
            // button_lastStep
            // 
            this.button_lastStep.Location = new System.Drawing.Point(850, 604);
            this.button_lastStep.Name = "button_lastStep";
            this.button_lastStep.Size = new System.Drawing.Size(156, 48);
            this.button_lastStep.TabIndex = 2;
            this.button_lastStep.Text = "上一步";
            this.button_lastStep.UseVisualStyleBackColor = true;
            this.button_lastStep.MouseClick += new System.Windows.Forms.MouseEventHandler(this.button_lastStep_MouseClick);
            // 
            // button_nextStep
            // 
            this.button_nextStep.Location = new System.Drawing.Point(1050, 604);
            this.button_nextStep.Name = "button_nextStep";
            this.button_nextStep.Size = new System.Drawing.Size(156, 48);
            this.button_nextStep.TabIndex = 3;
            this.button_nextStep.Text = "下一步";
            this.button_nextStep.UseVisualStyleBackColor = true;
            this.button_nextStep.MouseClick += new System.Windows.Forms.MouseEventHandler(this.button_nextStep_MouseClick);
            // 
            // label_res_top
            // 
            this.label_res_top.Font = new System.Drawing.Font("宋体", 18F);
            this.label_res_top.Location = new System.Drawing.Point(1000, 35);
            this.label_res_top.Name = "label_res_top";
            this.label_res_top.Size = new System.Drawing.Size(160, 40);
            this.label_res_top.TabIndex = 4;
            this.label_res_top.Text = "排序结果";
            // 
            // label_res
            // 
            this.label_res.Font = new System.Drawing.Font("宋体", 11F);
            this.label_res.Location = new System.Drawing.Point(1006, 101);
            this.label_res.Name = "label_res";
            this.label_res.Size = new System.Drawing.Size(154, 453);
            this.label_res.TabIndex = 5;
            // 
            // TopologicalSortForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1258, 664);
            this.Controls.Add(this.label_res);
            this.Controls.Add(this.label_res_top);
            this.Controls.Add(this.button_nextStep);
            this.Controls.Add(this.button_lastStep);
            this.Controls.Add(this.button_complete);
            this.Controls.Add(this.Button_create);
            this.Controls.Add(this.Button_select);
            this.Controls.Add(this.Button_delete);
            this.Name = "TopologicalSortForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseDown);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.ResumeLayout(false);

        }
        #endregion
        private Button Button_create;
        private Button Button_select;
        private Button Button_delete;

        public void initCustomComponent()
        {
            // StringFormat
            STRING_FORMAT = new StringFormat();
            STRING_FORMAT.Alignment = StringAlignment.Center;
            STRING_FORMAT.LineAlignment = StringAlignment.Center;

            // Size
            FONT = new Font("宋体", SIZE_INDEX);

            this.SuspendLayout();
        }

        private Button button_complete;
        private Button button_lastStep;
        private Button button_nextStep;
        private Label label_res_top;
        private Label label_res;
    }

    
}