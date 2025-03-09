namespace FiltringApp
{
    partial class ProfileForm
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
            components = new System.ComponentModel.Container();
            txtBiografia = new TextBox();
            txtIntereses = new TextBox();
            txtPreferencias = new TextBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            btnActualizar = new Button();
            contextMenuStrip1 = new ContextMenuStrip(components);
            menúUsuarioToolStripMenuItem = new ToolStripMenuItem();
            cerrarSesiónToolStripMenuItem = new ToolStripMenuItem();
            menuStrip1 = new MenuStrip();
            contextMenuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // txtBiografia
            // 
            txtBiografia.Location = new Point(187, 35);
            txtBiografia.Multiline = true;
            txtBiografia.Name = "txtBiografia";
            txtBiografia.Size = new Size(266, 124);
            txtBiografia.TabIndex = 0;
            // 
            // txtIntereses
            // 
            txtIntereses.Location = new Point(187, 165);
            txtIntereses.Multiline = true;
            txtIntereses.Name = "txtIntereses";
            txtIntereses.Size = new Size(266, 124);
            txtIntereses.TabIndex = 1;
            // 
            // txtPreferencias
            // 
            txtPreferencias.Location = new Point(187, 295);
            txtPreferencias.Multiline = true;
            txtPreferencias.Name = "txtPreferencias";
            txtPreferencias.Size = new Size(266, 124);
            txtPreferencias.TabIndex = 2;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(69, 88);
            label1.Name = "label1";
            label1.Size = new Size(54, 15);
            label1.TabIndex = 3;
            label1.Text = "Biografía";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(69, 218);
            label2.Name = "label2";
            label2.Size = new Size(53, 15);
            label2.TabIndex = 4;
            label2.Text = "Intereses";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(69, 348);
            label3.Name = "label3";
            label3.Size = new Size(71, 15);
            label3.TabIndex = 5;
            label3.Text = "Preferencias";
            // 
            // btnActualizar
            // 
            btnActualizar.Location = new Point(568, 198);
            btnActualizar.Name = "btnActualizar";
            btnActualizar.Size = new Size(142, 54);
            btnActualizar.TabIndex = 6;
            btnActualizar.Text = "Actualizar Perfil";
            btnActualizar.UseVisualStyleBackColor = true;
            btnActualizar.Click += btnActualizar_Click;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { menúUsuarioToolStripMenuItem, cerrarSesiónToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(149, 48);
            // 
            // menúUsuarioToolStripMenuItem
            // 
            menúUsuarioToolStripMenuItem.Name = "menúUsuarioToolStripMenuItem";
            menúUsuarioToolStripMenuItem.Size = new Size(148, 22);
            menúUsuarioToolStripMenuItem.Text = "Menú Usuario";
            // 
            // cerrarSesiónToolStripMenuItem
            // 
            cerrarSesiónToolStripMenuItem.Name = "cerrarSesiónToolStripMenuItem";
            cerrarSesiónToolStripMenuItem.Size = new Size(148, 22);
            cerrarSesiónToolStripMenuItem.Text = "Cerrar Sesión";
            // 
            // menuStrip1
            // 
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(800, 24);
            menuStrip1.TabIndex = 8;
            menuStrip1.Text = "menuStrip1";
            // 
            // ProfileForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(menuStrip1);
            Controls.Add(btnActualizar);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(txtPreferencias);
            Controls.Add(txtIntereses);
            Controls.Add(txtBiografia);
            MainMenuStrip = menuStrip1;
            Name = "ProfileForm";
            Text = "ProfileForm";
            contextMenuStrip1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtBiografia;
        private TextBox txtIntereses;
        private TextBox txtPreferencias;
        private Label label1;
        private Label label2;
        private Label label3;
        private Button btnActualizar;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem menúUsuarioToolStripMenuItem;
        private ToolStripMenuItem cerrarSesiónToolStripMenuItem;
        private MenuStrip menuStrip1;
    }
}