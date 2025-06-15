using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Text.Json;
using System.Diagnostics;

namespace FiltringApp
{
    public partial class MainForm : Form
    {
        private MySqlConnection conexion;
        private string cadenaConexion;
        private string usuarioAutenticado;
        private string rutaFoto = string.Empty;
        private const string BASE_URL = "http://localhost/page/";

        public MainForm(string usuario)
        {
            InitializeComponent();
            usuarioAutenticado = usuario;
            
            // Configurar el PictureBox
            pictureBoxFoto.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxFoto.BackColor = Color.White;
            pictureBoxFoto.BorderStyle = BorderStyle.FixedSingle;

            CargarConfiguracion();
            CargarDatosUsuario();
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

        private void CargarDatosUsuario()
        {
            try
            {
                if (conexion.State == ConnectionState.Closed)
                {
                    conexion.Open();
                }

                string consulta = "SELECT ID, User, Nombre, Apellido, Genero, Ubicacion, Foto FROM Usuario WHERE User = @usuario";
                MySqlCommand cmd = new MySqlCommand(consulta, conexion);
                cmd.Parameters.AddWithValue("@usuario", usuarioAutenticado);
                MySqlDataAdapter adaptador = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adaptador.Fill(dt);
                dataGridViewUsuario.DataSource = dt;

                // Ocultar las columnas ID y Foto
                if (dataGridViewUsuario.Columns.Contains("ID"))
                {
                    dataGridViewUsuario.Columns["ID"].Visible = false;
                }
                if (dataGridViewUsuario.Columns.Contains("Foto"))
                {
                    dataGridViewUsuario.Columns["Foto"].Visible = false;
                }

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    txtUser.Text = row["User"].ToString();
                    txtNombre.Text = row["Nombre"].ToString();
                    txtApellido.Text = row["Apellido"].ToString();
                    cmbGenero.SelectedItem = row["Genero"].ToString();
                    txtUbicacion.Text = row["Ubicacion"].ToString();
                    txtPassword.Text = "";

                    rutaFoto = row["Foto"].ToString();
                    
                    // Limpiar imagen anterior si existe
                    if (pictureBoxFoto.Image != null)
                    {
                        var oldImage = pictureBoxFoto.Image;
                        pictureBoxFoto.Image = null;
                        oldImage.Dispose();
                    }

                    // Construir la URL completa para la imagen
                    string imageUrl = rutaFoto;
                    if (!string.IsNullOrEmpty(rutaFoto))
                    {
                        try
                        {
                            pictureBoxFoto.Image = CargarImagenDesdeUrl(rutaFoto);
                            if (pictureBoxFoto.Image == null)
                            {
                                // Si falla la carga de la imagen, intentar cargar la imagen por defecto
                                pictureBoxFoto.Image = CargarImagenDesdeUrl("images/default-profile.png");
                            }
                        }
                        catch
                        {
                            // Si falla todo, no mostrar ninguna imagen
                            pictureBoxFoto.Image = null;
                        }
                    }
                    else
                    {
                        // Si no hay ruta de foto, cargar la imagen por defecto
                        pictureBoxFoto.Image = CargarImagenDesdeUrl("images/default-profile.png");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar datos: " + ex.Message);
            }
            finally
            {
                if (conexion.State == ConnectionState.Open)
                {
                    conexion.Close();
                }
            }
        }

        private bool SubirImagenAlServidor(string sourceFile, string fileName)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    string uploadUrl = BASE_URL + "api/upload_photo.php";
                    
                    // Crear los datos del formulario
                    string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
                    client.Headers.Add("Content-Type", "multipart/form-data; boundary=" + boundary);

                    using (var memStream = new MemoryStream())
                    {
                        var boundarybytes = System.Text.Encoding.ASCII.GetBytes("--" + boundary + "\r\n");
                        var endBoundaryBytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");

                        // Obtener el ID del usuario
                        int userId = -1;
                        if (conexion.State == ConnectionState.Closed)
                        {
                            conexion.Open();
                        }
                        try
                        {
                            string consultaId = "SELECT ID FROM Usuario WHERE User = @usuario";
                            using (MySqlCommand cmd = new MySqlCommand(consultaId, conexion))
                            {
                                cmd.Parameters.AddWithValue("@usuario", usuarioAutenticado);
                                object resultado = cmd.ExecuteScalar();
                                if (resultado != null)
                                {
                                    userId = Convert.ToInt32(resultado);
                                }
                            }
                        }
                        finally
                        {
                            if (conexion.State == ConnectionState.Open)
                            {
                                conexion.Close();
                            }
                        }

                        // Escribir el userId
                        string formItem = $"Content-Disposition: form-data; name=\"userId\"\r\n\r\n{userId}";
                        memStream.Write(boundarybytes, 0, boundarybytes.Length);
                        var formItemBytes = System.Text.Encoding.UTF8.GetBytes(formItem + "\r\n");
                        memStream.Write(formItemBytes, 0, formItemBytes.Length);

                        // Escribir el tipo
                        formItem = "Content-Disposition: form-data; name=\"tipo\"\r\n\r\nprincipal";
                        memStream.Write(boundarybytes, 0, boundarybytes.Length);
                        formItemBytes = System.Text.Encoding.UTF8.GetBytes(formItem + "\r\n");
                        memStream.Write(formItemBytes, 0, formItemBytes.Length);

                        // Escribir el archivo
                        string fileHeader = $"Content-Disposition: form-data; name=\"foto\"; filename=\"{fileName}\"\r\nContent-Type: application/octet-stream\r\n\r\n";
                        memStream.Write(boundarybytes, 0, boundarybytes.Length);
                        var fileHeaderBytes = System.Text.Encoding.UTF8.GetBytes(fileHeader);
                        memStream.Write(fileHeaderBytes, 0, fileHeaderBytes.Length);

                        var fileBytes = File.ReadAllBytes(sourceFile);
                        memStream.Write(fileBytes, 0, fileBytes.Length);

                        memStream.Write(endBoundaryBytes, 0, endBoundaryBytes.Length);

                        client.Headers.Add("Content-Length", memStream.Length.ToString());
                        byte[] result = client.UploadData(uploadUrl, memStream.ToArray());
                        string response = System.Text.Encoding.UTF8.GetString(result);

                        // Verificar la respuesta
                        var jsonResponse = System.Text.Json.JsonDocument.Parse(response);
                        if (jsonResponse.RootElement.GetProperty("success").GetBoolean())
                        {
                            // Obtener la URL de la imagen del servidor
                            string imageUrl = jsonResponse.RootElement.GetProperty("url").GetString();
                            rutaFoto = imageUrl; // Actualizar la ruta con la devuelta por el servidor
                            return true;
                        }
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                LogError("Error al subir la imagen al servidor", ex);
                MessageBox.Show($"Error al subir la imagen: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void btnCargarFoto_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "Archivos de imagen|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
                    openFileDialog.Title = "Seleccionar foto de perfil";

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string selectedFile = openFileDialog.FileName;
                        
                        // Validar el tamaño del archivo (máximo 5MB)
                        long maxFileSize = 5 * 1024 * 1024; // 5MB en bytes
                        FileInfo fileInfo = new FileInfo(selectedFile);
                        
                        if (fileInfo.Length > maxFileSize)
                        {
                            MessageBox.Show("La imagen seleccionada es demasiado grande. El tamaño máximo permitido es 5MB.",
                                          "Archivo demasiado grande",
                                          MessageBoxButtons.OK,
                                          MessageBoxIcon.Warning);
                            return;
                        }

                        Image originalImage = null;
                        try
                        {
                            originalImage = Image.FromFile(selectedFile);
                            
                            // Validar dimensiones mínimas
                            if (originalImage.Width < 100 || originalImage.Height < 100)
                            {
                                MessageBox.Show("La imagen es demasiado pequeña. Las dimensiones mínimas son 100x100 píxeles.",
                                              "Imagen demasiado pequeña",
                                              MessageBoxButtons.OK,
                                              MessageBoxIcon.Warning);
                                return;
                            }

                            // Generar nombre único para la foto
                            string fileName = $"{DateTime.Now:yyyyMMddHHmmss}{Path.GetExtension(selectedFile)}";
                            string tempPath = Path.Combine(Path.GetTempPath(), fileName);

                            // Limpiar imagen anterior del PictureBox
                            if (pictureBoxFoto.Image != null)
                            {
                                var oldImage = pictureBoxFoto.Image;
                                pictureBoxFoto.Image = null;
                                oldImage.Dispose();
                            }

                            // Redimensionar la imagen y mostrar previsualización
                            using (var resizedImage = RedimensionarImagen(originalImage, 800, 800))
                            {
                                // Crear una copia de la imagen redimensionada para el PictureBox
                                pictureBoxFoto.Image = new Bitmap(resizedImage);
                                
                                // Guardar temporalmente
                                resizedImage.Save(tempPath, originalImage.RawFormat);
                            }

                            // Subir al servidor
                            if (SubirImagenAlServidor(tempPath, fileName))
                            {
                                ActualizarFotoEnBaseDeDatos(rutaFoto);
                            }
                            else
                            {
                                // Si falla la subida, mostrar error y restaurar estado
                                MessageBox.Show("Error al subir la imagen al servidor", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                if (pictureBoxFoto.Image != null)
                                {
                                    var oldImage = pictureBoxFoto.Image;
                                    pictureBoxFoto.Image = null;
                                    oldImage.Dispose();
                                }
                            }

                            // Eliminar archivo temporal
                            if (File.Exists(tempPath))
                            {
                                File.Delete(tempPath);
                            }
                        }
                        finally
                        {
                            // Asegurar que la imagen original se libere
                            if (originalImage != null)
                            {
                                originalImage.Dispose();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogError("Error al cargar la imagen", ex);
                MessageBox.Show($"Error al cargar la imagen: {ex.Message}",
                              "Error",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
            }
        }

        private void ActualizarFotoEnBaseDeDatos(string rutaFoto)
        {
            try
            {
                if (conexion.State == ConnectionState.Closed)
                {
                    conexion.Open();
                }

                string consulta = "UPDATE Usuario SET Foto = @foto WHERE User = @usuario";
                using (MySqlCommand cmd = new MySqlCommand(consulta, conexion))
                {
                    cmd.Parameters.AddWithValue("@foto", rutaFoto);
                    cmd.Parameters.AddWithValue("@usuario", usuarioAutenticado);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar la foto en la base de datos: {ex.Message}",
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

        private Image RedimensionarImagen(Image imgOriginal, int anchoMaximo, int altoMaximo)
        {
            try
            {
                // Obtener las proporciones de la imagen original
                double ratioX = (double)anchoMaximo / imgOriginal.Width;
                double ratioY = (double)altoMaximo / imgOriginal.Height;
                double ratio = Math.Min(ratioX, ratioY);

                // Calcular las nuevas dimensiones manteniendo la proporción
                int nuevoAncho = (int)(imgOriginal.Width * ratio);
                int nuevoAlto = (int)(imgOriginal.Height * ratio);

                // Crear nueva imagen con las dimensiones calculadas
                var nuevaImagen = new Bitmap(nuevoAncho, nuevoAlto);

                try
                {
                    // Copiar y redimensionar la imagen original a la nueva
                    using (Graphics g = Graphics.FromImage(nuevaImagen))
                    {
                        // Configurar la calidad del redimensionamiento
                        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                        g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                        g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                        g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;

                        // Limpiar el fondo
                        g.Clear(Color.White);

                        // Dibujar la imagen redimensionada
                        var destRect = new Rectangle(0, 0, nuevoAncho, nuevoAlto);
                        var srcRect = new Rectangle(0, 0, imgOriginal.Width, imgOriginal.Height);
                        g.DrawImage(imgOriginal, destRect, srcRect, GraphicsUnit.Pixel);
                    }

                    return nuevaImagen;
                }
                catch
                {
                    nuevaImagen.Dispose();
                    throw;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al redimensionar la imagen: {ex.Message}", 
                               "Error", 
                               MessageBoxButtons.OK, 
                               MessageBoxIcon.Error);
                return null;
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            
            // Liberar recursos de la imagen
            if (pictureBoxFoto.Image != null)
            {
                pictureBoxFoto.Image.Dispose();
                pictureBoxFoto.Image = null;
            }
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtPassword.Text))
                {
                    MessageBox.Show("La contraseña no puede estar vacía.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (conexion.State == ConnectionState.Closed)
                {
                    conexion.Open();
                }

                string consulta = "UPDATE Usuario SET Password=@password, Nombre=@nombre, Apellido=@apellido, Genero=@genero, Ubicacion=@ubicacion, Foto=@foto WHERE User=@usuario";
                MySqlCommand cmd = new MySqlCommand(consulta, conexion);
                cmd.Parameters.AddWithValue("@usuario", usuarioAutenticado);
                cmd.Parameters.AddWithValue("@password", txtPassword.Text);
                cmd.Parameters.AddWithValue("@nombre", txtNombre.Text);
                cmd.Parameters.AddWithValue("@apellido", txtApellido.Text);
                cmd.Parameters.AddWithValue("@genero", cmbGenero.SelectedItem.ToString());
                cmd.Parameters.AddWithValue("@ubicacion", txtUbicacion.Text);
                cmd.Parameters.AddWithValue("@foto", rutaFoto);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Datos actualizados correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                CargarDatosUsuario();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar datos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conexion.State == ConnectionState.Open)
                {
                    conexion.Close();
                }
            }
        }

        private void abrirMatchesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int idUsuario = ObtenerIdUsuario(usuarioAutenticado);
            if (idUsuario != -1)
            {
                MatchesForm matchesForm = new MatchesForm(usuarioAutenticado, idUsuario);
                matchesForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Error al obtener el ID del usuario.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private int ObtenerIdUsuario(string usuario)
        {
            int idUsuario = -1;
            try
            {
                if (conexion.State == ConnectionState.Closed)
                {
                    conexion.Open();
                }

                string consulta = "SELECT ID FROM Usuario WHERE User = @usuario";
                MySqlCommand cmd = new MySqlCommand(consulta, conexion);
                cmd.Parameters.AddWithValue("@usuario", usuario);
                object resultado = cmd.ExecuteScalar();

                if (resultado != null)
                {
                    idUsuario = Convert.ToInt32(resultado);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener el ID del usuario: " + ex.Message);
            }
            finally
            {
                if (conexion.State == ConnectionState.Open)
                {
                    conexion.Close();
                }
            }
            return idUsuario;
        }

        private void cerrarSesiónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Limpiar recursos de imagen antes de cerrar
            if (pictureBoxFoto.Image != null)
            {
                pictureBoxFoto.Image.Dispose();
                pictureBoxFoto.Image = null;
            }
            
            this.Hide();  // Oculta el formulario actual antes de abrir el LogIn
            LogIn loginForm = new LogIn();
            loginForm.ShowDialog();
            this.Close(); //
        }

        private void abrirMensajesRecibidosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReceivedMessagesForm receivedMessagesForm = new ReceivedMessagesForm(usuarioAutenticado, ObtenerIdUsuario(usuarioAutenticado));
            receivedMessagesForm.ShowDialog();
        }

        private void irAlPerfilItem_Click(object sender, EventArgs e)
        {
            try
            {
                int idUsuario = ObtenerIdUsuario(usuarioAutenticado);
                if (idUsuario != -1)
                {
                    // Abrir la página web del perfil en el navegador predeterminado
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = $"http://localhost/page/profile.html?userId={idUsuario}",
                        UseShellExecute = true
                    });
                }
                else
                {
                    MessageBox.Show("Error al obtener el ID del usuario.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al abrir la página del perfil: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void abrirMatchesPendientesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int idUsuario = ObtenerIdUsuario(usuarioAutenticado);
            if (idUsuario != -1)
            {
                PendingMatchesForm pendingMatchesForm = new PendingMatchesForm(usuarioAutenticado, idUsuario);
                pendingMatchesForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Error al obtener el ID del usuario.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void verUsuariosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int idUsuario = ObtenerIdUsuario(usuarioAutenticado);
            if (idUsuario != -1)
            {
                UsersForm usersForm = new UsersForm(usuarioAutenticado, idUsuario);
                usersForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Error al obtener el ID del usuario.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminarUsuario_Click(object sender, EventArgs e)
        {
            DialogResult confirmacion = MessageBox.Show(
                "¿Estás seguro de que quieres eliminar tu cuenta? Esta acción no se puede deshacer.",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (confirmacion == DialogResult.Yes)
            {
                EliminarCuentaUsuario();
            }
        }

        private void EliminarCuentaUsuario()
        {
            try
            {
                if (conexion.State == ConnectionState.Closed)
                {
                    conexion.Open();
                }

                // Obtener el ID del usuario
                int idUsuario = ObtenerIdUsuarioSinCerrarConexion();
                if (idUsuario == -1)
                {
                    throw new Exception("No se pudo obtener el ID del usuario");
                }

                // Iniciar transacción
                MySqlTransaction transaction = conexion.BeginTransaction();

                try
                {
                    // Eliminar registros relacionados en otras tablas
                    string[] deleteQueries = {
                        "DELETE FROM Matches WHERE UsuarioID1 = @userId OR UsuarioID2 = @userId",
                        "DELETE FROM Mensajes WHERE EmisorID = @userId OR ReceptorID = @userId",
                        "DELETE FROM MatchesPendientes WHERE UsuarioSolicitanteID = @userId OR UsuarioSolicitadoID = @userId",
                        "DELETE FROM Usuario WHERE ID = @userId"
                    };

                    foreach (string query in deleteQueries)
                    {
                        using (MySqlCommand cmd = new MySqlCommand(query, conexion, transaction))
                        {
                            cmd.Parameters.AddWithValue("@userId", idUsuario);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    // Confirmar transacción
                    transaction.Commit();

                    MessageBox.Show("Cuenta eliminada correctamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Cerrar el formulario actual y abrir el formulario de inicio de sesión
                    this.Hide();
                    LogIn loginForm = new LogIn();
                    loginForm.ShowDialog();
                    this.Close();
                }
                catch (Exception ex)
                {
                    // Revertir transacción en caso de error
                    transaction.Rollback();
                    throw new Exception("Error al eliminar la cuenta: " + ex.Message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conexion.State == ConnectionState.Open)
                {
                    conexion.Close();
                }
            }
        }

        private int ObtenerIdUsuarioSinCerrarConexion()
        {
            try
            {
                string consulta = "SELECT ID FROM Usuario WHERE User = @usuario";
                using (MySqlCommand cmd = new MySqlCommand(consulta, conexion))
                {
                    cmd.Parameters.AddWithValue("@usuario", usuarioAutenticado);
                    object resultado = cmd.ExecuteScalar();
                    return resultado != null ? Convert.ToInt32(resultado) : -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener el ID del usuario: " + ex.Message);
                return -1;
            }
        }

        private void verEstadisticasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                int idUsuario = ObtenerIdUsuario(usuarioAutenticado);
                if (idUsuario != -1)
                {
                    // Abrir la página web de estadísticas en el navegador predeterminado
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = $"http://localhost/page/stats.html?userId={idUsuario}",
                        UseShellExecute = true
                    });
                }
                else
                {
                    MessageBox.Show("Error al obtener el ID del usuario.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al abrir la página de estadísticas: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LogError(string message, Exception ex)
        {
            string logMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message} - {ex.Message}";
            Debug.WriteLine(logMessage);
            
            try
            {
                string logPath = Path.Combine(Application.StartupPath, "app_log.txt");
                File.AppendAllText(logPath, logMessage + Environment.NewLine);
            }
            catch
            {
                // Si falla el logging a archivo, al menos tenemos el Debug.WriteLine
            }
        }

        private Image CargarImagenDesdeUrl(string relativePath)
        {
            if (string.IsNullOrEmpty(relativePath)) return null;

            try
            {
                string fullUrl = BASE_URL + relativePath;
                using (WebClient webClient = new WebClient())
                {
                    byte[] data = webClient.DownloadData(fullUrl);
                    using (MemoryStream mem = new MemoryStream(data))
                    {
                        return Image.FromStream(mem);
                    }
                }
            }
            catch (WebException webEx)
            {
                LogError($"Error de red al cargar la imagen desde {relativePath}", webEx);
                return null;
            }
            catch (Exception ex)
            {
                LogError($"Error al cargar la imagen desde {relativePath}", ex);
                return null;
            }
        }
    }
}