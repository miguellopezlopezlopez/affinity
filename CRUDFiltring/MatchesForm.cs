using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Drawing;

namespace FiltringApp
{
    public partial class MatchesForm : Form
    {
        private MySqlConnection conexion;
        private string cadenaConexion;
        private string usuarioAutenticado;
        private int idUsuarioAutenticado;
        private DataTable usuarios;
        private int indiceActual = 0;

        public MatchesForm(string usuario, int idUsuario)
        {
            InitializeComponent();
            usuarioAutenticado = usuario;
            idUsuarioAutenticado = idUsuario;
            CargarConfiguracion();
            CargarUsuarios();
        }

        private void CargarConfiguracion()
        {
            string configPath = "config.yml";
            if (System.IO.File.Exists(configPath))
            {
                var deserializer = new YamlDotNet.Serialization.DeserializerBuilder()
                    .WithNamingConvention(YamlDotNet.Serialization.NamingConventions.UnderscoredNamingConvention.Instance)
                    .Build();

                string yamlContent = System.IO.File.ReadAllText(configPath);
                var config = deserializer.Deserialize<Dictionary<string, string>>(yamlContent);

                if (config.ContainsKey("host") && config.ContainsKey("port") && config.ContainsKey("database") && config.ContainsKey("user") && config.ContainsKey("password"))
                {
                    cadenaConexion = $"server={config["host"]};port={config["port"]};database={config["database"]};user={config["user"]};password={config["password"]};";
                    conexion = new MySqlConnection(cadenaConexion);
                }
            }
        }

        private void CargarUsuarios()
        {
            try
            {
                conexion.Open();
                string consulta = "SELECT ID, User, Nombre, Ubicacion, Foto FROM Usuario WHERE ID != @idUsuario";
                MySqlCommand cmd = new MySqlCommand(consulta, conexion);
                cmd.Parameters.AddWithValue("@idUsuario", idUsuarioAutenticado);
                MySqlDataAdapter adaptador = new MySqlDataAdapter(cmd);
                usuarios = new DataTable();
                adaptador.Fill(usuarios);

                if (usuarios.Rows.Count > 0)
                {
                    MostrarUsuario(indiceActual);
                }
                else
                {
                    MessageBox.Show("No hay más usuarios disponibles.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar usuarios: " + ex.Message);
            }
            finally
            {
                conexion.Close();
            }
        }

        private void MostrarUsuario(int indice)
        {
            if (usuarios.Rows.Count > 0 && indice >= 0 && indice < usuarios.Rows.Count)
            {
                DataRow row = usuarios.Rows[indice];
                lblNombre.Text = "Nombre: " + row["Nombre"].ToString();
                lblUbicacion.Text = "Ubicación: " + row["Ubicacion"].ToString();

                string rutaFoto = row["Foto"].ToString();
                if (System.IO.File.Exists(rutaFoto))
                {
                    pictureBoxFoto.Image = Image.FromFile(rutaFoto);
                }
                else
                {
                    pictureBoxFoto.Image = null;
                }
            }
        }

        private void btnAnterior_Click(object sender, EventArgs e)
        {
            if (indiceActual > 0)
            {
                indiceActual--;
                MostrarUsuario(indiceActual);
            }
        }

        private void btnSiguiente_Click(object sender, EventArgs e)
        {
            if (indiceActual < usuarios.Rows.Count - 1)
            {
                indiceActual++;
                MostrarUsuario(indiceActual);
            }
        }

        private void btnEnviarSolicitud_Click(object sender, EventArgs e)
        {
            if (usuarios.Rows.Count > 0 && indiceActual >= 0 && indiceActual < usuarios.Rows.Count)
            {
                int idUsuarioSeleccionado = Convert.ToInt32(usuarios.Rows[indiceActual]["ID"]);

                try
                {
                    conexion.Open();

                    // Verificar si ya hay una solicitud enviada a este usuario
                    string verificarConsulta = "SELECT COUNT(*) FROM Matches WHERE ID_Sol = @idSolicitante AND ID_Acept = @idReceptor";
                    MySqlCommand verificarCmd = new MySqlCommand(verificarConsulta, conexion);
                    verificarCmd.Parameters.AddWithValue("@idSolicitante", idUsuarioAutenticado);
                    verificarCmd.Parameters.AddWithValue("@idReceptor", idUsuarioSeleccionado);
                    int coincidencias = Convert.ToInt32(verificarCmd.ExecuteScalar());

                    if (coincidencias > 0)
                    {
                        MessageBox.Show("Ya has enviado una solicitud de match a este usuario.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Insertar la solicitud en la tabla Matches
                    string consulta = "INSERT INTO Matches (ID_Sol, ID_Acept, Fecha_Solicitud) VALUES (@idSolicitante, @idReceptor, NOW())";
                    MySqlCommand cmd = new MySqlCommand(consulta, conexion);
                    cmd.Parameters.AddWithValue("@idSolicitante", idUsuarioAutenticado);
                    cmd.Parameters.AddWithValue("@idReceptor", idUsuarioSeleccionado);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Solicitud de match enviada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al enviar solicitud de match: " + ex.Message);
                }
                finally
                {
                    conexion.Close();
                }
            }
        }
    }
}
