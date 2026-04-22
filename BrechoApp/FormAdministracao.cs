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

        private void btnCategoriasFinanceiras_Click(object sender, EventArgs e)
        {
            using var form = new FormCadastroCategoriasFinanceiras();
            form.ShowDialog(this);
        }
    }
}
