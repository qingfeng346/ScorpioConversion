using System;
using System.IO;
using System.Text;
using System.Net;
using System.Threading;
using System.Collections.Generic;
public partial class TinyWindow : Gtk.Window {
	private const int READ_LENGTH = 4096;
	public class TinyPNGConfig {
		public string ApiKey;
		public string SourcePath;
		public string TargetPath;
	}
	public class TinyPNGInput {
		public int size;
		public string type;
	}
	public class TinyPNGOutput {
		public int size;
		public string type;
		public int width;
		public int height;
		public float ratio;
		public string url;
	}
	public class TinyPNGResponseData {
		public TinyPNGInput input;
		public TinyPNGOutput output;
		public string error;
		public string message;
	}
	public TinyWindow () : base (Gtk.WindowType.Toplevel)
	{
		this.Build ();
		ConversionUtil.Bind (TextBoxApiKey, ConfigKey.TinyApiKey, ConfigFile.TinyConfig);
		ConversionUtil.Bind (TextBoxSource, ConfigKey.TinySourcePath, ConfigFile.TinyConfig);
		ConversionUtil.Bind (TextBoxTarget, ConfigKey.TinyTargetPath, ConfigFile.TinyConfig);
	}
	void StartRun(ThreadStart start)
	{
		new Thread(start).Start();
	}
	void EndRun()
	{
	}
	protected void OnClickStart (object sender, EventArgs e)
	{
		StartRun (() => {
			StartTiny();
			EndRun ();
		});
	}
 	private void StartTiny()
    {
		var apiKey = this.TextBoxApiKey.Text;
		var source = this.TextBoxSource.Text;
		var target = this.TextBoxTarget.Text;
		var jpgFiles = Directory.GetFiles(source, "*.jpg", SearchOption.AllDirectories);
		var pngFiles = Directory.GetFiles(source, "*.png", SearchOption.AllDirectories);
		var files = new List<string>();
		files.AddRange(jpgFiles);
		files.AddRange(pngFiles);
		int length = files.Count;
        if (length == 0) return;
		Progress.Count = length;
        for (int i = 0; i < length; ++i) {
            var file = files[i];
			var fileName = file.Replace("\\","/").Replace (source.Replace("\\", "/"), "");
			Progress.Current = i + 1;
			Logger.info ("正在上传 : " + fileName + "  " + (i + 1) + "/" + length);
            var result = Upload("https://api.tinypng.com/shrink", apiKey, file);
            var data = JsonUtil.JsonToObject<TinyPNGResponseData>(result);
			ConversionLogger.info ("正在下载 : " + fileName);
			Download(data.output.url, target + "/" + fileName);
            Logger.info("下载文件[{0}]完成,原大小[{1}],压缩后文件大小[{2}],压缩率[{3:N2}%]", fileName, Util.GetMemory(data.input.size), Util.GetMemory(data.output.size), Convert.ToDouble(data.output.size) / Convert.ToDouble(data.input.size) * 100);
        }
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
        reader.Close();
        fileStream.Close();
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
		string path = System.IO.Path.GetDirectoryName (filename);
		if (!Directory.Exists (path)) Directory.CreateDirectory (path);
        Stream fileStream = new FileStream(filename, FileMode.Create);
        int readSize = 0;
        byte[] bytes = new byte[READ_LENGTH];
        while ((readSize = httpStream.Read(bytes, 0, READ_LENGTH)) > 0) {
            fileStream.Write(bytes, 0, readSize);
        }
        httpStream.Close();
        fileStream.Close();
    }
}


