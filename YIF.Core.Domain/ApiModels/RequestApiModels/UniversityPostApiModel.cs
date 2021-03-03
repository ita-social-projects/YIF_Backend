namespace YIF.Core.Domain.ApiModels.RequestApiModels
{
    public class UniversityPostApiModel
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
    }
}
