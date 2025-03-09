using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace FiltringApp
{
    public partial class ReplyMessageForm : Form
    {
        private MySqlConnection conexion;
        private string cadenaConexion;
        private int idEmisor;
        private int idReceptor;
        private string userReceptor;

        public ReplyMessageForm(int emisor, int receptor, string userReceptor)
        {
            InitializeComponent();
            idEmisor = emisor;
            idReceptor = receptor;
            this.userReceptor = userReceptor;
            CargarConfiguracion();
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

        private void btnEnviar_Click(object sender, EventArgs e)
        {
            string mensaje = txtMensaje.Text.Trim();

            if (string.IsNullOrEmpty(mensaje))
            {
                MessageBox.Show("No puedes enviar un mensaje vacío.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (conexion.State == ConnectionState.Closed)
                {
                    conexion.Open();
                }

                // Buscar si hay un match entre los usuarios
                string consultaMatch = "SELECT ID_Match FROM Matches WHERE (ID_Acept = @idEmisor AND ID_Sol = @idReceptor) OR (ID_Acept = @idReceptor AND ID_Sol = @idEmisor)";
                MySqlCommand cmdMatch = new MySqlCommand(consultaMatch, conexion);
                cmdMatch.Parameters.AddWithValue("@idEmisor", idEmisor);
                cmdMatch.Parameters.AddWithValue("@idReceptor", idReceptor);

                object matchIdObj = cmdMatch.ExecuteScalar();

                if (matchIdObj == null)
                {
                    MessageBox.Show("No tienes un match con este usuario. No puedes enviarle mensajes.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int idMatch = Convert.ToInt32(matchIdObj);

                // Insertar el mensaje en la base de datos
                string consultaMensaje = "INSERT INTO Mensaje (ID_Match, Fecha_Hora, Contenido, ID_Emisor, ID_Receptor) VALUES (@idMatch, NOW(), @contenido, @idEmisor, @idReceptor)";
                MySqlCommand cmdMensaje = new MySqlCommand(consultaMensaje, conexion);
                cmdMensaje.Parameters.AddWithValue("@idMatch", idMatch);
                cmdMensaje.Parameters.AddWithValue("@contenido", mensaje);
                cmdMensaje.Parameters.AddWithValue("@idEmisor", idEmisor);
                cmdMensaje.Parameters.AddWithValue("@idReceptor", idReceptor);

                cmdMensaje.ExecuteNonQuery();

                MessageBox.Show("Mensaje enviado con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al enviar mensaje: " + ex.Message);
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
