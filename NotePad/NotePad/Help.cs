using System.Windows.Forms;

namespace NotePad
{
    public partial class Help : Form
    {
        public Help()
        {
            InitializeComponent();
            label1.Text = $"Use this shortcut keys : \n" +
                $"Ctrl+N create new text document \n" +
                $"Ctrl+O open an exsiting document \n" +
                $"Ctrl+S save text document \n" +
                $"Ctrl+P print current document";
        }
    }
}
