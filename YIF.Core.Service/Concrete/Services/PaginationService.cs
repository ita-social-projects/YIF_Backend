using SendGrid.Helpers.Errors.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YIF.Core.Domain.ApiModels.ResponseApiModels;
using YIF.Core.Domain.ServiceInterfaces;

namespace YIF.Core.Service.Concrete.Services
{
    public class PaginationService : IPaginationService
    {
        public PageResponseApiModel<T> GetPageFromCollection<T>(IEnumerable<T> list, int pageNumber, int pageSize, string url = null)
        {
            if (pageSize <= 0)
                throw new BadRequestException("Введено неправильний розмір сторінки");

            var count = list.Count();
            var totalPages = (int)Math.Ceiling((double)count / pageSize);

            if (pageNumber <= 0 || pageNumber > totalPages)
                throw new BadRequestException("Введено неправильний номер сторінки");

            var itemsOnPage = list
                .OrderBy(x =>
                {
                    var test = x.GetType().GetProperty("Id").GetValue(x).ToString();
                    if (test != null)
                        return test;
                    return x.GetHashCode().ToString();
                })
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            // Add url for next and prev pages
            var currentUrl = new UriBuilder(url);
            UriBuilder nextPage = null;
            UriBuilder prevPage = null;

            var query = HttpUtility.ParseQueryString(currentUrl.Query);
            query["pageSize"] = pageSize.ToString();

            if (pageNumber + 1 <= totalPages)
            {
                nextPage = new UriBuilder(url);
                query["page"] = (pageNumber + 1).ToString();
                nextPage.Query = query.ToString();
            }

            if (pageNumber - 1 > 0)
            {
                prevPage = new UriBuilder(url);
                query["page"] = (pageNumber - 1).ToString();
                prevPage.Query = query.ToString();
            }

            var result = new PageResponseApiModel<T>
            {
                CurrentPage = pageNumber,
                PageSize = pageSize,
                TotalPages = totalPages,
                ResponseList = itemsOnPage,
                NextPage = nextPage?.ToString(),
                PrevPage = prevPage?.ToString()
            };

            return result;
        }
    }
}
