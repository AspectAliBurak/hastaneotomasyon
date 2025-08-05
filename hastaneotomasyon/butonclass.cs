using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hastaneotomasyon
{
    internal class ButonClass
    {
        public static void Yenile(DataGridView dgv, SqlDataAdapter da, DataTable dt)
        {
            dt.Clear();
            da.Fill(dt);
            dgv.DataSource = dt;
        }
    }

}
