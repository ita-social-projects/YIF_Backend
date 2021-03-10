﻿using System;
using System.Collections.Generic;
using System.Linq;
using YIF.Core.Domain.ApiModels.RequestApiModels;

namespace YIF_XUnitTests.Integration.YIF_Backend.Controllers.DataAttribute
{
    public static class SuperAdminInputAttribute
    {
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public static UniversityPostApiModel GetCorrectData
        {
            get
            {
                var name = RandomString(10);
                return new UniversityPostApiModel
                {
                    Name = name,
                    Abbreviation = "string",
                    Site = "https://localhost:44388/swagger/index.html",
                    Address = "string",
                    Phone = "380671111111",
                    Email = name + "@example.com",
                    Description = "string",
                    Lat = 0,
                    Lon = 0,
                    ImageApiModel = new ImageApiModel
                    {
                        Photo = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAQAAAAECAYAAACp8Z5+AAAATUlEQVQYV2NsCJsfwMHDPpOBgYHhx5ef6YyVoVNfGllri7GwMjOcOnDpFWNjxIKX6iZyYv///We4de7xK0aQFmYWppm///xlYGJgTAcAcGcbPQnK4IEAAAAASUVORK5CYII="
                    },
                    UniversityAdminEmail = name + "@example.com"
                };
            }
        }
        public static IEnumerable<object[]> GetWrongData
        {
            get
            {
                yield return new object[] {ContentHelper.GetStringContent(new
                {
                    Name = "string",
                    Abbreviation = "string",
                    Site = "string",
                    Address = "string",
                    Phone = "380671111111",
                    Email = "user@example.com",
                    Description = "string",
                    Lat = 0,
                    Lon = 0,
                    ImageApiModel = new ImageApiModel
                    {
                        Photo = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAQAAAAECAYAAACp8Z5+AAAATUlEQVQYV2NsCJsfwMHDPpOBgYHhx5ef6YyVoVNfGllri7GwMjOcOnDpFWNjxIKX6iZyYv///We4de7xK0aQFmYWppm///xlYGJgTAcAcGcbPQnK4IEAAAAASUVORK5CYII="
                    },
                    UniversityAdminEmail = "user@example.com"
                })};
                yield return new object[] {ContentHelper.GetStringContent(new
                {
                    Name = "string",
                    Abbreviation = "string",
                    Site = "https://localhost:44388/swagger/index.html",
                    Address = "string",
                    Phone = "string",
                    Email = "user@example.com",
                    Description = "string",
                    Lat = 0,
                    Lon = 0,
                    ImageApiModel = new ImageApiModel
                    {
                        Photo = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAQAAAAECAYAAACp8Z5+AAAATUlEQVQYV2NsCJsfwMHDPpOBgYHhx5ef6YyVoVNfGllri7GwMjOcOnDpFWNjxIKX6iZyYv///We4de7xK0aQFmYWppm///xlYGJgTAcAcGcbPQnK4IEAAAAASUVORK5CYII="
                    },
                    UniversityAdminEmail = "user@example.com"
                })};
                yield return new object[] {ContentHelper.GetStringContent(new
                {
                    Name = "string",
                    Abbreviation = "string",
                    Site = "https://localhost:44388/swagger/index.html",
                    Address = "string",
                    Phone = "380671111111",
                    Email = "string",
                    Description = "string",
                    Lat = 0,
                    Lon = 0,
                    ImageApiModel = new ImageApiModel
                    {
                        Photo = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAQAAAAECAYAAACp8Z5+AAAATUlEQVQYV2NsCJsfwMHDPpOBgYHhx5ef6YyVoVNfGllri7GwMjOcOnDpFWNjxIKX6iZyYv///We4de7xK0aQFmYWppm///xlYGJgTAcAcGcbPQnK4IEAAAAASUVORK5CYII="
                    },
                    UniversityAdminEmail = "user@example.com"
                })};
                yield return new object[] {ContentHelper.GetStringContent(new
                {
                    Name = "string",
                    Abbreviation = "string",
                    Site = "https://localhost:44388/swagger/index.html",
                    Address = "string",
                    Phone = "380671111111",
                    Email = "user@example.com",
                    Description = "string",
                    Lat = 0,
                    Lon = 0,
                    ImageApiModel = new ImageApiModel
                    {
                        Photo = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAQAAAAECAYAAACp8Z5+AAAATUlEQVQYV2NsCJsfwMHDPpOBgYHhx5ef6YyVoVNfGllri7GwMjOcOnDpFWNjxIKX6iZyYv///We4de7xK0aQFmYWppm///xlYGJgTAcAcGcbPQnK4IEAAAAASUVORK5CYII="
                    },
                    UniversityAdminEmail = "string"
                })};
                yield return new object[] {ContentHelper.GetStringContent(new
                {
                    Name = "string",
                    Abbreviation = "string",
                    Site = "https://localhost:44388/swagger/index.html",
                    Address = "string",
                    Phone = "380671111111",
                    Email = "user@example.com",
                    Description = "string",
                    Lat = 0,
                    Lon = 0,
                    ImageApiModel = new ImageApiModel
                    {
                        Photo = "string"
                    },
                    UniversityAdminEmail = "user@example.com"
                })};
            }
        }
    }
}
