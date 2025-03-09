using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace FiltringApp
{
    public partial class ReceivedMessagesForm : Form
    {
        private MySqlConnection conexion;
        private string cadenaConexion;
        private string usuarioAutenticado;
        private int idUsuarioAutenticado;

        public ReceivedMessagesForm(string usuario, int idUsuario)
        {
            InitializeComponent();
            usuarioAutenticado = usuario;
            idUsuarioAutenticado = idUsuario;
            CargarConfiguracion();
            CargarMensajesRecibidos();
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

        private void CargarMensajesRecibidos()
        {
            try
            {
                if (conexion.State == ConnectionState.Closed)
                {
                    conexion.Open();
                }

                string consulta = @"
                    SELECT m.ID_Mensaje, m.ID_Emisor, u.User AS Emisor, m.Fecha_Hora, m.Contenido 
                    FROM Mensaje m
                    JOIN Usuario u ON m.ID_Emisor = u.ID
                    WHERE m.ID_Receptor = @idUsuario
                    ORDER BY m.Fecha_Hora DESC";

                MySqlCommand cmd = new MySqlCommand(consulta, conexion);
                cmd.Parameters.AddWithValue("@idUsuario", idUsuarioAutenticado);

                MySqlDataAdapter adaptador = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adaptador.Fill(dt);

                dataGridViewMensajes.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar mensajes: " + ex.Message);
            }
            finally
            {
                if (conexion.State == ConnectionState.Open)
                {
                    conexion.Close();
                }
            }
        }

        private void dataGridViewMensajes_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridViewMensajes.Rows[e.RowIndex];

                int idEmisor = Convert.ToInt32(row.Cells["ID_Emisor"].Value);
                string userEmisor = row.Cells["Emisor"].Value.ToString();

                // Abrir la ventana de respuesta
                ReplyMessageForm replyForm = new ReplyMessageForm(idUsuarioAutenticado, idEmisor, userEmisor);
                replyForm.ShowDialog();
            }
        }
    }
}
