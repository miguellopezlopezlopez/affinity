using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace FiltringApp
{
    public partial class RegistroForm : Form
    {
        private MySqlConnection conexion;
        private string cadenaConexion;
        private string rutaFoto = string.Empty;

        public RegistroForm()
        {
            InitializeComponent();
            CargarConfiguracion();
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

        private void btnCargarFoto_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                Filter = "Archivos de imagen|*.jpg;*.jpeg;*.png;*.bmp"
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                rutaFoto = openFileDialog1.FileName;

                // Cargar la imagen y redimensionarla
                Image imgOriginal = Image.FromFile(rutaFoto);
                Image imgRedimensionada = RedimensionarImagen(imgOriginal, pictureBoxFoto.Width, pictureBoxFoto.Height);

                // Asignar la imagen redimensionada al PictureBox
                pictureBoxFoto.Image = imgRedimensionada;
                pictureBoxFoto.SizeMode = PictureBoxSizeMode.StretchImage;
            }
        }

        private Image RedimensionarImagen(Image img, int ancho, int alto)
        {
            Bitmap nuevaImagen = new Bitmap(ancho, alto);
            using (Graphics g = Graphics.FromImage(nuevaImagen))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(img, 0, 0, ancho, alto);
            }
            return nuevaImagen;
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            string usuario = txtUser.Text;
            string contraseña = txtPwd.Text;
            string nombre = txtNombre.Text;
            string apellido = txtApellido.Text;
            string genero = cmbGenero.SelectedItem.ToString();
            string ubicacion = txtUbicacion.Text;

            if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(contraseña) || string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(apellido) || string.IsNullOrEmpty(genero) || string.IsNullOrEmpty(ubicacion))
            {
                MessageBox.Show("Por favor, complete todos los campos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (usuario.Length < 3 || contraseña.Length < 6)
            {
                MessageBox.Show("El usuario debe tener al menos 3 caracteres y la contraseña al menos 6 caracteres.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                conexion.Open();
                string consulta = "INSERT INTO Usuario (User, Password, Nombre, Apellido, Genero, Ubicacion, Foto) VALUES (@user, @password, @nombre, @apellido, @genero, @ubicacion, @foto)";
                MySqlCommand cmd = new MySqlCommand(consulta, conexion);
                cmd.Parameters.AddWithValue("@user", usuario);
                cmd.Parameters.AddWithValue("@password", contraseña);
                cmd.Parameters.AddWithValue("@nombre", nombre);
                cmd.Parameters.AddWithValue("@apellido", apellido);
                cmd.Parameters.AddWithValue("@genero", genero);
                cmd.Parameters.AddWithValue("@ubicacion", ubicacion);
                cmd.Parameters.AddWithValue("@foto", rutaFoto);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Registro completado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al registrar usuario: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conexion.Close();
            }
        }
    }
}
