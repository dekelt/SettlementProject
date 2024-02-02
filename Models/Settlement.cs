using SettlementProject.Data;
using System.Data;
using System.Data.SqlClient;

namespace SettlementProject.Models
{
    public class Settlement
    {

        public int Id { get; set; }
        public string SettlementName { get; set; }

        public Settlement()
        {

        }


    }
}
