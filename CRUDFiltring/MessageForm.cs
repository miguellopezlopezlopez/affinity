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

                // 1️⃣ Verificar si ya existe un match entre los usuarios
                string matchQuery = "SELECT ID_Match FROM Matches WHERE (ID_Acept = @idReceptor AND ID_Sol = @idEmisor) OR (ID_Acept = @idEmisor AND ID_Sol = @idReceptor)";
                MySqlCommand matchCmd = new MySqlCommand(matchQuery, conexion);
                matchCmd.Parameters.AddWithValue("@idEmisor", idEmisor);
                matchCmd.Parameters.AddWithValue("@idReceptor", idReceptor);
                object matchId = matchCmd.ExecuteScalar();

                // 2️⃣ Si no hay match, crearlo
                if (matchId == null)
                {
                    MySqlCommand crearMatch = new MySqlCommand("INSERT INTO Matches (ID_Acept, ID_Sol, Fecha_Solicitud) VALUES (@idReceptor, @idEmisor, NOW())", conexion);
                    crearMatch.Parameters.AddWithValue("@idEmisor", idEmisor);
                    crearMatch.Parameters.AddWithValue("@idReceptor", idReceptor);
                    crearMatch.ExecuteNonQuery();
                    matchId = crearMatch.LastInsertedId;
                }

                // 3️⃣ Insertar el mensaje en la base de datos
                MySqlCommand insertCmd = new MySqlCommand("INSERT INTO Mensaje (ID_Match, ID_Emisor, ID_Receptor, Contenido, Fecha_Hora) VALUES (@match, @emisor, @receptor, @contenido, NOW())", conexion);
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
