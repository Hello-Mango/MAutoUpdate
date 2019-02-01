using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Diagnostics;
using System.Threading;
using System.Xml;

namespace MAutoUpdate
{
    static class Program
    {
        static bool f;
        static Process pCurrent = Process.GetCurrentProcess();
        static Mutex m = new Mutex(true, pCurrent.MainModule.FileName.Replace(":", "").Replace(@"\", "") + "MAutoUpdate", out f);//互斥，

        /// <summary>
        /// 程序主入口
        /// </summary>
        /// <param name="args">[0]程序名称，[1]静默更新 0：否 1：是 [3] 0：程序启动检测 1：手动点击检查更新按钮（在于忽略更新的情况下，手动检测时候还是要弹出来的） </param>
        [STAThread]
        static void Main(string[] args)
        {
            if (f)
            {
                try
                {
                    string programName = args[0];
                    string silentUpdate = args[1];
                    string isClickUpdate = "0";
                    string localAddress = AppDomain.CurrentDomain.BaseDirectory;
                    if (args.Length == 3)
                    {
                        isClickUpdate = args[2];
                    }
                    if (string.IsNullOrEmpty(programName) == false)
                    {
                        UpdateWork updateWork = new UpdateWork(programName, localAddress, isClickUpdate);
                        if (updateWork.UpdateVerList.Count > 0)
                        {
                            /* 当前用户是管理员的时候，直接启动应用程序 
                             * 如果不是管理员，则使用启动对象启动程序，以确保使用管理员身份运行 
                             */
                            //获得当前登录的Windows用户标示 
                            System.Security.Principal.WindowsIdentity identity = System.Security.Principal.WindowsIdentity.GetCurrent();
                            //创建Windows用户主题 
                            Application.EnableVisualStyles();
                            System.Security.Principal.WindowsPrincipal principal = new System.Security.Principal.WindowsPrincipal(identity);
                            //判断当前登录用户是否为管理员 
                            if (principal.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator))
                            {
                                if (silentUpdate == "1")
                                {
                                    updateWork.Do();
                                }
                                else
                                {
                                    Application.EnableVisualStyles();
                                    Application.SetCompatibleTextRenderingDefault(false);
                                    Application.Run(new MainForm(updateWork));
                                }
                            }
                            else
                            {
                                string result = Environment.GetEnvironmentVariable("systemdrive");
                                if (AppDomain.CurrentDomain.BaseDirectory.Contains(result))
                                {
                                    //创建启动对象 
                                    ProcessStartInfo startInfo = new ProcessStartInfo
                                    {
                                        //设置运行文件 
                                        FileName = System.Windows.Forms.Application.ExecutablePath,
                                        //设置启动动作,确保以管理员身份运行 
                                        Verb = "runas",

                                        Arguments = " " + programName + " " + silentUpdate
                                    };
                                    //如果不是管理员，则启动UAC 
                                    System.Diagnostics.Process.Start(startInfo);
                                }
                                else
                                {
                                    if (silentUpdate == "1")
                                    {
                                        updateWork.Do();
                                    }
                                    else
                                    {
                                        Application.EnableVisualStyles();
                                        Application.SetCompatibleTextRenderingDefault(false);
                                        Application.Run(new MainForm(updateWork));
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}