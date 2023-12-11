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
    public partial class FrmMovimientosInventario : Form
    {

        public Logica.Models.Movimiento MiMovimientoLocal { get; set; }

        public DataTable DtListaDetalleProductos { get; set; }


        public FrmMovimientosInventario()
        {
            InitializeComponent();

            MiMovimientoLocal = new Logica.Models.Movimiento();

            DtListaDetalleProductos = new DataTable();

        }

        private void BtnAplicar_Click_1(object sender, EventArgs e)
        {

                DialogResult respuesta = MessageBox.Show("Desea continiar", "????", MessageBoxButtons.YesNo);
            if (ValidarMovimiento())
            {

                

                MiMovimientoLocal.Fecha = DtpFecha.Value.Date;
                MiMovimientoLocal.Anotaciones = TxtAnotaciones.Text.Trim();

                MiMovimientoLocal.MiTipo.MovimientoTipoID = Convert.ToInt32(CboxTipo.SelectedValue);


                MiMovimientoLocal.MiUsuario = Globales.ObjetosGlobales.MiUsuarioGlobal;

                TrasladarDetalles();

                if (MiMovimientoLocal.Agregar())
                {

                    MessageBox.Show("El Movimiento se ha agregado correctamente",
                        ":)", MessageBoxButtons.OK);





                }



            }


        }

        private void TrasladarDetalles()
        {

            foreach (DataRow item in DtListaDetalleProductos.Rows)
            {

                Logica.Models.MovimientoDetalle NuevoDetalle = new Logica.Models.MovimientoDetalle();

                NuevoDetalle.CantidadMovimiento = Convert.ToDecimal(item["CantidadMovimiento"]);

                NuevoDetalle.Costo = Convert.ToDecimal(item["Costo"]);

                NuevoDetalle.PrecioUnitario = Convert.ToDecimal(item["PrecioUnitario"]);

                NuevoDetalle.SubTotal = Convert.ToDecimal(item["SubTotal"]);

                NuevoDetalle.TotalIVA = Convert.ToDecimal(item["TotalIVA"]);


                NuevoDetalle.MiProducto.ProductoID = Convert.ToInt32(item["ProductoID"]);


                MiMovimientoLocal.Detalles.Add(NuevoDetalle);

            }

        }


        private bool ValidarMovimiento()
        {
            bool R = false;

            if (DtpFecha.Value.Date <= DateTime.Now.Date &&
                CboxTipo.SelectedIndex > -1 &&
                DtListaDetalleProductos.Rows.Count > 0)
            {
                R = true;
            }
            else
            {
                if (DtpFecha.Value.Date > DateTime.Now.Date)
                {
                    MessageBox.Show("La fecha del movimiento no puede " +
                        "ser superior a la fecha actual", "Error de Validación",
                        MessageBoxButtons.OK);
                    return false;
                }

                if (CboxTipo.SelectedIndex == -1)
                {
                    MessageBox.Show("Debe seleccionar un tipo de movimiento",
                        "Error de Validación", MessageBoxButtons.OK);
                    return false;
                }

                if (DtListaDetalleProductos == null || DtListaDetalleProductos.Rows.Count == 0)
                {
                    MessageBox.Show("No se puede procesar un movimiento sin detalles",
                        "Error de Validación", MessageBoxButtons.OK);
                    return false;
                }

            }

            return R;
        }


        private void BtnCancelar_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmMovimientosInventario_Load(object sender, EventArgs e)
        {
            MdiParent = Globales.ObjetosGlobales.MiFormularioPrincipal;

            CargarComboMovimientos();

            LimpiarFormulario();
        }

        private void LimpiarFormulario()
        {
            DtpFecha.Value = DateTime.Now.Date;
            CboxTipo.SelectedIndex = -1;
            TxtAnotaciones.Clear();



            DtListaDetalleProductos = MiMovimientoLocal.AsignarEsquemaDelDetalle();

            DgvListaDetalle.DataSource = DtListaDetalleProductos;


            LblTotalCosto.Text = "0";
            LblTotalGranTotal.Text = "0";
            LblTotalImpuestos.Text = "0";
            LblTotalSubTotal.Text = "0";

        }



        private void CargarComboMovimientos()
        {
            Logica.Models.MovimientoTipo MiTipo = new Logica.Models.MovimientoTipo();

            DataTable dt = new DataTable();

            dt = MiTipo.Listar();

            if (dt != null && dt.Rows.Count > 0)
            {

                CboxTipo.ValueMember = "id";
                CboxTipo.DisplayMember = "Descripcion";

                CboxTipo.DataSource = dt;

                CboxTipo.SelectedIndex = -1;
            }

        }

        private void BtnAgregar_Click_1(object sender, EventArgs e)
        {


            Form FormDetalleProducto = new Formularios.FrmMovimientosInventarioDetalleProducto();

            DialogResult resp = FormDetalleProducto.ShowDialog();

            if (resp == DialogResult.OK)
            {

                DgvListaDetalle.DataSource = DtListaDetalleProductos;

                Totalizar();

            }

        }

        private void Totalizar()
        {
            decimal TotalCosto = 0;
            decimal TotalSubtotal = 0;
            decimal TotalImpuestos = 0;
            decimal Total = 0;

            if (DtListaDetalleProductos != null && DtListaDetalleProductos.Rows.Count > 0)
            {
                foreach (DataRow item in DtListaDetalleProductos.Rows)
                {
                    decimal Cantidad = Convert.ToDecimal(item["CantidadMovimiento"]);
                    TotalCosto += Convert.ToDecimal(item["Costo"]) * Cantidad;
                    TotalSubtotal += Convert.ToDecimal(item["SubTotal"]) * Cantidad;
                    TotalImpuestos += Convert.ToDecimal(item["TotalIVA"]) * Cantidad;
                    Total += TotalSubtotal + TotalImpuestos;
                }
            }

            LblTotalCosto.Text = string.Format("{0:C2}", TotalCosto);
            LblTotalSubTotal.Text = string.Format("{0:C2}", TotalSubtotal);
            LblTotalImpuestos.Text = string.Format("{0:C2}", TotalImpuestos);
            LblTotalGranTotal.Text = string.Format("{0:C2}", Total);
        }



    }
}
