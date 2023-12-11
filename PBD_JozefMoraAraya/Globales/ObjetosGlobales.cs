using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace PBD_JozefMoraAraya.Globales
{
    public static class ObjetosGlobales
    {
        public static Form MiFormularioPrincipal = new Formularios.FrmPrincipal();

        public static Formularios.FrmUsuarioGestion
            MiFormularioDeGestionDeUsuarios = new Formularios.FrmUsuarioGestion();

        public static Logica.Models.Usuario MiUsuarioGlobal = new Logica.Models.Usuario();

        public static Formularios.FrmMovimientosInventario
            MiFormularioMovimientos = new Formularios.FrmMovimientosInventario();

        public static Formularios.FrmProductoGestion
            MiFormularioPoductoGestion = new Formularios.FrmProductoGestion();

    }
}
