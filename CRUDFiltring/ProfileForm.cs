using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using System.Collections.Generic;
using FiltringApp;

namespace FiltringApp
{
    public partial class ProfileForm : Form
    {
        private MySqlConnection conexion;
        private string cadenaConexion;
        private string usuarioAutenticado;

        public ProfileForm(string usuario)
        {
            InitializeComponent();
            usuarioAutenticado = usuario;
            CargarConfiguracion();
            CargarDatosPerfil();
            ConfigurarMenu();
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

        private void CargarDatosPerfil()
        {
            try
            {
                if (conexion.State == ConnectionState.Closed)
                {
                    conexion.Open();
                }

                string consulta = @"SELECT p.Biografia, p.Intereses, p.Preferencias
                                    FROM Perfil p
                                    INNER JOIN Usuario u ON p.ID_User = u.ID
                                    WHERE u.User = @usuario";

                MySqlCommand cmd = new MySqlCommand(consulta, conexion);
                cmd.Parameters.AddWithValue("@usuario", usuarioAutenticado);
                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    txtBiografia.Text = reader["Biografia"].ToString();
                    txtIntereses.Text = reader["Intereses"].ToString();
                    txtPreferencias.Text = reader["Preferencias"].ToString();
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los datos del perfil: " + ex.Message);
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
                if (conexion.State == ConnectionState.Closed)
                {
                    conexion.Open();
                }

                string consultaPerfil = @"INSERT INTO Perfil (ID_User, Biografia, Intereses, Preferencias)
                                          VALUES ((SELECT ID FROM Usuario WHERE User = @usuario), @biografia, @intereses, @preferencias)
                                          ON DUPLICATE KEY UPDATE 
                                          Biografia=@biografia, Intereses=@intereses, Preferencias=@preferencias;";

                MySqlCommand cmdPerfil = new MySqlCommand(consultaPerfil, conexion);
                cmdPerfil.Parameters.AddWithValue("@usuario", usuarioAutenticado);
                cmdPerfil.Parameters.AddWithValue("@biografia", txtBiografia.Text);
                cmdPerfil.Parameters.AddWithValue("@intereses", txtIntereses.Text);
                cmdPerfil.Parameters.AddWithValue("@preferencias", txtPreferencias.Text);
                cmdPerfil.ExecuteNonQuery();

                MessageBox.Show("Perfil actualizado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar el perfil: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conexion.State == ConnectionState.Open)
                {
                    conexion.Close();
                }
            }
        }

        // Configuración del menú
        private void ConfigurarMenu()
        {
            MenuStrip menuStrip = new MenuStrip();
            ToolStripMenuItem menuOpciones = new ToolStripMenuItem("Opciones");

            ToolStripMenuItem volverAlMenuItem = new ToolStripMenuItem("Volver al Menú");
            volverAlMenuItem.Click += menúDeUsuarioToolStripMenuItem_Click;

            ToolStripMenuItem cerrarSesionItem = new ToolStripMenuItem("Cerrar Sesión");
            cerrarSesionItem.Click += cerrarSesiónToolStripMenuItem1_Click;

            menuOpciones.DropDownItems.Add(volverAlMenuItem);
            menuOpciones.DropDownItems.Add(cerrarSesionItem);
            menuStrip.Items.Add(menuOpciones);
            Controls.Add(menuStrip);
            MainMenuStrip = menuStrip;
        }

        // Evento para cerrar sesión y volver a LogIn
        private void cerrarSesiónToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Hide();
            LogIn loginForm = new LogIn();
            loginForm.Show();
            this.Close();
        }

        // Evento para volver al menú de usuario
        private void menúDeUsuarioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            MainForm mainForm = new MainForm(usuarioAutenticado);
            mainForm.Show();
            this.Close();
        }
    }
}
