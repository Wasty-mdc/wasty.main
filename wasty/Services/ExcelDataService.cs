using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wasty.Services
{
    public class ExcelDataService
    {
        public async Task<DataTable> ReadXlsFromPath(string filePath)
        {
            filePath = "C://vs//excel_recogidas_new.xlsx";
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentNullException(nameof(filePath), "Excel file path cannot be null or empty.");
            }

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"Excel file not found at path: {filePath}");
            }

            string connectionString = string.Empty;
            if (filePath.Contains(".xlsx"))
                connectionString = $"Provider=Microsoft.ACE.OLEDB.16.0;Data Source='{filePath}';Extended Properties='Excel 12.0;HDR=YES;'";
            else
                connectionString = $"Provider=Microsoft.ACE.OLEDB.16.0;Data Source='{filePath}';Extended Properties='Excel 8.0 Xml;HDR=YES;Readonly=True'";

            try
            {
                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    connection.Open();

                    // Get the name of the first sheet in the Excel file
                    DataTable schemaTable = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    if (schemaTable == null || schemaTable.Rows.Count == 0)
                    {
                        throw new Exception("Could not retrieve Excel sheet information.");
                    }
                    string sheetName = schemaTable.Rows[0]["TABLE_NAME"].ToString();

                    // Create an OleDbCommand to select all data from the sheet
                    using (OleDbCommand command = new OleDbCommand($"SELECT distinct cif_recogida, nombre_recogida, " +
                        $"direccion_recogida, cp_recogida, poblacion_recogida, provincia_recogida, email_recogida, " +
                        $"telf_recogida, nombre_centro, direccion_centro, cp_centro, poblacion_centro, provincia_centro, " +
                        $"email_centro, telf_centro, nima_codigo FROM [{sheetName}]", connection))
                    {
                        using (OleDbDataAdapter adapter = new OleDbDataAdapter(command))
                        {
                            DataTable dataTable = new DataTable();
                            adapter.Fill(dataTable);
                            return dataTable;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error reading Excel file: {ex.Message}");
            }
        }
    }
}