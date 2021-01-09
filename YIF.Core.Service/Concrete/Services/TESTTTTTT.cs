using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;

namespace YIF.Core.Service.Concrete.Services
{

    internal class TestRecaptcha
    {
        //private readonly IConfiguration _configuration;
        //internal TestRecaptcha(IConfiguration configuration)
        //{
        //    _configuration = configuration;
        //}
        internal static bool TestGetRecaptcha()
        {
            HttpClient httpClient = new HttpClient();

            var res = httpClient.GetAsync($"https://www.google.com/recaptcha/api.js?render=6Le3gRkaAAAAADJIzK5jv3HegJ7VzkuS0XiBa-mK").Result;

            if (res.StatusCode != HttpStatusCode.OK)
                return false;

            string JSONres = res.Content.ReadAsStringAsync().Result;
            dynamic JSONdata = JObject.Parse(JSONres);

            if (JSONdata.success != "true")
                return false;

            return true;
        }
    }
}
