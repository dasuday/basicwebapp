using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.Data;

namespace MyCoreApp.Pages
{
    public class IndexModel : PageModel
    {
        /* private readonly ILogger<IndexModel> _logger;

         public IndexModel(ILogger<IndexModel> logger)
         {
             _logger = logger;
         }*/

        public string sampledata;

        public void OnGet()
        {
            string connectionString = "Data Source = (localdb)\\ProjectModels; Initial Catalog = baseline; Integrated Security = True; Connect Timeout = 30; Encrypt = False; TrustServerCertificate = False; ApplicationIntent = ReadWrite; MultiSubnetFailover = False";
            SqlConnection connection = new SqlConnection(connectionString);

            connection.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM customer", connection);

            SqlDataReader rdr = cmd.ExecuteReader();

            DataTable dt = new DataTable();
            dt.Load(rdr);

            sampledata = print_results(dt);


            connection.Close();
        }

        string print_results(DataTable data)
        {
            String sdata = "";
            //Console.WriteLine();
            Dictionary<string, int> colWidths = new Dictionary<string, int>();

            foreach (DataColumn col in data.Columns)
            {
                sdata = sdata + col.ColumnName + " ";
                var maxLabelSize = data.Rows.OfType<DataRow>()
                        .Select(m => (m.Field<object>(col.ColumnName)?.ToString() ?? "").Length)
                        .OrderByDescending(m => m).FirstOrDefault();

                colWidths.Add(col.ColumnName, maxLabelSize);
                for (int i = 0; i < maxLabelSize - col.ColumnName.Length + 14; i++) Console.Write(" ");
            }

            Console.WriteLine();
            sdata = sdata + "\n";

            foreach (DataRow dataRow in data.Rows)
            {
                for (int j = 0; j < dataRow.ItemArray.Length; j++)
                {
                    sdata = sdata + dataRow.ItemArray[j].ToString() + " ";
                    Console.Write(dataRow.ItemArray[j]);
                    for (int i = 0; i < colWidths[data.Columns[j].ColumnName] - dataRow.ItemArray[j].ToString().Length + 14; i++) Console.Write(" ");
                }
                sdata = sdata + "\n";

                Console.WriteLine();
            }

            return sdata;
        }
    }

    
}