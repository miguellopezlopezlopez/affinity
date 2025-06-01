using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace FiltringApp
{
    public partial class PendingMatchesForm : Form
    {
        private MySqlConnection conexion;
        private string cadenaConexion;
        private int idUsuarioAutenticado;
        private string usuarioAutenticado;

        public PendingMatchesForm(string usuario, int idUsuario)
        {
            InitializeComponent();
            usuarioAutenticado = usuario;
            idUsuarioAutenticado = idUsuario;
            CargarConfiguracion();
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
                else
                {
                    MessageBox.Show("Error en la configuración YAML: faltan parámetros obligatorios.");
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Archivo de configuración no encontrado.");
                this.Close();
            }
        }

        private void PendingMatchesForm_Load(object sender, EventArgs e)
        {
            this.Text = $"Solicitudes de Match - {usuarioAutenticado}";
            CargarSolicitudes();
        }

        private void CargarSolicitudes()
        {
            try
            {
                if (conexion.State == ConnectionState.Closed)
                {
                    conexion.Open();
                }

                string consulta = "SELECT m.ID_Match, u.User, u.Nombre FROM Matches m JOIN Usuario u ON m.ID_Sol = u.ID WHERE m.ID_Acept = @idUsuario AND m.Fecha_Aceptado IS NULL";
                MySqlCommand cmd = new MySqlCommand(consulta, conexion);
                cmd.Parameters.AddWithValue("@idUsuario", idUsuarioAutenticado);
                MySqlDataAdapter adaptador = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adaptador.Fill(dt);
                dataGridViewSolicitudes.DataSource = dt;

                // Configure columns display for better UI
                if (dataGridViewSolicitudes.Columns.Count > 0)
                {
                    dataGridViewSolicitudes.Columns["ID_Match"].HeaderText = "ID";
                    dataGridViewSolicitudes.Columns["User"].HeaderText = "Usuario";
                    dataGridViewSolicitudes.Columns["Nombre"].HeaderText = "Nombre";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar solicitudes de match: " + ex.Message,
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conexion.State == ConnectionState.Open)
                {
                    conexion.Close();
                }
            }
        }

        private void btnAceptarMatch_Click(object sender, EventArgs e)
        {
            if (dataGridViewSolicitudes.SelectedRows.Count > 0)
            {
                int idMatch = Convert.ToInt32(dataGridViewSolicitudes.SelectedRows[0].Cells["ID_Match"].Value);
                string nombreUsuario = dataGridViewSolicitudes.SelectedRows[0].Cells["Nombre"].Value.ToString();

                try
                {
                    if (conexion.State == ConnectionState.Closed)
                    {
                        conexion.Open();
                    }

                    string actualizarMatch = "UPDATE Matches SET Fecha_Aceptado = NOW() WHERE ID_Match = @idMatch";
                    MySqlCommand cmd = new MySqlCommand(actualizarMatch, conexion);
                    cmd.Parameters.AddWithValue("@idMatch", idMatch);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show($"Match con {nombreUsuario} aceptado. Ahora pueden enviarse mensajes.",
                                    "Match Aceptado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarSolicitudes();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al aceptar el match: " + ex.Message,
                                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    if (conexion.State == ConnectionState.Open)
                    {
                        conexion.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("Por favor, seleccione un match para aceptar.",
                                "Selección requerida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnRechazarMatch_Click(object sender, EventArgs e)
        {
            if (dataGridViewSolicitudes.SelectedRows.Count > 0)
            {
                int idMatch = Convert.ToInt32(dataGridViewSolicitudes.SelectedRows[0].Cells["ID_Match"].Value);
                string nombreUsuario = dataGridViewSolicitudes.SelectedRows[0].Cells["Nombre"].Value.ToString();

                try
                {
                    if (conexion.State == ConnectionState.Closed)
                    {
                        conexion.Open();
                    }

                    string eliminarMatch = "DELETE FROM Matches WHERE ID_Match = @idMatch";
                    MySqlCommand cmd = new MySqlCommand(eliminarMatch, conexion);
                    cmd.Parameters.AddWithValue("@idMatch", idMatch);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show($"Match con {nombreUsuario} rechazado.",
                                    "Match Rechazado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarSolicitudes();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al rechazar el match: " + ex.Message,
                                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    if (conexion.State == ConnectionState.Open)
                    {
                        conexion.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("Por favor, seleccione un match para rechazar.",
                                "Selección requerida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnRefrescar_Click(object sender, EventArgs e)
        {
            CargarSolicitudes();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}