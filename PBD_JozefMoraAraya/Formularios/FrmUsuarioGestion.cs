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
    public partial class FrmUsuarioGestion : Form
    {
        private Logica.Models.Usuario MiUsuarioLocal { get; set; }
        public FrmUsuarioGestion()
        {
            InitializeComponent();

            MiUsuarioLocal = new Logica.Models.Usuario();

        }

        private void DgvListaUsuarios_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void FrmUsuarioGestion_Load(object sender, EventArgs e)
        {
            MdiParent = Globales.ObjetosGlobales.MiFormularioPrincipal;

            CargarComboRolesDeUsuario();

            CargarListaUsuarios(CbVerActivos.Checked);

            ActivarBotonAgregar();
        }

        private void CargarComboRolesDeUsuario()
        {
            Logica.Models.UsuarioRol MiRol = new Logica.Models.UsuarioRol();

            DataTable dt = new DataTable();

            dt = MiRol.Listar();

            if (dt != null && dt.Rows.Count > 0)
            {
                CboxUsuarioTipoRol.ValueMember = "id";
                CboxUsuarioTipoRol.DisplayMember = "Descripcion";

                CboxUsuarioTipoRol.DataSource = dt;

                CboxUsuarioTipoRol.SelectedIndex = -1;

            }

        }

        private void CargarListaUsuarios(bool VerActivos, string FiltroBusqueda = "")
        {
            Logica.Models.Usuario miusuario = new Logica.Models.Usuario();

            DataTable lista = new DataTable();

            if (VerActivos)
            {
                lista = miusuario.ListarActivos(FiltroBusqueda);
                DgvListaUsuarios.DataSource = lista;
            }
            else
            {
                lista = miusuario.ListarInactivos(FiltroBusqueda);
                DgvListaUsuarios.DataSource = lista;
            }

        }

        private bool ValidarDatosRequeridos(bool OmitirContrasennia = false)
        {
            bool R = false;


            if (!string.IsNullOrEmpty(TxtUsuarioCedula.Text.Trim()) &&
                !string.IsNullOrEmpty(TxtUsuarioNombre.Text.Trim()) &&
                !string.IsNullOrEmpty(TxtUsuarioCorreo.Text.Trim()) &&
                CboxUsuarioTipoRol.SelectedIndex > -1
                )
            {
                if (OmitirContrasennia)
                {

                    R = true;
                }
                else
                {

                    if (!string.IsNullOrEmpty(TxtUsuarioContrasennia.Text.Trim()))
                    {
                        R = true;
                    }
                    else
                    {
                        //CONTRASEÑA
                        if (string.IsNullOrEmpty(TxtUsuarioContrasennia.Text.Trim()))
                        {
                            MessageBox.Show("Debe digitar la Contraseña", "Error de validación", MessageBoxButtons.OK);
                            return false;
                        }
                    }
                }
            }
            else
            {

                //CEDULA
                if (string.IsNullOrEmpty(TxtUsuarioCedula.Text.Trim()))
                {
                    MessageBox.Show("Debe digitar la Cédula", "Error de validación", MessageBoxButtons.OK);
                    return false;
                }

                //NOMBRE
                if (string.IsNullOrEmpty(TxtUsuarioNombre.Text.Trim()))
                {
                    MessageBox.Show("Debe digitar el Nombre", "Error de validación", MessageBoxButtons.OK);
                    return false;
                }

                //CORREO
                if (string.IsNullOrEmpty(TxtUsuarioCorreo.Text.Trim()))
                {
                    MessageBox.Show("Debe digitar el Correo", "Error de validación", MessageBoxButtons.OK);
                    return false;
                }

                //ROL DE USUARIO
                if (CboxUsuarioTipoRol.SelectedIndex == -1)
                {
                    MessageBox.Show("Debe Seleccionar un Rol de Usuario", "Error de validación", MessageBoxButtons.OK);
                    return false;
                }

            }

            return R;
        }

        private void BtnAgregar_Click_1(object sender, EventArgs e)
        {
            if (ValidarDatosRequeridos())
            {
                MiUsuarioLocal = new Logica.Models.Usuario();

                MiUsuarioLocal.Cedula = TxtUsuarioCedula.Text.Trim();
                MiUsuarioLocal.Nombre = TxtUsuarioNombre.Text.Trim();
                MiUsuarioLocal.Correo = TxtUsuarioCorreo.Text.Trim();
                MiUsuarioLocal.Telefono = TxtUsuarioTelefono.Text.Trim();
                MiUsuarioLocal.MiUsuarioRol.UsuarioRolID = Convert.ToInt32(CboxUsuarioTipoRol.SelectedValue);

                MiUsuarioLocal.Contrasennia = TxtUsuarioContrasennia.Text.Trim();
                MiUsuarioLocal.Direccion = TxtUsuarioDireccion.Text.Trim();

                bool CedulaOk = MiUsuarioLocal.ConsultarPorCedula(MiUsuarioLocal.Cedula);

                bool CorreoOk = MiUsuarioLocal.ConsultarPorCorreo(MiUsuarioLocal.Correo);

                if (CedulaOk == false && CorreoOk == false)
                { 
                    string Pregunta = string.Format("¿Está seguro de agregar al usuario {0}?", MiUsuarioLocal.Nombre);
                    DialogResult respuesta = MessageBox.Show(Pregunta, "???", MessageBoxButtons.YesNo);

                    if (respuesta == DialogResult.Yes)
                    {
                        bool ok = MiUsuarioLocal.Agregar();

                        if (ok)
                        {
                            MessageBox.Show("Usuario ingresado correctamente!", ":)", MessageBoxButtons.OK);

                            LimpiarForm();
                            CargarListaUsuarios(CbVerActivos.Checked);

                        }
                        else
                        {
                            MessageBox.Show("El Usuario no se pudo agregar...", ":(", MessageBoxButtons.OK);
                        }
                    }
                }
            }
    }

        private void BtnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarForm();
            ActivarBotonAgregar();
        }

        private void LimpiarForm()
        {
            TxtUsuarioCodigo.Clear();
            TxtUsuarioCedula.Clear();
            TxtUsuarioNombre.Clear();
            TxtUsuarioCorreo.Clear();
            TxtTelefono.Clear();
            TxtUsuarioContrasennia.Clear();
            TxtUsuarioDireccion.Clear();

            CboxUsuarioTipoRol.SelectedIndex = -1;

            CbUsuarioActivo.Checked = false;

        }

        private void DgvListaUsuarios_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (DgvListaUsuarios.SelectedRows.Count == 1)
            {
                LimpiarForm();


                DataGridViewRow MiDgvFila = DgvListaUsuarios.SelectedRows[0];
                int IDUsuario = Convert.ToInt32(MiDgvFila.Cells["ColUsuarioID"].Value);

                MiUsuarioLocal = new Logica.Models.Usuario();
                MiUsuarioLocal = MiUsuarioLocal.ConsultarPorID(IDUsuario);

                if (MiUsuarioLocal != null && MiUsuarioLocal.UsuarioID > 0)
                {


                    TxtUsuarioCodigo.Text = MiUsuarioLocal.UsuarioID.ToString();
                    TxtUsuarioCedula.Text = MiUsuarioLocal.Cedula;
                    TxtUsuarioNombre.Text = MiUsuarioLocal.Nombre;
                    TxtUsuarioCorreo.Text = MiUsuarioLocal.Correo;
                    TxtUsuarioTelefono.Text = MiUsuarioLocal.Telefono;
                    TxtUsuarioDireccion.Text = MiUsuarioLocal.Direccion;


                    CboxUsuarioTipoRol.SelectedValue = MiUsuarioLocal.MiUsuarioRol.UsuarioRolID;
                    CbUsuarioActivo.Checked = MiUsuarioLocal.Activo;

                    ActivarBotonesModificarYEliminar();

                }
            }
        }


        

        private void ActivarBotonAgregar()
        {
            BtnAgregar.Enabled = true;
            BtnModificar.Enabled = false;
            BtnEliminar.Enabled = false;
        }

        private void ActivarBotonesModificarYEliminar()
        {
            BtnAgregar.Enabled = false;
            BtnModificar.Enabled = true;
            BtnEliminar.Enabled = true;
        }


        private void DgvListaUsuarios_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            DgvListaUsuarios.ClearSelection();
        }

        private void BtnModificar_Click(object sender, EventArgs e)
        {
            if (ValidarDatosRequeridos(true))
            {
                MiUsuarioLocal.Nombre = TxtUsuarioNombre.Text.Trim();
                MiUsuarioLocal.Cedula = TxtUsuarioCedula.Text.Trim();
                MiUsuarioLocal.Correo = TxtUsuarioCorreo.Text.Trim();
                MiUsuarioLocal.Telefono = TxtUsuarioTelefono.Text.Trim();
                MiUsuarioLocal.MiUsuarioRol.UsuarioRolID = Convert.ToInt32(CboxUsuarioTipoRol.SelectedValue);
                MiUsuarioLocal.Direccion = TxtUsuarioDireccion.Text.Trim();


                MiUsuarioLocal.Contrasennia = TxtUsuarioContrasennia.Text.Trim();


                if (MiUsuarioLocal.ConsultarPorID())
                {
                    DialogResult Resp = MessageBox.Show("¿Desea modificar el usuario?", "???",
                                                           MessageBoxButtons.YesNo);
                    if (Resp == DialogResult.Yes)
                    {
                        if (MiUsuarioLocal.Actualizar())
                        {
                            MessageBox.Show("Usuario modificado correctamente!", ":)", MessageBoxButtons.OK);

                            LimpiarForm();
                            CargarListaUsuarios(CbVerActivos.Checked);
                            ActivarBotonAgregar();
                        }
                    }
                }
            }
        }

        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            if (CbVerActivos.Checked)
            {
                if (MiUsuarioLocal.UsuarioID > 0)
                {
                    string msg = string.Format("¿Está seguro de eliminar al usuario {0}?", MiUsuarioLocal.Nombre);

                    DialogResult respuesta = MessageBox.Show(msg, "Confirmación requerida", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (respuesta == DialogResult.Yes && MiUsuarioLocal.Eliminar())
                    {
                        MessageBox.Show("El Usuario ha sido eliminado", "!!!", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        LimpiarForm();
                        CargarListaUsuarios(CbVerActivos.Checked);
                        ActivarBotonAgregar();
                    }
                }
            }
            else
            {
                if (MiUsuarioLocal.UsuarioID > 0)
                {
                    string msg = string.Format("¿Está seguro de activar al usuario {0}?", MiUsuarioLocal.Nombre);

                    DialogResult respuesta = MessageBox.Show(msg, "Confirmación requerida", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (respuesta == DialogResult.Yes && MiUsuarioLocal.Activar())
                    {
                        MessageBox.Show("El Usuario ha sido activado", "!!!", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        LimpiarForm();
                        CargarListaUsuarios(CbVerActivos.Checked);
                        ActivarBotonAgregar();
                    }
                }

            }
        }


        private void TxtUsuarioCedula_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = Tools.Validaciones.CaracteresNumeros(e);
        }

        private void TxtUsuarioNombre_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = Tools.Validaciones.CaracteresTexto(e);
        }

        private void TxtUsuarioCorreo_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = Tools.Validaciones.CaracteresTexto(e, false, true);
        }

        private void TxtUsuarioTelefono_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = Tools.Validaciones.CaracteresNumeros(e);
        }

        private void TxtUsuarioContrasennia_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = Tools.Validaciones.CaracteresTexto(e);
        }

        private void TxtUsuarioDireccion_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = Tools.Validaciones.CaracteresTexto(e);
        }


        private void BtnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CbVerActivos_CheckedChanged(object sender, EventArgs e)
        {
            CargarListaUsuarios(CbVerActivos.Checked);

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
                CargarListaUsuarios(CbVerActivos.Checked, TxtBuscar.Text.Trim());
            }
            else
            {
                CargarListaUsuarios(CbVerActivos.Checked);
            }
        }

        private void CbVerActivos_CheckedChanged_1(object sender, EventArgs e)
        {

        }

        private void BtnLimpiar_Click_1(object sender, EventArgs e)
        {
            LimpiarForm();
            ActivarBotonAgregar();
        }
    }
}
