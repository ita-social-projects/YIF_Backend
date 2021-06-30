using System.ComponentModel.DataAnnotations.Schema;

namespace YIF.Core.Data.Entities
{
    public class Discipline  :  BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set;}
        
        [ForeignKey("LectortId")]
        public string LectorId { get; set; }
        public Lector Lector { get; set; }

        [ForeignKey("SpecialityId")]
        public string SpecialityId { get; set; }
        public Specialty Speciality { get; set; }
        
    }
}
