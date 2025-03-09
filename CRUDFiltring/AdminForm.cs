using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using System.Collections.Generic;
using System.Drawing;
using FiltringApp;

namespace FiltringApp

{
    public partial class AdminForm : Form
    {
        private MySqlConnection conexion;
        private string cadenaConexion;
        private string rutaFoto = string.Empty;
        private string usuarioAutenticado; // Usuario que ha iniciado sesión

        public AdminForm(string usuario)
        {
            InitializeComponent();
            usuarioAutenticado = usuario; // Guardamos el usuario autenticado
            CargarConfiguracion();
            CargarUsuarios();
        }

        public AdminForm()
        {
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

        private void CargarUsuarios()
        {
            try
            {
                if (conexion.State == ConnectionState.Closed)
                {
                    conexion.Open();
                }

                string consulta = "SELECT ID, User, Password, Nombre, Apellido, Genero, Ubicacion, Foto FROM Usuario";
                MySqlDataAdapter adaptador = new MySqlDataAdapter(consulta, conexion);
                DataTable dt = new DataTable();
                adaptador.Fill(dt);
                dataGridViewUsuarios.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar usuarios: " + ex.Message);
            }
            finally
            {
                if (conexion.State == ConnectionState.Open)
                {
                    conexion.Close();
                }
            }
        }

        private void dataGridViewUsuarios_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridViewUsuarios.Rows[e.RowIndex];

                txtUser.Text = row.Cells["User"].Value.ToString();
                txtPassword.Text = row.Cells["Password"].Value.ToString();
                txtNombre.Text = row.Cells["Nombre"].Value.ToString();
                txtApellido.Text = row.Cells["Apellido"].Value.ToString();
                cmbGenero.SelectedItem = row.Cells["Genero"].Value.ToString();
                txtUbicacion.Text = row.Cells["Ubicacion"].Value.ToString();

                rutaFoto = row.Cells["Foto"].Value.ToString();
                if (File.Exists(rutaFoto))
                {
                    pictureBoxFoto.Image = Image.FromFile(rutaFoto);
                }
                else
                {
                    pictureBoxFoto.Image = null;
                }
            }
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            if (dataGridViewUsuarios.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un usuario para actualizar.");
                return;
            }

            int id = Convert.ToInt32(dataGridViewUsuarios.SelectedRows[0].Cells["ID"].Value);

            try
            {
                if (conexion.State == ConnectionState.Closed)
                {
                    conexion.Open();
                }

                string consulta = "UPDATE Usuario SET User=@user, Password=@password, Nombre=@nombre, Apellido=@apellido, Genero=@genero, Ubicacion=@ubicacion, Foto=@foto WHERE ID=@id";
                MySqlCommand cmd = new MySqlCommand(consulta, conexion);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@user", txtUser.Text);
                cmd.Parameters.AddWithValue("@password", txtPassword.Text);
                cmd.Parameters.AddWithValue("@nombre", txtNombre.Text);
                cmd.Parameters.AddWithValue("@apellido", txtApellido.Text);
                cmd.Parameters.AddWithValue("@genero", cmbGenero.SelectedItem.ToString());
                cmd.Parameters.AddWithValue("@ubicacion", txtUbicacion.Text);
                cmd.Parameters.AddWithValue("@foto", rutaFoto);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Usuario actualizado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarUsuarios();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar usuario: " + ex.Message);
            }
            finally
            {
                if (conexion.State == ConnectionState.Open)
                {
                    conexion.Close();
                }
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
                Image imgOriginal = Image.FromFile(rutaFoto);
                Image imgRedimensionada = RedimensionarImagen(imgOriginal, pictureBoxFoto.Width, pictureBoxFoto.Height);

                pictureBoxFoto.Image = imgRedimensionada;
                pictureBoxFoto.SizeMode = PictureBoxSizeMode.StretchImage;
            }
        }

        private Image RedimensionarImagen(Image img, int ancho, int alto)
        {
            Bitmap nuevaImagen = new Bitmap(ancho, alto);
            using (Graphics g = Graphics.FromImage(nuevaImagen))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(img, 0, 0, ancho, alto);
            }
            return nuevaImagen;
        }

        private void btnEliminarUsuario_Click(object sender, EventArgs e)
        {
            if (dataGridViewUsuarios.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un usuario para eliminar.");
                return;
            }

            int id = Convert.ToInt32(dataGridViewUsuarios.SelectedRows[0].Cells["ID"].Value);

            DialogResult confirmacion = MessageBox.Show("¿Está seguro de eliminar este usuario?", "Confirmar Eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirmacion == DialogResult.Yes)
            {
                try
                {
                    if (conexion.State == ConnectionState.Closed)
                    {
                        conexion.Open();
                    }

                    string consulta = "DELETE FROM Usuario WHERE ID=@id";
                    MySqlCommand cmd = new MySqlCommand(consulta, conexion);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Usuario eliminado correctamente.");
                    CargarUsuarios();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al eliminar usuario: " + ex.Message);
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

        private void irATuPerfilToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProfileForm profileForm = new ProfileForm(usuarioAutenticado);
            profileForm.ShowDialog();
        }

        private void cerrarSesiónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
