namespace Milkyfie.Models
{
    public class LedgerFilters
    {
        public int UserID { get; set; }
        public string CreatedOn { get; set; }
    }

    public class UserFilter
    {
       
        public string PhoneNumber { get; set; }
        public string Role { get; set; }
    }
}
