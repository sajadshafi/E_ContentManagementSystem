using System;
using System.Collections.Generic;
using System.Text;
using Wiser.API.BL.Config;
using Wiser_WEB_API.Filters;

namespace Wiser.API.BL.Helpers
{
    public class PaginationHelper
    {
        public static PagedResponse<T> CreatePagedReponse<T>(T pagedData, PaginationFilter validFilter, int totalRecords)
        {
            var response = new PagedResponse<T>(pagedData, validFilter.PageNumber, validFilter.PageSize);
            var totalPages = ((double)totalRecords / (double)validFilter.PageSize);
            response.TotalPages = Convert.ToInt32(Math.Ceiling(totalPages));
            response.FirstPage = 1;
            response.LastPage = response.TotalPages;
            response.TotalRecords = totalRecords;
            if (validFilter.PageNumber >= 1 && validFilter.PageNumber < response.LastPage)
                response.NextPage = validFilter.PageNumber + 1;
            else
                response.NextPage = null;

            if (validFilter.PageNumber - 1 >= 1 && validFilter.PageNumber <= response.LastPage)
                response.PreviousPage = validFilter.PageNumber - 1;
            else
                response.PreviousPage = null;

            return response;
        }
    }
}
