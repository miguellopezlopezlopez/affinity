namespace FiltringApp
{
    partial class PendingMatchesForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.DataGridView dataGridViewSolicitudes;
        private System.Windows.Forms.Button btnAceptarMatch;
        private System.Windows.Forms.Button btnRechazarMatch;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Button btnRefrescar;
        private System.Windows.Forms.Button btnCerrar;

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
            dataGridViewSolicitudes = new DataGridView();
            btnAceptarMatch = new Button();
            btnRechazarMatch = new Button();
            btnRefrescar = new Button();
            btnCerrar = new Button();
            lblTitulo = new Label();

            ((System.ComponentModel.ISupportInitialize)dataGridViewSolicitudes).BeginInit();
            SuspendLayout();

            // Configuración del formulario
            this.Text = "Solicitudes de Match Pendientes";
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = System.Drawing.Color.White;
            this.Padding = new Padding(20);

            // Título
            lblTitulo.Text = "Solicitudes de Match Pendientes";
            lblTitulo.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            lblTitulo.ForeColor = System.Drawing.Color.FromArgb(33, 33, 33);
            lblTitulo.Dock = DockStyle.Top;
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            lblTitulo.Height = 50;

            // DataGridView
            dataGridViewSolicitudes.Dock = DockStyle.Fill;
            dataGridViewSolicitudes.BackgroundColor = System.Drawing.Color.White;
            dataGridViewSolicitudes.BorderStyle = BorderStyle.None;
            dataGridViewSolicitudes.AllowUserToAddRows = false;
            dataGridViewSolicitudes.AllowUserToDeleteRows = false;
            dataGridViewSolicitudes.AllowUserToResizeRows = false;
            dataGridViewSolicitudes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewSolicitudes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewSolicitudes.MultiSelect = false;
            dataGridViewSolicitudes.ReadOnly = true;
            dataGridViewSolicitudes.RowHeadersVisible = false;
            dataGridViewSolicitudes.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(255, 69, 120);
            dataGridViewSolicitudes.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewSolicitudes.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            dataGridViewSolicitudes.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(245, 245, 245);
            dataGridViewSolicitudes.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(33, 33, 33);
            dataGridViewSolicitudes.ColumnHeadersHeight = 40;
            dataGridViewSolicitudes.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewSolicitudes.EnableHeadersVisualStyles = false;
            dataGridViewSolicitudes.GridColor = System.Drawing.Color.FromArgb(240, 240, 240);
            dataGridViewSolicitudes.Location = new Point(20, 70);
            dataGridViewSolicitudes.Size = new Size(760, 450);

            // Panel de botones
            Panel buttonPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 60,
                Padding = new Padding(0, 10, 0, 10)
            };

            // Botón Aceptar Match
            btnAceptarMatch.Text = "✓ Aceptar Match";
            btnAceptarMatch.Size = new Size(150, 40);
            btnAceptarMatch.Location = new Point(20, 10);
            btnAceptarMatch.FlatStyle = FlatStyle.Flat;
            btnAceptarMatch.BackColor = System.Drawing.Color.FromArgb(76, 175, 80);
            btnAceptarMatch.ForeColor = System.Drawing.Color.White;
            btnAceptarMatch.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            btnAceptarMatch.Cursor = Cursors.Hand;
            btnAceptarMatch.FlatAppearance.BorderSize = 0;
            btnAceptarMatch.Click += new EventHandler(btnAceptarMatch_Click);

            // Botón Rechazar Match
            btnRechazarMatch.Text = "✕ Rechazar Match";
            btnRechazarMatch.Size = new Size(150, 40);
            btnRechazarMatch.Location = new Point(190, 10);
            btnRechazarMatch.FlatStyle = FlatStyle.Flat;
            btnRechazarMatch.BackColor = System.Drawing.Color.FromArgb(244, 67, 54);
            btnRechazarMatch.ForeColor = System.Drawing.Color.White;
            btnRechazarMatch.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            btnRechazarMatch.Cursor = Cursors.Hand;
            btnRechazarMatch.FlatAppearance.BorderSize = 0;
            btnRechazarMatch.Click += new EventHandler(btnRechazarMatch_Click);

            // Botón Refrescar
            btnRefrescar.Text = "↻ Refrescar";
            btnRefrescar.Size = new Size(150, 40);
            btnRefrescar.Location = new Point(360, 10);
            btnRefrescar.FlatStyle = FlatStyle.Flat;
            btnRefrescar.BackColor = System.Drawing.Color.FromArgb(33, 150, 243);
            btnRefrescar.ForeColor = System.Drawing.Color.White;
            btnRefrescar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            btnRefrescar.Cursor = Cursors.Hand;
            btnRefrescar.FlatAppearance.BorderSize = 0;
            btnRefrescar.Click += new EventHandler(btnRefrescar_Click);

            // Botón Cerrar
            btnCerrar.Text = "Cerrar";
            btnCerrar.Size = new Size(150, 40);
            btnCerrar.Location = new Point(530, 10);
            btnCerrar.FlatStyle = FlatStyle.Flat;
            btnCerrar.BackColor = System.Drawing.Color.FromArgb(158, 158, 158);
            btnCerrar.ForeColor = System.Drawing.Color.White;
            btnCerrar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            btnCerrar.Cursor = Cursors.Hand;
            btnCerrar.FlatAppearance.BorderSize = 0;
            btnCerrar.Click += new EventHandler(btnCerrar_Click);

            // Agregar controles al formulario
            buttonPanel.Controls.Add(btnAceptarMatch);
            buttonPanel.Controls.Add(btnRechazarMatch);
            buttonPanel.Controls.Add(btnRefrescar);
            buttonPanel.Controls.Add(btnCerrar);

            this.Controls.Add(buttonPanel);
            this.Controls.Add(dataGridViewSolicitudes);
            this.Controls.Add(lblTitulo);

            ((System.ComponentModel.ISupportInitialize)dataGridViewSolicitudes).EndInit();
            ResumeLayout(false);
        }
    }
}