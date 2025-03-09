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

                    rutaFoto = row["Foto"].ToString();
                    if (File.Exists(rutaFoto))
                    {
                        pictureBoxFoto.Image = Image.FromFile(rutaFoto);
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
    }
}
