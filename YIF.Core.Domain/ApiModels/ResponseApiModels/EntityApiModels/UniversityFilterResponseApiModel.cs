using System;
using System.Collections.Generic;
using System.Text;

namespace YIF.Core.Domain.ApiModels.ResponseApiModels
{
    public class UniversityFilterResponseApiModel
    {
        public string Id { get; set; }
        /// <summary>
        /// Name of university
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Description of university
        /// </summary> 
        public string Description { get; set; }
        /// <summary>
        /// Path of the university image in project directory
        /// </summary>
        public string ImagePath { get; set; }


        public UniversityFilterResponseApiModel(
            string Id = null
            ,string Name = null
            ,string Description = null
            ,string ImagePath = null)
        {
            this.Id = Id;
            this.Name = Name;
            this.Description = Description;
            this.ImagePath = ImagePath;
        }
    }
}
