namespace FiltringApp
{
    partial class UsersForm
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
            dataGridViewUsuarios = new DataGridView();
            btnRefrescar = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridViewUsuarios).BeginInit();
            SuspendLayout();

            // Configuración del formulario
            FormStyles.ApplyFormStyle(this);
            this.ClientSize = new Size(1000, 600);
            this.Padding = new Padding(20);

            // Título
            Label titleLabel = new Label
            {
                Text = "Usuarios",
                Dock = DockStyle.Top,
                TextAlign = ContentAlignment.MiddleCenter,
                Height = 40
            };
            FormStyles.ApplyLabelStyle(titleLabel, true);

            // DataGridView
            FormStyles.ApplyDataGridViewStyle(dataGridViewUsuarios);
            dataGridViewUsuarios.Dock = DockStyle.Fill;
            dataGridViewUsuarios.Margin = new Padding(0, 20, 0, 20);
            dataGridViewUsuarios.CellDoubleClick += dataGridViewUsuarios_CellDoubleClick;

            // Panel de botones
            Panel buttonPanel = new Panel
            {
                Height = 60,
                Dock = DockStyle.Bottom,
                Padding = new Padding(0, 10, 0, 10)
            };

            // Botón refrescar
            FormStyles.ApplyMainButtonStyle(btnRefrescar);
            btnRefrescar.Text = "Refrescar";
            btnRefrescar.Width = 150;
            btnRefrescar.Location = new Point(buttonPanel.Width / 2 - 75, 10);

            // Agregar controles
            buttonPanel.Controls.Add(btnRefrescar);

            Controls.Add(buttonPanel);
            Controls.Add(dataGridViewUsuarios);
            Controls.Add(titleLabel);

            ((System.ComponentModel.ISupportInitialize)dataGridViewUsuarios).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dataGridViewUsuarios;
        private Button btnRefrescar;
    }
}