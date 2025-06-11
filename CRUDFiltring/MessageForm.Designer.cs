namespace FiltringApp
{
    partial class MessageForm
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
            lblReceptor = new Label();
            txtMensaje = new TextBox();
            btnEnviar = new Button();
            txtMensajes = new TextBox();
            lblEstado = new Label();
            panelMensajes = new Panel();
            panelInferior = new Panel();
            SuspendLayout();

            // Configuración del formulario
            FormStyles.ApplyFormStyle(this);
            this.ClientSize = new Size(800, 600);
            this.MinimumSize = new Size(600, 400);

            // Panel de mensajes
            panelMensajes.AutoScroll = true;
            panelMensajes.BackColor = FormStyles.WhiteColor;
            panelMensajes.BorderStyle = BorderStyle.FixedSingle;
            panelMensajes.Dock = DockStyle.Fill;
            panelMensajes.Padding = new Padding(10);

            // TextBox de mensajes
            txtMensajes.Dock = DockStyle.Fill;
            txtMensajes.Multiline = true;
            txtMensajes.ReadOnly = true;
            txtMensajes.ScrollBars = ScrollBars.Vertical;
            txtMensajes.BackColor = FormStyles.WhiteColor;
            txtMensajes.BorderStyle = BorderStyle.None;
            txtMensajes.Font = FormStyles.RegularFont;
            txtMensajes.ForeColor = FormStyles.DarkColor;
            txtMensajes.Margin = new Padding(10);
            panelMensajes.Controls.Add(txtMensajes);

            // Panel inferior
            panelInferior.Height = 100;
            panelInferior.Dock = DockStyle.Bottom;
            panelInferior.BackColor = FormStyles.LightColor;
            panelInferior.Padding = new Padding(10);

            // Label receptor
            FormStyles.ApplyLabelStyle(lblReceptor, true);
            lblReceptor.Dock = DockStyle.Top;
            lblReceptor.TextAlign = ContentAlignment.MiddleCenter;
            lblReceptor.BackColor = FormStyles.PrimaryColor;
            lblReceptor.ForeColor = FormStyles.WhiteColor;
            lblReceptor.Padding = new Padding(10);
            lblReceptor.Height = 50;

            // TextBox mensaje
            FormStyles.ApplyTextBoxStyle(txtMensaje);
            txtMensaje.Multiline = true;
            txtMensaje.Height = 60;
            txtMensaje.Dock = DockStyle.Fill;
            txtMensaje.Margin = new Padding(10);

            // Botón enviar
            FormStyles.ApplyMainButtonStyle(btnEnviar);
            btnEnviar.Text = "Enviar";
            btnEnviar.Width = 120;
            btnEnviar.Dock = DockStyle.Right;
            btnEnviar.Margin = new Padding(10);

            // Label estado
            FormStyles.ApplyLabelStyle(lblEstado);
            lblEstado.Dock = DockStyle.Bottom;
            lblEstado.TextAlign = ContentAlignment.MiddleCenter;
            lblEstado.Height = 30;
            lblEstado.BackColor = FormStyles.WhiteColor;
            lblEstado.Padding = new Padding(5);

            // Organización de controles
            panelInferior.Controls.Add(btnEnviar);
            panelInferior.Controls.Add(txtMensaje);

            Controls.Add(panelMensajes);
            Controls.Add(panelInferior);
            Controls.Add(lblReceptor);
            Controls.Add(lblEstado);

            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblReceptor;
        private Label lblEstado;
        private TextBox txtMensaje;
        private TextBox txtMensajes;
        private Button btnEnviar;
        private Panel panelMensajes;
        private Panel panelInferior;
    }
}