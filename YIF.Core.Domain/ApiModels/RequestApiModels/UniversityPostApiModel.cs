using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace YIF.Core.Domain.ApiModels.RequestApiModels
{
    public class UniversityPostApiModel
    {
        [BindRequired]
        public string Name { get; set; }
        [BindRequired]
        public string Abbreviation { get; set; }
        [BindRequired]
        [Url]
        public string Site { get; set; }
        [BindRequired]
        public string Address { get; set; }
        [BindRequired]
        [Phone]
        public string Phone { get; set; }
        [BindRequired]
        [EmailAddress]
        public string Email { get; set; }
        [BindRequired]
        public string Description { get; set; }
        [BindRequired]
        public float Lat { get; set; }
        [BindRequired]
        public float Lon { get; set; }
        [BindRequired]
        public ImageApiModel ImageApiModel { get; set; }
        [BindRequired]
        [EmailAddress]
        public string UniversityAdminEmail { get; set; }
    }
}
