using System;
using System.Collections.Generic;
using System.Text;

namespace YIF.Core.Domain.ApiModels.ResponseApiModels
{
    public class UniversitiesPageResponseApiModel
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public string NextPage { get; set; }
        public string PrevPage { get; set; }
        public IEnumerable<UniversityResponseApiModel> Universities { get; set; }
    }
}
