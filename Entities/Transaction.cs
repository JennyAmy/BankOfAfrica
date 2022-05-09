using System;

namespace BankOfAfricaAPI.Entities
{
    public class Transaction
    {
        public int TransactionId { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public string SenderAccountNo { get; set; }
        public string ReceiverAccountNo { get; set; }
        public decimal AmountSent { get; set; }
        public DateTime TransactionDate { get; set; } = DateTime.Now;
    }
}
