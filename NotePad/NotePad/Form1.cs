using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace NotePad
{
    public partial class notepad : Form
    {
        private string basepath = Environment.CurrentDirectory;
        private static bool docChnages;
        private static string defaultName;
        private static bool saved;
        private static string savedFilePath;
        private static bool openDoc;
        private static string copiedText;
        private static string cuttedText;

        System.Drawing.Printing.PrintDocument document = new System.Drawing.Printing.PrintDocument();

        public notepad()
        {
            InitializeComponent();
            PrepareForm();
            this.Text = defaultName;
            this.Size = new Size(800, 600);
            document.PrintPage += Document_PrintPage;
        }

        private void Document_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawString(txtBody.Text, new Font("Tahoma", 20, FontStyle.Regular), Brushes.Black, 20, 20);
        }

        private void PrepareForm()
        {
            docChnages = false;
            defaultName = "new document";
            saved = false;
            savedFilePath = "";
            txtBody.Clear();
            this.Text = defaultName;
            this.Text.TrimEnd(new[] { '*' });
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (docChnages)
            {
                DialogResult dialogResult = MessageBox.Show("آیا تغییرات در فایل را ذخیره می کنید ؟ ", "ذخیره تغییرات",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                switch (dialogResult)
                {
                    case DialogResult.Yes:
                        SaveFile();
                        PrepareForm();
                        break;
                    case DialogResult.No:
                        PrepareForm();
                        break;
                    default:
                        break;
                }
            }
            else
            {
                PrepareForm();
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (docChnages)
            {
                DialogResult dialogResult = MessageBox.Show("آیا تغییرات در فایل را ذخیره می کنید ؟ ", "ذخیره تغییرات",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                switch (dialogResult)
                {
                    case DialogResult.Yes:
                        SaveFile();
                        OpenFile();
                        break;
                    case DialogResult.No:
                        OpenFile();
                        break;
                    default:
                        break;
                }
            }
            else
            {
                OpenFile();
            }
        }

        private void SaveFile()
        {
            if (!saved && string.IsNullOrEmpty(savedFilePath))
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Text Documents (*.txt)|*.txt";
                saveFileDialog.FileName = defaultName + ".txt";
                saveFileDialog.Title = "ذخیره فایل متنی";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    StreamWriter streamWriter = File.CreateText(saveFileDialog.FileName);
                    streamWriter.Close();
                    File.WriteAllText(saveFileDialog.FileName, txtBody.Text);
                    MessageBox.Show("فایل با موفقیت ذخیره گردید", "ذخیره فایل", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    saved = true;
                    docChnages = false;
                    savedFilePath = saveFileDialog.FileName;
                    this.Text = Path.GetFileName(saveFileDialog.FileName).Split('.')[0];
                }
            }
            else
            {
                File.WriteAllText(savedFilePath, txtBody.Text);
                this.Text = this.Text.TrimEnd(new[] { '*' });
            }
        }

        private void OpenFile()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Text Documents (*.txt)|*.txt";
            dialog.Title = "باز کردن فایل متنی";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                using (StreamReader sr = File.OpenText(dialog.FileName))
                {
                    txtBody.Text = sr.ReadToEnd();
                }
                this.Text = Path.GetFileName(dialog.FileName).Split('.')[0];
                savedFilePath = dialog.FileName;
                saved = true;
                openDoc = true;
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFile();
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            document.DocumentName = this.Text;
            printDialog.Document = document;
            printDialog.AllowSelection = true;
            printDialog.AllowSomePages = true;
            if(printDialog.ShowDialog() == DialogResult.OK) document.Print();
        }

        private void txtBody_KeyPress(object sender, KeyPressEventArgs e)
        {
            docChnages = true;
            this.Text = this.Text.TrimEnd(new[] { '*' });
            this.Text += "*";
        } 

        private void foreColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            if(colorDialog.ShowDialog() == DialogResult.OK)
                txtBody.ForeColor = colorDialog.Color;
        }

        private void backColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
             ColorDialog colorDialog = new ColorDialog();
            if(colorDialog.ShowDialog() == DialogResult.OK)
                txtBody.BackColor = colorDialog.Color;
        }

        private void fontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FontDialog font = new FontDialog();
            font.ShowEffects = true;
            if(font.ShowDialog() == DialogResult.OK)
                txtBody.Font = font.Font; 
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            copiedText = txtBody.Text;
            MessageBox.Show("متن کپی شد");
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtBody.Text += copiedText;
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
             copiedText = txtBody.Text;
            txtBody.Clear();
            MessageBox.Show("متن کات شد");
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtBody.Clear();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new About().ShowDialog();
        }

        private void helpSystemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Help().ShowDialog();
        }
    }
}
