using Shell32;
using System;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Windows.Forms;

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
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                label1.Text = openFileDialog1.FileName;
                Properties.Settings.Default.ShortcutFileName = openFileDialog1.FileName;
            }
        }

        private void label_TextChanged(object sender, EventArgs e)
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

        private void button4_Click(object sender, EventArgs e)
        {
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                label2.Text = openFileDialog2.FileName;
                Properties.Settings.Default.SoundFileName = openFileDialog2.FileName;
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (!File.Exists(openFileDialog1.FileName))
            {
                MessageBox.Show("Choose your Roblox Shortcut!", "Error", MessageBoxButtons.OK);
                return;
            }
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
            var ooflocation = Path.Combine(location, @"..\", "content", "sounds", "ouch.ogg");
            if (!File.Exists(ooflocation))
            {
                MessageBox.Show("Couldn't find ouch.ogg! Shortcut target may not be Roblox OR the path of the sound may have changed. Your Roblox also might not be on the latest version.", "Error", MessageBoxButtons.OK);
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
            label1.Text = Properties.Settings.Default.ShortcutFileName;
            label2.Text = "Old Oof";
        }

        private void RobloxOofReplacer_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.Save();
        }

        private void RobloxOofReplacer_Load(object sender, EventArgs e)
        {
            version.Text = "v" + System.Windows.Forms.Application.ProductVersion;
            versionToolTip.SetToolTip(version, "v" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString());
            openFileDialog1.FileName = Properties.Settings.Default.ShortcutFileName;
            openFileDialog2.FileName = Properties.Settings.Default.SoundFileName;
            label1.Text = Properties.Settings.Default.ShortcutFileName;
            if (Properties.Settings.Default.SoundFileName == "")
                label2.Text = "Old Oof";
            else
                label2.Text = Properties.Settings.Default.SoundFileName;
        }
    }
}