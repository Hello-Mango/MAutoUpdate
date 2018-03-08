using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace MAutoUpdate
{
    public class LocalInfo
    {
        public String LocalVersion { get; set; }
        public String LastUdpate { get; set; }
        public String ServerUpdateUrl { get; set; }

        private String url = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Local.xml");

        public void SaveReg(String subKey)
        {
            RegistryKey Key;
            Key = Registry.CurrentUser;
            //Key = Key.OpenSubKey("SOFTWARE\\GoodMES\\Update");
            Key = Key.OpenSubKey(subKey, true);

            foreach (var item in this.GetType().GetProperties())
            {
                Key.SetValue(item.Name.ToString(), this.GetType().GetProperty(item.Name.ToString()).GetValue(this, null).ToString());
            }
        }
        public void LoadReg(String subKey)
        {
            //获取本地配置文件
            RegistryKey Key;
            Key = Registry.CurrentUser;
            Key = Key.OpenSubKey(subKey);

            foreach (var item in this.GetType().GetProperties())
            {
                this.GetType().GetProperty(item.Name.ToString()).SetValue(this, Key.GetValue(item.Name.ToString()).ToString(), null);
            }
            Key.Close();
        }
        public void LoadXml()
        {
            XmlDocument xdoc = new XmlDocument();
            xdoc.Load(url);
            var root = xdoc.DocumentElement;
            var listNodes = root.SelectNodes("/LocalUpdate");
            foreach (XmlNode item in listNodes)
            {
                RemoteInfo remote = new RemoteInfo();
                foreach (XmlNode pItem in item.ChildNodes)
                {
                    this.GetType().GetProperty(pItem.Name).SetValue(this, pItem.InnerText, null);
                }
            }
        }
        public void SaveXml()
        {
            XmlDocument xdoc = new XmlDocument();
            xdoc.Load(url);
            var root = xdoc.DocumentElement;
            var listNodes = root.SelectNodes("/LocalUpdate");
            foreach (XmlNode item in listNodes)
            {
                foreach (XmlNode pItem in item.ChildNodes)
                {
                    // Key.SetValue(item.Name.ToString(), this.GetType().GetProperty(item.Name.ToString()).GetValue(this, null).ToString());
                    pItem.InnerText = this.GetType().GetProperty(pItem.Name.ToString()).GetValue(this, null).ToString();
                }
            }
            xdoc.Save(url);
        }
    }
}
