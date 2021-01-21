using SendGrid.Helpers.Errors.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF.Core.Domain.ApiModels.ResponseApiModels;
using YIF.Core.Domain.ServiceInterfaces;

namespace YIF.Core.Service.Concrete.Services
{
    public class PaginationService : IPaginationService
    {
        public PageResponseApiModel<T> GetPageFromCollection<T>(IEnumerable<T> list, PageApiModel pageModel)
        {
            if (pageModel.PageSize <= 0)
                throw new BadRequestException("Введено неправильний розмір сторінки");

            var count = list.Count();
            var totalPages = (int)Math.Ceiling((double)count / pageModel.PageSize);

            if (pageModel.Page <= 0 || pageModel.Page > totalPages)
                throw new BadRequestException("Введено неправильний номер сторінки");

            var itemsOnPage = list
                .OrderBy(x =>
                {
                    var test = x.GetType().GetProperty("Id").GetValue(x).ToString();
                    if (test != null)
                        return test;
                    return x.GetHashCode().ToString();
                })
                .Skip((pageModel.Page - 1) * pageModel.PageSize)
                .Take(pageModel.PageSize);

            // Add url for next and prev pages
            var currentUrl = new UriBuilder(pageModel.Url);
            UriBuilder nextPage = null;
            UriBuilder prevPage = null;

            var query = HttpUtility.ParseQueryString(currentUrl.Query);
            query["pageSize"] = pageModel.PageSize.ToString();

            if (pageModel.Page + 1 <= totalPages)
            {
                nextPage = new UriBuilder(pageModel.Url);
                query["page"] = (pageModel.Page + 1).ToString();
                nextPage.Query = query.ToString();
            }

            if (pageModel.Page - 1 > 0)
            {
                prevPage = new UriBuilder(pageModel.Url);
                query["page"] = (pageModel.Page - 1).ToString();
                prevPage.Query = query.ToString();
            }

            var result = new PageResponseApiModel<T>
            {
                CurrentPage = pageModel.Page,
                PageSize = pageModel.PageSize,
                TotalPages = totalPages,
                ResponseList = itemsOnPage,
                NextPage = nextPage?.ToString(),
                PrevPage = prevPage?.ToString()
            };

            return result;
        }
    }
}
