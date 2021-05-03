using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PhoneBook
{
    public partial class Form1 : Form
    {
        private string path = Path.Combine(Environment.CurrentDirectory, "phonebook.txt");
        public Form1()
        {
            InitializeComponent();
            UpdatePhoneBookList();
        }



        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs()) return;
            SavePhoneNumber();
        }

        private void SavePhoneNumber()
        {
            string phoneNumber = $"{txtFname.Text}\t{txtLname.Text}\t\t{txtPhone.Text}\t\t{txtAddress.Text}";
          
            List<string> phonesList = File.ReadAllLines(path).ToList();
            phonesList.Add(phoneNumber);
            File.WriteAllLines(path, phonesList.ToArray());
            MessageBox.Show("شماره تماس با موفقیت ذخیره گردید", "دفترچه تلفن", MessageBoxButtons.OK, MessageBoxIcon.Information);
            UpdatePhoneBookList();
        }

        private bool ValidateInputs()
        {
            bool validated = true;
            foreach (var item in inputs.Controls)
            {
                if (item.GetType() != typeof(TextBox)) continue;
                TextBox textBox = ((TextBox)item);
                if (string.IsNullOrWhiteSpace(textBox.Text.Trim()))
                {
                    textBox.BackColor = Color.IndianRed;
                    validated = false;
                }
            }

            return validated;
        }

        private void UpdatePhoneBookList()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine($"نام\tنام خانوادگی\tشماره تماس\t\tآدرس");

            if (!File.Exists(path))
            {
                using (var file = File.CreateText(path))
                {
                    file.Close();
                }
            }
            string[] phones = File.ReadAllLines(path);
            foreach (var phone in phones)
            {
                builder.AppendLine(phone);
            }
            txtList.Text = builder.ToString();
            ClearInputs();
        }

        private void ClearInputs()
        {
            foreach (var item in inputs.Controls)
            {
                if (item.GetType() != typeof(TextBox)) continue;
                ((TextBox)item).Clear();
            }
            txtFname.Focus();
        }

        private void txtFname_TextChanged(object sender, EventArgs e)
        {
            if (sender.GetType() != typeof(TextBox)) return;
            ((TextBox)sender).BackColor = Color.White;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("fa-IR");
        }
    }
}
