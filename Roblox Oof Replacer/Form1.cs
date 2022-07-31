using Shell32;
using System;
using System.IO;
using System.Net.Http;
using System.Windows.Forms;
using System.Configuration;
using System.Reflection;
using Octokit;
using System.Diagnostics;
using System.Net;
using System.Drawing;
using System.Globalization;
using System.Threading.Tasks;
using System.Linq;

namespace Roblox_Oof_Replacer
{
    public partial class RobloxOofReplacer : Form
    {
        public RobloxOofReplacer()
        {
            InitializeComponent();
        }

        public static System.Windows.Forms.Label ContextMenuLabel;

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
                if (Properties.Resources.uuhhh == null)
                {
                    using (HttpClient client = new())
                    {
                        //download file (slower)
                        byte[] fileBytes = await client.GetByteArrayAsync("https://www.dropbox.com/s/8204ehoprc90zvf/uuhhh.ogg?dl=1");
                        File.WriteAllBytes(ooflocation, fileBytes);
                    }
                }
                File.WriteAllBytes(ooflocation, Properties.Resources.uuhhh);
                /* using (HttpClient client = new())
                {
                    byte[] fileBytes = await client.GetByteArrayAsync("https://www.dropbox.com/s/8204ehoprc90zvf/uuhhh.ogg?dl=1");
                    File.WriteAllBytes(ooflocation, fileBytes);
                }
                */
            }
            else
            {
                if (!File.Exists(openFileDialog2.FileName))
                {
                    MessageBox.Show("Sound file provided does not exist.", "Error", MessageBoxButtons.OK);
                    return;
                }
                if (Path.GetExtension(openFileDialog2.FileName) != ".ogg" && Path.GetExtension(openFileDialog2.FileName) != ".wav" && Path.GetExtension(openFileDialog2.FileName) != ".mp3")
                {
                    MessageBox.Show("Sound file type is not valid! Valid Types: .ogg, .wav, .mp3", "Error", MessageBoxButtons.OK);
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

        public static bool HasInternetConnection(int timeoutMs = 10000, string url = null)
        {
            try
            {
                url ??= CultureInfo.InstalledUICulture switch
                {
                    { Name: var n } when n.StartsWith("fa") => // Iran
                        "http://www.aparat.com",
                    { Name: var n } when n.StartsWith("zh") => // China
                        "http://www.baidu.com",
                    _ =>
                        "http://www.gstatic.com/generate_204",
                };

                using (HttpClient client = new())
                {
                    client.DefaultRequestHeaders.ConnectionClose = false;
                    client.Timeout = TimeSpan.FromMilliseconds(timeoutMs);
                    using (var response = client.GetAsync(url))
                    {
                        response.Wait();
                        return response != null;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        private async void RobloxOofReplacer_Load(object sender, EventArgs e)
        {
            version.Text = "v" + System.Windows.Forms.Application.ProductVersion;
#if DEBUG
            version.Text += "-Debug";
#endif

            if (Properties.Settings.Default.SettingsUpgradeRequired)
            {
                try
                {
                    Properties.Settings.Default.Upgrade();
                    Properties.Settings.Default.SettingsUpgradeRequired = false;
                    Properties.Settings.Default.Save();
                }
                catch (Exception ex)
                {
                    DialogResult result = MessageBox.Show("Failed to import old Settings! " + ex, "Error", MessageBoxButtons.RetryCancel);
                    if (result == DialogResult.Retry)
                    {
                        RobloxOofReplacer_Load(sender, e);
                    }
                    return;
                }
            }

            if (Properties.Settings.Default.PatchNotes != String.Empty && Properties.Settings.Default.PatchNotesVersion != String.Empty)
            {
                PatchNotesForm form = new PatchNotesForm();
                form.Location = Location;
                Control[] title = form.Controls.Find("label1", true);
                if (title.Length > 0 && title[0] != null)
                    title[0].Text = "Patch Notes " + Properties.Settings.Default.PatchNotesVersion;
                Control[] description = form.Controls.Find("label2", true);
                if (description.Length > 0 && description[0] != null)
                {
                    int distance = form.Height - (description[0].Location.Y + description[0].Height);
                    description[0].Text = Properties.Settings.Default.PatchNotes;
                    form.Height = description[0].Location.Y + description[0].Height + distance;
                }

                Properties.Settings.Default.PatchNotes = String.Empty;
                Properties.Settings.Default.PatchNotesVersion = String.Empty;
                form.ShowDialog();
            }

            Version assemblyversion = Assembly.GetExecutingAssembly().GetName().Version;
            versionToolTip.SetToolTip(version, "Build " + assemblyversion.Build + ", Revision " + assemblyversion.Revision);
            var desktopshortcut = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Roblox Player.lnk");
            if (Properties.Settings.Default.ShortcutFileName != string.Empty && (File.Exists(Properties.Settings.Default.ShortcutFileName) || Directory.Exists(Properties.Settings.Default.ShortcutFileName)))
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
            else
            {
                Properties.Settings.Default.ShortcutFileName = Properties.Settings.Default.GetType()
                                                 .GetProperty(nameof(Properties.Settings.Default.ShortcutFileName))
                                                 .GetCustomAttribute<System.Configuration.DefaultSettingValueAttribute>()
                                                 .Value;
            }
            if (Properties.Settings.Default.SoundFileName != string.Empty && File.Exists(Properties.Settings.Default.SoundFileName))
            {
                    openFileDialog2.FileName = Properties.Settings.Default.SoundFileName;
                    if (Properties.Settings.Default.SoundFileName == String.Empty)
                        label2.Text = "Old Oof";
                    else
                        label2.Text = Properties.Settings.Default.SoundFileName;
            }
            else
            {
                Properties.Settings.Default.SoundFileName = Properties.Settings.Default.GetType()
                                                 .GetProperty(nameof(Properties.Settings.Default.SoundFileName))
                                                 .GetCustomAttribute<System.Configuration.DefaultSettingValueAttribute>()
                                                 .Value;
            }
            // check for updates
            if (!HasInternetConnection())
            {
                DialogResult result = MessageBox.Show("An Internet connection is required to check for updates.", "Unable to Check for Updates", MessageBoxButtons.RetryCancel);
                if (result == DialogResult.Retry)
                {
                    RobloxOofReplacer_Load(sender, e);
                }
                return;
            }
            var githubclient = new GitHubClient(new ProductHeaderValue("roblox-oof-replacer"));
            var rateLimitInfo = await githubclient.Miscellaneous.GetRateLimits();
            var rateLimit = rateLimitInfo.Resources.Core;
            if (rateLimit == null || rateLimit.Remaining > 0)
            {
                var releases = await githubclient.Repository.Release.GetAll("MrMeowzz", "roblox-oof-replacer");
                var latest = releases[0];
                if (Version.Parse(latest.TagName.Remove(0, 1)) > Version.Parse(System.Windows.Forms.Application.ProductVersion) && !latest.Prerelease)
                {
                    DialogResult result = MessageBox.Show("A new update is available! (" + latest.TagName + ") Would you like to download it now?", "New Update Available", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        using (HttpClient client = new())
                        {
                            var link = "https://github.com/MrMeowzz/roblox-oof-replacer/releases/latest/download/Roblox.Oof.Replacer-x86.exe";
                            if (Environment.Is64BitProcess)
                                link = "https://github.com/MrMeowzz/roblox-oof-replacer/releases/latest/download/Roblox.Oof.Replacer-x64.exe";
                            var response = await client.GetAsync(link);

                            if (response.StatusCode != HttpStatusCode.OK)
                            {
                                result = MessageBox.Show("There was an error trying to get the download file. It may not be uploaded yet or it is temporarily removed. Would you like to go to the releases page?", "Error", MessageBoxButtons.YesNo);
                                if (result == DialogResult.Yes)
                                {
                                    Process.Start(new ProcessStartInfo()
                                    {
                                        FileName = "https://github.com/MrMeowzz/roblox-oof-replacer/releases/latest/",
                                        UseShellExecute = true
                                    });
                                }
                                return;
                            }
                            Hide();
                            ProgressBarForm form = new ProgressBarForm();
                            form.Location = Location;
                            form.Show();
                            Control[] title = form.Controls.Find("label1", true);
                            if (title.Length > 0 && title[0] != null)
                                title[0].Text = "Downloading " + latest.TagName;
                            Control[] description = form.Controls.Find("label2", true);
                            Control[] progressbar = form.Controls.Find("progressBar1", true);
                            int? descriptiondistance = null;
                            if (description.Length > 0 && description[0] != null)
                            {
                                if (progressbar.Length > 0 && progressbar[0] != null)
                                    descriptiondistance = progressbar[0].Location.Y - (description[0].Location.Y + description[0].Height);
                                string cutoffstring = latest.Body;
                                if (cutoffstring.Split("\n").Last().Contains("Commit Changes"))
                                {
                                    int endinglines = 2;
                                    for (int i = 0; i < endinglines; i++)
                                        cutoffstring = cutoffstring.Remove(cutoffstring.LastIndexOf(Environment.NewLine));
                                }
                                description[0].Text = cutoffstring;
                                Properties.Settings.Default.PatchNotes = cutoffstring;
                                Properties.Settings.Default.PatchNotesVersion = latest.TagName;
                            }
                            
                            if (progressbar.Length > 0 && progressbar[0] != null && description.Length > 0 && description[0] != null)
                            {
                                int distance = form.Height - (progressbar[0].Location.Y + progressbar[0].Height);
                                if (descriptiondistance != null)
                                    progressbar[0].Location = new Point(progressbar[0].Location.X, (int)(description[0].Location.Y + description[0].Height + descriptiondistance));
                                form.Height = progressbar[0].Location.Y + progressbar[0].Height + distance;
                            }
                            /*
                            byte[] fileBytes = await client.GetByteArrayAsync(link);
                            var newfilename = Path.GetFileNameWithoutExtension(link) + "-NEW" + Path.GetExtension(link);
                            var oldfilename = AppDomain.CurrentDomain.FriendlyName;
                            File.WriteAllBytes(newfilename, fileBytes);
                            Process.Start(new ProcessStartInfo()
                            {
                                Arguments = "/C choice /C Y /N /D Y /T 3 & Del \"" + System.Windows.Forms.Application.ExecutablePath + "\" & if exist " + newfilename + " (taskkill /IM " + newfilename + " & rename \"" + newfilename + "\" \"" + oldfilename + "\" & start " + Path.GetFileName(link) + ")",
                                WindowStyle = ProcessWindowStyle.Hidden,
                                CreateNoWindow = true,
                                FileName = "cmd.exe"
                            });                            
                            System.Windows.Forms.Application.Exit();
                            */
                        }
                    }
                }
            }
            else
            {
                DialogResult result = MessageBox.Show("Unable to check for updates, a new one may be available. Would you like to go to the releases page?", "Unable to Check for Updates", MessageBoxButtons.YesNo);
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
                if (File.Exists(data[0]) && (Path.GetExtension(data[0]) == ".ogg" || Path.GetExtension(data[0]) == ".wav" || Path.GetExtension(data[0]) == ".mp3"))
                    openFileDialog2.FileName = Path.GetFullPath(data[0]);
                    label2.Text = Path.GetFullPath(data[0]);
            }
        }

        private void label2_DragEnter(object sender, DragEventArgs e)
        {
            string[] data = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            if (e.Data.GetDataPresent(DataFormats.FileDrop) && File.Exists(data[0]) && (Path.GetExtension(data[0]) == ".ogg" || Path.GetExtension(data[0]) == ".wav" || Path.GetExtension(data[0]) == ".mp3"))
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

        private void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch(e.ClickedItem.Text)
            {
                case "Copy Path":
                    if (ContextMenuLabel != null)
                        Clipboard.SetText(ContextMenuLabel.Text);
                    break;
            }
        }

        private void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ContextMenuStrip owner = sender as ContextMenuStrip;
            if (owner != null)
            {
                System.Windows.Forms.Label sourceControl = (System.Windows.Forms.Label)owner.SourceControl;
                if (sourceControl.Text == "Old Oof")
                {
                    e.Cancel = true;
                    return;
                }
                ContextMenuLabel = (System.Windows.Forms.Label)sourceControl;
            }
        }
    }
}