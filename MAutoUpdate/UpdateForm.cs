using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace MAutoUpdate
{
    public partial class UpdateForm : Form
    {
        public delegate void UpdateUI(int step);//声明一个更新主线程的委托
        public UpdateUI UpdateUIDelegate;
        private UpdateWork work;
        public UpdateForm(UpdateWork _work)
        {
            work = _work;
            InitializeComponent();
            UpdateUIDelegate = new UpdateUI((obj) =>
            {
                this.updateBar.Value = obj;
            });
            work.OnUpdateProgess += new UpdateWork.UpdateProgess((obj) =>
              {
                  this.Invoke(UpdateUIDelegate, (int)obj);
              });
        }

        private void UpdateForm_Load(object sender, EventArgs e)
        {
            ThreadPool.QueueUserWorkItem((obj) =>
            {
                try
                {
                    work.Do();
                    this.DialogResult = DialogResult.OK;
                }
                catch (Exception EX)
                {
                    MessageBox.Show(EX.Message);
                }
            });
        }
    }
}
