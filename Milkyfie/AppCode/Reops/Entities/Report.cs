using Milkyfie.Models;

namespace Milkyfie.AppCode.Reops.Entities
{
    public class Report
    {



    }
    public class Ledger
    {
        public int LedgerID { get; set; }
        public int WalletID { get; set; }
        public decimal Amount { get; set; }
        public decimal LastAmount { get; set; }
        public decimal CurrentAmount { get; set; }
        public string TransactionType { get; set; }
        public string CreatedOn { get; set; }
        public string Naration { get; set; }
        public ApplicationUser User { get; set; }
        public OrderSummary Order { get; set; }
    }

    public class FOSCollecionFilter
    {

        public int Srno { get; set; }
        public string UserName { get; set; }
        public string Users { get; set; }
        public decimal Balance { get; set; }
        public decimal FoSBalance { get; set; }
       
    }
}