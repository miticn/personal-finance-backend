using Finance.Models;
using Transaction.Models;

namespace Transaction.Database.Entities
{
    public class TransactionSplitEntity
    {
        public string TransactionId { get; set; }
        public string catcode { get; set; }
        public double amount { get; set; }
        public virtual TransactionEntity Transaction { get; set; }
        public virtual CategoryEntity Category { get; set; }
    }
}