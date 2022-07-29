using Shell32;
using System;
using System.IO;
using System.Net.Http;
using System.Windows.Forms;
using System.Configuration;
using System.Reflection;
using Octokit;
using System.Diagnostics;

namespace Roblox_Oof_Replacer
{
    public partial class RobloxOofReplacer : Form
    {
        public RobloxOofReplacer()
        {
            InitializeComponent();
        }

        public static string GetShortcutTargetFile(string shortcutFilename)
        {
            string pathOnly = System.IO.Path.GetDirectoryName(shortcutFilename);
            string filenameOnly = System.IO.Path.GetFileName(shortcutFilename);

            Shell shell = new Shell();
            Folder folder = shell.NameSpace(pathOnly);
            FolderItem folderItem = folder.ParseName(filenameOnly);
            if (folderItem != null)
            {
                Shell32.ShellLinkObject link = (Shell32.ShellLinkObject)folderItem.GetLink;
                return link.Path;
            }

            return string.Empty;
        }


        private void button3_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.FileName == String.Empty)
                openFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                label1.Text = openFileDialog1.FileName;
                folderBrowserDialog1.SelectedPath = String.Empty;
                Properties.Settings.Default.ShortcutFileName = openFileDialog1.FileName;
            }
        }

        /* private void label_TextChanged(object sender, EventArgs e)
        {
            Label lbl = (Label)sender;
            if (lbl.Image != null) return;
            using (Graphics cg = lbl.CreateGraphics())
            {
                SizeF lblsize = new SizeF(lbl.Width, lbl.Height);
                SizeF textsize = cg.MeasureString(lbl.Text, lbl.Font, lblsize);
                while (textsize.Width > lblsize.Width - (lblsize.Width * 0.1))
                {
                    lbl.Font = new Font(lbl.Font.Name, lbl.Font.Size - 1, lbl.Font.Style);
                    textsize = cg.MeasureString(lbl.Text, lbl.Font, lblsize);
                    if (lbl.Font.Size < 6) break;
                }
            }
        }
        */

        private void button4_Click(object sender, EventArgs e)
        {
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                label2.Text = openFileDialog2.FileName;
                Properties.Settings.Default.SoundFileName = openFileDialog2.FileName;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.SelectedPath == String.Empty)
            {
                var robloxpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Roblox", "Versions");
                if (Directory.Exists(robloxpath))
                {
                    folderBrowserDialog1.InitialDirectory = robloxpath;
                }
                else
                    folderBrowserDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            }
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                label1.Text = folderBrowserDialog1.SelectedPath;
                openFileDialog1.FileName = String.Empty;
                Properties.Settings.Default.ShortcutFileName = folderBrowserDialog1.SelectedPath;
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (!File.Exists(openFileDialog1.FileName) && !Directory.Exists(folderBrowserDialog1.SelectedPath))
            {
                MessageBox.Show("Choose your Roblox Shortcut/Folder!", "Error", MessageBoxButtons.OK);
                return;
            }
            string? ooflocation = null;
            if (File.Exists(openFileDialog1.FileName))
            {
                if (Path.GetExtension(openFileDialog1.FileName) != ".lnk")
                {
                    MessageBox.Show("Shortcut file provided is not a .lnk file!", "Error", MessageBoxButtons.OK);
                    return;
                }
                var location = GetShortcutTargetFile(openFileDialog1.FileName);
                if (location == string.Empty)
                {
                    MessageBox.Show("Failed to locate target of shortcut!", "Error", MessageBoxButtons.OK);
                    return;
                }
                ooflocation = Path.Combine(location, @"..\", "content", "sounds", "ouch.ogg");
            }
            else if (Directory.Exists(folderBrowserDialog1.SelectedPath))
            {
                ooflocation = Path.Combine(folderBrowserDialog1.SelectedPath, "content", "sounds", "ouch.ogg");
            }  
            if (ooflocation == null || !File.Exists(ooflocation))
            {
                MessageBox.Show("Couldn't find ouch.ogg! Shortcut target or folder may not be Roblox OR the path of the sound may have changed. Your Roblox also might not be on the latest version.", "Error", MessageBoxButtons.OK);
                return;
            }
            var oofname = openFileDialog2.SafeFileName;
            if (openFileDialog2.FileName == string.Empty)
            {
                oofname = "Old Oof";
                using (HttpClient client = new())
                {
                    byte[] fileBytes = await client.GetByteArrayAsync("https://www.dropbox.com/s/8204ehoprc90zvf/uuhhh.ogg?dl=1");
                    File.WriteAllBytes(ooflocation, fileBytes);
                }
            }
            else
            {
                if (!File.Exists(openFileDialog2.FileName))
                {
                    MessageBox.Show("Sound file provided does not exist.", "Error", MessageBoxButtons.OK);
                    return;
                }
                if (Path.GetExtension(openFileDialog2.FileName) != ".ogg")
                {
                    MessageBox.Show("Sound file provided is not an ogg!", "Error", MessageBoxButtons.OK);
                    return;
                }
                File.Copy(openFileDialog2.FileName, ooflocation, true);
            }
            MessageBox.Show("Successfully replaced current oof sound with " + oofname + "!", "Success", MessageBoxButtons.OK);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Reset();
            openFileDialog1.FileName = Properties.Settings.Default.ShortcutFileName;
            openFileDialog2.FileName = Properties.Settings.Default.SoundFileName;
            folderBrowserDialog1.SelectedPath = string.Empty;
            label1.Text = Properties.Settings.Default.ShortcutFileName;
            label2.Text = "Old Oof";
            var desktopshortcut = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Roblox Player.lnk");
            if (File.Exists(desktopshortcut))
            {
                openFileDialog1.FileName = desktopshortcut;
                Properties.Settings.Default.ShortcutFileName = desktopshortcut;
                label1.Text = desktopshortcut;
            }
        }

        private void RobloxOofReplacer_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.Save();
        }

        private async void RobloxOofReplacer_Load(object sender, EventArgs e)
        {
            version.Text = "v" + System.Windows.Forms.Application.ProductVersion;
#if DEBUG
            version.Text += "-Debug";
#endif
            Version assemblyversion = Assembly.GetExecutingAssembly().GetName().Version;
            versionToolTip.SetToolTip(version, "Build " + assemblyversion.Build + ", Revision " + assemblyversion.Revision);
            var desktopshortcut = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Roblox Player.lnk");
            if (Properties.Settings.Default.ShortcutFileName != string.Empty)
            {
                if (File.Exists(Properties.Settings.Default.ShortcutFileName))
                    openFileDialog1.FileName = Properties.Settings.Default.ShortcutFileName;
                else
                    folderBrowserDialog1.SelectedPath = Properties.Settings.Default.ShortcutFileName;

                label1.Text = Properties.Settings.Default.ShortcutFileName;
            }
            else if (File.Exists(desktopshortcut))
            {
                openFileDialog1.FileName = desktopshortcut;
                Properties.Settings.Default.ShortcutFileName = desktopshortcut;
                label1.Text = desktopshortcut;
            }
            if (Properties.Settings.Default.SoundFileName != string.Empty)
            {
                openFileDialog2.FileName = Properties.Settings.Default.SoundFileName;
                if (Properties.Settings.Default.SoundFileName == String.Empty)
                    label2.Text = "Old Oof";
                else
                    label2.Text = Properties.Settings.Default.SoundFileName;
            }
            // check for updates
            var githubclient = new GitHubClient(new ProductHeaderValue("roblox-oof-replacer"));
            var rateLimitInfo = await githubclient.Miscellaneous.GetRateLimits();
            var rateLimit = rateLimitInfo.Resources.Core;
            if (rateLimit == null || rateLimit.Remaining > 0)
            {
                var releases = await githubclient.Repository.Release.GetAll("MrMeowzz", "roblox-oof-replacer");
                var latest = releases[0];
                if (Version.Parse(latest.TagName.Remove(0, 1)) > Version.Parse(System.Windows.Forms.Application.ProductVersion))
                {
                    DialogResult result = MessageBox.Show("A new update is available! (" + latest.TagName + ") Would you like to download it now?", "New Update Available", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        using (HttpClient client = new())
                        {
                            var link = "https://github.com/MrMeowzz/roblox-oof-replacer/releases/latest/download/Roblox.Oof.Replacer-x86.exe";
                            if (Environment.Is64BitProcess)
                                link = "https://github.com/MrMeowzz/roblox-oof-replacer/releases/latest/download/Roblox.Oof.Replacer-x64.exe";

                            byte[] fileBytes = await client.GetByteArrayAsync(link);
                            var newfilename = Path.GetFileNameWithoutExtension(link) + "-NEW" + Path.GetExtension(link);
                            var oldfilename = AppDomain.CurrentDomain.FriendlyName;
                            File.WriteAllBytes(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), newfilename), fileBytes);
                            Process.Start(new ProcessStartInfo()
                            {
                                Arguments = "/C choice /C Y /N /D Y /T 3 & Del \"" + System.Windows.Forms.Application.ExecutablePath + "\" & if exist " + newfilename + " (taskkill /IM " + newfilename + " & rename \"" + newfilename + "\" \"" + oldfilename + "\" & start " + Path.GetFileName(link) + ")",
                                WindowStyle = ProcessWindowStyle.Hidden,
                                CreateNoWindow = true,
                                FileName = "cmd.exe"
                            });
                            System.Windows.Forms.Application.Exit();
                        }
                    }
                }
            }
            else
            {
                DialogResult result = MessageBox.Show("Unable to check for updates, a new one may be available. Would you like to go to the latest release?", "Unable to Check for Updates", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    Process.Start(new ProcessStartInfo()
                    {
                        FileName = "https://github.com/MrMeowzz/roblox-oof-replacer/releases/latest/",
                        UseShellExecute = true
                    });
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.ShortcutFileName = Properties.Settings.Default.GetType()
                                                 .GetProperty(nameof(Properties.Settings.Default.ShortcutFileName))
                                                 .GetCustomAttribute<System.Configuration.DefaultSettingValueAttribute>()
                                                 .Value;
            openFileDialog1.FileName = Properties.Settings.Default.ShortcutFileName;
            folderBrowserDialog1.SelectedPath = string.Empty;
            label1.Text = Properties.Settings.Default.ShortcutFileName;
            var desktopshortcut = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Roblox Player.lnk");
            if (File.Exists(desktopshortcut))
            {
                openFileDialog1.FileName = desktopshortcut;
                Properties.Settings.Default.ShortcutFileName = desktopshortcut;
                label1.Text = desktopshortcut;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.SoundFileName = Properties.Settings.Default.GetType()
                                                 .GetProperty(nameof(Properties.Settings.Default.SoundFileName))
                                                 .GetCustomAttribute<System.Configuration.DefaultSettingValueAttribute>()
                                                 .Value;
            openFileDialog2.FileName = Properties.Settings.Default.SoundFileName;
            label2.Text = "Old Oof";
        }

        private void label1_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] data = (string[])e.Data.GetData(DataFormats.FileDrop, false);
                if (File.Exists(data[0]) && Path.GetExtension(data[0]) == ".lnk")
                    openFileDialog1.FileName = Path.GetFullPath(data[0]);
                    label1.Text = Path.GetFullPath(data[0]);
            }
        }

        private void label1_DragEnter(object sender, DragEventArgs e)
        {
            string[] data = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            if (e.Data.GetDataPresent(DataFormats.FileDrop) && File.Exists(data[0]) && Path.GetExtension(data[0]) == ".lnk")
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void label2_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] data = (string[])e.Data.GetData(DataFormats.FileDrop, false);
                if (File.Exists(data[0]) && Path.GetExtension(data[0]) == ".ogg")
                    openFileDialog2.FileName = Path.GetFullPath(data[0]);
                    label2.Text = Path.GetFullPath(data[0]);
            }
        }

        private void label2_DragEnter(object sender, DragEventArgs e)
        {
            string[] data = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            if (e.Data.GetDataPresent(DataFormats.FileDrop) && File.Exists(data[0]) && Path.GetExtension(data[0]) == ".ogg")
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void DisallowUrlExtensions(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string extension = Path.GetExtension(openFileDialog1.FileName);
            if (extension == ".url") 
                e.Cancel = true;
        }
    }
}