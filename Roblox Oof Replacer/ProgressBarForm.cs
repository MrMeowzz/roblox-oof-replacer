using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Handlers;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Roblox_Oof_Replacer
{
    public partial class ProgressBarForm : Form
    {
        public ProgressBarForm()
        {
            InitializeComponent();
        }

        private async void ProgressBarForm_Shown(object sender, EventArgs e)
        {
            var link = "https://github.com/MrMeowzz/roblox-oof-replacer/releases/latest/download/Roblox.Oof.Replacer-x86.exe";
            if (Environment.Is64BitProcess)
                link = "https://github.com/MrMeowzz/roblox-oof-replacer/releases/latest/download/Roblox.Oof.Replacer-x64.exe";
            var handler = new HttpClientHandler() { AllowAutoRedirect = true };
            var ph = new ProgressMessageHandler(handler);

            ph.HttpReceiveProgress += (_, args) =>
            {
                System.Diagnostics.Debug.WriteLine(args.ProgressPercentage);
                progressBar1.BeginInvoke(
                    (MethodInvoker)delegate() 
                    {
                        //workaround for animation thats annoying cause its so slow
                        if (args.ProgressPercentage == progressBar1.Maximum)
                        {
                            progressBar1.Value = args.ProgressPercentage;
                            progressBar1.Value -= 1;
                        }
                        else
                            progressBar1.Value = args.ProgressPercentage + 1;
                        progressBar1.Value = args.ProgressPercentage;
                    }
                );
            };
            string newfilename = AppDomain.CurrentDomain.FriendlyName + "-NEW" + Path.GetExtension(link);
            using (HttpClient client = new(ph))
            {
                byte[] fileBytes = await client.GetByteArrayAsync(link);
                File.WriteAllBytes(newfilename, fileBytes);
            }
            string oldfilename = AppDomain.CurrentDomain.FriendlyName + Path.GetExtension(link);
            StreamWriter sw = new StreamWriter(Path.Combine(Directory.GetCurrentDirectory(), "cmd.bat"));
            sw.WriteLine("attrib \"" + oldfilename + "\"" +
         " -a -s -r -h");
            sw.WriteLine(":Repeat");
            sw.WriteLine("del " + "\"" + oldfilename + "\"");
            sw.WriteLine("if exist \"" + oldfilename + "\"" +
               " goto Repeat");
            sw.WriteLine("rename \"" + newfilename + "\" \"" + oldfilename + "\"");
            sw.WriteLine("start \"\" \"" + oldfilename + "\"");
            sw.WriteLine("del cmd.bat");
            sw.Close();

            Process process = new Process();

            process.StartInfo.FileName = "cmd.bat";

            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;
            try
            {
                process.Start();
            }
            catch
            {
                Close();
            }
            Application.Exit();
        }
    }
}
