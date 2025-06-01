using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace FiltringApp
{
    public partial class MessageForm : Form
    {
        private MySqlConnection conexion;
        private string cadenaConexion;
        private int idEmisor;
        private int idReceptor;

        public MessageForm(int idEmisor, int idReceptor, string nombreReceptor, string userReceptor)
        {
            InitializeComponent();
            this.idEmisor = idEmisor;
            this.idReceptor = idReceptor;
            lblReceptor.Text = $"Enviar mensaje a {nombreReceptor} ({userReceptor})";
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
                MessageBox.Show("El mensaje no puede estar vacío.");
                return;
            }

            try
            {
                conexion.Open();

                // Verificar si existe un match aceptado entre los usuarios
                string matchQuery = @"SELECT ID_Match FROM Matches 
                             WHERE ((ID_Sol = @idEmisor AND ID_Acept = @idReceptor) 
                             OR (ID_Sol = @idReceptor AND ID_Acept = @idEmisor)) 
                             AND Fecha_Aceptado IS NOT NULL";

                MySqlCommand matchCmd = new MySqlCommand(matchQuery, conexion);
                matchCmd.Parameters.AddWithValue("@idEmisor", idEmisor);
                matchCmd.Parameters.AddWithValue("@idReceptor", idReceptor);
                object matchId = matchCmd.ExecuteScalar();

                if (matchId == null)
                {
                    MessageBox.Show("No puedes enviar mensajes a este usuario sin antes haber hecho match.",
                                    "Match requerido", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Insertar el mensaje en la base de datos
                MySqlCommand insertCmd = new MySqlCommand(
                    "INSERT INTO Mensaje (ID_Match, ID_Emisor, ID_Receptor, Contenido, Fecha_Hora) VALUES (@match, @emisor, @receptor, @contenido, NOW())",
                    conexion);

                insertCmd.Parameters.AddWithValue("@match", matchId);
                insertCmd.Parameters.AddWithValue("@emisor", idEmisor);
                insertCmd.Parameters.AddWithValue("@receptor", idReceptor);
                insertCmd.Parameters.AddWithValue("@contenido", mensaje);
                insertCmd.ExecuteNonQuery();

                MessageBox.Show("Mensaje enviado correctamente.");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al enviar mensaje: " + ex.Message);
            }
            finally
            {
                conexion.Close();
            }
        }
    }
}
