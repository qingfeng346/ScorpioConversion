using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using System.Net;
public class UpdateUtil
{
    public delegate void Action();
    private const string Url = "http://www.fengyuezhu.com/app.php?app=ScorpioConversion";
    private const string VersionUrl = "http://www.fengyuezhu.com/project/ScorpioConversion/version.v";
    private static readonly Encoding Encode = Encoding.UTF8;
    private static Control control;
    public static string Version { get; private set; }
    public static void Init(Control control)
    {
        UpdateUtil.control = control;
        Version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
    }
    public static void Exec(Action action)
    {
        control.Invoke(action);
    }
    public static string toString(Stream stream) {
        if (stream == null) return "";
        return Encode.GetString(toByteArray(stream));
    }
    /// <summary> 字节流转成byte[] </summary>
    public static byte[] toByteArray(Stream stream)
    {
        if (stream == null) return null;
        MemoryStream result = new MemoryStream();
        int length = 0;
        byte[] bytes = new byte[8192];
        while ((length = stream.Read(bytes, 0, 8192)) > 0) {
            result.Write(bytes, 0, length);
        }
        return result.ToArray();
    }
    public static void CheckVersion(bool showerror)
    {
        new Thread(() => {
            try
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(VersionUrl);
                request.Timeout = 30000;                    //设定超时时间30秒
                HttpWebResponse response = null;
                try {
                    response = (HttpWebResponse)request.GetResponse(); ;
                } catch (WebException ex) {
                    response = (HttpWebResponse)ex.Response;
                }
                if (response.StatusCode >= HttpStatusCode.OK && response.StatusCode < HttpStatusCode.Ambiguous) {
                    var str = toString(response.GetResponseStream());
                    Exec(() => {
                        if (str != Version) {
                            var result = MessageBox.Show("检测到最新版本 : v" + str + "\n是否立刻去下载？", "发现新版本", MessageBoxButtons.YesNoCancel);
                            if (result == DialogResult.Yes) {
                                OpenUrl();
                            }
                        } else if (showerror) {
                            MessageBox.Show("当前已经是最新版本");
                        }
                    });
                } else if (showerror) {
                    Exec(() => { MessageBox.Show("请求版本号出错 : " + (int)response.StatusCode + " " + response.StatusCode); });
                }
                response.Close();
            } catch (Exception e) {
                if (showerror) {
                    Exec(() => { MessageBox.Show("请求版本号出错 : " + e.ToString()); });
                }
            }
        }).Start();
    }
    public static void OpenUrl()
    {
        System.Diagnostics.Process.Start(Url);
    }
}