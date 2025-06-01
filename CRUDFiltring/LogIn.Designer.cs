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
            label1.Location = new Point(320, 93);
            label1.Name = "label1";
            label1.Size = new Size(78, 15);
            label1.TabIndex = 0;
            label1.Text = "Bienvenido a ";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(336, 131);
            label2.Name = "label2";
            label2.Size = new Size(46, 15);
            label2.TabIndex = 1;
            label2.Text = "Affinity";
            label2.Click += label2_Click;
            // 
            // txtUser
            // 
            txtUser.Location = new Point(251, 171);
            txtUser.Name = "txtUser";
            txtUser.Size = new Size(228, 23);
            txtUser.TabIndex = 2;
            // 
            // txtPwd
            // 
            txtPwd.Location = new Point(251, 227);
            txtPwd.Name = "txtPwd";
            txtPwd.PasswordChar = '*';
            txtPwd.Size = new Size(228, 23);
            txtPwd.TabIndex = 3;
            // 
            // btnLogIn
            // 
            btnLogIn.Location = new Point(299, 272);
            btnLogIn.Name = "btnLogIn";
            btnLogIn.Size = new Size(129, 47);
            btnLogIn.TabIndex = 4;
            btnLogIn.Text = "Iniciar Sesión";
            btnLogIn.UseVisualStyleBackColor = true;
            btnLogIn.Click += btnLogIn_Click;
            // 
            // btnRegistro
            // 
            btnRegistro.Location = new Point(549, 272);
            btnRegistro.Name = "btnRegistro";
            btnRegistro.Size = new Size(129, 47);
            btnRegistro.TabIndex = 5;
            btnRegistro.Text = "¿No tiene cuenta? ¡Regístrese!";
            btnRegistro.UseVisualStyleBackColor = true;
            btnRegistro.Click += btnRegistro_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(177, 174);
            label3.Name = "label3";
            label3.Size = new Size(50, 15);
            label3.TabIndex = 6;
            label3.Text = "Usuario:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(177, 230);
            label4.Name = "label4";
            label4.Size = new Size(70, 15);
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
            Name = "LogIn";
            Text = "Form1";
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