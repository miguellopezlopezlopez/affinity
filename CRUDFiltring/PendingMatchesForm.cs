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
            this.Load += new EventHandler(PendingMatchesForm_Load);
            usuarioAutenticado = usuario;
            idUsuarioAutenticado = idUsuario;
            CargarConfiguracion();
        }

        private void CargarConfiguracion()
        {
            string configPath = "config.yml";
            try
            {
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
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar la configuración: {ex.Message}");
                this.Close();
            }
        }

        private void PendingMatchesForm_Load(object sender, EventArgs e)
        {
            try
            {
                this.Text = $"Solicitudes de Match - {usuarioAutenticado}";
                CargarSolicitudes();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error en el evento Load: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarSolicitudes()
        {
            try
            {
                if (conexion == null)
                {
                    MessageBox.Show("Error: La conexión no está inicializada.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (conexion.State == ConnectionState.Closed)
                {
                    conexion.Open();
                }

                string consulta = @"
                    SELECT 
                        m.ID_Match,
                        u.User as 'Usuario',
                        u.Nombre,
                        DATE_FORMAT(m.Fecha_Solicitud, '%d/%m/%Y %H:%i') as 'Fecha Solicitud'
                    FROM Matches m 
                    JOIN Usuario u ON m.ID_Sol = u.ID 
                    WHERE m.ID_Acept = @idUsuario 
                    AND m.Fecha_Aceptado IS NULL
                    ORDER BY m.Fecha_Solicitud DESC";

                MySqlCommand cmd = new MySqlCommand(consulta, conexion);
                cmd.Parameters.AddWithValue("@idUsuario", idUsuarioAutenticado);
                
                MySqlDataAdapter adaptador = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adaptador.Fill(dt);
                
                dataGridViewSolicitudes.DataSource = dt;

                // Configurar las columnas del DataGridView
                if (dataGridViewSolicitudes.Columns.Count > 0)
                {
                    dataGridViewSolicitudes.Columns["ID_Match"].HeaderText = "ID";
                    dataGridViewSolicitudes.Columns["ID_Match"].Width = 50;
                    
                    dataGridViewSolicitudes.Columns["Usuario"].HeaderText = "Usuario";
                    dataGridViewSolicitudes.Columns["Usuario"].Width = 150;
                    
                    dataGridViewSolicitudes.Columns["Nombre"].HeaderText = "Nombre";
                    dataGridViewSolicitudes.Columns["Nombre"].Width = 200;
                    
                    dataGridViewSolicitudes.Columns["Fecha Solicitud"].Width = 150;
                }

                // Mostrar mensaje si no hay solicitudes
                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("No tienes solicitudes de match pendientes.", 
                                  "Información", 
                                  MessageBoxButtons.OK, 
                                  MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar solicitudes de match: " + ex.Message,
                              "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conexion != null && conexion.State == ConnectionState.Open)
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
                string usuario = dataGridViewSolicitudes.SelectedRows[0].Cells["Usuario"].Value.ToString();

                DialogResult confirmacion = MessageBox.Show(
                    $"¿Estás seguro de que quieres aceptar el match con {nombreUsuario} (@{usuario})?",
                    "Confirmar Match",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (confirmacion == DialogResult.Yes)
                {
                    try
                    {
                        if (conexion.State == ConnectionState.Closed)
                        {
                            conexion.Open();
                        }

                        // Primero verificamos que el match aún esté pendiente
                        string verificarMatch = "SELECT COUNT(*) FROM Matches WHERE ID_Match = @idMatch AND Fecha_Aceptado IS NULL";
                        MySqlCommand cmdVerificar = new MySqlCommand(verificarMatch, conexion);
                        cmdVerificar.Parameters.AddWithValue("@idMatch", idMatch);
                        int matchesPendientes = Convert.ToInt32(cmdVerificar.ExecuteScalar());

                        if (matchesPendientes == 0)
                        {
                            MessageBox.Show(
                                "Esta solicitud de match ya no está disponible. Puede que haya sido procesada o eliminada.",
                                "Match no disponible",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning
                            );
                            CargarSolicitudes();
                            return;
                        }

                        string actualizarMatch = "UPDATE Matches SET Fecha_Aceptado = NOW() WHERE ID_Match = @idMatch";
                        MySqlCommand cmd = new MySqlCommand(actualizarMatch, conexion);
                        cmd.Parameters.AddWithValue("@idMatch", idMatch);
                        cmd.ExecuteNonQuery();

                        MessageBox.Show(
                            $"¡Match aceptado con éxito!\n\nAhora puedes chatear con {nombreUsuario} (@{usuario}).",
                            "Match Aceptado",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information
                        );
                        
                        CargarSolicitudes();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(
                            $"Error al aceptar el match: {ex.Message}",
                            "Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error
                        );
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
            else
            {
                MessageBox.Show(
                    "Por favor, selecciona una solicitud de match para aceptar.",
                    "Selección requerida",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
            }
        }

        private void btnRechazarMatch_Click(object sender, EventArgs e)
        {
            if (dataGridViewSolicitudes.SelectedRows.Count > 0)
            {
                int idMatch = Convert.ToInt32(dataGridViewSolicitudes.SelectedRows[0].Cells["ID_Match"].Value);
                string nombreUsuario = dataGridViewSolicitudes.SelectedRows[0].Cells["Nombre"].Value.ToString();
                string usuario = dataGridViewSolicitudes.SelectedRows[0].Cells["Usuario"].Value.ToString();

                DialogResult confirmacion = MessageBox.Show(
                    $"¿Estás seguro de que quieres rechazar el match con {nombreUsuario} (@{usuario})?\n\nEsta acción no se puede deshacer.",
                    "Confirmar Rechazo",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                if (confirmacion == DialogResult.Yes)
                {
                    try
                    {
                        if (conexion.State == ConnectionState.Closed)
                        {
                            conexion.Open();
                        }

                        // Primero verificamos que el match aún exista
                        string verificarMatch = "SELECT COUNT(*) FROM Matches WHERE ID_Match = @idMatch";
                        MySqlCommand cmdVerificar = new MySqlCommand(verificarMatch, conexion);
                        cmdVerificar.Parameters.AddWithValue("@idMatch", idMatch);
                        int matchesExistentes = Convert.ToInt32(cmdVerificar.ExecuteScalar());

                        if (matchesExistentes == 0)
                        {
                            MessageBox.Show(
                                "Esta solicitud de match ya no existe en el sistema.",
                                "Match no encontrado",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning
                            );
                            CargarSolicitudes();
                            return;
                        }

                        string eliminarMatch = "DELETE FROM Matches WHERE ID_Match = @idMatch";
                        MySqlCommand cmd = new MySqlCommand(eliminarMatch, conexion);
                        cmd.Parameters.AddWithValue("@idMatch", idMatch);
                        cmd.ExecuteNonQuery();

                        MessageBox.Show(
                            $"Has rechazado el match con {nombreUsuario}.",
                            "Match Rechazado",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information
                        );
                        
                        CargarSolicitudes();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(
                            $"Error al rechazar el match: {ex.Message}",
                            "Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error
                        );
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
            else
            {
                MessageBox.Show(
                    "Por favor, selecciona una solicitud de match para rechazar.",
                    "Selección requerida",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
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