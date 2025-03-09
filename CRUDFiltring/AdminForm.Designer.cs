namespace FiltringApp
{
    partial class AdminForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            cmbGenero = new ComboBox();
            label7 = new Label();
            label6 = new Label();
            label5 = new Label();
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            txtUbicacion = new TextBox();
            txtPassword = new TextBox();
            txtNombre = new TextBox();
            txtApellido = new TextBox();
            txtUser = new TextBox();
            btnCargarFoto = new Button();
            pictureBoxFoto = new PictureBox();
            btnActualizar = new Button();
            dataGridViewUsuarios = new DataGridView();
            label1 = new Label();
            btnEliminarUsuario = new Button();
            menuStrip1 = new MenuStrip();
            opcionesToolStripMenuItem = new ToolStripMenuItem();
            cerrarSesiónToolStripMenuItem = new ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)pictureBoxFoto).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridViewUsuarios).BeginInit();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // cmbGenero
            // 
            cmbGenero.FormattingEnabled = true;
            cmbGenero.Items.AddRange(new object[] { "Masculino", "Femenino", "Prefiero no decirlo" });
            cmbGenero.Location = new Point(165, 375);
            cmbGenero.Name = "cmbGenero";
            cmbGenero.Size = new Size(197, 23);
            cmbGenero.TabIndex = 46;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(41, 380);
            label7.Name = "label7";
            label7.Size = new Size(118, 15);
            label7.TabIndex = 45;
            label7.Text = "Seleccione su genero";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(89, 407);
            label6.Name = "label6";
            label6.Size = new Size(60, 15);
            label6.TabIndex = 44;
            label6.Text = "Ubicacion";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(89, 349);
            label5.Name = "label5";
            label5.Size = new Size(51, 15);
            label5.TabIndex = 43;
            label5.Text = "Apellido";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(89, 320);
            label4.Name = "label4";
            label4.Size = new Size(51, 15);
            label4.TabIndex = 42;
            label4.Text = "Nombre";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(89, 288);
            label3.Name = "label3";
            label3.Size = new Size(67, 15);
            label3.TabIndex = 41;
            label3.Text = "Contraseña";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(89, 262);
            label2.Name = "label2";
            label2.Size = new Size(47, 15);
            label2.TabIndex = 40;
            label2.Text = "Usuario";
            // 
            // txtUbicacion
            // 
            txtUbicacion.Location = new Point(165, 404);
            txtUbicacion.Name = "txtUbicacion";
            txtUbicacion.Size = new Size(198, 23);
            txtUbicacion.TabIndex = 39;
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(164, 288);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(198, 23);
            txtPassword.TabIndex = 38;
            // 
            // txtNombre
            // 
            txtNombre.Location = new Point(164, 317);
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(198, 23);
            txtNombre.TabIndex = 37;
            // 
            // txtApellido
            // 
            txtApellido.Location = new Point(164, 346);
            txtApellido.Name = "txtApellido";
            txtApellido.Size = new Size(198, 23);
            txtApellido.TabIndex = 36;
            // 
            // txtUser
            // 
            txtUser.Location = new Point(164, 259);
            txtUser.Name = "txtUser";
            txtUser.Size = new Size(198, 23);
            txtUser.TabIndex = 35;
            // 
            // btnCargarFoto
            // 
            btnCargarFoto.Location = new Point(652, 349);
            btnCargarFoto.Name = "btnCargarFoto";
            btnCargarFoto.Size = new Size(89, 43);
            btnCargarFoto.TabIndex = 34;
            btnCargarFoto.Text = "Seleccione su foto de perfil";
            btnCargarFoto.UseVisualStyleBackColor = true;
            btnCargarFoto.Click += btnCargarFoto_Click;
            // 
            // pictureBoxFoto
            // 
            pictureBoxFoto.Location = new Point(625, 221);
            pictureBoxFoto.Name = "pictureBoxFoto";
            pictureBoxFoto.Size = new Size(135, 122);
            pictureBoxFoto.TabIndex = 33;
            pictureBoxFoto.TabStop = false;
            // 
            // btnActualizar
            // 
            btnActualizar.Location = new Point(429, 323);
            btnActualizar.Name = "btnActualizar";
            btnActualizar.Size = new Size(147, 46);
            btnActualizar.TabIndex = 32;
            btnActualizar.Text = "Actualizar Usuario";
            btnActualizar.UseVisualStyleBackColor = true;
            btnActualizar.Click += btnActualizar_Click;
            // 
            // dataGridViewUsuarios
            // 
            dataGridViewUsuarios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewUsuarios.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewUsuarios.Location = new Point(139, 60);
            dataGridViewUsuarios.Name = "dataGridViewUsuarios";
            dataGridViewUsuarios.RowTemplate.Height = 25;
            dataGridViewUsuarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewUsuarios.Size = new Size(620, 150);
            dataGridViewUsuarios.TabIndex = 31;
            dataGridViewUsuarios.CellClick += dataGridViewUsuarios_CellClick;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 118);
            label1.Name = "label1";
            label1.Size = new Size(111, 15);
            label1.TabIndex = 30;
            label1.Text = "Gestión de Usuarios";
            // 
            // btnEliminarUsuario
            // 
            btnEliminarUsuario.Location = new Point(429, 257);
            btnEliminarUsuario.Name = "btnEliminarUsuario";
            btnEliminarUsuario.Size = new Size(147, 46);
            btnEliminarUsuario.TabIndex = 47;
            btnEliminarUsuario.Text = "Eliminar Usuario";
            btnEliminarUsuario.UseVisualStyleBackColor = true;
            btnEliminarUsuario.Click += btnEliminarUsuario_Click;
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { opcionesToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(800, 24);
            menuStrip1.TabIndex = 48;
            menuStrip1.Text = "menuStrip1";
            // 
            // opcionesToolStripMenuItem
            // 
            opcionesToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { cerrarSesiónToolStripMenuItem });
            opcionesToolStripMenuItem.Name = "opcionesToolStripMenuItem";
            opcionesToolStripMenuItem.Size = new Size(69, 20);
            opcionesToolStripMenuItem.Text = "Opciones";
            // 
            // cerrarSesiónToolStripMenuItem
            // 
            cerrarSesiónToolStripMenuItem.Name = "cerrarSesiónToolStripMenuItem";
            cerrarSesiónToolStripMenuItem.Size = new Size(180, 22);
            cerrarSesiónToolStripMenuItem.Text = "Cerrar Sesión";
            cerrarSesiónToolStripMenuItem.Click += cerrarSesiónToolStripMenuItem_Click;
            // 
            // AdminForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnEliminarUsuario);
            Controls.Add(cmbGenero);
            Controls.Add(label7);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(txtUbicacion);
            Controls.Add(txtPassword);
            Controls.Add(txtNombre);
            Controls.Add(txtApellido);
            Controls.Add(txtUser);
            Controls.Add(btnCargarFoto);
            Controls.Add(pictureBoxFoto);
            Controls.Add(btnActualizar);
            Controls.Add(dataGridViewUsuarios);
            Controls.Add(label1);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "AdminForm";
            Text = "AdminForm";
            ((System.ComponentModel.ISupportInitialize)pictureBoxFoto).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridViewUsuarios).EndInit();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ComboBox cmbGenero;
        private Label label7;
        private Label label6;
        private Label label5;
        private Label label4;
        private Label label3;
        private Label label2;
        private TextBox txtUbicacion;
        private TextBox txtPassword;
        private TextBox txtNombre;
        private TextBox txtApellido;
        private TextBox txtUser;
        private Button btnCargarFoto;
        private PictureBox pictureBoxFoto;
        private Button btnActualizar;
        private DataGridView dataGridViewUsuarios;
        private Label label1;
        private Button btnEliminarUsuario;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem opcionesToolStripMenuItem;
        private ToolStripMenuItem cerrarSesiónToolStripMenuItem;
    }
}