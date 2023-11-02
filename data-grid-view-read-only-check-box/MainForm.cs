using System.ComponentModel;

namespace data_grid_view_read_only_combo_box
{
    public partial class MainForm : Form
    {
        public MainForm() => InitializeComponent();
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            dataGridView.DataSource = Recordset;
            dataGridView.RowHeadersVisible = false;
            dataGridView.AllowUserToAddRows = false;
            foreach (DataGridViewColumn col in dataGridView.Columns)
            {
                if(col is DataGridViewCheckBoxColumn checkBoxColumn)
                {
                    checkBoxColumn.Width = 50;
                    checkBoxColumn.HeaderText = string.Empty;
                }
                else
                {
                    col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
            }
            Recordset.Add(new MockDatabaseRecord { TextOne = "A B C", TextTwo = "D E F" });
            Recordset.Add(new MockDatabaseRecord { TextOne = "D E F", TextTwo = "G H I" });
            Recordset.Add(new MockDatabaseRecord { TextOne = "A B C", TextTwo = "G H I" });

            foreach (var record in Recordset)
            {
                SetMatch(record, "E");
            }
            BeginInvoke(()=> dataGridView.CurrentCell = null);
        }
        BindingList<MockDatabaseRecord> Recordset { get; } = new BindingList<MockDatabaseRecord>();
        void SetMatch(MockDatabaseRecord record, string match)
        {
            var rowIndex = Recordset.IndexOf(record);
            var checkOneCell = dataGridView[
                    columnIndex: dataGridView.Columns[nameof(MockDatabaseRecord.CheckOne)].Index,
                    rowIndex: rowIndex];

            var checkTwoCell = dataGridView[
                    columnIndex: dataGridView.Columns[nameof(MockDatabaseRecord.CheckTwo)].Index,
                    rowIndex: rowIndex];

            checkOneCell.ReadOnly = (record.TextOne ?? string.Empty).Contains(match, StringComparison.OrdinalIgnoreCase);
            checkTwoCell.ReadOnly = (record.TextTwo ?? string.Empty).Contains(match, StringComparison.OrdinalIgnoreCase);


            checkOneCell.Style.BackColor = checkOneCell.ReadOnly ? Color.LightSalmon : Color.LightGreen;
            checkTwoCell.Style.BackColor = checkTwoCell.ReadOnly ? Color.LightSalmon : Color.LightGreen;
        }
    }
}

class MockDatabaseRecord
{
    public bool CheckOne { get; set; }
    public string? TextOne {  get; set; }
    public bool CheckTwo { get; set; }
    public string? TextTwo {  get; set; }
}