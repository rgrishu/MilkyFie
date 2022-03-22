namespace Milkyfie.Models
{
    public class Dashboard
    {
        public int TotalOrderSchedule { get; set; }
        public int TodayOrder { get; set; }
        public int TotalCustomers { get; set; }
        public ApplicationUser User { get; set; }
    }
    public class DashboardApi
    {
        public int TotalOrderSchedule { get; set; }
        public int TodayOrder { get; set; }
        public int TotalCustomers { get; set; }
        public string Balance { get; set; }
    }
}
