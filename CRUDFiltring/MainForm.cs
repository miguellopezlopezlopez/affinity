using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using System.Collections.Generic;
using System.Drawing;

namespace FiltringApp
{
    public partial class MainForm : Form
    {
        private MySqlConnection conexion;
        private string cadenaConexion;
        private string usuarioAutenticado;
        private string rutaFoto = string.Empty;

        public MainForm(string usuario)
        {
            InitializeComponent();
            usuarioAutenticado = usuario;
            CargarConfiguracion();
            CargarDatosUsuario();
        }

        private void CargarConfiguracion()
        {
            string configPath = "config.yml";
            if (File.Exists(configPath))
            {
                var deserializer = new DeserializerBuilder()
                    .WithNamingConvention(UnderscoredNamingConvention.Instance)
                    .Build();

                string yamlContent = File.ReadAllText(configPath);
                Dictionary<string, string> config = deserializer.Deserialize<Dictionary<string, string>>(yamlContent);

                if (config.ContainsKey("host") && config.ContainsKey("port") && config.ContainsKey("database") && config.ContainsKey("user") && config.ContainsKey("password"))
                {
                    cadenaConexion = $"server={config["host"]};port={config["port"]};database={config["database"]};user={config["user"]};password={config["password"]};";
                    conexion = new MySqlConnection(cadenaConexion);
                }
                else
                {
                    MessageBox.Show("Error en la configuración YAML: faltan parámetros obligatorios.");
                    Environment.Exit(1);
                }
            }
            else
            {
                MessageBox.Show("Archivo de configuración no encontrado.");
                Environment.Exit(1);
            }
        }

        private void CargarDatosUsuario()
        {
            try
            {
                if (conexion.State == ConnectionState.Closed)
                {
                    conexion.Open();
                }

                string consulta = "SELECT ID, User, Nombre, Apellido, Genero, Ubicacion, Foto FROM Usuario WHERE User = @usuario";
                MySqlCommand cmd = new MySqlCommand(consulta, conexion);
                cmd.Parameters.AddWithValue("@usuario", usuarioAutenticado);
                MySqlDataAdapter adaptador = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adaptador.Fill(dt);
                dataGridViewUsuario.DataSource = dt;

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    txtUser.Text = row["User"].ToString();
                    txtNombre.Text = row["Nombre"].ToString();
                    txtApellido.Text = row["Apellido"].ToString();
                    cmbGenero.SelectedItem = row["Genero"].ToString();
                    txtUbicacion.Text = row["Ubicacion"].ToString();

                    // No cargamos la contraseña por seguridad
                    txtPassword.Text = "";

                    rutaFoto = row["Foto"].ToString();
                    if (File.Exists(rutaFoto))
                    {
                        pictureBoxFoto.Image = Image.FromFile(rutaFoto);
                        pictureBoxFoto.SizeMode = PictureBoxSizeMode.StretchImage;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar datos: " + ex.Message);
            }
            finally
            {
                if (conexion.State == ConnectionState.Open)
                {
                    conexion.Close();
                }
            }
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtPassword.Text))
                {
                    MessageBox.Show("La contraseña no puede estar vacía.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (conexion.State == ConnectionState.Closed)
                {
                    conexion.Open();
                }

                string consulta = "UPDATE Usuario SET Password=@password, Nombre=@nombre, Apellido=@apellido, Genero=@genero, Ubicacion=@ubicacion, Foto=@foto WHERE User=@usuario";
                MySqlCommand cmd = new MySqlCommand(consulta, conexion);
                cmd.Parameters.AddWithValue("@usuario", usuarioAutenticado);
                cmd.Parameters.AddWithValue("@password", txtPassword.Text);
                cmd.Parameters.AddWithValue("@nombre", txtNombre.Text);
                cmd.Parameters.AddWithValue("@apellido", txtApellido.Text);
                cmd.Parameters.AddWithValue("@genero", cmbGenero.SelectedItem.ToString());
                cmd.Parameters.AddWithValue("@ubicacion", txtUbicacion.Text);
                cmd.Parameters.AddWithValue("@foto", rutaFoto);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Datos actualizados correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                CargarDatosUsuario();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar datos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conexion.State == ConnectionState.Open)
                {
                    conexion.Close();
                }
            }
        }

        private void abrirMatchesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int idUsuario = ObtenerIdUsuario(usuarioAutenticado);
            if (idUsuario != -1)
            {
                MatchesForm matchesForm = new MatchesForm(usuarioAutenticado, idUsuario);
                matchesForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Error al obtener el ID del usuario.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private int ObtenerIdUsuario(string usuario)
        {
            int idUsuario = -1;
            try
            {
                if (conexion.State == ConnectionState.Closed)
                {
                    conexion.Open();
                }

                string consulta = "SELECT ID FROM Usuario WHERE User = @usuario";
                MySqlCommand cmd = new MySqlCommand(consulta, conexion);
                cmd.Parameters.AddWithValue("@usuario", usuario);
                object resultado = cmd.ExecuteScalar();

                if (resultado != null)
                {
                    idUsuario = Convert.ToInt32(resultado);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener el ID del usuario: " + ex.Message);
            }
            finally
            {
                if (conexion.State == ConnectionState.Open)
                {
                    conexion.Close();
                }
            }
            return idUsuario;
        }

        private void cerrarSesiónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();  // Oculta el formulario actual antes de abrir el LogIn
            LogIn loginForm = new LogIn();
            loginForm.ShowDialog();
            this.Close(); //
        }

        private void abrirMensajesRecibidosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReceivedMessagesForm receivedMessagesForm = new ReceivedMessagesForm(usuarioAutenticado, ObtenerIdUsuario(usuarioAutenticado));
            receivedMessagesForm.ShowDialog();
        }

        private void irAlPerfilItem_Click(object sender, EventArgs e)
        {
            try
            {
                int idUsuario = ObtenerIdUsuario(usuarioAutenticado);
                if (idUsuario != -1)
                {
                    // Abrir la página web del perfil en el navegador predeterminado
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = $"http://localhost/page/profile.html?userId={idUsuario}",
                        UseShellExecute = true
                    });
                }
                else
                {
                    MessageBox.Show("Error al obtener el ID del usuario.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al abrir la página del perfil: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void abrirMatchesPendientesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int idUsuario = ObtenerIdUsuario(usuarioAutenticado);
            if (idUsuario != -1)
            {
                PendingMatchesForm pendingMatchesForm = new PendingMatchesForm(usuarioAutenticado, idUsuario);
                pendingMatchesForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Error al obtener el ID del usuario.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private Image RedimensionarImagen(Image imgOriginal, int ancho, int alto)
        {
            Bitmap imgRedimensionada = new Bitmap(ancho, alto);
            using (Graphics g = Graphics.FromImage(imgRedimensionada))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(imgOriginal, 0, 0, ancho, alto);
            }
            return imgRedimensionada;
        }

        private void btnCargarFoto_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                Filter = "Archivos de imagen|*.jpg;*.jpeg;*.png;*.bmp"
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                rutaFoto = openFileDialog1.FileName;

                // Cargar la imagen y redimensionarla
                Image imgOriginal = Image.FromFile(rutaFoto);
                Image imgRedimensionada = RedimensionarImagen(imgOriginal, pictureBoxFoto.Width, pictureBoxFoto.Height);

                // Asignar la imagen redimensionada al PictureBox
                pictureBoxFoto.Image = imgRedimensionada;
                pictureBoxFoto.SizeMode = PictureBoxSizeMode.StretchImage;
            }
        }

        private void verUsuariosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int idUsuario = ObtenerIdUsuario(usuarioAutenticado);
            if (idUsuario != -1)
            {
                UsersForm usersForm = new UsersForm(usuarioAutenticado, idUsuario);
                usersForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Error al obtener el ID del usuario.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminarUsuario_Click(object sender, EventArgs e)
        {
            // Mostrar mensaje de confirmación
            DialogResult result = MessageBox.Show(
                "¿Está seguro que desea eliminar su cuenta? Esta acción no se puede deshacer y se eliminarán todos sus datos.",
                "Confirmar eliminación de cuenta",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                EliminarCuentaUsuario();
            }
        }
        private void EliminarCuentaUsuario()
        {
            MySqlTransaction transaction = null;
            try
            {
                // Obtener el ID del usuario antes de iniciar la transacción
                int idUsuario = ObtenerIdUsuarioSinCerrarConexion();

                if (idUsuario == -1)
                {
                    MessageBox.Show("No se pudo obtener el ID del usuario.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Asegurar que la conexión está abierta
                if (conexion.State == ConnectionState.Closed)
                {
                    conexion.Open();
                }

                // Iniciar la transacción
                transaction = conexion.BeginTransaction();

                // 1. Eliminar mensajes recibidos por el usuario (usando el nombre correcto de columna: ID_Receptor)
                string eliminarMensajesRecibidos = "DELETE FROM Mensaje WHERE ID_Receptor = @idUsuario";
                MySqlCommand cmdMensajesRecibidos = new MySqlCommand(eliminarMensajesRecibidos, conexion, transaction);
                cmdMensajesRecibidos.Parameters.AddWithValue("@idUsuario", idUsuario);
                cmdMensajesRecibidos.ExecuteNonQuery();

                // 2. Eliminar mensajes enviados por el usuario (usando el nombre correcto de columna: ID_Emisor)
                string eliminarMensajesEnviados = "DELETE FROM Mensaje WHERE ID_Emisor = @idUsuario";
                MySqlCommand cmdMensajesEnviados = new MySqlCommand(eliminarMensajesEnviados, conexion, transaction);
                cmdMensajesEnviados.Parameters.AddWithValue("@idUsuario", idUsuario);
                cmdMensajesEnviados.ExecuteNonQuery();

                // 3. Eliminar matches pendientes y confirmados
                // Nota: En tu esquema la tabla se llama "Matches" no "Match"
                string eliminarMatches = "DELETE FROM Matches WHERE ID_Acept = @idUsuario OR ID_Sol = @idUsuario";
                MySqlCommand cmdMatches = new MySqlCommand(eliminarMatches, conexion, transaction);
                cmdMatches.Parameters.AddWithValue("@idUsuario", idUsuario);
                cmdMatches.ExecuteNonQuery();

                // 4. Eliminar el perfil del usuario si existe
                string eliminarPerfil = "DELETE FROM Perfil WHERE ID_User = @idUsuario";
                MySqlCommand cmdPerfil = new MySqlCommand(eliminarPerfil, conexion, transaction);
                cmdPerfil.Parameters.AddWithValue("@idUsuario", idUsuario);
                cmdPerfil.ExecuteNonQuery();

                // 5. Finalmente, eliminar el usuario
                string eliminarUsuario = "DELETE FROM Usuario WHERE ID = @idUsuario";
                MySqlCommand cmdUsuario = new MySqlCommand(eliminarUsuario, conexion, transaction);
                cmdUsuario.Parameters.AddWithValue("@idUsuario", idUsuario);
                cmdUsuario.ExecuteNonQuery();

                // Confirmar la transacción
                transaction.Commit();

                MessageBox.Show("Cuenta eliminada correctamente. La aplicación se cerrará ahora.",
                    "Cuenta eliminada", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Cerrar la aplicación y volver al formulario de login
                this.Hide();
                LogIn loginForm = new LogIn();
                loginForm.ShowDialog();
                this.Close();
            }
            catch (Exception ex)
            {
                // Solo intentar hacer rollback si la transacción existe y la conexión está abierta
                if (transaction != null && conexion != null && conexion.State == ConnectionState.Open)
                {
                    try
                    {
                        transaction.Rollback();
                    }
                    catch
                    {
                        // Ignorar errores durante el rollback
                    }
                }

                MessageBox.Show("Error al eliminar la cuenta: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conexion != null && conexion.State == ConnectionState.Open)
                {
                    conexion.Close();
                }
            }
        }

        // Método para obtener el ID del usuario sin cerrar la conexión
        private int ObtenerIdUsuarioSinCerrarConexion()
        {
            int idUsuario = -1;
            bool conexionEstabaCerrada = (conexion.State == ConnectionState.Closed);

            try
            {
                if (conexionEstabaCerrada)
                {
                    conexion.Open();
                }

                string consulta = "SELECT ID FROM Usuario WHERE User = @usuario";
                MySqlCommand cmd = new MySqlCommand(consulta, conexion);
                cmd.Parameters.AddWithValue("@usuario", usuarioAutenticado);
                object resultado = cmd.ExecuteScalar();

                if (resultado != null)
                {
                    idUsuario = Convert.ToInt32(resultado);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener el ID del usuario: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                // Si hubo una excepción y nosotros abrimos la conexión, la cerramos
                if (conexionEstabaCerrada && conexion.State == ConnectionState.Open)
                {
                    conexion.Close();
                }
            }

            // Importante: NO cerramos la conexión aquí para mantenerla abierta para la transacción

            return idUsuario;
        }

        private void verEstadisticasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                int idUsuario = ObtenerIdUsuario(usuarioAutenticado);
                if (idUsuario != -1)
                {
                    // Abrir la página web de estadísticas en el navegador predeterminado
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = $"http://localhost/page/stats.html?userId={idUsuario}",
                        UseShellExecute = true
                    });
                }
                else
                {
                    MessageBox.Show("Error al obtener el ID del usuario.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al abrir la página de estadísticas: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}