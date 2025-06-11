namespace FiltringApp
{
    partial class LogIn
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            label2 = new Label();
            txtUser = new TextBox();
            txtPwd = new TextBox();
            btnLogIn = new Button();
            btnRegistro = new Button();
            label3 = new Label();
            label4 = new Label();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 16F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(320, 50);
            label1.Name = "label1";
            label1.Size = new Size(138, 30);
            label1.TabIndex = 0;
            label1.Text = "Bienvenido a";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 16F, FontStyle.Regular, GraphicsUnit.Point);
            label2.ForeColor = Color.FromArgb(102, 126, 234);
            label2.Location = new Point(345, 80);
            label2.Name = "label2";
            label2.Size = new Size(81, 30);
            label2.TabIndex = 1;
            label2.Text = "Affinity";
            // 
            // txtUser
            // 
            txtUser.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            txtUser.Location = new Point(251, 171);
            txtUser.Name = "txtUser";
            txtUser.Size = new Size(300, 25);
            txtUser.TabIndex = 2;
            // 
            // txtPwd
            // 
            txtPwd.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            txtPwd.Location = new Point(251, 227);
            txtPwd.Name = "txtPwd";
            txtPwd.PasswordChar = '•';
            txtPwd.Size = new Size(300, 25);
            txtPwd.TabIndex = 3;
            // 
            // btnLogIn
            // 
            btnLogIn.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            btnLogIn.Location = new Point(299, 272);
            btnLogIn.Name = "btnLogIn";
            btnLogIn.Size = new Size(200, 47);
            btnLogIn.TabIndex = 4;
            btnLogIn.Text = "Iniciar Sesión";
            btnLogIn.UseVisualStyleBackColor = true;
            btnLogIn.Click += btnLogIn_Click;
            // 
            // btnRegistro
            // 
            btnRegistro.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            btnRegistro.Location = new Point(299, 332);
            btnRegistro.Name = "btnRegistro";
            btnRegistro.Size = new Size(200, 47);
            btnRegistro.TabIndex = 5;
            btnRegistro.Text = "¿No tienes cuenta? ¡Regístrate!";
            btnRegistro.UseVisualStyleBackColor = true;
            btnRegistro.Click += btnRegistro_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            label3.Location = new Point(251, 151);
            label3.Name = "label3";
            label3.Size = new Size(59, 19);
            label3.TabIndex = 6;
            label3.Text = "Usuario:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            label4.Location = new Point(251, 207);
            label4.Name = "label4";
            label4.Size = new Size(82, 19);
            label4.TabIndex = 7;
            label4.Text = "Contraseña:";
            // 
            // LogIn
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(btnRegistro);
            Controls.Add(btnLogIn);
            Controls.Add(txtPwd);
            Controls.Add(txtUser);
            Controls.Add(label2);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "LogIn";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Affinity - Iniciar Sesión";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private TextBox txtUser;
        private TextBox txtPwd;
        private Button btnLogIn;
        private Button btnRegistro;
        private Label label3;
        private Label label4;
    }
}