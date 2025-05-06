using System.Data;
using System.Text.Encodings.Web;
using System.Text.Json;

public static class DataTableExtensions
{
    public static string ToJson(this DataTable dataTable)
    {
        if (dataTable == null)
        {
            return "null";
        }

        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        return JsonSerializer.Serialize(dataTable.Rows.Cast<DataRow>().Select(row =>
        {
            var rowData = new System.Dynamic.ExpandoObject() as IDictionary<string, object>;
            foreach (DataColumn column in dataTable.Columns)
            {
                rowData.Add(column.ColumnName, row[column] == DBNull.Value ? "" : row[column]);
            }
            return rowData;
        }).ToList(), options);
    }
}