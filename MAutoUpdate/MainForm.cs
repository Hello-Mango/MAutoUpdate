using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace MAutoUpdate
{
    public partial class MainForm : Form
    {
        UpdateWork updateWork;
        public MainForm(UpdateWork _updateWork)
        {
            InitializeComponent();
            updateWork = _updateWork;
            var res = _updateWork.UpdateVerList[_updateWork.UpdateVerList.Count - 1].VersionDesc;

            var temp = WebRequest.Create(res);
            var stream = temp.GetResponse().GetResponseStream();
            using (StreamReader reader = new StreamReader(stream, System.Text.Encoding.Default))
            {
                string text = reader.ReadToEnd();
                this.lblContent.Text = text;
            }
            //foreach (var item in _updateWork.UpdateVerList[_updateWork.UpdateVerList.Count - 1].VersionDesc.Split('$'))
            //{
            //    this.lblContent.Text = this.lblContent.Text + item + Environment.NewLine;
            //}
        }
        #region 让窗体变成可移动
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

        [DllImport("User32.dll")]
        private static extern IntPtr WindowFromPoint(Point p);

        public const int WM_SYSCOMMAND = 0x0112;
        public const int SC_MOVE = 0xF010;
        public const int HTCAPTION = 0x0002;
        private IntPtr moveObject = IntPtr.Zero;    //拖动窗体的句柄

        private void PNTop_MouseDown(object sender, MouseEventArgs e)
        {
            if (moveObject == IntPtr.Zero)
            {
                if (this.Parent != null)
                {
                    moveObject = this.Parent.Handle;
                }
                else
                {
                    moveObject = this.Handle;
                }
            }
            ReleaseCapture();
            SendMessage(moveObject, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
        }

        #endregion


        /// <summary>
        /// 如果以后更新,则将更新程序关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdateLater_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnUpdateNow_Click(object sender, EventArgs e)
        {
            this.Visible = false;//隐藏当前窗口
            UpdateForm updateForm = new UpdateForm(updateWork);
            if (updateForm.ShowDialog() == DialogResult.OK)
            {
                Application.Exit();
            }
        }

        private void btnIgnore_Click(object sender, EventArgs e)
        {
            updateWork.IgnoreThisVersion();
            Application.Exit();
        }
    }
}
