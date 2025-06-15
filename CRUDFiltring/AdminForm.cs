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
    public partial class AdminForm : Form
    {
        private MySqlConnection conexion;
        private string cadenaConexion;
        private string usuarioAutenticado;
        private int idUsuarioAutenticado;

        public AdminForm(string usuario, int idUsuario)
        {
            InitializeComponent();
            usuarioAutenticado = usuario;
            idUsuarioAutenticado = idUsuario;
            
            // Configurar el formulario
            this.Text = "Panel de Administración";
            this.Size = new Size(1200, 700);
            this.MinimumSize = new Size(800, 500);
            this.StartPosition = FormStartPosition.CenterScreen;

            CargarConfiguracion();
            ConfigurarControles();
            CargarUsuarios();
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
                Dictionary<string, string> config = deserializer.Deserialize<Dictionary<string, string>>(yamlContent);

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

        private void ConfigurarControles()
        {
            // Panel izquierdo para la lista de usuarios
            Panel panelIzquierdo = new Panel
            {
                Dock = DockStyle.Left,
                Width = 800
            };

            // DataGridView para mostrar usuarios
            dataGridViewUsuarios = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            dataGridViewUsuarios.SelectionChanged += DataGridViewUsuarios_SelectionChanged;
            panelIzquierdo.Controls.Add(dataGridViewUsuarios);

            // Panel derecho para edición
            Panel panelDerecho = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(10)
            };

            // Campos de edición
            TableLayoutPanel layoutEdicion = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                ColumnCount = 2,
                RowCount = 6,
                AutoSize = true,
                Padding = new Padding(10),
                CellBorderStyle = TableLayoutPanelCellBorderStyle.Single
            };

            // Labels y TextBoxes
            Label lblUsername = new Label { Text = "Usuario:", Dock = DockStyle.Fill };
            txtUsername = new TextBox { Dock = DockStyle.Fill };

            Label lblNombre = new Label { Text = "Nombre:", Dock = DockStyle.Fill };
            txtNombre = new TextBox { Dock = DockStyle.Fill };

            Label lblApellido = new Label { Text = "Apellido:", Dock = DockStyle.Fill };
            txtApellido = new TextBox { Dock = DockStyle.Fill };

            Label lblUbicacion = new Label { Text = "Ubicación:", Dock = DockStyle.Fill };
            txtUbicacion = new TextBox { Dock = DockStyle.Fill };

            Label lblGenero = new Label { Text = "Género:", Dock = DockStyle.Fill };
            txtGenero = new TextBox { Dock = DockStyle.Fill };

            // Agregar controles al layout
            layoutEdicion.Controls.Add(lblUsername, 0, 0);
            layoutEdicion.Controls.Add(txtUsername, 1, 0);
            layoutEdicion.Controls.Add(lblNombre, 0, 1);
            layoutEdicion.Controls.Add(txtNombre, 1, 1);
            layoutEdicion.Controls.Add(lblApellido, 0, 2);
            layoutEdicion.Controls.Add(txtApellido, 1, 2);
            layoutEdicion.Controls.Add(lblUbicacion, 0, 3);
            layoutEdicion.Controls.Add(txtUbicacion, 1, 3);
            layoutEdicion.Controls.Add(lblGenero, 0, 4);
            layoutEdicion.Controls.Add(txtGenero, 1, 4);

            // Botones
            FlowLayoutPanel panelBotones = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                Padding = new Padding(10),
                FlowDirection = FlowDirection.LeftToRight
            };

            btnActualizar = new Button
            {
                Text = "Actualizar",
                AutoSize = true,
                Padding = new Padding(10, 5, 10, 5)
            };
            btnActualizar.Click += BtnActualizar_Click;

            btnEliminar = new Button
            {
                Text = "Eliminar",
                AutoSize = true,
                Padding = new Padding(10, 5, 10, 5)
            };
            btnEliminar.Click += BtnEliminar_Click;

            btnRefrescar = new Button
            {
                Text = "Refrescar",
                AutoSize = true,
                Padding = new Padding(10, 5, 10, 5)
            };
            btnRefrescar.Click += BtnRefrescar_Click;

            panelBotones.Controls.AddRange(new Control[] { btnActualizar, btnEliminar, btnRefrescar });

            // MenuStrip
            MenuStrip menuStrip = new MenuStrip();
            ToolStripMenuItem menuArchivo = new ToolStripMenuItem("Archivo");
            ToolStripMenuItem menuCerrarSesion = new ToolStripMenuItem("Cerrar Sesión");
            menuCerrarSesion.Click += MenuCerrarSesion_Click;
            menuArchivo.DropDownItems.Add(menuCerrarSesion);
            menuStrip.Items.Add(menuArchivo);

            // Agregar todo al panel derecho
            panelDerecho.Controls.Add(layoutEdicion);
            panelDerecho.Controls.Add(panelBotones);

            // Agregar los paneles al formulario
            this.Controls.Add(menuStrip);
            this.Controls.Add(panelDerecho);
            this.Controls.Add(panelIzquierdo);
            this.MainMenuStrip = menuStrip;
        }

        private void CargarUsuarios()
        {
            try
            {
                if (conexion.State == ConnectionState.Closed)
                {
                    conexion.Open();
                }

                string consulta = @"
                    SELECT ID, User, Nombre, Apellido, Genero, Ubicacion 
                    FROM Usuario 
                    ORDER BY ID";

                MySqlCommand cmd = new MySqlCommand(consulta, conexion);
                MySqlDataAdapter adaptador = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adaptador.Fill(dt);

                dataGridViewUsuarios.DataSource = dt;

                // Configurar columnas
                if (dataGridViewUsuarios.Columns.Contains("ID"))
                {
                    dataGridViewUsuarios.Columns["ID"].Visible = false;
                }
                
                // Renombrar columnas
                dataGridViewUsuarios.Columns["User"].HeaderText = "Usuario";
                dataGridViewUsuarios.Columns["Nombre"].HeaderText = "Nombre";
                dataGridViewUsuarios.Columns["Apellido"].HeaderText = "Apellido";
                dataGridViewUsuarios.Columns["Genero"].HeaderText = "Género";
                dataGridViewUsuarios.Columns["Ubicacion"].HeaderText = "Ubicación";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar usuarios: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conexion.State == ConnectionState.Open)
                {
                    conexion.Close();
                }
            }
        }

        private void DataGridViewUsuarios_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewUsuarios.CurrentRow != null)
            {
                txtUsername.Text = dataGridViewUsuarios.CurrentRow.Cells["User"].Value.ToString();
                txtNombre.Text = dataGridViewUsuarios.CurrentRow.Cells["Nombre"].Value.ToString();
                txtApellido.Text = dataGridViewUsuarios.CurrentRow.Cells["Apellido"].Value.ToString();
                txtUbicacion.Text = dataGridViewUsuarios.CurrentRow.Cells["Ubicacion"].Value.ToString();
                txtGenero.Text = dataGridViewUsuarios.CurrentRow.Cells["Genero"].Value.ToString();

                // Deshabilitar edición para el usuario admin
                bool esAdmin = dataGridViewUsuarios.CurrentRow.Cells["ID"].Value.ToString() == "1";
                btnEliminar.Enabled = !esAdmin;
                btnActualizar.Enabled = !esAdmin;
                txtUsername.Enabled = !esAdmin;
                txtNombre.Enabled = !esAdmin;
                txtApellido.Enabled = !esAdmin;
                txtUbicacion.Enabled = !esAdmin;
                txtGenero.Enabled = !esAdmin;
            }
        }

        private void BtnActualizar_Click(object sender, EventArgs e)
        {
            if (dataGridViewUsuarios.CurrentRow == null) return;

            try
            {
                if (conexion.State == ConnectionState.Closed)
                {
                    conexion.Open();
                }

                int idUsuario = Convert.ToInt32(dataGridViewUsuarios.CurrentRow.Cells["ID"].Value);

                string consulta = @"
                    UPDATE Usuario 
                    SET User = @username, 
                        Nombre = @nombre, 
                        Apellido = @apellido, 
                        Ubicacion = @ubicacion, 
                        Genero = @genero 
                    WHERE ID = @id";

                MySqlCommand cmd = new MySqlCommand(consulta, conexion);
                cmd.Parameters.AddWithValue("@username", txtUsername.Text);
                cmd.Parameters.AddWithValue("@nombre", txtNombre.Text);
                cmd.Parameters.AddWithValue("@apellido", txtApellido.Text);
                cmd.Parameters.AddWithValue("@ubicacion", txtUbicacion.Text);
                cmd.Parameters.AddWithValue("@genero", txtGenero.Text);
                cmd.Parameters.AddWithValue("@id", idUsuario);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Usuario actualizado correctamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarUsuarios();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar usuario: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conexion.State == ConnectionState.Open)
                {
                    conexion.Close();
                }
            }
        }

        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            if (dataGridViewUsuarios.CurrentRow == null) return;

            int idUsuario = Convert.ToInt32(dataGridViewUsuarios.CurrentRow.Cells["ID"].Value);
            string username = dataGridViewUsuarios.CurrentRow.Cells["User"].Value.ToString();

            if (idUsuario == 1)
            {
                MessageBox.Show("No se puede eliminar el usuario administrador", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (MessageBox.Show($"¿Está seguro de eliminar al usuario {username}?", "Confirmar eliminación",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    if (conexion.State == ConnectionState.Closed)
                    {
                        conexion.Open();
                    }

                    string consulta = "DELETE FROM Usuario WHERE ID = @id";
                    MySqlCommand cmd = new MySqlCommand(consulta, conexion);
                    cmd.Parameters.AddWithValue("@id", idUsuario);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Usuario eliminado correctamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarUsuarios();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al eliminar usuario: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void BtnRefrescar_Click(object sender, EventArgs e)
        {
            CargarUsuarios();
        }

        private void MenuCerrarSesion_Click(object sender, EventArgs e)
        {
            this.Close();
            Application.OpenForms["LogIn"]?.Show();
        }

        private DataGridView dataGridViewUsuarios;
        private TextBox txtUsername;
        private TextBox txtNombre;
        private TextBox txtApellido;
        private TextBox txtUbicacion;
        private TextBox txtGenero;
        private Button btnActualizar;
        private Button btnEliminar;
        private Button btnRefrescar;
    }
} 