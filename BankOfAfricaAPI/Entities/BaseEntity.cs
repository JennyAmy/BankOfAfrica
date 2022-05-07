using System;

namespace BankOfAfricaAPI.Entities
{
    public class BaseEntity
    {
        public DateTime? LastUpdatedOn { get; set; }
        public int? LastUpdatedBy { get; set; }
    }
}
