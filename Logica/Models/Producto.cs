using Logica.Tools;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace Logica.Models
{
    public class Producto
    {

        public Producto()
        {
            MiProductoCategoria = new ProductoCategoria();
        }

        public int ProductoID { get; set; }
        public string CodigoBarras { get; set; }
        public string NombreProducto { get; set; }
        public decimal Costo { get; set; }
        public decimal Utilidad { get; set; }
        public decimal SubTotal { get; set; }
        public decimal TasaImpuesto { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal CantidadStock { get; set; }
        public bool Activo { get; set; }

        ProductoCategoria MiProductoCategoria { get; set; }

        

        public bool Agregar()
        {
            bool R = false;

            Conexion MiCnn = new Conexion();

            MiCnn.ListaDeParametros.Add(new SqlParameter("@CodigoBarras", this.CodigoBarras));
            MiCnn.ListaDeParametros.Add(new SqlParameter("@NombreProducto", this.NombreProducto));
            MiCnn.ListaDeParametros.Add(new SqlParameter("@Costo", this.Costo));
            MiCnn.ListaDeParametros.Add(new SqlParameter("@Utilidad", this.Utilidad));
            MiCnn.ListaDeParametros.Add(new SqlParameter("@SubTotal", this.SubTotal));
            MiCnn.ListaDeParametros.Add(new SqlParameter("@TasaImpuesto", this.TasaImpuesto));
            MiCnn.ListaDeParametros.Add(new SqlParameter("@PrecioUnitario", this.PrecioUnitario));
            MiCnn.ListaDeParametros.Add(new SqlParameter("@CantidadStock", this.CantidadStock));
            MiCnn.ListaDeParametros.Add(new SqlParameter("@Activo", this.Activo));
            MiCnn.ListaDeParametros.Add(new SqlParameter("@ProductoCategoriaID", this.MiProductoCategoria.ProductoCategoriaID));

            int resultado = MiCnn.EjecutarDML("[SPProductoAgregar]");

            if (resultado > 0) R = true;

            return R;
        }

        public bool Actualizar()
        {
            bool R = false;

            Conexion MiCnn = new Conexion();


            MiCnn.ListaDeParametros.Add(new SqlParameter("@CodigoBarras", this.CodigoBarras));
            MiCnn.ListaDeParametros.Add(new SqlParameter("@NombreProducto", this.NombreProducto));
            MiCnn.ListaDeParametros.Add(new SqlParameter("@Utilidad", this.Utilidad));
            MiCnn.ListaDeParametros.Add(new SqlParameter("@SubTotal", this.SubTotal));
            MiCnn.ListaDeParametros.Add(new SqlParameter("@TasaImpuesto", this.TasaImpuesto));
            MiCnn.ListaDeParametros.Add(new SqlParameter("@PrecioUnitario", this.PrecioUnitario));
            MiCnn.ListaDeParametros.Add(new SqlParameter("@CantidadStock", this.CantidadStock));
            MiCnn.ListaDeParametros.Add(new SqlParameter("@Activo", this.Activo));
            MiCnn.ListaDeParametros.Add(new SqlParameter("@ProductoCategoriaID", this.MiProductoCategoria.ProductoCategoriaID));

            MiCnn.ListaDeParametros.Add(new SqlParameter("@ID", this.ProductoID));

            int resultado = MiCnn.EjecutarDML("SPProductoActualizar");

            if (resultado > 0) R = true;

            return R;
        }

        public bool Eliminar()
        {
            bool R = false;

            Conexion MiCnn = new Conexion();

            MiCnn.ListaDeParametros.Add(new SqlParameter("@ID", this.ProductoID));

            int resultado = MiCnn.EjecutarDML("SPProductoEliminar");

            if (resultado > 0) R = true;

            return R;
        }

        public bool ConsultarPorID()
        {
            bool R = false;

            Conexion MyCnn = new Conexion();

            MyCnn.ListaDeParametros.Add(new SqlParameter("@ID", this.ProductoID));

            DataTable DatosProducto = new DataTable();

            DatosProducto = MyCnn.EjecutarSelect("SPProductoConsultarPorID");

            if (DatosProducto != null && DatosProducto.Rows.Count > 0)
            {
                R = true;
            }

            return R;
        }




        

        public bool ConsultarPorCodigoBarras(string CodigoBarras)
        {
            bool R = false;

            Conexion MiCnn = new Conexion();

            MiCnn.ListaDeParametros.Add(new SqlParameter("@CodigoBarras", CodigoBarras));

            DataTable dt = new DataTable();

            dt = MiCnn.EjecutarSelect("SPProductoConsultarPorCodigoBarras");

            if (dt != null && dt.Rows.Count > 0) R = true;

            return R;
        }



        public DataTable Listar(bool VerActivos = true, string Filtro = "")
        {
            DataTable R = new DataTable();

            Conexion MiCnn = new Conexion();

            MiCnn.ListaDeParametros.Add(new SqlParameter("@VerActivos", true));
            MiCnn.ListaDeParametros.Add(new SqlParameter("@Filtro", Filtro));

            R = MiCnn.EjecutarSelect("SPProductoCategoriaListar");

            return R;
        }


        public DataTable ListarEnMovimientoDetalleProducto(bool VerActivos = true, string Filtro = "")
        {
            DataTable R = new DataTable();

            Conexion MyCnn = new Conexion();

            MyCnn.ListaDeParametros.Add(new SqlParameter("@VerActivos", VerActivos));
            MyCnn.ListaDeParametros.Add(new SqlParameter("@Filtro", Filtro));

            R = MyCnn.EjecutarSelect("SPProductoCategoriaListar");

            return R;
        }

    }
}
