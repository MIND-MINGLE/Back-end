using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Response.Dashboard
{
    public class DashboardStatsResponse
    {
        public decimal TotalRevenue { get; set; }
        public int TotalPatients { get; set; }
        public int TotalTherapists { get; set; }
        public int TotalAppointments { get; set; }
        public List<RevenueByMonthResponse> RevenueByMonth { get; set; } = new List<RevenueByMonthResponse>();
    }

    public class RevenueByMonthResponse
    {
        public string Month { get; set; }
        public decimal Revenue { get; set; }
    }
}
