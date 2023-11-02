Your question reads **Disable checkbox cell in DataGridView C#** and to do that you can obtain the `DataGridViewCell` using the `columnIndex` and `rowIndex` and set its `ReadOnly` property. You might still want to disable the entire row, but I'll answer the question you asked.

[![screenshot][1]][1]

Suppose your `DataGridView` is bound to a list of `MockDatabaseRecord` records (or a data table, works the same way).

```
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
    .
    .
    .
}
```
___
Then, for example, you could try iterating the recordset and applying some kind of criteria like this a `SetMatch` filter which sets the checkbox to `ReadOnly` if the text contains the string which in this case is "E".

```
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

    // Set background color.
    checkOneCell.Style.BackColor = checkOneCell.ReadOnly ? Color.LightSalmon : Color.LightGreen;
    checkTwoCell.Style.BackColor = checkTwoCell.ReadOnly ? Color.LightSalmon : Color.LightGreen;
}
```

___

**Record class**

```
class MockDatabaseRecord
{
    public bool CheckOne { get; set; }
    public string? TextOne {  get; set; }
    public bool CheckTwo { get; set; }
    public string? TextTwo {  get; set; }
}
```


  [1]: https://i.stack.imgur.com/k8R8A.png