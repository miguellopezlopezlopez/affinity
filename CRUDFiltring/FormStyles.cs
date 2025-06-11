using System.Drawing;
using System.Windows.Forms;

namespace FiltringApp
{
    public static class FormStyles
    {
        // Colores principales
        public static Color PrimaryColor = ColorTranslator.FromHtml("#6c5ce7");
        public static Color SecondaryColor = ColorTranslator.FromHtml("#a29bfe");
        public static Color AccentColor = Color.FromArgb(102, 126, 234); // #667eea
        public static Color TextColor = Color.FromArgb(44, 62, 80); // #2c3e50
        public static Color DarkColor = ColorTranslator.FromHtml("#2d3436");
        public static Color LightColor = ColorTranslator.FromHtml("#f9f9f9");
        public static Color WhiteColor = Color.White;

        // Fuentes
        public static Font TitleFont = new Font("Segoe UI", 16, FontStyle.Regular);
        public static Font MainFont = new Font("Segoe UI", 10, FontStyle.Regular);
        public static Font SubtitleFont = new Font("Segoe UI", 12, FontStyle.Bold);
        public static Font RegularFont = new Font("Segoe UI", 10, FontStyle.Regular);
        public static Font SmallFont = new Font("Segoe UI", 9, FontStyle.Regular);

        // Estilos para botones principales
        public static void ApplyMainButtonStyle(Button button)
        {
            button.BackColor = AccentColor;
            button.ForeColor = Color.White;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Cursor = Cursors.Hand;
        }

        // Estilos para botones secundarios
        public static void ApplySecondaryButtonStyle(Button button)
        {
            button.BackColor = Color.White;
            button.ForeColor = AccentColor;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderColor = AccentColor;
            button.FlatAppearance.BorderSize = 1;
            button.Cursor = Cursors.Hand;
        }

        // Estilos para TextBox
        public static void ApplyTextBoxStyle(TextBox textBox)
        {
            textBox.BorderStyle = BorderStyle.FixedSingle;
            textBox.BackColor = Color.FromArgb(248, 249, 250); // #f8f9fa
        }

        // Estilos para DataGridView
        public static void ApplyDataGridViewStyle(DataGridView dgv)
        {
            dgv.BackgroundColor = WhiteColor;
            dgv.BorderStyle = BorderStyle.None;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgv.EnableHeadersVisualStyles = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.AllowUserToOrderColumns = true;
            dgv.AllowUserToResizeRows = false;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.MultiSelect = false;
            dgv.ReadOnly = true;
            dgv.RowHeadersVisible = false;
            dgv.ShowEditingIcon = false;

            // Estilo del encabezado
            dgv.ColumnHeadersDefaultCellStyle.BackColor = PrimaryColor;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = WhiteColor;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.Padding = new Padding(10, 0, 10, 0);
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgv.ColumnHeadersHeight = 40;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            // Estilo de las celdas
            dgv.DefaultCellStyle.BackColor = WhiteColor;
            dgv.DefaultCellStyle.ForeColor = DarkColor;
            dgv.DefaultCellStyle.Font = RegularFont;
            dgv.DefaultCellStyle.Padding = new Padding(10, 5, 10, 5);
            dgv.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgv.RowTemplate.Height = 35;
            dgv.RowTemplate.DefaultCellStyle.SelectionBackColor = ColorTranslator.FromHtml("#f0f0f0");
            dgv.RowTemplate.DefaultCellStyle.SelectionForeColor = DarkColor;

            // Estilo alternado de filas
            dgv.AlternatingRowsDefaultCellStyle.BackColor = ColorTranslator.FromHtml("#f9f9f9");
            dgv.AlternatingRowsDefaultCellStyle.Font = RegularFont;
            dgv.AlternatingRowsDefaultCellStyle.Padding = new Padding(10, 5, 10, 5);
            dgv.AlternatingRowsDefaultCellStyle.SelectionBackColor = ColorTranslator.FromHtml("#f0f0f0");
            dgv.AlternatingRowsDefaultCellStyle.SelectionForeColor = DarkColor;

            // Eliminar la última fila vacía
            dgv.AllowUserToAddRows = false;
        }

        // Estilos para Labels
        public static void ApplyLabelStyle(Label label, bool isTitle = false)
        {
            if (isTitle)
            {
                label.Font = TitleFont;
                label.ForeColor = TextColor;
            }
            else
            {
                label.Font = MainFont;
                label.ForeColor = TextColor;
            }
        }

        // Estilos para ComboBox
        public static void ApplyComboBoxStyle(ComboBox comboBox)
        {
            comboBox.Font = RegularFont;
            comboBox.FlatStyle = FlatStyle.Flat;
            comboBox.BackColor = WhiteColor;
            comboBox.MinimumSize = new Size(0, 30);
        }

        // Aplicar estilo base al formulario
        public static void ApplyFormStyle(Form form)
        {
            form.BackColor = Color.White;
            form.Font = MainFont;
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MaximizeBox = false;
        }
    }
} 