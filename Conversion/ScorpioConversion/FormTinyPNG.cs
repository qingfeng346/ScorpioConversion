using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Threading;
namespace ScorpioConversion
{
    public partial class FormTinyPNG : Form {
        private const int READ_LENGTH = 4096;
        private string ProgressText = "";
        private string Working = "";
        private bool timeEnable = false;        //计时器是否能使用
        private TinyPNGConfig Config = new TinyPNGConfig();
        public FormTinyPNG()
        {
            InitializeComponent();
        }
        private void FormTinyPNG_Load(object sender, EventArgs e)
        {
            this.timerProgress.Enabled = true;
            this.Config = GetConfig();
            this.textSourcePath.Text = this.Config.SourcePath;
            this.textTargetPath.Text = this.Config.TargetPath;
            this.textApiKey.Text = this.Config.ApiKey;
        }
        void SetEnable(bool enable)
        {
            this.textSourcePath.Enabled = enable;
            this.textTargetPath.Enabled = enable;
            this.textApiKey.Enabled = enable;
            this.buttonTransform.Enabled = enable;
        }
        void StartRun(ThreadStart start)
        {
            SetEnable(false);
            new Thread(start).Start();
        }
        void EndRun()
        {
            timeEnable = false;
        }
        private TinyPNGConfig GetConfig()
        {
            string str = FileUtil.GetFileString(Util.CurrentDirectory + "/TinyPNGConfig.ini");
            if (string.IsNullOrEmpty(str)) return new TinyPNGConfig();
            return JsonUtil.JsonToObject<TinyPNGConfig>(str);
        }
        private void SaveConfig()
        {
            FileUtil.CreateFile(Util.CurrentDirectory + "/TinyPNGConfig.ini", JsonUtil.ObjectToJson(Config));
        }
        private void timerProgress_Tick(object sender, EventArgs e)
        {
            if (timeEnable == false) {
                timeEnable = true;
                SetEnable(true);
            }
            this.toolStripStatusProgress.Text = ProgressText;
            this.toolStripStatusWorking.Text = Working;
        }
        private void buttonTransform_Click(object sender, EventArgs e)
        {
            StartRun(() => {
                StartTiny();
                EndRun();
            });
        }
        private void StartTiny()
        {
            var apiKey = this.Config.ApiKey;
            var outPath = this.Config.TargetPath;
            var jpgFiles = Directory.GetFiles(this.Config.SourcePath, "*.jpg", SearchOption.AllDirectories);
            var pngFiles = Directory.GetFiles(this.Config.SourcePath, "*.png", SearchOption.AllDirectories);
            var files = new List<string>();
            files.AddRange(jpgFiles);
            files.AddRange(pngFiles);
            int length = files.Count;
            if (length == 0) return;
            for (int i=0;i< length; ++i) {
                var file = files[i];
                var fileName = Path.GetFileName(file);
                this.ProgressText = "进度 : " + (i + 1) + "/" + length;
                this.Working = "正在上传 : " + fileName;
                Console.WriteLine(file);
                var result = Upload("https://api.tinypng.com/shrink", apiKey, file);
                Console.WriteLine(result);
                var data = JsonUtil.JsonToObject<TinyPNGResponseData>(result);
                this.Working = "正在下载 : " + fileName;
                Download(data.output.url, outPath + "/" + fileName);
            }
            this.Working = "全部处理完成";
        }
        private string Upload(string url, string apikey, string file)
        {
            string username = "api";
            string password = apikey;
            string usernameAndPassword = username + ":" + password;
            FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
            BinaryReader reader = new BinaryReader(fileStream);
            HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(new Uri(url));
            httpRequest.Method = "POST";
            httpRequest.Headers.Add("Authorization", string.Format("Basic {0}", Convert.ToBase64String(new ASCIIEncoding().GetBytes(usernameAndPassword))));
            httpRequest.AllowWriteStreamBuffering = false;
            httpRequest.Credentials = new CredentialCache
            {
                {
                    new Uri(url),
                    "Basic",
                    new NetworkCredential(username, password)
                }
            };
            httpRequest.Timeout = 300000;
            httpRequest.ContentType = "image/png";
            httpRequest.ContentLength = fileStream.Length;
            Stream postStream = httpRequest.GetRequestStream();
            int readSize = 0;
            byte[] bytes = new byte[READ_LENGTH];
            while ((readSize = reader.Read(bytes, 0, READ_LENGTH)) > 0) {
                postStream.Write(bytes, 0, readSize);
            }
            postStream.Close();
            var responseStream = httpRequest.GetResponse().GetResponseStream();
            MemoryStream retStream = new MemoryStream();
            while ((readSize = responseStream.Read(bytes, 0, READ_LENGTH)) != 0) {
                retStream.Write(bytes, 0, readSize);
            }
            byte[] ret = retStream.ToArray();
            responseStream.Close();
            retStream.Close();
            return Encoding.ASCII.GetString(ret);
        }
        public void Download(string url, string filename)
        {
            HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)httpRequest.GetResponse();
            Stream httpStream = response.GetResponseStream();
            Stream fileStream = new FileStream(filename, FileMode.Create);
            int readSize = 0;
            byte[] bytes = new byte[READ_LENGTH];
            while ((readSize = httpStream.Read(bytes, 0, READ_LENGTH)) > 0) {
                fileStream.Write(bytes, 0, readSize);
            }
            httpStream.Close();
            fileStream.Close();
        }

        private void textApiKey_TextChanged(object sender, EventArgs e)
        {
            this.Config.ApiKey = textApiKey.Text;
            SaveConfig();
        }

        private void textSourcePath_TextChanged(object sender, EventArgs e)
        {
            this.Config.SourcePath = textSourcePath.Text;
            SaveConfig();
        }

        private void textTargetPath_TextChanged(object sender, EventArgs e)
        {
            this.Config.TargetPath = textTargetPath.Text;
            SaveConfig();
        }
    }
}
