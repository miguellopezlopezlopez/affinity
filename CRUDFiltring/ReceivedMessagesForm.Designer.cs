namespace FiltringApp
{
    partial class ReceivedMessagesForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.DataGridView dataGridViewMensajes;
        private System.Windows.Forms.Label label1;

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
            this.dataGridViewMensajes = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMensajes)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewMensajes
            // 
            this.dataGridViewMensajes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewMensajes.Location = new System.Drawing.Point(30, 50);
            this.dataGridViewMensajes.Name = "dataGridViewMensajes";
            this.dataGridViewMensajes.RowTemplate.Height = 25;
            this.dataGridViewMensajes.Size = new System.Drawing.Size(740, 300);
            this.dataGridViewMensajes.TabIndex = 0;
            this.dataGridViewMensajes.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewMensajes_CellDoubleClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(30, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(177, 21);
            this.label1.TabIndex = 1;
            this.label1.Text = "Mensajes Recibidos 📩";
            // 
            // ReceivedMessagesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 400);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataGridViewMensajes);
            this.Name = "ReceivedMessagesForm";
            this.Text = "Affinity - Mensajes Recibidos";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMensajes)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
