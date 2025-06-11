using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.IO;

namespace FiltringApp
{
    public partial class MatchesForm : Form
    {
        private MySqlConnection conexion;
        private string cadenaConexion;
        private string usuarioAutenticado;
        private int idUsuarioAutenticado;
        private DataTable usuarios;
        private int indiceActual = 0;
        private FlowLayoutPanel galeriaFotos;
        private const string BASE_URL = "http://localhost/page/";
        private const int MAX_IMAGENES_GALERIA = 6;

        public MatchesForm(string usuario, int idUsuario)
        {
            InitializeComponent();
            usuarioAutenticado = usuario;
            idUsuarioAutenticado = idUsuario;
            ConfigurarGaleriaFotos();
            CargarConfiguracion();
            CargarUsuarios();
            AplicarEstilosModernos();
        }

        private void AplicarEstilosModernos()
        {
            // Estilos para los botones con efectos hover
            AplicarEfectoHoverBoton(btnAnterior, Color.FromArgb(220, 220, 220));
            AplicarEfectoHoverBoton(btnSiguiente, Color.FromArgb(220, 220, 220));
            AplicarEfectoHoverBoton(btnEnviarSolicitud, Color.FromArgb(240, 50, 100));
        }

        private void AplicarEfectoHoverBoton(Button btn, Color colorHover)
        {
            Color colorOriginal = btn.BackColor;

            btn.MouseEnter += (s, e) => {
                btn.BackColor = colorHover;
                btn.Cursor = Cursors.Hand;
            };

            btn.MouseLeave += (s, e) => {
                btn.BackColor = colorOriginal;
                btn.Cursor = Cursors.Default;
            };
        }

        private void ConfigurarGaleriaFotos()
        {
            galeriaFotos = new FlowLayoutPanel();
            galeriaFotos.Location = new System.Drawing.Point(400, 60);
            galeriaFotos.Size = new System.Drawing.Size(375, 545);
            galeriaFotos.AutoScroll = true;
            galeriaFotos.BackColor = System.Drawing.Color.White;
            galeriaFotos.BorderStyle = BorderStyle.FixedSingle;
            galeriaFotos.Padding = new Padding(8);
            galeriaFotos.WrapContents = true;
            galeriaFotos.FlowDirection = FlowDirection.LeftToRight;
            this.Controls.Add(galeriaFotos);

            // Agregar título a la galería
            Label lblGaleria = new Label();
            lblGaleria.Text = "Galería de Fotos";
            lblGaleria.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            lblGaleria.ForeColor = System.Drawing.Color.FromArgb(64, 64, 64);
            lblGaleria.Location = new System.Drawing.Point(400, 30);
            lblGaleria.Size = new System.Drawing.Size(200, 25);
            this.Controls.Add(lblGaleria);
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
            }
        }

        private void CargarUsuarios()
        {
            try
            {
                conexion.Open();
                string consulta = @"
                    SELECT DISTINCT u.ID, u.User, u.Nombre, u.Apellido, u.Ubicacion, u.Foto,
                    p.Biografia, p.Intereses, p.Preferencias, p.FotoPrincipal,
                    GROUP_CONCAT(fp.URL) as GaleriaFotos
                    FROM Usuario u
                    LEFT JOIN perfil p ON u.ID = p.ID_User
                    LEFT JOIN fotosperfil fp ON u.ID = fp.ID_User
                    WHERE u.ID != @idUsuario 
                    AND u.ID != 1
                    GROUP BY u.ID, u.User, u.Nombre, u.Apellido, u.Ubicacion, u.Foto, p.Biografia, p.Intereses, p.Preferencias, p.FotoPrincipal";

                MySqlCommand cmd = new MySqlCommand(consulta, conexion);
                cmd.Parameters.AddWithValue("@idUsuario", idUsuarioAutenticado);
                MySqlDataAdapter adaptador = new MySqlDataAdapter(cmd);
                usuarios = new DataTable();
                adaptador.Fill(usuarios);

                if (usuarios.Rows.Count > 0)
                {
                    MostrarUsuario(indiceActual);
                }
                else
                {
                    MessageBox.Show("No hay más usuarios disponibles.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar usuarios: " + ex.Message);
            }
            finally
            {
                conexion.Close();
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
                        Image originalImage = Image.FromStream(mem);
                        return originalImage;
                    }
                }
            }
            catch (Exception ex)
            {
                return null; // Fallar silenciosamente para mejor UX
            }
        }

        private Image RedimensionarImagen(Image imgOriginal, int anchoMaximo, int altoMaximo)
        {
            // Obtener las proporciones de la imagen original
            double ratioX = (double)anchoMaximo / imgOriginal.Width;
            double ratioY = (double)altoMaximo / imgOriginal.Height;
            double ratio = Math.Min(ratioX, ratioY);

            // Calcular las nuevas dimensiones manteniendo la proporción
            int nuevoAncho = (int)(imgOriginal.Width * ratio);
            int nuevoAlto = (int)(imgOriginal.Height * ratio);

            // Crear nueva imagen con las dimensiones calculadas
            Bitmap nuevaImagen = new Bitmap(nuevoAncho, nuevoAlto);
            using (Graphics g = Graphics.FromImage(nuevaImagen))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.DrawImage(imgOriginal, 0, 0, nuevoAncho, nuevoAlto);
            }

            return nuevaImagen;
        }

        private PictureBox CrearPictureBoxGaleria(Image imagen)
        {
            PictureBox pb = new PictureBox();

            // Calcular tamaño óptimo para mostrar máximo 6 imágenes (3x2)
            int tamañoImagen = 110; // Reducido para que quepan 6 imágenes cómodamente
            pb.Size = new Size(tamañoImagen, tamañoImagen);
            pb.SizeMode = PictureBoxSizeMode.Zoom;
            pb.BorderStyle = BorderStyle.FixedSingle;
            pb.Margin = new Padding(5);
            pb.BackColor = System.Drawing.Color.White;
            pb.Cursor = Cursors.Hand;

            // Redimensionar imagen para optimizar memoria
            Image imagenRedimensionada = RedimensionarImagen(imagen, tamañoImagen * 2, tamañoImagen * 2);
            pb.Image = imagenRedimensionada;

            // Agregar efectos hover mejorados
            pb.MouseEnter += (s, e) => {
                pb.BorderStyle = BorderStyle.Fixed3D;
                pb.BackColor = System.Drawing.Color.FromArgb(245, 245, 245);
                pb.Size = new Size(tamañoImagen + 5, tamañoImagen + 5); // Efecto de crecimiento sutil
            };

            pb.MouseLeave += (s, e) => {
                pb.BorderStyle = BorderStyle.FixedSingle;
                pb.BackColor = System.Drawing.Color.White;
                pb.Size = new Size(tamañoImagen, tamañoImagen);
            };

            // Agregar click para ver imagen en tamaño completo
            pb.Click += (s, e) => MostrarImagenCompleta(imagen);

            return pb;
        }

        private void MostrarImagenCompleta(Image imagen)
        {
            Form visorImagen = new Form();
            PictureBox pb = new PictureBox();
            Label lblCerrar = new Label();

            visorImagen.WindowState = FormWindowState.Maximized;
            visorImagen.FormBorderStyle = FormBorderStyle.None;
            visorImagen.BackColor = Color.Black;
            visorImagen.KeyPreview = true;

            pb.Dock = DockStyle.Fill;
            pb.SizeMode = PictureBoxSizeMode.Zoom;
            pb.Image = imagen;
            pb.Cursor = Cursors.Hand;

            // Etiqueta para instrucciones
            lblCerrar.Text = "Presiona ESC o haz clic para cerrar";
            lblCerrar.ForeColor = Color.White;
            lblCerrar.BackColor = Color.FromArgb(100, 0, 0, 0);
            lblCerrar.Font = new Font("Segoe UI", 12F);
            lblCerrar.Size = new Size(300, 30);
            lblCerrar.TextAlign = ContentAlignment.MiddleCenter;
            lblCerrar.Location = new Point(50, 50);

            // Cerrar con click o ESC
            pb.Click += (s, e) => visorImagen.Close();
            visorImagen.KeyDown += (s, e) => { if (e.KeyCode == Keys.Escape) visorImagen.Close(); };

            visorImagen.Controls.Add(pb);
            visorImagen.Controls.Add(lblCerrar);
            lblCerrar.BringToFront();

            visorImagen.ShowDialog();
        }

        private void MostrarUsuario(int indice)
        {
            if (usuarios.Rows.Count > 0 && indice >= 0 && indice < usuarios.Rows.Count)
            {
                DataRow row = usuarios.Rows[indice];
                string nombreCompleto = row["Nombre"].ToString() + " " + row["Apellido"].ToString();
                lblNombre.Text = nombreCompleto;
                lblUbicacion.Text = "📍 " + row["Ubicacion"].ToString();

                string biografia = row["Biografia"].ToString();
                string intereses = row["Intereses"].ToString();
                string preferencias = row["Preferencias"].ToString();

                // Mejorar formato del texto de información
                string textoInfo = "";
                if (!string.IsNullOrEmpty(biografia))
                    textoInfo += $"📝 Biografía:\n{biografia}\n\n";
                if (!string.IsNullOrEmpty(intereses))
                    textoInfo += $"❤️ Intereses:\n{intereses}\n\n";
                if (!string.IsNullOrEmpty(preferencias))
                    textoInfo += $"⭐ Preferencias:\n{preferencias}";

                lblInfoUsuario.Text = textoInfo;

                // Cargar foto principal
                string fotoPrincipal = row["FotoPrincipal"].ToString();
                string fotoUsuario = row["Foto"].ToString();

                Image imagen = CargarImagenDesdeUrl(fotoPrincipal);
                if (imagen == null)
                {
                    imagen = CargarImagenDesdeUrl(fotoUsuario);
                }

                pictureBoxFoto.Image = imagen;

                // Limpiar galería anterior
                foreach (Control control in galeriaFotos.Controls)
                {
                    if (control is PictureBox pb && pb.Image != null)
                    {
                        pb.Image.Dispose(); // Liberar memoria
                    }
                }
                galeriaFotos.Controls.Clear();

                // Mostrar fotos de galería (máximo 6)
                string galeriaFotosStr = row["GaleriaFotos"].ToString();
                if (!string.IsNullOrEmpty(galeriaFotosStr))
                {
                    string[] fotosUrls = galeriaFotosStr.Split(',');
                    int contador = 0;

                    foreach (string url in fotosUrls)
                    {
                        if (!string.IsNullOrEmpty(url) && contador < MAX_IMAGENES_GALERIA)
                        {
                            Image img = CargarImagenDesdeUrl(url.Trim());
                            if (img != null)
                            {
                                PictureBox pbGaleria = CrearPictureBoxGaleria(img);
                                galeriaFotos.Controls.Add(pbGaleria);
                                contador++;
                            }
                        }
                    }
                }

                // Actualizar estado de botones
                btnAnterior.Enabled = indiceActual > 0;
                btnSiguiente.Enabled = indiceActual < usuarios.Rows.Count - 1;

                // Actualizar título con contador
                this.Text = $"Explorar Matches - {indiceActual + 1} de {usuarios.Rows.Count}";
            }
        }

        private void btnAnterior_Click(object sender, EventArgs e)
        {
            if (indiceActual > 0)
            {
                indiceActual--;
                MostrarUsuario(indiceActual);
            }
        }

        private void btnSiguiente_Click(object sender, EventArgs e)
        {
            if (indiceActual < usuarios.Rows.Count - 1)
            {
                indiceActual++;
                MostrarUsuario(indiceActual);
            }
        }

        private void btnEnviarSolicitud_Click(object sender, EventArgs e)
        {
            if (usuarios.Rows.Count > 0 && indiceActual >= 0 && indiceActual < usuarios.Rows.Count)
            {
                int idUsuarioSeleccionado = Convert.ToInt32(usuarios.Rows[indiceActual]["ID"]);
                string nombreUsuario = usuarios.Rows[indiceActual]["Nombre"].ToString();

                // Confirmación con diseño mejorado
                DialogResult resultado = MessageBox.Show(
                    $"¿Estás seguro de que quieres enviar un match a {nombreUsuario}?",
                    "Confirmar Match",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (resultado == DialogResult.No) return;

                try
                {
                    conexion.Open();

                    // Verificar si ya hay una solicitud enviada a este usuario
                    string verificarConsulta = "SELECT COUNT(*) FROM Matches WHERE ID_Sol = @idSolicitante AND ID_Acept = @idReceptor";
                    MySqlCommand verificarCmd = new MySqlCommand(verificarConsulta, conexion);
                    verificarCmd.Parameters.AddWithValue("@idSolicitante", idUsuarioAutenticado);
                    verificarCmd.Parameters.AddWithValue("@idReceptor", idUsuarioSeleccionado);
                    int coincidencias = Convert.ToInt32(verificarCmd.ExecuteScalar());

                    if (coincidencias > 0)
                    {
                        MessageBox.Show($"Ya has enviado una solicitud de match a {nombreUsuario}.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Insertar la solicitud en la tabla Matches
                    string consulta = "INSERT INTO Matches (ID_Sol, ID_Acept, Fecha_Solicitud) VALUES (@idSolicitante, @idReceptor, NOW())";
                    MySqlCommand cmd = new MySqlCommand(consulta, conexion);
                    cmd.Parameters.AddWithValue("@idSolicitante", idUsuarioAutenticado);
                    cmd.Parameters.AddWithValue("@idReceptor", idUsuarioSeleccionado);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show($"¡Solicitud de match enviada a {nombreUsuario}! 💖", "¡Éxito!", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Opcional: Avanzar al siguiente usuario automáticamente
                    if (indiceActual < usuarios.Rows.Count - 1)
                    {
                        indiceActual++;
                        MostrarUsuario(indiceActual);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al enviar solicitud de match: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    conexion.Close();
                }
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            // Liberar recursos de imágenes al cerrar el formulario
            if (pictureBoxFoto.Image != null)
            {
                pictureBoxFoto.Image.Dispose();
            }

            foreach (Control control in galeriaFotos.Controls)
            {
                if (control is PictureBox pb && pb.Image != null)
                {
                    pb.Image.Dispose();
                }
            }

            base.OnFormClosed(e);
        }

        // Método adicional para mejorar la experiencia del usuario
        private void ActualizarContadorUsuarios()
        {
            if (usuarios != null && usuarios.Rows.Count > 0)
            {
                lblTitulo.Text = $"Explora Usuarios ({indiceActual + 1}/{usuarios.Rows.Count})";
            }
        }

        // Soporte para navegación con teclado
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Left:
                    if (btnAnterior.Enabled)
                        btnAnterior_Click(null, null);
                    return true;
                case Keys.Right:
                    if (btnSiguiente.Enabled)
                        btnSiguiente_Click(null, null);
                    return true;
                case Keys.Space:
                case Keys.Enter:
                    btnEnviarSolicitud_Click(null, null);
                    return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}