using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using System.Drawing;

namespace FiltringApp
{
    public partial class MessageForm : Form
    {
        private readonly int idEmisor;
        private readonly int idReceptor;
        private readonly string nombreReceptor;
        private readonly string userReceptor;
        private ChatConnection chatConnection;
        private MySqlConnection conexion;
        private string cadenaConexion;
        private int? idMatch;

        public MessageForm(int idEmisor, int idReceptor, string nombreReceptor, string userReceptor)
        {
            InitializeComponent();
            
            this.idEmisor = idEmisor;
            this.idReceptor = idReceptor;
            this.nombreReceptor = nombreReceptor;
            this.userReceptor = userReceptor;

            // Configurar el formulario
            this.Text = "Chat";
            this.Size = new Size(600, 500);
            this.MinimumSize = new Size(400, 300);

            // Configurar los controles existentes
            ConfigureExistingControls();

            // Configurar eventos
            ConfigureEvents();

            lblReceptor.Text = $"Chat con {nombreReceptor} (@{userReceptor})";
            lblEstado.Text = "Conectando...";

            // Cargar configuración de base de datos y verificar match
            CargarConfiguracion();
            VerificarMatch();

            // Configurar la conexión del chat en tiempo real
            ConfigurarChat();
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
                var config = deserializer.Deserialize<Dictionary<string, string>>(yamlContent);

                if (config.ContainsKey("host") && config.ContainsKey("port") && config.ContainsKey("database") && config.ContainsKey("user") && config.ContainsKey("password"))
                {
                    cadenaConexion = $"server={config["host"]};port={config["port"]};database={config["database"]};user={config["user"]};password={config["password"]};";
                    conexion = new MySqlConnection(cadenaConexion);
                }
            }
        }

        private void VerificarMatch()
        {
            try
            {
                if (conexion.State == ConnectionState.Closed)
                {
                    conexion.Open();
                }

                string consultaMatch = @"
                    SELECT ID_Match 
                    FROM Matches 
                    WHERE ((ID_Acept = @idEmisor AND ID_Sol = @idReceptor) 
                           OR (ID_Acept = @idReceptor AND ID_Sol = @idEmisor))
                    AND Fecha_Aceptado IS NOT NULL";

                MySqlCommand cmd = new MySqlCommand(consultaMatch, conexion);
                cmd.Parameters.AddWithValue("@idEmisor", idEmisor);
                cmd.Parameters.AddWithValue("@idReceptor", idReceptor);

                object resultado = cmd.ExecuteScalar();
                
                if (resultado != null)
                {
                    idMatch = Convert.ToInt32(resultado);
                }
                else
                {
                    MessageBox.Show("No existe un match activo con este usuario.",
                                  "Error",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Warning);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al verificar el match: {ex.Message}",
                              "Error",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
            }
            finally
            {
                if (conexion.State == ConnectionState.Open)
                {
                    conexion.Close();
                }
            }
        }

        private void ConfigureEvents()
        {
            btnEnviar.Click -= btnEnviar_Click;
            btnEnviar.Click += btnEnviar_Click;

            txtMensaje.KeyPress -= TxtMensaje_KeyPress;
            txtMensaje.KeyPress += TxtMensaje_KeyPress;
        }

        private void TxtMensaje_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter && !ModifierKeys.HasFlag(Keys.Shift))
            {
                e.Handled = true;
                btnEnviar_Click(sender, EventArgs.Empty);
            }
        }

        private void ConfigureExistingControls()
        {
            txtMensajes.Multiline = true;
            txtMensajes.ReadOnly = true;
            txtMensajes.ScrollBars = ScrollBars.Vertical;
            txtMensajes.Font = new Font("Segoe UI", 9.75F);
            txtMensajes.BackColor = Color.White;

            txtMensaje.Multiline = true;
            txtMensaje.Font = new Font("Segoe UI", 9.75F);
            txtMensaje.AcceptsReturn = true;

            btnEnviar.Font = new Font("Segoe UI", 9.75F);
            btnEnviar.Cursor = Cursors.Hand;

            lblReceptor.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblReceptor.AutoSize = true;

            lblEstado.Font = new Font("Segoe UI", 9F);
            lblEstado.ForeColor = Color.Gray;
            lblEstado.AutoSize = true;
        }

        private async void ConfigurarChat()
        {
            try
            {
                chatConnection = new ChatConnection(idEmisor, userReceptor);
                chatConnection.MessageReceived += ChatConnection_MessageReceived;
                chatConnection.ConnectionStatusChanged += ChatConnection_ConnectionStatusChanged;
                await chatConnection.ConnectAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al conectar al chat: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblEstado.Text = "Error de conexión";
                lblEstado.ForeColor = Color.Red;
            }
        }

        private void ChatConnection_MessageReceived(object sender, ChatMessageEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => ChatConnection_MessageReceived(sender, e)));
                return;
            }

            string mensajeFormateado = $"[{e.Fecha:HH:mm:ss}] {e.EmisorNombre}: {e.Contenido}\r\n";
            txtMensajes.AppendText(mensajeFormateado);
            txtMensajes.ScrollToCaret();

            // Guardar el mensaje recibido en la base de datos
            GuardarMensajeEnBD(e.Contenido, idReceptor, idEmisor);
        }

        private void ChatConnection_ConnectionStatusChanged(object sender, string status)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => ChatConnection_ConnectionStatusChanged(sender, status)));
                return;
            }

            lblEstado.Text = status;
            lblEstado.ForeColor = status.Contains("Error") ? Color.Red : Color.Gray;
        }

        private async void btnEnviar_Click(object sender, EventArgs e)
        {
            if (chatConnection == null)
            {
                MessageBox.Show("No hay conexión con el servidor de chat.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string mensaje = txtMensaje.Text.Trim();
            if (string.IsNullOrEmpty(mensaje))
            {
                MessageBox.Show("El mensaje no puede estar vacío.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                btnEnviar.Enabled = false;
                
                // Enviar mensaje por el chat en tiempo real
                await chatConnection.SendMessageAsync(idReceptor, mensaje);

                // Guardar mensaje en la base de datos
                GuardarMensajeEnBD(mensaje, idEmisor, idReceptor);

                txtMensaje.Clear();
                string mensajeFormateado = $"[{DateTime.Now:HH:mm:ss}] Tú: {mensaje}\r\n";
                txtMensajes.AppendText(mensajeFormateado);
                txtMensajes.ScrollToCaret();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al enviar el mensaje: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnEnviar.Enabled = true;
                txtMensaje.Focus();
            }
        }

        private void GuardarMensajeEnBD(string contenido, int emisor, int receptor)
        {
            if (!idMatch.HasValue) return;

            try
            {
                if (conexion.State == ConnectionState.Closed)
                {
                    conexion.Open();
                }

                string consultaMensaje = @"
                    INSERT INTO Mensaje (ID_Match, Fecha_Hora, Contenido, ID_Emisor, ID_Receptor) 
                    VALUES (@idMatch, NOW(), @contenido, @idEmisor, @idReceptor)";

                MySqlCommand cmd = new MySqlCommand(consultaMensaje, conexion);
                cmd.Parameters.AddWithValue("@idMatch", idMatch.Value);
                cmd.Parameters.AddWithValue("@contenido", contenido);
                cmd.Parameters.AddWithValue("@idEmisor", emisor);
                cmd.Parameters.AddWithValue("@idReceptor", receptor);

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                // Log el error pero no interrumpir el flujo del chat
                Console.WriteLine($"Error al guardar mensaje en BD: {ex.Message}");
            }
            finally
            {
                if (conexion.State == ConnectionState.Open)
                {
                    conexion.Close();
                }
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            chatConnection?.Disconnect();
            if (conexion != null)
            {
                if (conexion.State == ConnectionState.Open)
                {
                    conexion.Close();
                }
                conexion.Dispose();
            }
        }
    }
}
