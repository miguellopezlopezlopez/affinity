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
            this.lblTitulo = new System.Windows.Forms.Label();
            this.pictureBoxFoto = new System.Windows.Forms.PictureBox();
            this.lblNombre = new System.Windows.Forms.Label();
            this.lblUbicacion = new System.Windows.Forms.Label();
            this.btnSiguiente = new System.Windows.Forms.Button();
            this.btnAnterior = new System.Windows.Forms.Button();
            this.btnEnviarSolicitud = new System.Windows.Forms.Button();
            this.lblInfoUsuario = new System.Windows.Forms.Label();
            this.panelInfoUsuario = new System.Windows.Forms.Panel();
            this.groupBoxInfo = new System.Windows.Forms.GroupBox();

            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxFoto)).BeginInit();
            this.groupBoxInfo.SuspendLayout();
            this.SuspendLayout();

            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblTitulo.ForeColor = System.Drawing.Color.FromArgb(64, 64, 64);
            this.lblTitulo.Location = new System.Drawing.Point(25, 15);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(200, 32);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Explora Usuarios";

            // 
            // pictureBoxFoto
            // 
            this.pictureBoxFoto.Location = new System.Drawing.Point(25, 60);
            this.pictureBoxFoto.Name = "pictureBoxFoto";
            this.pictureBoxFoto.Size = new System.Drawing.Size(350, 280);
            this.pictureBoxFoto.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxFoto.TabIndex = 1;
            this.pictureBoxFoto.TabStop = false;
            this.pictureBoxFoto.BackColor = System.Drawing.Color.White;
            this.pictureBoxFoto.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            // 
            // lblNombre
            // 
            this.lblNombre.AutoSize = true;
            this.lblNombre.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblNombre.ForeColor = System.Drawing.Color.FromArgb(51, 51, 51);
            this.lblNombre.Location = new System.Drawing.Point(25, 355);
            this.lblNombre.Name = "lblNombre";
            this.lblNombre.Size = new System.Drawing.Size(200, 30);
            this.lblNombre.TabIndex = 2;

            // 
            // lblUbicacion
            // 
            this.lblUbicacion.AutoSize = true;
            this.lblUbicacion.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblUbicacion.ForeColor = System.Drawing.Color.FromArgb(102, 102, 102);
            this.lblUbicacion.Location = new System.Drawing.Point(25, 390);
            this.lblUbicacion.Name = "lblUbicacion";
            this.lblUbicacion.Size = new System.Drawing.Size(200, 21);
            this.lblUbicacion.TabIndex = 3;

            // 
            // groupBoxInfo
            // 
            this.groupBoxInfo.Text = "Información Personal";
            this.groupBoxInfo.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.groupBoxInfo.ForeColor = System.Drawing.Color.FromArgb(64, 64, 64);
            this.groupBoxInfo.Location = new System.Drawing.Point(25, 425);
            this.groupBoxInfo.Size = new System.Drawing.Size(350, 120);
            this.groupBoxInfo.TabIndex = 4;

            // 
            // panelInfoUsuario
            // 
            this.panelInfoUsuario.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.panelInfoUsuario.Location = new System.Drawing.Point(10, 25);
            this.panelInfoUsuario.Size = new System.Drawing.Size(330, 85);
            this.panelInfoUsuario.TabIndex = 0;
            this.panelInfoUsuario.AutoScroll = true;

            // 
            // lblInfoUsuario
            // 
            this.lblInfoUsuario.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblInfoUsuario.ForeColor = System.Drawing.Color.FromArgb(64, 64, 64);
            this.lblInfoUsuario.Location = new System.Drawing.Point(5, 5);
            this.lblInfoUsuario.Name = "lblInfoUsuario";
            this.lblInfoUsuario.Size = new System.Drawing.Size(315, 75);
            this.lblInfoUsuario.TabIndex = 0;

            // 
            // btnAnterior
            // 
            this.btnAnterior.Location = new System.Drawing.Point(25, 565);
            this.btnAnterior.Name = "btnAnterior";
            this.btnAnterior.Size = new System.Drawing.Size(100, 40);
            this.btnAnterior.TabIndex = 5;
            this.btnAnterior.Text = "◀ Anterior";
            this.btnAnterior.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnAnterior.BackColor = System.Drawing.Color.FromArgb(230, 230, 230);
            this.btnAnterior.ForeColor = System.Drawing.Color.FromArgb(64, 64, 64);
            this.btnAnterior.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAnterior.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(200, 200, 200);
            this.btnAnterior.UseVisualStyleBackColor = false;
            this.btnAnterior.Click += new System.EventHandler(this.btnAnterior_Click);

            // 
            // btnEnviarSolicitud
            // 
            this.btnEnviarSolicitud.Location = new System.Drawing.Point(140, 565);
            this.btnEnviarSolicitud.Name = "btnEnviarSolicitud";
            this.btnEnviarSolicitud.Size = new System.Drawing.Size(120, 40);
            this.btnEnviarSolicitud.TabIndex = 6;
            this.btnEnviarSolicitud.Text = "💖 Match";
            this.btnEnviarSolicitud.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnEnviarSolicitud.BackColor = System.Drawing.Color.FromArgb(255, 69, 120);
            this.btnEnviarSolicitud.ForeColor = System.Drawing.Color.White;
            this.btnEnviarSolicitud.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEnviarSolicitud.FlatAppearance.BorderSize = 0;
            this.btnEnviarSolicitud.UseVisualStyleBackColor = false;
            this.btnEnviarSolicitud.Click += new System.EventHandler(this.btnEnviarSolicitud_Click);

            // 
            // btnSiguiente
            // 
            this.btnSiguiente.Location = new System.Drawing.Point(275, 565);
            this.btnSiguiente.Name = "btnSiguiente";
            this.btnSiguiente.Size = new System.Drawing.Size(100, 40);
            this.btnSiguiente.TabIndex = 7;
            this.btnSiguiente.Text = "Siguiente ▶";
            this.btnSiguiente.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnSiguiente.BackColor = System.Drawing.Color.FromArgb(230, 230, 230);
            this.btnSiguiente.ForeColor = System.Drawing.Color.FromArgb(64, 64, 64);
            this.btnSiguiente.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSiguiente.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(200, 200, 200);
            this.btnSiguiente.UseVisualStyleBackColor = false;
            this.btnSiguiente.Click += new System.EventHandler(this.btnSiguiente_Click);

            // 
            // MatchesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 650);
            this.Controls.Add(this.btnSiguiente);
            this.Controls.Add(this.btnEnviarSolicitud);
            this.Controls.Add(this.btnAnterior);
            this.Controls.Add(this.groupBoxInfo);
            this.Controls.Add(this.lblUbicacion);
            this.Controls.Add(this.lblNombre);
            this.Controls.Add(this.pictureBoxFoto);
            this.Controls.Add(this.lblTitulo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MatchesForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Explorar Matches";
            this.BackColor = System.Drawing.Color.FromArgb(248, 248, 248);

            // Agregar controles
            this.groupBoxInfo.Controls.Add(this.panelInfoUsuario);
            this.panelInfoUsuario.Controls.Add(this.lblInfoUsuario);

            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxFoto)).EndInit();
            this.groupBoxInfo.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}