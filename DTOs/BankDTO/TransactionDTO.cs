using System;

namespace BankOfAfricaAPI.DTOs.BankDTO
{
    public class TransactionDTO
    {
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public string SenderAccountNo { get; set; }
        public string ReceiverAccountNo { get; set; }
        public string AmountSent { get; set; }
    }
}
