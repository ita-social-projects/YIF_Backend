using System;

namespace YIF.Core.Data.Entities
{
    public enum IoEStatus
    {
        Default,
        PendingChanges,
        Modified,
        Verified
    }

    public class IoEBuffer : BaseEntity
    {
        public string Name { get; set; }
        public string Abbreviation { get; set; }
        public string Site { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public float Lat { get; set; }
        public float Lon { get; set; }
        public bool IsBanned { get; set; }
        public InstitutionOfEducationType InstitutionOfEducationType { get; set; }
        public DateTime StartOfCampaign { get; set; }
        public DateTime EndOfCampaign { get; set; }
        public bool IsDeleted { get; set; }
        public IoEStatus IoEStatus { get; set; }
        public string Comment { get; set; }
    }
}
