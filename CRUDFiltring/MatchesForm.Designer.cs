namespace FiltringApp
{
    partial class MatchesForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.PictureBox pictureBoxFoto;
        private System.Windows.Forms.Label lblNombre;
        private System.Windows.Forms.Label lblUbicacion;
        private System.Windows.Forms.Button btnSiguiente;
        private System.Windows.Forms.Button btnAnterior;
        private System.Windows.Forms.Button btnEnviarSolicitud;
        private System.Windows.Forms.Label lblInfoUsuario;
        private System.Windows.Forms.Panel panelInfoUsuario;
        private System.Windows.Forms.GroupBox groupBoxInfo;

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
            lblTitulo = new Label();
            pictureBoxFoto = new PictureBox();
            lblNombre = new Label();
            lblUbicacion = new Label();
            btnSiguiente = new Button();
            btnAnterior = new Button();
            btnEnviarSolicitud = new Button();
            lblInfoUsuario = new Label();
            panelInfoUsuario = new Panel();
            groupBoxInfo = new GroupBox();
            ((System.ComponentModel.ISupportInitialize)pictureBoxFoto).BeginInit();
            panelInfoUsuario.SuspendLayout();
            groupBoxInfo.SuspendLayout();
            SuspendLayout();
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point);
            lblTitulo.ForeColor = Color.FromArgb(64, 64, 64);
            lblTitulo.Location = new Point(25, 15);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(206, 32);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Explora Usuarios";
            // 
            // pictureBoxFoto
            // 
            pictureBoxFoto.BackColor = Color.White;
            pictureBoxFoto.BorderStyle = BorderStyle.FixedSingle;
            pictureBoxFoto.Location = new Point(25, 60);
            pictureBoxFoto.Name = "pictureBoxFoto";
            pictureBoxFoto.Size = new Size(350, 280);
            pictureBoxFoto.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxFoto.TabIndex = 1;
            pictureBoxFoto.TabStop = false;
            // 
            // lblNombre
            // 
            lblNombre.AutoSize = true;
            lblNombre.Font = new Font("Segoe UI", 16F, FontStyle.Bold, GraphicsUnit.Point);
            lblNombre.ForeColor = Color.FromArgb(51, 51, 51);
            lblNombre.Location = new Point(25, 355);
            lblNombre.Name = "lblNombre";
            lblNombre.Size = new Size(0, 30);
            lblNombre.TabIndex = 2;
            // 
            // lblUbicacion
            // 
            lblUbicacion.AutoSize = true;
            lblUbicacion.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            lblUbicacion.ForeColor = Color.FromArgb(102, 102, 102);
            lblUbicacion.Location = new Point(25, 390);
            lblUbicacion.Name = "lblUbicacion";
            lblUbicacion.Size = new Size(0, 21);
            lblUbicacion.TabIndex = 3;
            // 
            // btnSiguiente
            // 
            btnSiguiente.BackColor = Color.FromArgb(230, 230, 230);
            btnSiguiente.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
            btnSiguiente.FlatStyle = FlatStyle.Flat;
            btnSiguiente.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            btnSiguiente.ForeColor = Color.FromArgb(64, 64, 64);
            btnSiguiente.Location = new Point(275, 565);
            btnSiguiente.Name = "btnSiguiente";
            btnSiguiente.Size = new Size(100, 40);
            btnSiguiente.TabIndex = 7;
            btnSiguiente.Text = "Siguiente ▶";
            btnSiguiente.UseVisualStyleBackColor = false;
            btnSiguiente.Click += btnSiguiente_Click;
            // 
            // btnAnterior
            // 
            btnAnterior.BackColor = Color.FromArgb(230, 230, 230);
            btnAnterior.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
            btnAnterior.FlatStyle = FlatStyle.Flat;
            btnAnterior.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            btnAnterior.ForeColor = Color.FromArgb(64, 64, 64);
            btnAnterior.Location = new Point(25, 565);
            btnAnterior.Name = "btnAnterior";
            btnAnterior.Size = new Size(100, 40);
            btnAnterior.TabIndex = 5;
            btnAnterior.Text = "◀ Anterior";
            btnAnterior.UseVisualStyleBackColor = false;
            btnAnterior.Click += btnAnterior_Click;
            // 
            // btnEnviarSolicitud
            // 
            btnEnviarSolicitud.BackColor = Color.FromArgb(255, 69, 120);
            btnEnviarSolicitud.FlatAppearance.BorderSize = 0;
            btnEnviarSolicitud.FlatStyle = FlatStyle.Flat;
            btnEnviarSolicitud.Font = new Font("Segoe UI", 11F, FontStyle.Bold, GraphicsUnit.Point);
            btnEnviarSolicitud.ForeColor = Color.White;
            btnEnviarSolicitud.Location = new Point(140, 565);
            btnEnviarSolicitud.Name = "btnEnviarSolicitud";
            btnEnviarSolicitud.Size = new Size(120, 40);
            btnEnviarSolicitud.TabIndex = 6;
            btnEnviarSolicitud.Text = "💖 Match";
            btnEnviarSolicitud.UseVisualStyleBackColor = false;
            btnEnviarSolicitud.Click += btnEnviarSolicitud_Click;
            // 
            // lblInfoUsuario
            // 
            lblInfoUsuario.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            lblInfoUsuario.ForeColor = Color.FromArgb(64, 64, 64);
            lblInfoUsuario.Location = new Point(5, 5);
            lblInfoUsuario.Name = "lblInfoUsuario";
            lblInfoUsuario.Size = new Size(315, 168);
            lblInfoUsuario.TabIndex = 0;
            // 
            // panelInfoUsuario
            // 
            panelInfoUsuario.AutoScroll = true;
            panelInfoUsuario.Controls.Add(lblInfoUsuario);
            panelInfoUsuario.Location = new Point(10, 25);
            panelInfoUsuario.Name = "panelInfoUsuario";
            panelInfoUsuario.Size = new Size(330, 173);
            panelInfoUsuario.TabIndex = 0;
            // 
            // groupBoxInfo
            // 
            groupBoxInfo.Controls.Add(panelInfoUsuario);
            groupBoxInfo.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            groupBoxInfo.ForeColor = Color.FromArgb(64, 64, 64);
            groupBoxInfo.Location = new Point(25, 355);
            groupBoxInfo.Name = "groupBoxInfo";
            groupBoxInfo.Size = new Size(350, 204);
            groupBoxInfo.TabIndex = 4;
            groupBoxInfo.TabStop = false;
            groupBoxInfo.Text = "Información Personal";
            // 
            // MatchesForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(248, 248, 248);
            ClientSize = new Size(800, 650);
            Controls.Add(btnSiguiente);
            Controls.Add(btnEnviarSolicitud);
            Controls.Add(btnAnterior);
            Controls.Add(groupBoxInfo);
            Controls.Add(lblUbicacion);
            Controls.Add(lblNombre);
            Controls.Add(pictureBoxFoto);
            Controls.Add(lblTitulo);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "MatchesForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Explorar Matches";
            ((System.ComponentModel.ISupportInitialize)pictureBoxFoto).EndInit();
            panelInfoUsuario.ResumeLayout(false);
            groupBoxInfo.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }
    }
}