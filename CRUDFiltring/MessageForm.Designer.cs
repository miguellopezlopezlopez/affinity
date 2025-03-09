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
            SuspendLayout();
            // 
            // lblReceptor
            // 
            lblReceptor.AutoSize = true;
            lblReceptor.Location = new Point(101, 143);
            lblReceptor.Name = "lblReceptor";
            lblReceptor.Size = new Size(38, 15);
            lblReceptor.TabIndex = 0;
            lblReceptor.Text = "label1";
            // 
            // txtMensaje
            // 
            txtMensaje.Location = new Point(284, 31);
            txtMensaje.Multiline = true;
            txtMensaje.Name = "txtMensaje";
            txtMensaje.Size = new Size(430, 216);
            txtMensaje.TabIndex = 1;
            // 
            // btnEnviar
            // 
            btnEnviar.Location = new Point(363, 288);
            btnEnviar.Name = "btnEnviar";
            btnEnviar.Size = new Size(222, 45);
            btnEnviar.TabIndex = 2;
            btnEnviar.Text = "Enviar Mensaje";
            btnEnviar.UseVisualStyleBackColor = true;
            btnEnviar.Click += btnEnviar_Click;
            // 
            // MessageForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnEnviar);
            Controls.Add(txtMensaje);
            Controls.Add(lblReceptor);
            Name = "MessageForm";
            Text = "MessageForm";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblReceptor;
        private TextBox txtMensaje;
        private Button btnEnviar;
    }
}