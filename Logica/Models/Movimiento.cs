using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica.Models
{
    public class Movimiento
    {
        public Movimiento()
        {
            MiTipo = new MovimientoTipo();
            MiUsuario = new Usuario();

            Detalles = new List<MovimientoDetalle>();

        }

        public int MovimientoID { get; set; }

        public DateTime Fecha { get; set; }

        public int NumeroMovimiento { get; set; }

        public string Anotaciones { get; set; }

        public bool Agregar()
        {
            bool R = false;

 
            Conexion MyCnn = new Conexion();

            MyCnn.ListaDeParametros.Add(new SqlParameter("@Fecha", this.Fecha));
            MyCnn.ListaDeParametros.Add(new SqlParameter("@Anotaciones", this.Anotaciones));
            MyCnn.ListaDeParametros.Add(new SqlParameter("@TipoMovimiento", this.MiTipo.MovimientoTipoID));
            MyCnn.ListaDeParametros.Add(new SqlParameter("@UsuarioID", this.MiUsuario.UsuarioID));


            Object RetornoSPAgregar = MyCnn.EjecutarSELECTEscalar("SPMovimientosAgregarEncabezado");

            int IDMovimientoRecienCreado;

            if (RetornoSPAgregar != null)
            {

                IDMovimientoRecienCreado = Convert.ToInt32(RetornoSPAgregar.ToString());


                this.MovimientoID = IDMovimientoRecienCreado;

                foreach (MovimientoDetalle item in this.Detalles)
                {


                    Conexion MyCnnDetalle = new Conexion();

                    MyCnnDetalle.ListaDeParametros.Add(new SqlParameter("@IDMovimiento", IDMovimientoRecienCreado));
                    MyCnnDetalle.ListaDeParametros.Add(new SqlParameter("@IDProducto", item.MiProducto.ProductoID));
                    MyCnnDetalle.ListaDeParametros.Add(new SqlParameter("@Cantidad", item.CantidadMovimiento));
                    MyCnnDetalle.ListaDeParametros.Add(new SqlParameter("@Costo", item.Costo));
                    MyCnnDetalle.ListaDeParametros.Add(new SqlParameter("@SubTotal", item.SubTotal));
                    MyCnnDetalle.ListaDeParametros.Add(new SqlParameter("@TotalIVA", item.TotalIVA));
                    MyCnnDetalle.ListaDeParametros.Add(new SqlParameter("@PrecioUnitario", item.PrecioUnitario));

                    MyCnnDetalle.EjecutarDML("SPMovimientosAgregarDetalle");

                }

                R = true;
            }

            return R;
        }

        public bool Eliminar()
        {
            bool R = false;

            return R;
        }

        public bool ConsultarPorID()
        {
            bool R = false;

            return R;
        }

        public DataTable Listar()
        {
            DataTable R = new DataTable();


            return R;
        }


        public MovimientoTipo MiTipo { get; set; }
        public Usuario MiUsuario { get; set; }


        public List<MovimientoDetalle> Detalles { get; set; }


        public DataTable AsignarEsquemaDelDetalle()
        {
            DataTable R = new DataTable();

            Conexion MyCnn = new Conexion();


            R = MyCnn.EjecutarSelect("SPMovimientoCargarDetalle", true);

            R.PrimaryKey = null;

            return R;
        }

    }
}
