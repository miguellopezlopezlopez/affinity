namespace FiltringApp
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private Label label1;
        private DataGridView dataGridViewUsuario;
        private Button btnActualizar;
        private Button btnCargarFoto;
        private PictureBox pictureBoxFoto;
        private TextBox txtUser;
        private TextBox txtApellido;
        private TextBox txtNombre;
        private TextBox txtPassword;
        private TextBox txtUbicacion;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label lblPassword;
        private ComboBox cmbGenero;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem opcionesMenu;
        private ToolStripMenuItem irAlPerfilItem;
        private ToolStripMenuItem cerrarSesionItem;
        private ToolStripMenuItem verEstadisticasToolStripMenuItem;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            label1 = new Label();
            dataGridViewUsuario = new DataGridView();
            btnActualizar = new Button();
            btnCargarFoto = new Button();
            pictureBoxFoto = new PictureBox();
            txtUser = new TextBox();
            txtApellido = new TextBox();
            txtNombre = new TextBox();
            txtPassword = new TextBox();
            txtUbicacion = new TextBox();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            label7 = new Label();
            lblPassword = new Label();
            cmbGenero = new ComboBox();
            menuStrip1 = new MenuStrip();
            opcionesMenu = new ToolStripMenuItem();
            irAlPerfilItem = new ToolStripMenuItem();
            abrirMatchesToolStripMenuItem = new ToolStripMenuItem();
            verUsuariosToolStripMenuItem = new ToolStripMenuItem();
            abrirMatchesPendientesToolStripMenuItem = new ToolStripMenuItem();
            abrirMensajesRecibidosToolStripMenuItem = new ToolStripMenuItem();
            verEstadisticasToolStripMenuItem = new ToolStripMenuItem();
            cerrarSesionItem = new ToolStripMenuItem();
            btnEliminarUsuario = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridViewUsuario).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxFoto).BeginInit();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(325, 23);
            label1.Name = "label1";
            label1.Size = new Size(111, 15);
            label1.TabIndex = 0;
            label1.Text = "Gestión de Usuarios";
            // 
            // dataGridViewUsuario
            // 
            dataGridViewUsuario.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewUsuario.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewUsuario.Location = new Point(110, 69);
            dataGridViewUsuario.Name = "dataGridViewUsuario";
            dataGridViewUsuario.Size = new Size(620, 150);
            dataGridViewUsuario.TabIndex = 1;
            // 
            // btnActualizar
            // 
            btnActualizar.Location = new Point(400, 332);
            btnActualizar.Name = "btnActualizar";
            btnActualizar.Size = new Size(147, 46);
            btnActualizar.TabIndex = 2;
            btnActualizar.Text = "Actualizar Usuario";
            btnActualizar.Click += btnActualizar_Click;
            // 
            // btnCargarFoto
            // 
            btnCargarFoto.Location = new Point(623, 358);
            btnCargarFoto.Name = "btnCargarFoto";
            btnCargarFoto.Size = new Size(89, 43);
            btnCargarFoto.TabIndex = 17;
            btnCargarFoto.Text = "Seleccione su foto de perfil";
            btnCargarFoto.Click += btnCargarFoto_Click;
            // 
            // pictureBoxFoto
            // 
            pictureBoxFoto.Location = new Point(596, 230);
            pictureBoxFoto.Name = "pictureBoxFoto";
            pictureBoxFoto.Size = new Size(135, 122);
            pictureBoxFoto.TabIndex = 16;
            pictureBoxFoto.TabStop = false;
            // 
            // txtUser
            // 
            txtUser.Location = new Point(186, 230);
            txtUser.Name = "txtUser";
            txtUser.ReadOnly = true;
            txtUser.Size = new Size(180, 23);
            txtUser.TabIndex = 3;
            // 
            // txtApellido
            // 
            txtApellido.Location = new Point(186, 290);
            txtApellido.Name = "txtApellido";
            txtApellido.Size = new Size(180, 23);
            txtApellido.TabIndex = 6;
            // 
            // txtNombre
            // 
            txtNombre.Location = new Point(186, 260);
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(180, 23);
            txtNombre.TabIndex = 5;
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(186, 350);
            txtPassword.Name = "txtPassword";
            txtPassword.PasswordChar = '*';
            txtPassword.Size = new Size(180, 23);
            txtPassword.TabIndex = 8;
            // 
            // txtUbicacion
            // 
            txtUbicacion.Location = new Point(186, 320);
            txtUbicacion.Name = "txtUbicacion";
            txtUbicacion.Size = new Size(180, 23);
            txtUbicacion.TabIndex = 7;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(110, 233);
            label2.Name = "label2";
            label2.Size = new Size(50, 15);
            label2.TabIndex = 18;
            label2.Text = "Usuario:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(110, 263);
            label3.Name = "label3";
            label3.Size = new Size(54, 15);
            label3.TabIndex = 19;
            label3.Text = "Nombre:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(110, 293);
            label4.Name = "label4";
            label4.Size = new Size(54, 15);
            label4.TabIndex = 20;
            label4.Text = "Apellido:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(110, 323);
            label5.Name = "label5";
            label5.Size = new Size(63, 15);
            label5.TabIndex = 21;
            label5.Text = "Ubicación:";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(110, 383);
            label6.Name = "label6";
            label6.Size = new Size(48, 15);
            label6.TabIndex = 22;
            label6.Text = "Género:";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(580, 230);
            label7.Name = "label7";
            label7.Size = new Size(0, 15);
            label7.TabIndex = 23;
            // 
            // lblPassword
            // 
            lblPassword.AutoSize = true;
            lblPassword.Location = new Point(110, 353);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(70, 15);
            lblPassword.TabIndex = 24;
            lblPassword.Text = "Contraseña:";
            // 
            // cmbGenero
            // 
            cmbGenero.FormattingEnabled = true;
            cmbGenero.Items.AddRange(new object[] { "Masculino", "Femenino", "Otro" });
            cmbGenero.Location = new Point(186, 380);
            cmbGenero.Name = "cmbGenero";
            cmbGenero.Size = new Size(180, 23);
            cmbGenero.TabIndex = 9;
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { opcionesMenu });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(800, 24);
            menuStrip1.TabIndex = 30;
            menuStrip1.Text = "menuStrip1";
            // 
            // opcionesMenu
            // 
            opcionesMenu.DropDownItems.AddRange(new ToolStripItem[] { irAlPerfilItem, abrirMatchesToolStripMenuItem, verUsuariosToolStripMenuItem, abrirMatchesPendientesToolStripMenuItem, abrirMensajesRecibidosToolStripMenuItem, verEstadisticasToolStripMenuItem, cerrarSesionItem });
            opcionesMenu.Name = "opcionesMenu";
            opcionesMenu.Size = new Size(69, 20);
            opcionesMenu.Text = "Opciones";
            // 
            // irAlPerfilItem
            // 
            irAlPerfilItem.Name = "irAlPerfilItem";
            irAlPerfilItem.Size = new Size(209, 22);
            irAlPerfilItem.Text = "Ir al Perfil";
            irAlPerfilItem.Click += irAlPerfilItem_Click;
            // 
            // abrirMatchesToolStripMenuItem
            // 
            abrirMatchesToolStripMenuItem.Name = "abrirMatchesToolStripMenuItem";
            abrirMatchesToolStripMenuItem.Size = new Size(209, 22);
            abrirMatchesToolStripMenuItem.Text = "Abrir Matches";
            abrirMatchesToolStripMenuItem.Click += abrirMatchesToolStripMenuItem_Click;
            // 
            // verUsuariosToolStripMenuItem
            // 
            verUsuariosToolStripMenuItem.Name = "verUsuariosToolStripMenuItem";
            verUsuariosToolStripMenuItem.Size = new Size(209, 22);
            verUsuariosToolStripMenuItem.Text = "Ver Usuarios";
            verUsuariosToolStripMenuItem.Click += verUsuariosToolStripMenuItem_Click;
            // 
            // abrirMatchesPendientesToolStripMenuItem
            // 
            abrirMatchesPendientesToolStripMenuItem.Name = "abrirMatchesPendientesToolStripMenuItem";
            abrirMatchesPendientesToolStripMenuItem.Size = new Size(209, 22);
            abrirMatchesPendientesToolStripMenuItem.Text = "Abrir Matches Pendientes";
            abrirMatchesPendientesToolStripMenuItem.Click += abrirMatchesPendientesToolStripMenuItem_Click;
            // 
            // abrirMensajesRecibidosToolStripMenuItem
            // 
            abrirMensajesRecibidosToolStripMenuItem.Name = "abrirMensajesRecibidosToolStripMenuItem";
            abrirMensajesRecibidosToolStripMenuItem.Size = new Size(209, 22);
            abrirMensajesRecibidosToolStripMenuItem.Text = "Abrir Mensajes Recibidos";
            abrirMensajesRecibidosToolStripMenuItem.Click += abrirMensajesRecibidosToolStripMenuItem_Click;
            // 
            // verEstadisticasToolStripMenuItem
            // 
            verEstadisticasToolStripMenuItem.Name = "verEstadisticasToolStripMenuItem";
            verEstadisticasToolStripMenuItem.Text = "Ver Estadísticas";
            verEstadisticasToolStripMenuItem.Click += verEstadisticasToolStripMenuItem_Click;
            // 
            // cerrarSesionItem
            // 
            cerrarSesionItem.Name = "cerrarSesionItem";
            cerrarSesionItem.Size = new Size(209, 22);
            cerrarSesionItem.Text = "Cerrar Sesión";
            cerrarSesionItem.Click += cerrarSesiónToolStripMenuItem_Click;
            // 
            // btnEliminarUsuario
            // 
            btnEliminarUsuario.Location = new Point(400, 277);
            btnEliminarUsuario.Name = "btnEliminarUsuario";
            btnEliminarUsuario.Size = new Size(147, 46);
            btnEliminarUsuario.TabIndex = 31;
            btnEliminarUsuario.Text = "Eliminar Usuario";
            btnEliminarUsuario.Click += btnEliminarUsuario_Click;
            // 
            // MainForm
            // 
            ClientSize = new Size(800, 450);
            Controls.Add(btnEliminarUsuario);
            Controls.Add(cmbGenero);
            Controls.Add(label7);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(lblPassword);
            Controls.Add(txtUbicacion);
            Controls.Add(txtPassword);
            Controls.Add(txtNombre);
            Controls.Add(txtApellido);
            Controls.Add(txtUser);
            Controls.Add(menuStrip1);
            Controls.Add(label1);
            Controls.Add(dataGridViewUsuario);
            Controls.Add(btnActualizar);
            Controls.Add(btnCargarFoto);
            Controls.Add(pictureBoxFoto);
            MainMenuStrip = menuStrip1;
            Name = "MainForm";
            Text = "MainForm";
            Load += MainForm_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridViewUsuario).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxFoto).EndInit();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();

        }
        private ToolStripMenuItem abrirMatchesToolStripMenuItem;
        private ToolStripMenuItem abrirMensajesRecibidosToolStripMenuItem;
        private ToolStripMenuItem abrirMatchesPendientesToolStripMenuItem;

        private void MainForm_Load(object sender, EventArgs e)
        {
            // Se ejecuta cuando se carga el formulario
            // Podemos agregar código adicional aquí si es necesario
        }
        private ToolStripMenuItem verUsuariosToolStripMenuItem;
        private Button btnEliminarUsuario;
    }
}