using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PBD_JozefMoraAraya.Formularios
{
    public partial class FrmPrincipal : Form
    {
        public FrmPrincipal()
        {
            InitializeComponent();
        }

        private void mANTENIMIENTOSToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void MnuGestionUsuarios_Click(object sender, EventArgs e)
        {
            if (!Globales.ObjetosGlobales.MiFormularioDeGestionDeUsuarios.Visible)
            {
                Globales.ObjetosGlobales.MiFormularioDeGestionDeUsuarios = new FrmUsuarioGestion();

                Globales.ObjetosGlobales.MiFormularioDeGestionDeUsuarios.Show();
            }
        }

        private void entradasYSalidasDeInventarioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Globales.ObjetosGlobales.MiFormularioMovimientos.Visible)
            {
                Globales.ObjetosGlobales.MiFormularioMovimientos = new FrmMovimientosInventario();
                Globales.ObjetosGlobales.MiFormularioMovimientos.Show();
            }
        }

        private void FrmPrincipal_Load(object sender, EventArgs e)
        {

        }

        private void MnuGestionProductos_Click(object sender, EventArgs e)
        {
            if (!Globales.ObjetosGlobales.MiFormularioPoductoGestion.Visible)
            {
                Globales.ObjetosGlobales.MiFormularioPoductoGestion = new FrmProductoGestion();
                Globales.ObjetosGlobales.MiFormularioPoductoGestion.Show();
            }
        }
    }
}
