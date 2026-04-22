using System;
using System.Windows.Forms;

namespace BrechoApp
{
    public partial class FormAdministracao : Form
    {
        public FormAdministracao()
        {
            InitializeComponent();
        }

        private void btnVoltar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
