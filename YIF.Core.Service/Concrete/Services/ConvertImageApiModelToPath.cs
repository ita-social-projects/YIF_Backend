using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace YIF.Core.Service.Concrete.Services
{
    static class ConvertImageApiModelToPath
    {
        /// <summary>
        /// To save Base64 photo into server path
        /// </summary>
        /// <param name="base64">Image in string format. With data:image/png;base64</param>        
        /// <param name="path">Path where it shoud locate</param>
        /// <returns>File name</returns>
        public static string FromBase64ToImageFilePath(string base64, string path)
        {
            if (base64.Contains(","))
            {
                base64 = base64.Split(',')[1];
            }

            if (!Directory.Exists(path)) { Directory.CreateDirectory(path); }

            string ext = ".jpg";
            var fileName = Guid.NewGuid().ToString("D") + ext;
            string filePathSave = Path.Combine(path, fileName);

            //Convert Base64 Encoded string to Byte Array.
            byte[] imageBytes = Convert.FromBase64String(base64);
            File.WriteAllBytes(filePathSave, imageBytes);

            return fileName;
        }
    }
}
