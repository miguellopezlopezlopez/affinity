namespace FiltringApp
{
    partial class MatchesForm
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
            label1 = new Label();
            ((System.ComponentModel.ISupportInitialize)dataGridViewUsuarios).BeginInit();
            SuspendLayout();
            // 
            // dataGridViewUsuarios
            // 
            dataGridViewUsuarios.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewUsuarios.Location = new Point(76, 12);
            dataGridViewUsuarios.Name = "dataGridViewUsuarios";
            dataGridViewUsuarios.RowTemplate.Height = 25;
            dataGridViewUsuarios.Size = new Size(620, 312);
            dataGridViewUsuarios.TabIndex = 0;
            dataGridViewUsuarios.CellDoubleClick += dataGridViewUsuarios_CellDoubleClick;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(208, 358);
            label1.Name = "label1";
            label1.Size = new Size(341, 15);
            label1.TabIndex = 1;
            label1.Text = "Haga doble clic en el usuario al que le quiera enviar un mensaje";
            // 
            // MatchesForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(label1);
            Controls.Add(dataGridViewUsuarios);
            Name = "MatchesForm";
            Text = "MatchesForm";
            ((System.ComponentModel.ISupportInitialize)dataGridViewUsuarios).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dataGridViewUsuarios;
        private Label label1;
    }
}