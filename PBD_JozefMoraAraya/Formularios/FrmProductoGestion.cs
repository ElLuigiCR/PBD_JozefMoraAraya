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
    public partial class FrmProductoGestion : Form
    {
        private Logica.Models.Producto MiProductoLocal { get; set; }
        public FrmProductoGestion()
        {
            InitializeComponent();
            MiProductoLocal = new Logica.Models.Producto();
        }

        private void FrmProductoGestion_Load(object sender, EventArgs e)
        {
            MdiParent = Globales.ObjetosGlobales.MiFormularioPrincipal;

            CargarComboCategoriaDeProducto();

            CargarListaProducto(CbVerActivos.Checked);

        }

        private void CargarComboCategoriaDeProducto()
        {
            Logica.Models.ProductoCategoria MiRol = new Logica.Models.ProductoCategoria();

            DataTable dt = new DataTable();

            dt = MiRol.Listar();

            if (dt != null && dt.Rows.Count > 0)
            {
                CboxTipoProducto.ValueMember = "id";
                CboxTipoProducto.DisplayMember = "Descripcion";

                CboxTipoProducto.DataSource = dt;

                CboxTipoProducto.SelectedIndex = -1;

            }

        }

        private void CargarListaProducto(bool VerActivos, string FiltroBusqueda = "")
        {
            Logica.Models.Usuario miusuario = new Logica.Models.Usuario();

            DataTable lista = new DataTable();

            if (VerActivos)
            {
                lista = miusuario.ListarActivos(FiltroBusqueda);
                DgvListaProducto.DataSource = lista;
            }
            else
            {
                lista = miusuario.ListarInactivos(FiltroBusqueda);
                DgvListaProducto.DataSource = lista;
            }

        }

        private Logica.Models.Producto GetMiProductoLocal()
        {
            return MiProductoLocal;
        }

            
        private void BtnAgregar_Click(object sender, EventArgs e)
                {
                        this.MiProductoLocal = new Logica.Models.Producto();

                        this.MiProductoLocal.CodigoBarras = TxtCodigoBarras.Text.Trim();
                        this.MiProductoLocal.NombreProducto = TxtNombreProducto.Text.Trim();

                        if (decimal.TryParse(TxtCosto.Text.Trim(), out decimal costo))
                        {
                            this.MiProductoLocal.Costo = costo;
                        }

                        if (decimal.TryParse(TxtUtilidad.Text.Trim(), out decimal utilidad))
                        {
                            this.MiProductoLocal.Utilidad = utilidad;
                        }

                        MiProductoLocal.MiProductoCategoria.ProductoCategoriaID = Convert.ToInt32(CboxTipoProducto.SelectedValue);

                        if (decimal.TryParse(TxtTasaImpuesto.Text.Trim(), out decimal tasaImpuesto))
                        {
                            this.MiProductoLocal.TasaImpuesto = tasaImpuesto;
                        }

                        if (decimal.TryParse(TxtSubTotal.Text.Trim(), out decimal subTotal))
                        {
                            this.MiProductoLocal.SubTotal = subTotal;
                        }

                        if (decimal.TryParse(TxtPrecioUnitario.Text.Trim(), out decimal precioUnitario))
                        {
                            this.MiProductoLocal.PrecioUnitario = precioUnitario;
                        }

                        if (decimal.TryParse(TxtCantidadStock.Text.Trim(), out decimal cantidadStock))
                        {
                            this.MiProductoLocal.CantidadStock = cantidadStock;
                        }
        }


        private void BtnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarForm();
        }

        private void LimpiarForm()
        {
            TxtCodigoBarras.Clear();
            TxtNombreProducto.Clear();
            TxtCosto.Clear();
            TxtUtilidad.Clear();
            TxtSubTotal.Clear();
            TxtTasaImpuesto.Clear();
            TxtPrecioUnitario.Clear();
            TxtCantidadStock.Clear();
            TxtCantidadStock.Clear();

            CboxTipoProducto.SelectedIndex = -1;

            CbProductoActivo.Checked = false;

        }

        private void ActivarBotonesModificarYEliminar()
        {
            BtnAgregar.Enabled = false;
            BtnModificar.Enabled = true;
            BtnEliminar.Enabled = true;
        }

        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            if (CbVerActivos.Checked)
            {
                if (MiProductoLocal.ProductoID > 0)
                {
                    string msg = string.Format("¿Está seguro de eliminar al usuario {0}?", MiProductoLocal.Nombre);

                    DialogResult respuesta = MessageBox.Show(msg, "Confirmación requerida", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (respuesta == DialogResult.Yes && MiProductoLocal.Eliminar())
                    {
                        MessageBox.Show("El Usuario ha sido eliminado", "!!!", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        LimpiarForm();
                        CargarListaProducto(CbVerActivos.Checked);
                        ActivarBotonAgregar();
                    }
                }
            }
            else
            {
                if (MiProductoLocal.ProductoID > 0)
                {
                    string msg = string.Format("¿Está seguro de activar al usuario {0}?", MiProductoLocal.Nombre);

                    DialogResult respuesta = MessageBox.Show(msg, "Confirmación requerida", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (respuesta == DialogResult.Yes && MiProductoLocal.Activar())
                    {
                        MessageBox.Show("El Usuario ha sido activado", "!!!", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        LimpiarForm();
                        CargarListaProducto(CbVerActivos.Checked);
                        ActivarBotonAgregar();
                    }
                }

            }
    }
        private void TxtCodigoBarras_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = Tools.Validaciones.CaracteresNumeros(e);
        }

        private void TxtNombreProducto_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = Tools.Validaciones.CaracteresTexto(e);
        }

        private void TxtCosto_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = Tools.Validaciones.CaracteresTexto(e);
        }

        private void TxtUtilidad_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = Tools.Validaciones.CaracteresTexto(e);
        }

        private void TxtSubTotal_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = Tools.Validaciones.CaracteresTexto(e);
        }

        private void TxtTasaImpuesto_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = Tools.Validaciones.CaracteresTexto(e);
        }

        private void TxtPrecioUnitario_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = Tools.Validaciones.CaracteresTexto(e);
        }

        private void TxtCantidadStock_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = Tools.Validaciones.CaracteresTexto(e);
        }

        private void BtnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CbVerActivos_CheckedChanged(object sender, EventArgs e)
        {
            CargarListaProducto(CbVerActivos.Checked);

            if (CbVerActivos.Checked)
            {
                BtnEliminar.Text = "ELIMINAR";
            }
            else
            {
                BtnEliminar.Text = "ACTIVAR";
            }
        }

        private void TxtBuscar_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(TxtBuscar.Text.Trim()) && TxtBuscar.Text.Count() >= 3)
            {
                CargarListaProducto(CbVerActivos.Checked, TxtBuscar.Text.Trim());
            }
            else
            {
                CargarListaProducto(CbVerActivos.Checked);
            }
        }
    }
}
