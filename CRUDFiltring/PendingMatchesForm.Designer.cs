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
            this.lblTitulo = new System.Windows.Forms.Label();
            this.dataGridViewSolicitudes = new System.Windows.Forms.DataGridView();
            this.btnAceptarMatch = new System.Windows.Forms.Button();
            this.btnRechazarMatch = new System.Windows.Forms.Button();
            this.btnRefrescar = new System.Windows.Forms.Button();
            this.btnCerrar = new System.Windows.Forms.Button();

            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSolicitudes)).BeginInit();
            this.SuspendLayout();

            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblTitulo.Location = new System.Drawing.Point(120, 20);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(250, 25);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Solicitudes de Match Pendientes";

            // 
            // dataGridViewSolicitudes
            // 
            this.dataGridViewSolicitudes.AllowUserToAddRows = false;
            this.dataGridViewSolicitudes.AllowUserToDeleteRows = false;
            this.dataGridViewSolicitudes.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewSolicitudes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewSolicitudes.Location = new System.Drawing.Point(20, 60);
            this.dataGridViewSolicitudes.Name = "dataGridViewSolicitudes";
            this.dataGridViewSolicitudes.ReadOnly = true;
            this.dataGridViewSolicitudes.RowTemplate.Height = 25;
            this.dataGridViewSolicitudes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewSolicitudes.Size = new System.Drawing.Size(440, 200);
            this.dataGridViewSolicitudes.TabIndex = 1;

            // 
            // btnAceptarMatch
            // 
            this.btnAceptarMatch.Location = new System.Drawing.Point(20, 280);
            this.btnAceptarMatch.Name = "btnAceptarMatch";
            this.btnAceptarMatch.Size = new System.Drawing.Size(140, 40);
            this.btnAceptarMatch.TabIndex = 2;
            this.btnAceptarMatch.Text = "Aceptar Match";
            this.btnAceptarMatch.UseVisualStyleBackColor = true;
            this.btnAceptarMatch.Click += new System.EventHandler(this.btnAceptarMatch_Click);

            // 
            // btnRechazarMatch
            // 
            this.btnRechazarMatch.Location = new System.Drawing.Point(170, 280);
            this.btnRechazarMatch.Name = "btnRechazarMatch";
            this.btnRechazarMatch.Size = new System.Drawing.Size(140, 40);
            this.btnRechazarMatch.TabIndex = 3;
            this.btnRechazarMatch.Text = "Rechazar Match";
            this.btnRechazarMatch.UseVisualStyleBackColor = true;
            this.btnRechazarMatch.Click += new System.EventHandler(this.btnRechazarMatch_Click);

            // 
            // btnRefrescar
            // 
            this.btnRefrescar.Location = new System.Drawing.Point(320, 280);
            this.btnRefrescar.Name = "btnRefrescar";
            this.btnRefrescar.Size = new System.Drawing.Size(140, 40);
            this.btnRefrescar.TabIndex = 4;
            this.btnRefrescar.Text = "Refrescar";
            this.btnRefrescar.UseVisualStyleBackColor = true;
            this.btnRefrescar.Click += new System.EventHandler(this.btnRefrescar_Click);

            // 
            // btnCerrar
            // 
            this.btnCerrar.Location = new System.Drawing.Point(170, 330);
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.Size = new System.Drawing.Size(140, 30);
            this.btnCerrar.TabIndex = 5;
            this.btnCerrar.Text = "Cerrar";
            this.btnCerrar.UseVisualStyleBackColor = true;
            this.btnCerrar.Click += new System.EventHandler(this.btnCerrar_Click);

            // 
            // PendingMatchesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(480, 380);
            this.Controls.Add(this.btnCerrar);
            this.Controls.Add(this.btnRefrescar);
            this.Controls.Add(this.btnRechazarMatch);
            this.Controls.Add(this.btnAceptarMatch);
            this.Controls.Add(this.dataGridViewSolicitudes);
            this.Controls.Add(this.lblTitulo);
            this.Name = "PendingMatchesForm";
            this.Text = "Solicitudes de Match";
            this.Load += new System.EventHandler(this.PendingMatchesForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSolicitudes)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}