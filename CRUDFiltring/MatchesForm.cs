using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace FiltringApp
{
    public partial class MatchesForm : Form
    {
        private MySqlConnection conexion;
        private string cadenaConexion;
        private string usuarioAutenticado;
        private int idUsuarioAutenticado;

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

                MessageForm mensajeForm = new MessageForm(idUsuarioAutenticado, idReceptor, nombreReceptor, userReceptor);
                mensajeForm.ShowDialog();
            }
        }
    }
}
