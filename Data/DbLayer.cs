using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml;
using System.ComponentModel;
using System.Data;
using System.Threading.Tasks;
using OfficeOpenXml;
using System.IO;

namespace BniSittingManager.Data
{
    public class DbLayer : IDbLayer
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public DbLayer(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }
        public async Task<int> ExecuteSPAsyncgenerate(string spName, SqlParameter[] parameters)
        {
            using SqlConnection con = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand(spName, con);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 300;

            if (parameters != null)
                cmd.Parameters.AddRange(parameters);

            await con.OpenAsync();

            return await cmd.ExecuteNonQueryAsync();
        }

        public async Task<DataTable> ExecuteSPAsync(string spName, SqlParameter[] parameters)
        {
            using SqlConnection con = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand(spName, con);

            cmd.CommandType = CommandType.StoredProcedure;

            if (parameters != null)
                cmd.Parameters.AddRange(parameters);

            await con.OpenAsync();

            using SqlDataReader reader = await cmd.ExecuteReaderAsync();

            DataTable dt = new DataTable();
            dt.Load(reader);

            return dt;
        }

        public async Task<object> ExecuteScalarAsync(string query, SqlParameter[] parameters = null)
        {
            using SqlConnection con = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand(query, con);

            if (parameters != null)
                cmd.Parameters.AddRange(parameters);

            await con.OpenAsync();
            return await cmd.ExecuteScalarAsync();
        }

        public async Task<DataSet> ExecuteSPWithMultipleResultsAsync(string spName, SqlParameter[] parameters = null)
        {
            using SqlConnection con = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand(spName, con)
            {
                CommandType = CommandType.StoredProcedure
            };

            if (parameters != null)
                cmd.Parameters.AddRange(parameters);

            await con.OpenAsync();

            using SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            return ds;
        }
        public byte[] ExportToExcel(DataTable dt, string sheetName)
        {
            // EPPlus 8+ license setup
            ExcelPackage.License.SetNonCommercialOrganization("BniSittingManager");

            using var package = new ExcelPackage();
            var ws = package.Workbook.Worksheets.Add(sheetName);

            // headers
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                ws.Cells[1, i + 1].Value = dt.Columns[i].ColumnName;
            }

            // data
            for (int r = 0; r < dt.Rows.Count; r++)
            {
                for (int c = 0; c < dt.Columns.Count; c++)
                {
                    ws.Cells[r + 2, c + 1].Value = dt.Rows[r][c];
                }
            }

            return package.GetAsByteArray();
        }



    }
}
