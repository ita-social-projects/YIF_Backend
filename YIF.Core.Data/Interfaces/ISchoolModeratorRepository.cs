using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YIF.Core.Data.Entities;

namespace YIF.Core.Data.Interfaces
{
    public interface ISchoolModeratorRepository<K> : IDisposable
        where K : class
    {
        Task<string> AddSchoolModerator(SchoolModerator schoolModerator);
    }
}
