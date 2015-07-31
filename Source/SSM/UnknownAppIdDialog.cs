using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace SSM
{
    /// <summary>
    /// Prompts the user to enter a name for a screenshot.
    /// </summary>
    public partial class UnknownAppIdDialog : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see
        /// cref="T:SSM.UnknownAppIdDialog"/> class with the specified file
        /// name.
        /// </summary>
        /// <param name="path">The file name of the screenshot to show.</param>
        public UnknownAppIdDialog(string path)
        {
            InitializeComponent();

            FileName = path;
        }

        /// <summary>
        /// Gets the file name of the screenshot shown on the form.
        /// </summary>
        public string FileName { get; private set; }

        /// <summary>
        /// Gets or sets the name of the game shown on the form.
        /// </summary>
        public string GameName
        {
            get { return GameNameInput.Text; }
            set { GameNameInput.Text = value; }
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void GameNameInput_TextChanged(object sender, EventArgs e)
        {
            Ok.Enabled = (GameNameInput.TextLength > 0);
        }

        private void Ok_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Thumbnail_Click(object sender, EventArgs e)
        {
            if (System.IO.File.Exists(FileName))
            {
                Process p = Process.Start(FileName);
                if (p != null)
                    p.WaitForExit();
            }
        }

        private void UnknownAppIdDialog_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (Thumbnail != null && Thumbnail.Image != null)
            {
                Thumbnail.Image.Dispose();
                Thumbnail.Image = null;
            }
        }

        private void UnknownAppIdDialog_Load(object sender, EventArgs e)
        {
            try
            {
                Thumbnail.Image = Image.FromFile(FileName);
            }
            catch (System.IO.FileNotFoundException ex)
            {
                System.Diagnostics.Trace.WriteLine("UnknownAppIdDialog called for invalid file " + ex.FileName);
                Close();
            }
        }
    }
}
