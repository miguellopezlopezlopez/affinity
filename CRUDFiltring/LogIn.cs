using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using System.Collections.Generic;

namespace FiltringApp
{
    public partial class LogIn : Form
    {
        private MySqlConnection conexion;
        private string cadenaConexion;
        private Dictionary<string, string> config;

        public LogIn()
        {
            InitializeComponent();
            CargarConfiguracion();
            EjecutarScriptSQL(); // Ejecutar el script SQL al iniciar la aplicación
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

        private void EjecutarScriptSQL()
        {
            string scriptPath = "filtringDB.sql"; // Asegúrate de que el archivo está en el mismo directorio que el ejecutable

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
                MessageBox.Show("El archivo filtringDB.sql no se encontró. Asegúrate de que el script esté en la carpeta del ejecutable.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLogIn_Click(object sender, EventArgs e)
        {
            string usuario = txtUser.Text.Trim();
            string contraseña = txtPwd.Text.Trim();

            if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(contraseña))
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

                string consulta = "SELECT ID, User FROM Usuario WHERE User = @usuario AND Password = @contraseña";
                MySqlCommand cmd = new MySqlCommand(consulta, conexion);
                cmd.Parameters.AddWithValue("@usuario", usuario);
                cmd.Parameters.AddWithValue("@contraseña", contraseña);
                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    int idUsuario = Convert.ToInt32(reader["ID"]);
                    string usuarioAutenticado = reader["User"].ToString();

                    MessageBox.Show("Inicio de sesión exitoso", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Hide(); // Oculta LogIn, pero no lo cierra

                    Form formularioDestino;
                    if (usuario.ToLower() == "admin")
                    {
                        formularioDestino = new AdminForm(usuarioAutenticado);
                    }
                    else
                    {
                        formularioDestino = new MainForm(usuarioAutenticado);
                    }

                    formularioDestino.ShowDialog();
                    this.Show(); // Muestra nuevamente LogIn cuando el otro formulario se cierre
                }
                else
                {
                    MessageBox.Show("Usuario o contraseña incorrectos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                reader.Close();
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
            RegistroForm registroForm = new RegistroForm();
            registroForm.ShowDialog();
        }
    }
}
