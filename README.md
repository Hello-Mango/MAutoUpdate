

# MAutoUpdate
## 配置说明
Server.xml
```xml
<?xml version="1.0" encoding="utf-8" ?>
<ServerUpdate>
  <item>
    <ApplicationStart>demo.exe</ApplicationStart>//更新后启动的程序
    <AppName>Print</AppName>//程序名称（暂时没用）
    <MinVersion>1.0.0.0</MinVersion>//最小版本号（暂时没用）
    <ReleaseDate>2017/9/28 9:25:24</ReleaseDate>//发布时间
    <ReleaseUrl>http://localhost/WebApp/update/1.1.0.0.zip</ReleaseUrl>//下载地址
    <ReleaseVersion>1.1.0.0</ReleaseVersion>//发布版本号
    <VersionDesc>更新说明文件的链接地址信息，rtf的格式</VersionDesc>//更新界面提示信息
    <UpdateMode>Increment</UpdateMode>//Cover表示覆盖更新，Increment表示增量更新
  </item>
</ServerUpdate>
```
Local.xml
```xml
<LocalUpdate>
  <LocalVersion>1.0.0.0</LocalVersion>//本地版本号
  <LastUdpate>2016/9/28 9:25:24</LastUdpate>//最后更新时间
  <ServerUpdateUrl>http://localhost/TestWeb/Server.xml</ServerUpdateUrl>//server.xml文件地址
</LocalUpdate>
```
## 使用方法
启动更新程序方法
```C#
 [STAThread]
        static void Main()
        {
            String path = AppDomain.CurrentDomain.BaseDirectory + "MAutoUpdate.exe";//程序路径
            //同时启动自动更新程序
            if (File.Exists(path))//判断是否存在更新程序
            {
                ProcessStartInfo processStartInfo = new ProcessStartInfo()
                {
                    FileName = "MAutoUpdate.exe",
                    Arguments = " MAutoUpdate.Test 1"//参数信息,程序名称 0：弹窗更新 1：静默更新
                };
                Process proc = Process.Start(processStartInfo);
                if (proc != null)
                {
                    proc.WaitForExit();
                }
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
```
