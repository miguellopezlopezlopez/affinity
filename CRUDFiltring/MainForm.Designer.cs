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
        private ComboBox cmbGenero;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem opcionesMenu;
        private ToolStripMenuItem irAlPerfilItem;
        private ToolStripMenuItem cerrarSesionItem;

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
            cmbGenero = new ComboBox();
            menuStrip1 = new MenuStrip();
            opcionesMenu = new ToolStripMenuItem();
            irAlPerfilItem = new ToolStripMenuItem();
            abrirMatchesToolStripMenuItem = new ToolStripMenuItem();
            cerrarSesionItem = new ToolStripMenuItem();
            abrirMensajesRecibidosToolStripMenuItem = new ToolStripMenuItem();
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
            txtUser.Location = new Point(0, 0);
            txtUser.Name = "txtUser";
            txtUser.Size = new Size(100, 23);
            txtUser.TabIndex = 0;
            // 
            // txtApellido
            // 
            txtApellido.Location = new Point(0, 0);
            txtApellido.Name = "txtApellido";
            txtApellido.Size = new Size(100, 23);
            txtApellido.TabIndex = 0;
            // 
            // txtNombre
            // 
            txtNombre.Location = new Point(0, 0);
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(100, 23);
            txtNombre.TabIndex = 0;
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(0, 0);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(100, 23);
            txtPassword.TabIndex = 0;
            // 
            // txtUbicacion
            // 
            txtUbicacion.Location = new Point(0, 0);
            txtUbicacion.Name = "txtUbicacion";
            txtUbicacion.Size = new Size(100, 23);
            txtUbicacion.TabIndex = 0;
            // 
            // label2
            // 
            label2.Location = new Point(0, 0);
            label2.Name = "label2";
            label2.Size = new Size(100, 23);
            label2.TabIndex = 0;
            // 
            // label3
            // 
            label3.Location = new Point(0, 0);
            label3.Name = "label3";
            label3.Size = new Size(100, 23);
            label3.TabIndex = 0;
            // 
            // label4
            // 
            label4.Location = new Point(0, 0);
            label4.Name = "label4";
            label4.Size = new Size(100, 23);
            label4.TabIndex = 0;
            // 
            // label5
            // 
            label5.Location = new Point(0, 0);
            label5.Name = "label5";
            label5.Size = new Size(100, 23);
            label5.TabIndex = 0;
            // 
            // label6
            // 
            label6.Location = new Point(0, 0);
            label6.Name = "label6";
            label6.Size = new Size(100, 23);
            label6.TabIndex = 0;
            // 
            // label7
            // 
            label7.Location = new Point(0, 0);
            label7.Name = "label7";
            label7.Size = new Size(100, 23);
            label7.TabIndex = 0;
            // 
            // cmbGenero
            // 
            cmbGenero.Location = new Point(0, 0);
            cmbGenero.Name = "cmbGenero";
            cmbGenero.Size = new Size(121, 23);
            cmbGenero.TabIndex = 0;
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
            opcionesMenu.DropDownItems.AddRange(new ToolStripItem[] { irAlPerfilItem, abrirMatchesToolStripMenuItem, abrirMensajesRecibidosToolStripMenuItem, cerrarSesionItem });
            opcionesMenu.Name = "opcionesMenu";
            opcionesMenu.Size = new Size(69, 20);
            opcionesMenu.Text = "Opciones";
            // 
            // irAlPerfilItem
            // 
            irAlPerfilItem.Name = "irAlPerfilItem";
            irAlPerfilItem.Size = new Size(206, 22);
            irAlPerfilItem.Text = "Ir al Perfil";
            // 
            // abrirMatchesToolStripMenuItem
            // 
            abrirMatchesToolStripMenuItem.Name = "abrirMatchesToolStripMenuItem";
            abrirMatchesToolStripMenuItem.Size = new Size(206, 22);
            abrirMatchesToolStripMenuItem.Text = "Abrir Matches";
            abrirMatchesToolStripMenuItem.Click += abrirMatchesToolStripMenuItem_Click;
            // 
            // cerrarSesionItem
            // 
            cerrarSesionItem.Name = "cerrarSesionItem";
            cerrarSesionItem.Size = new Size(206, 22);
            cerrarSesionItem.Text = "Cerrar Sesión";
            cerrarSesionItem.Click += cerrarSesiónToolStripMenuItem_Click;
            // 
            // abrirMensajesRecibidosToolStripMenuItem
            // 
            abrirMensajesRecibidosToolStripMenuItem.Name = "abrirMensajesRecibidosToolStripMenuItem";
            abrirMensajesRecibidosToolStripMenuItem.Size = new Size(206, 22);
            abrirMensajesRecibidosToolStripMenuItem.Text = "Abrir Mensajes Recibidos";
            abrirMensajesRecibidosToolStripMenuItem.Click += abrirMensajesRecibidosToolStripMenuItem_Click;
            // 
            // MainForm
            // 
            ClientSize = new Size(800, 450);
            Controls.Add(menuStrip1);
            Controls.Add(label1);
            Controls.Add(dataGridViewUsuario);
            Controls.Add(btnActualizar);
            Controls.Add(btnCargarFoto);
            Controls.Add(pictureBoxFoto);
            MainMenuStrip = menuStrip1;
            Name = "MainForm";
            Text = "MainForm";
            ((System.ComponentModel.ISupportInitialize)dataGridViewUsuario).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxFoto).EndInit();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }
        private ToolStripMenuItem abrirMatchesToolStripMenuItem;
        private ToolStripMenuItem abrirMensajesRecibidosToolStripMenuItem;
    }
}
