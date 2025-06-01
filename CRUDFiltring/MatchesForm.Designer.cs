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

            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxFoto)).BeginInit();
            this.SuspendLayout();

            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblTitulo.Location = new System.Drawing.Point(130, 20);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(200, 25);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Explora Usuarios";

            // 
            // pictureBoxFoto
            // 
            this.pictureBoxFoto.Location = new System.Drawing.Point(100, 60);
            this.pictureBoxFoto.Name = "pictureBoxFoto";
            this.pictureBoxFoto.Size = new System.Drawing.Size(250, 250);
            this.pictureBoxFoto.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxFoto.TabIndex = 1;
            this.pictureBoxFoto.TabStop = false;

            // 
            // lblNombre
            // 
            this.lblNombre.AutoSize = true;
            this.lblNombre.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblNombre.Location = new System.Drawing.Point(100, 320);
            this.lblNombre.Name = "lblNombre";
            this.lblNombre.Size = new System.Drawing.Size(80, 21);
            this.lblNombre.TabIndex = 2;
            this.lblNombre.Text = "Nombre: ";

            // 
            // lblUbicacion
            // 
            this.lblUbicacion.AutoSize = true;
            this.lblUbicacion.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblUbicacion.Location = new System.Drawing.Point(100, 350);
            this.lblUbicacion.Name = "lblUbicacion";
            this.lblUbicacion.Size = new System.Drawing.Size(90, 21);
            this.lblUbicacion.TabIndex = 3;
            this.lblUbicacion.Text = "Ubicación: ";

            // 
            // btnAnterior
            // 
            this.btnAnterior.Location = new System.Drawing.Point(50, 400);
            this.btnAnterior.Name = "btnAnterior";
            this.btnAnterior.Size = new System.Drawing.Size(80, 40);
            this.btnAnterior.TabIndex = 4;
            this.btnAnterior.Text = "< Anterior";
            this.btnAnterior.UseVisualStyleBackColor = true;
            this.btnAnterior.Click += new System.EventHandler(this.btnAnterior_Click);

            // 
            // btnSiguiente
            // 
            this.btnSiguiente.Location = new System.Drawing.Point(320, 400);
            this.btnSiguiente.Name = "btnSiguiente";
            this.btnSiguiente.Size = new System.Drawing.Size(80, 40);
            this.btnSiguiente.TabIndex = 5;
            this.btnSiguiente.Text = "Siguiente >";
            this.btnSiguiente.UseVisualStyleBackColor = true;
            this.btnSiguiente.Click += new System.EventHandler(this.btnSiguiente_Click);

            // 
            // btnEnviarSolicitud
            // 
            this.btnEnviarSolicitud.Location = new System.Drawing.Point(150, 400);
            this.btnEnviarSolicitud.Name = "btnEnviarSolicitud";
            this.btnEnviarSolicitud.Size = new System.Drawing.Size(150, 40);
            this.btnEnviarSolicitud.TabIndex = 6;
            this.btnEnviarSolicitud.Text = "Enviar Match";
            this.btnEnviarSolicitud.UseVisualStyleBackColor = true;
            this.btnEnviarSolicitud.Click += new System.EventHandler(this.btnEnviarSolicitud_Click);

            // 
            // MatchesForm
            // 
            this.ClientSize = new System.Drawing.Size(450, 470);
            this.Controls.Add(this.btnEnviarSolicitud);
            this.Controls.Add(this.btnSiguiente);
            this.Controls.Add(this.btnAnterior);
            this.Controls.Add(this.lblUbicacion);
            this.Controls.Add(this.lblNombre);
            this.Controls.Add(this.pictureBoxFoto);
            this.Controls.Add(this.lblTitulo);
            this.Name = "MatchesForm";
            this.Text = "Explorar Matches";

            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxFoto)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
