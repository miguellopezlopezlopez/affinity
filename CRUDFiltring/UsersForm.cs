using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace FiltringApp
{
    public partial class UsersForm : Form
    {
        private MySqlConnection conexion;
        private string cadenaConexion;
        private string usuarioAutenticado;
        private int idUsuarioAutenticado;

        public UsersForm(string usuario, int idUsuario)
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
                var deserializer = new DeserializerBuilder()
                    .WithNamingConvention(UnderscoredNamingConvention.Instance)
                    .Build();

                string yamlContent = System.IO.File.ReadAllText(configPath);
                Dictionary<string, string> config = deserializer.Deserialize<Dictionary<string, string>>(yamlContent);

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
                string consulta = "SELECT ID, User, Nombre, Apellido, Genero, Ubicacion FROM Usuario WHERE ID != @idUsuario";
                MySqlCommand cmd = new MySqlCommand(consulta, conexion);
                cmd.Parameters.AddWithValue("@idUsuario", idUsuarioAutenticado);
                MySqlDataAdapter adaptador = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adaptador.Fill(dt);
                dataGridViewUsuarios.DataSource = dt;
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

        private void dataGridViewUsuarios_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int idReceptor = Convert.ToInt32(dataGridViewUsuarios.Rows[e.RowIndex].Cells["ID"].Value);
                string nombreReceptor = dataGridViewUsuarios.Rows[e.RowIndex].Cells["Nombre"].Value.ToString();
                string userReceptor = dataGridViewUsuarios.Rows[e.RowIndex].Cells["User"].Value.ToString();

                // Verificar si existe un match aceptado entre ambos usuarios
                try
                {
                    conexion.Open();

                    string matchQuery = @"SELECT COUNT(*) FROM Matches 
                                 WHERE ((ID_Sol = @idEmisor AND ID_Acept = @idReceptor) 
                                 OR (ID_Sol = @idReceptor AND ID_Acept = @idEmisor)) 
                                 AND Fecha_Aceptado IS NOT NULL";

                    MySqlCommand matchCmd = new MySqlCommand(matchQuery, conexion);
                    matchCmd.Parameters.AddWithValue("@idEmisor", idUsuarioAutenticado);
                    matchCmd.Parameters.AddWithValue("@idReceptor", idReceptor);
                    int matchCount = Convert.ToInt32(matchCmd.ExecuteScalar());

                    if (matchCount == 0)
                    {
                        MessageBox.Show("No puedes enviar mensajes a este usuario sin antes haber hecho match.",
                                        "Match requerido", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    // Si hay match, abrir el formulario de mensaje
                    MessageForm mensajeForm = new MessageForm(idUsuarioAutenticado, idReceptor, nombreReceptor, userReceptor);
                    mensajeForm.ShowDialog();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al verificar el match: " + ex.Message);
                }
                finally
                {
                    if (conexion.State == ConnectionState.Open)
                    {
                        conexion.Close();
                    }
                }
            }
        }
    }
}
