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
using SoundTouch.Net.NAudioSupport;
using NAudio.Wave;
using NAudio.Vorbis;
using System.Collections.Generic;

namespace Roblox_Oof_Replacer
{
    public partial class RobloxOofReplacer : Form
    {
        public RobloxOofReplacer()
        {
            InitializeComponent();
        }

        public static System.Windows.Forms.Label ContextMenuLabel;

        public static string[] ReplaceTypes = 
        {
            "Death Sound",
            "Footstep",
            "Mouse",
        };

        public static string GetShortcutTargetFile(string shortcutFilename)
        {
            string pathOnly = Path.GetDirectoryName(shortcutFilename);
            string filenameOnly = Path.GetFileName(shortcutFilename);

            Shell shell = new Shell();
            Folder folder = shell.NameSpace(pathOnly);
            FolderItem folderItem = folder.ParseName(filenameOnly);
            if (folderItem != null)
            {
                ShellLinkObject link = (ShellLinkObject)folderItem.GetLink;
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
            switch (Properties.Settings.Default.ReplaceType)
            {
                case "Death Sound": 
                case "Footstep":
                    {
                        openFileDialog2.Filter = "Sound files (*.ogg;*.mp3;*.wav)|*.ogg;*.mp3;*.wav";
                        openFileDialog2.DefaultExt = "ogg";
                        openFileDialog2.Title = "Choose Sound";
                        openFileDialog2.Multiselect = false;
                        break;
                    }
                case "Mouse":
                    {
                        openFileDialog2.Filter = "Image files (*.png;*.jpg;*.jpeg;*.gif;*.ico)|*.png;*.jpg;*.jpeg;*.gif;*.ico";
                        openFileDialog2.DefaultExt = "ogg";
                        openFileDialog2.Title = "Choose Images";
                        openFileDialog2.Multiselect = true;
                        break;
                    }
            }
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                label2.Text = openFileDialog2.FileName;
                switch (Properties.Settings.Default.ReplaceType)
                {
                    case "Death Sound":
                        {
                            Properties.Settings.Default.SoundFileName = openFileDialog2.FileName;
                            break;
                        }
                    case "Footstep":
                        {
                            Properties.Settings.Default.FootstepSoundFileName = openFileDialog2.FileName;
                            break;
                        }
                    case "Mouse":
                        {
                            label2.Text = string.Join(", ", openFileDialog2.FileNames);
                            Properties.Settings.Default.MouseImageFileName = openFileDialog2.FileNames;
                            break;
                        }
                }
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

        public static byte[] ImageToByte(Image img)
        {
            using (var stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }

        private static System.Drawing.Image resizeImage(System.Drawing.Image imgToResize, Size size)
        {
            //Get the image current width  
            int sourceWidth = imgToResize.Width;
            //Get the image current height  
            int sourceHeight = imgToResize.Height;
            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;
            //Calulate  width with new desired size  
            nPercentW = ((float)size.Width / (float)sourceWidth);
            //Calculate height with new desired size  
            nPercentH = ((float)size.Height / (float)sourceHeight);
            if (nPercentH < nPercentW)
                nPercent = nPercentH;
            else
                nPercent = nPercentW;
            //New Width  
            int destWidth = (int)(sourceWidth * nPercent);
            //New Height  
            int destHeight = (int)(sourceHeight * nPercent);
            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((System.Drawing.Image)b);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            // Draw image with new width and height  
            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();
            return (System.Drawing.Image)b;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (!File.Exists(openFileDialog1.FileName) && !Directory.Exists(folderBrowserDialog1.SelectedPath))
            {
                MessageBox.Show("Choose your Roblox Shortcut/Folder!", "Error", MessageBoxButtons.OK);
                return;
            }
            string? soundlocation = null;
            string[] cursorlocations = new string[4];
            if (File.Exists(openFileDialog1.FileName))
            {
                if (Path.GetExtension(openFileDialog1.FileName) != ".lnk")
                {
                    MessageBox.Show("Shortcut file provided is not a .lnk file!", "Error", MessageBoxButtons.OK);
                    return;
                }
                var location = GetShortcutTargetFile(openFileDialog1.FileName);
                if (location == string.Empty || location == null)
                {
                    MessageBox.Show("Failed to locate target of shortcut!", "Error", MessageBoxButtons.OK);
                    return;
                }
                switch (Properties.Settings.Default.ReplaceType)
                {
                    case "Death Sound":
                        {
                            soundlocation = Path.Combine(location, @"..\", "content", "sounds", "ouch.ogg");
                            break;
                        }
                    case "Footstep":
                        {
                            soundlocation = Path.Combine(location, @"..\", "content", "sounds", "action_footsteps_plastic.mp3");
                            break;
                        }
                    case "Mouse":
                        {
                            cursorlocations[0] = Path.Combine(location, @"..\", "content", "textures", "Cursors", "KeyboardMouse", "ArrowFarCursor.png");
                            cursorlocations[1] = Path.Combine(location, @"..\", "content", "textures", "Cursors", "KeyboardMouse", "ArrowCursor.png");
                            cursorlocations[2] = Path.Combine(location, @"..\", "content", "textures", "MouseLockedCursor.png");
                            cursorlocations[3] = Path.Combine(location, @"..\", "content", "textures", "Cursors", "KeyboardMouse", "IBeamCursor.png");
                            break;
                        }
                }
                
            }
            else if (Directory.Exists(folderBrowserDialog1.SelectedPath))
            {
                soundlocation = Path.Combine(folderBrowserDialog1.SelectedPath, "content", "sounds", "ouch.ogg");
            }
            if (cursorlocations != null && cursorlocations.Length > 0)
            {
                string? foundnull = null;
                for (int i = 0; i < cursorlocations.Length; i++)
                    if ((cursorlocations[i] == null || !File.Exists(cursorlocations[i])) && i < openFileDialog2.FileNames.Length && Path.GetFileNameWithoutExtension(cursorlocations[i]) == Path.GetFileNameWithoutExtension(openFileDialog2.FileNames[i].TrimEnd(char.Parse("\"")).TrimStart(char.Parse("\""))))
                    {
                        foundnull = Path.GetFileName(cursorlocations[i]);
                        break;
                    }
                if (foundnull != null)
                {
                    MessageBox.Show("Couldn't find " + foundnull + "! Shortcut target or folder may not be Roblox OR the path of the sound may have changed. Your Roblox also might not be on the latest version.", "Error", MessageBoxButtons.OK);
                    return;
                }
            }
            else if (soundlocation == null || !File.Exists(soundlocation))
            {
                MessageBox.Show("Couldn't find Arrow Images! Shortcut target or folder may not be Roblox OR the path of the sound may have changed. Your Roblox also might not be on the latest version.", "Error", MessageBoxButtons.OK);
                return;
            }
            if ((soundlocation == null || !File.Exists(soundlocation)) && (cursorlocations == null || cursorlocations.Length <= 0))
            {
                string message = string.Empty;
                switch (Properties.Settings.Default.ReplaceType)
                {
                    case "Death Sound":
                        {
                            message = "Couldn't find ouch.ogg! Shortcut target or folder may not be Roblox OR the path of the sound may have changed. Your Roblox also might not be on the latest version.";
                            break;
                        }
                    case "Footstep":
                        {
                            message = "Couldn't find action_footsteps_plastic.mp3! Shortcut target or folder may not be Roblox OR the path of the sound may have changed. Your Roblox also might not be on the latest version.";
                            break;
                        }
                }
                MessageBox.Show(message, "Error", MessageBoxButtons.OK);
                return;
            }
            switch (Properties.Settings.Default.ReplaceType)
            {
                case "Death Sound":
                case "Footstep":
                    {
                        var oofname = openFileDialog2.SafeFileName;
                        if (openFileDialog2.FileName == string.Empty)
                        {
                            switch (Properties.Settings.Default.ReplaceType)
                            {
                                case "Death Sound":
                                    {
                                        oofname = "Old Oof";
                                        if (Properties.Resources.uuhhh == null)
                                        {
                                            using (HttpClient client = new())
                                            {
                                                //download file (slower)
                                                byte[] fileBytes = await client.GetByteArrayAsync("https://www.dropbox.com/s/8204ehoprc90zvf/uuhhh.ogg?dl=1");
                                                File.WriteAllBytes(soundlocation, fileBytes);
                                            }
                                        }
                                        File.WriteAllBytes(soundlocation, Properties.Resources.uuhhh);
                                        /* using (HttpClient client = new())
                                        {
                                            byte[] fileBytes = await client.GetByteArrayAsync("https://www.dropbox.com/s/8204ehoprc90zvf/uuhhh.ogg?dl=1");
                                            File.WriteAllBytes(soundlocation, fileBytes);
                                        }
                                        */
                                        break;
                                    }
                                case "Footstep":
                                    {
                                        oofname = "Default Footsteps";
                                        if (Properties.Resources.action_footsteps_plastic == null)
                                        {
                                            DialogResult result = MessageBox.Show("Failed to get action_footsteps_plastic.mp3 from Resources!", "Error", MessageBoxButtons.RetryCancel);
                                            if (result == DialogResult.Retry)
                                            {
                                                button7_Click(sender, e);
                                            }
                                        }
                                        File.WriteAllBytes(soundlocation, Properties.Resources.action_footsteps_plastic);
                                        break;
                                    }
                            }
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
                            
                            if (Properties.Settings.Default.ReplaceType == "Footstep")
                            {
                                WaveStream reader = Path.GetExtension(openFileDialog2.FileName) == ".ogg"
                                    ? new VorbisWaveReader(openFileDialog2.FileName)
                                    : new MediaFoundationReader(openFileDialog2.FileName);
                                SoundTouchWaveStream stream = new(new WaveChannel32(reader));
                                stream.Rate = 0.54;
                                using (WaveFileWriter writer = new(soundlocation, stream.WaveFormat))
                                {
                                    byte[] buffer = new byte[Convert.ToInt32(stream.Length / stream.Rate)];
                                    stream.Read(buffer, 0, buffer.Length);
                                    writer.Write(buffer, 0, buffer.Length);
                                    writer.Flush();
                                }
                            }
                            else
                                File.Copy(openFileDialog2.FileName, soundlocation, true);
                        }
                        string soundtype = "oof";
                        if (Properties.Settings.Default.ReplaceType == "Footstep")
                            soundtype = "footstep";
                        MessageBox.Show("Successfully replaced current " + soundtype + " sound with " + oofname + "!", "Success", MessageBoxButtons.OK);
                        break;
                    }
                case "Mouse":
                    {

                        string[] cursornames = openFileDialog2.SafeFileNames;
                        List<string> cursorsreplaced = new();
                        if (openFileDialog2.FileName == string.Empty)
                        {
                            cursornames = new string[] { "Default Mouse" };
                            if (Properties.Resources.ArrowCursor == null)
                            {
                                DialogResult result = MessageBox.Show("Failed to get ArrowCursor.png from Resources!", "Error", MessageBoxButtons.RetryCancel);
                                if (result == DialogResult.Retry)
                                {
                                    button7_Click(sender, e);
                                }
                                return;
                            }
                            if (Properties.Resources.ArrowFarCursor == null)
                            {
                                DialogResult result = MessageBox.Show("Failed to get ArrowFarCursor.png from Resources!", "Error", MessageBoxButtons.RetryCancel);
                                if (result == DialogResult.Retry)
                                {
                                    button7_Click(sender, e);
                                }
                                return;
                            }
                            if (Properties.Resources.MouseLockedCursor == null)
                            {
                                DialogResult result = MessageBox.Show("Failed to get MouseLockedCursor.png from Resources!", "Error", MessageBoxButtons.RetryCancel);
                                if (result == DialogResult.Retry)
                                {
                                    button7_Click(sender, e);
                                }
                                return;
                            }
                            if (Properties.Resources.IBeamCursor == null)
                            {
                                DialogResult result = MessageBox.Show("Failed to get IBeamCursor.png from Resources!", "Error", MessageBoxButtons.RetryCancel);
                                if (result == DialogResult.Retry)
                                {
                                    button7_Click(sender, e);
                                }
                                return;
                            }
                            File.WriteAllBytes(cursorlocations[0], ImageToByte(Properties.Resources.ArrowFarCursor));
                            File.WriteAllBytes(cursorlocations[1], ImageToByte(Properties.Resources.ArrowCursor));
                            File.WriteAllBytes(cursorlocations[2], ImageToByte(Properties.Resources.MouseLockedCursor));
                            File.WriteAllBytes(cursorlocations[3], ImageToByte(Properties.Resources.IBeamCursor));
                            cursorsreplaced.Clear();
                            for (int i = 0; i < cursorlocations.Length; i++)
                                cursorsreplaced.Add(Path.GetFileName(cursorlocations[i]));
                        }
                        else
                        {
                            string[] trimmedfilenames = new string[Properties.Settings.Default.MouseImageFileName.Length];
                            cursornames = new string[Properties.Settings.Default.MouseImageFileName.Length];
                            for (int i = 0; i < Properties.Settings.Default.MouseImageFileName.Length; i++)
                            {
                                trimmedfilenames[i] = Properties.Settings.Default.MouseImageFileName[i].TrimEnd(char.Parse("\"")).TrimStart(char.Parse("\""));
                                cursornames[i] = Path.GetFileName(trimmedfilenames[i]);
                            }
                            for (int i = 0; i < trimmedfilenames.Length; i++)
                            {
                                if (!File.Exists(trimmedfilenames[i]))
                                {
                                    MessageBox.Show(trimmedfilenames[i]);
                                    MessageBox.Show("Image file " + Path.GetFileName(trimmedfilenames[i]) + " does not exist.", "Error", MessageBoxButtons.OK);
                                    return;
                                }
                                if (Path.GetExtension(trimmedfilenames[i]) != ".png" && Path.GetExtension(trimmedfilenames[i]) != ".jpg" && Path.GetExtension(trimmedfilenames[i]) != ".jpeg" && Path.GetExtension(trimmedfilenames[i]) != ".gif" && Path.GetExtension(trimmedfilenames[i]) != ".ico")
                                {
                                    MessageBox.Show("The type for Image file " + Path.GetFileName(trimmedfilenames[i]) + " is not valid! Valid Types: .png, .jpg, .jpeg, .gif, .ico", "Error", MessageBoxButtons.OK);
                                    return;
                                }
                            }
                            if (trimmedfilenames.Length > cursorlocations.Length)
                            {
                                MessageBox.Show("Too many files selected. Max of " + cursorlocations.Length + " is able to be selected for the ArrowFarCursor, ArrowCursor, and the IBeamCursor.", "Error", MessageBoxButtons.OK);
                                return;
                            }
                            if (trimmedfilenames.Length == 1)
                            {
                                cursorsreplaced.Clear();
                                Image image = resizeImage(Image.FromFile(trimmedfilenames[0]), new Size(64, 64));
                                for (int i = 0; i < cursorlocations.Length; i++)
                                {
                                    if (File.Exists(cursorlocations[i]))
                                        File.Delete(cursorlocations[i]);
                                    image.Save(cursorlocations[i], System.Drawing.Imaging.ImageFormat.Png);
                                    cursorsreplaced.Add(Path.GetFileName(cursorlocations[i]));
                                }
                            }
                            else
                            {
                                bool foundmatchingname = false;
                                for (int i = 0; i < trimmedfilenames.Length; i++)
                                {
                                    if (cursorlocations[i] != null && Path.GetFileNameWithoutExtension(cursorlocations[i]) == Path.GetFileNameWithoutExtension(trimmedfilenames[i]))
                                    {
                                        foundmatchingname = true;
                                        Image image = resizeImage(Image.FromFile(trimmedfilenames[i]), new Size(64, 64));
                                        if (File.Exists(cursorlocations[i]))
                                            File.Delete(cursorlocations[i]);
                                        image.Save(cursorlocations[i], System.Drawing.Imaging.ImageFormat.Png);
                                        cursorsreplaced.Add(Path.GetFileName(cursorlocations[i]));
                                    }
                                }
                                if (!foundmatchingname)
                                {
                                    for (int i = 0; i < trimmedfilenames.Length; i++)
                                    {
                                        if (cursorlocations[i] != null)
                                        {
                                            Image image = resizeImage(Image.FromFile(trimmedfilenames[i]), new Size(64, 64));
                                            if (File.Exists(cursorlocations[i]))
                                                File.Delete(cursorlocations[i]);
                                            image.Save(cursorlocations[i], System.Drawing.Imaging.ImageFormat.Png);
                                            cursorsreplaced.Add(Path.GetFileName(cursorlocations[i]));
                                        }
                                    }
                                }
                            }
                        }
                        MessageBox.Show("Successfully replaced " + string.Join(", ", cursorsreplaced) + " with " + string.Join(", ", cursornames) + "!", "Success", MessageBoxButtons.OK);
                        break;
                    }
                default:
                    if (MessageBox.Show("Unknown Error while trying to replace current type!", "Error", MessageBoxButtons.RetryCancel) == DialogResult.Retry)
                        button1_Click(sender, e);
                    break;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Reset();
            openFileDialog1.FileName = Properties.Settings.Default.ShortcutFileName;
            openFileDialog2.FileName = Properties.Settings.Default.SoundFileName;
            folderBrowserDialog1.SelectedPath = string.Empty;
            label1.Text = Properties.Settings.Default.ShortcutFileName;
            //label2.Text = "Old Oof";
            UpdateArrows();
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
                Properties.Settings.Default.ShortcutFileName = Properties.Settings.Default.Properties["ShortcutFileName"].DefaultValue as string;
            }
            
            if (Properties.Settings.Default.SoundFileName == String.Empty || !File.Exists(Properties.Settings.Default.SoundFileName))
            {
                Properties.Settings.Default.SoundFileName = Properties.Settings.Default.Properties["SoundFileName"].DefaultValue as string;
            }
            if (Properties.Settings.Default.FootstepSoundFileName == String.Empty || !File.Exists(Properties.Settings.Default.FootstepSoundFileName))
            {
                Properties.Settings.Default.FootstepSoundFileName = Properties.Settings.Default.Properties["FootstepSoundFileName"].DefaultValue as string;
            }
            UpdateArrows();
            
            if (ReplaceTypes.Contains(Properties.Settings.Default.ReplaceType))
            {
                label4.Text = Properties.Settings.Default.ReplaceType;
                UpdateArrows();
            }
            else
            {
                Properties.Settings.Default.ReplaceType = Properties.Settings.Default.Properties["ReplaceType"].DefaultValue as string;
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
            Properties.Settings.Default.ShortcutFileName = Properties.Settings.Default.Properties["ShortcutFileName"].DefaultValue as string;
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
            switch (Properties.Settings.Default.ReplaceType)
            {
                case "Death Sound":
                    {
                        Properties.Settings.Default.SoundFileName = Properties.Settings.Default.Properties["SoundFileName"].DefaultValue as string;
                        openFileDialog2.FileName = Properties.Settings.Default.SoundFileName;
                        if (Properties.Settings.Default.SoundFileName == string.Empty)
                            label2.Text = "Old Oof";
                        else
                            label2.Text = Properties.Settings.Default.SoundFileName;
                        break;
                    }
                case "Footstep":
                    {
                        Properties.Settings.Default.FootstepSoundFileName = Properties.Settings.Default.Properties["FootstepSoundFileName"].DefaultValue as string;
                        openFileDialog2.FileName = Properties.Settings.Default.FootstepSoundFileName;
                        if (Properties.Settings.Default.FootstepSoundFileName == string.Empty)
                            label2.Text = "Default Footsteps";
                        else
                            label2.Text = Properties.Settings.Default.FootstepSoundFileName;
                        break;
                    }
                case "Mouse":
                    {
                        Properties.Settings.Default.MouseImageFileName = Properties.Settings.Default.Properties["MouseImageFileName"].DefaultValue as string[];
                        openFileDialog2.FileName = string.Empty;
                        if (Properties.Settings.Default.MouseImageFileName != null)
                        {
                            for (int i = 0; i < Properties.Settings.Default.MouseImageFileName.
                                    Length; i++)
                            {
                                if (!File.Exists(Properties.Settings.Default.MouseImageFileName[i]))
                                {
                                    openFileDialog2.FileName = string.Empty;
                                    Properties.Settings.Default.MouseImageFileName = Properties.Settings.Default.Properties["MouseImageFileName"].DefaultValue as string[];
                                    break;
                                }
                                openFileDialog2.FileName += "\"" + Properties.Settings.Default.MouseImageFileName[i] + "\"";
                            }
                        }
                        if (Properties.Settings.Default.MouseImageFileName == null || Properties.Settings.Default.MouseImageFileName.Length <= 0)
                            label2.Text = "Default Mouse";
                        else
                            label2.Text = string.Join(", ", Properties.Settings.Default.MouseImageFileName);
                        break;
                    }
            }
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
            FileDialog fileDialog = sender as FileDialog;
            string extension = Path.GetExtension(fileDialog.FileName);
            if (extension == ".url" || fileDialog.FileNames.Length > 4) 
                e.Cancel = true;
        }

        private void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch(e.ClickedItem.Text)
            {
                case "Copy Path":
                    if (ContextMenuLabel != null)
                        Clipboard.SetText(ContextMenuLabel.Text.Replace(",", ""));
                    break;
                case "Open in File Explorer":
                    string argument = "/select, \"" + ContextMenuLabel.Text.Split(",")[0] + "\"";

                    Process.Start("explorer.exe", argument);
                    break;
            }
        }

        public static string AddOrdinal(int num)
        {
            if (num <= 0) return num.ToString();

            switch (num % 100)
            {
                case 11:
                case 12:
                case 13:
                    return num + "th";
            }

            switch (num % 10)
            {
                case 1:
                    return num + "st";
                case 2:
                    return num + "nd";
                case 3:
                    return num + "rd";
                default:
                    return num + "th";
            }
        }

        private void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ContextMenuStrip owner = sender as ContextMenuStrip;
            if (owner != null)
            {
                System.Windows.Forms.Label sourceControl = (System.Windows.Forms.Label)owner.SourceControl;
                if (!Directory.Exists(sourceControl.Text) && !File.Exists(sourceControl.Text) && !sourceControl.Text.Contains(","))
                {
                    e.Cancel = true;
                    return;
                }
                foreach (ToolStripMenuItem item in contextMenuStrip1.Items)
                {
                    item.DropDownItems.Clear();
                    if (sourceControl.Text.Split(",").Length > 1)
                    {
                        for (int i = 0; i < sourceControl.Text.Split(",").Length; i++)
                            item.DropDownItems.Add(AddOrdinal(i + 1) + " Path");
                    }
                }
                ContextMenuLabel = sourceControl;
            }
        }

        private void UpdateArrows()
        {
            var index = Array.IndexOf(ReplaceTypes, Properties.Settings.Default.ReplaceType);
            button8.Visible = index - 1 >= 0;
            button9.Visible = index + 1 < ReplaceTypes.Length;

            switch (Properties.Settings.Default.ReplaceType)
            {
                case "Death Sound":
                    {
                        openFileDialog2.FileName = Properties.Settings.Default.SoundFileName;
                        if (Properties.Settings.Default.SoundFileName == string.Empty)
                            label2.Text = "Old Oof";
                        else
                            label2.Text = Properties.Settings.Default.SoundFileName;
                        button4.Text = "Pick Sound";
                        break;
                        
                    }
                case "Footstep":
                    {
                        openFileDialog2.FileName = Properties.Settings.Default.FootstepSoundFileName;
                        if (Properties.Settings.Default.FootstepSoundFileName == string.Empty)
                            label2.Text = "Default Footsteps";
                        else
                            label2.Text = Properties.Settings.Default.FootstepSoundFileName;
                        button4.Text = "Pick Sound";
                        break;
                    }
                case "Mouse":
                    {
                        openFileDialog2.FileName = string.Empty;
                        if (Properties.Settings.Default.MouseImageFileName != null)
                        {
                            for (int i = 0; i < Properties.Settings.Default.MouseImageFileName.
                                Length; i++)
                            {
                                if (!File.Exists(Properties.Settings.Default.MouseImageFileName[i]))
                                {
                                    openFileDialog2.FileName = string.Empty;
                                    Properties.Settings.Default.MouseImageFileName = Properties.Settings.Default.Properties["MouseImageFileName"].DefaultValue as string[];
                                    break;
                                }
                                openFileDialog2.FileName += "\"" + Properties.Settings.Default.MouseImageFileName[i] + "\"";
                            }
                        }
                        if (Properties.Settings.Default.MouseImageFileName == null || Properties.Settings.Default.MouseImageFileName.Length <= 0)
                            label2.Text = "Default Mouse";
                        else
                            label2.Text = string.Join(", ", Properties.Settings.Default.MouseImageFileName);
                        button4.Text = "Pick Images";
                        break;
                    }
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            var index = Array.IndexOf(ReplaceTypes, Properties.Settings.Default.ReplaceType);
            Properties.Settings.Default.ReplaceType = ReplaceTypes[index - 1];
            label4.Text = Properties.Settings.Default.ReplaceType;
            UpdateArrows();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            var index = Array.IndexOf(ReplaceTypes, Properties.Settings.Default.ReplaceType);
            Properties.Settings.Default.ReplaceType = ReplaceTypes[index + 1];
            label4.Text = Properties.Settings.Default.ReplaceType;
            UpdateArrows();
        }

        private void DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string resultString = System.Text.RegularExpressions.Regex.Match(e.ClickedItem.Text, @"\d+").Value;
            if (resultString != string.Empty)
            {
                int number = Int32.Parse(resultString);
                if (number <= ContextMenuLabel.Text.Split(", ").Length && number > 0)
                {
                    string stringToUse = ContextMenuLabel.Text.Split(", ")[number - 1];
                    MessageBox.Show(stringToUse);
                    switch (e.ClickedItem.OwnerItem.Text)
                    {
                        case "Copy Path":
                            if (ContextMenuLabel != null)
                                Clipboard.SetText(stringToUse);
                            break;
                        case "Open in File Explorer":
                            string argument = "/select, \"" + stringToUse + "\"";

                            Process.Start("explorer.exe", argument);
                            break;
                    }
                }
            }
        }
    }
}