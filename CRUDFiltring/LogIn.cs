using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using System.Collections.Generic;
using System.Net.Http;

namespace FiltringApp
{
    public partial class LogIn : Form
    {
        private MySqlConnection conexion;
        private string cadenaConexion;
        private Dictionary<string, string> config;
        private readonly string baseUrl;

        public LogIn()
        {
            InitializeComponent();
            CargarConfiguracion();
            EjecutarScriptSQL(); // Ejecutar el script SQL al iniciar la aplicacin
            baseUrl = "http://localhost"; // This should come from config in production
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
                config = deserializer.Deserialize<Dictionary<string, string>>(yamlContent);

                if (config.ContainsKey("host") && config.ContainsKey("port") && config.ContainsKey("database") && config.ContainsKey("user") && config.ContainsKey("password"))
                {
                    cadenaConexion = $"server={config["host"]};port={config["port"]};database={config["database"]};user={config["user"]};password={config["password"]};";
                    conexion = new MySqlConnection(cadenaConexion);
                }
                else
                {
                    MessageBox.Show("Error en la configuracin YAML: faltan parmetros obligatorios.");
                    Environment.Exit(1);
                }
            }
            else
            {
                MessageBox.Show("Archivo de configuracin no encontrado.");
                Environment.Exit(1);
            }
        }

        private void EjecutarScriptSQL()
        {
            string scriptPath = "filtringDB.sql";

            if (File.Exists(scriptPath))
            {
                try
                {
                    string script = File.ReadAllText(scriptPath);

                    // Conectarse sin especificar la base de datos para asegurarnos de que se pueda crear
                    string cadenaConexionInicial = $"server={config["host"]};port={config["port"]};user={config["user"]};password={config["password"]};";
                    using (MySqlConnection conn = new MySqlConnection(cadenaConexionInicial))
                    {
                        conn.Open();
                        MySqlCommand cmd = new MySqlCommand(script, conn);
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al ejecutar el script SQL: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("El archivo filtringDB.sql no se encontr. Asegrate de que el script est en la carpeta del ejecutable.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnLogIn_Click(object sender, EventArgs e)
        {
            string usuario = txtUser.Text.Trim();
            string contrasea = txtPwd.Text.Trim();

            if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(contrasea))
            {
                MessageBox.Show("Por favor, complete todos los campos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (conexion.State == ConnectionState.Closed)
                {
                    conexion.Open();
                }

                string consulta = "SELECT ID, User FROM Usuario WHERE User = @usuario AND Password = @contrasea";
                MySqlCommand cmd = new MySqlCommand(consulta, conexion);
                cmd.Parameters.AddWithValue("@usuario", usuario);
                cmd.Parameters.AddWithValue("@contrasea", contrasea);
                MySqlDataReader resultado = cmd.ExecuteReader();

                if (resultado.HasRows)
                {
                    resultado.Read();
                    string usuarioAutenticado = resultado.GetString("User");
                    int userId = resultado.GetInt32("ID");
                    
                    try
                    {
                        // Intentar establecer la sesión web
                        using (var httpClient = new HttpClient())
                        {
                            var response = await httpClient.GetAsync($"{baseUrl}/page/api/set_session.php?userId={userId}");
                            if (!response.IsSuccessStatusCode)
                            {
                                MessageBox.Show("Error al establecer la sesión web. Algunas funciones pueden no estar disponibles.", 
                                    "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }

                        Form formularioDestino = new MainForm(usuarioAutenticado);
                        this.Hide();
                        formularioDestino.ShowDialog();
                        this.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al conectar con el servidor web: {ex.Message}\nAlgunas funciones pueden no estar disponibles.", 
                            "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        
                        // Continuar con la aplicación de escritorio aunque falle la web
                        Form formularioDestino = new MainForm(usuarioAutenticado);
                        this.Hide();
                        formularioDestino.ShowDialog();
                        this.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Usuario o contraseña incorrectos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                resultado.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conexion.State == ConnectionState.Open)
                {
                    conexion.Close();
                }
            }
        }

        private void btnRegistro_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = $"{baseUrl}/page/register.html",
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al abrir la página web: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = $"{baseUrl}/page/index.html",
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al abrir la página web: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
