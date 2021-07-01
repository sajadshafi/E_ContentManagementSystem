using System;
using System.Collections.Generic;
using System.Text;

namespace Wiser.API.BL.Config
{
    public class Response<T>
    {
        public bool Success { get; set; } = true;
        public T Data { get; set; }
        public string Message { get; set; }
        public int? Count { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }

    public class PagedResponse<T> : Response<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }
        public int FirstPage { get; set; }
        public int LastPage { get; set; }
        public int? NextPage { get; set; }
        public int? PreviousPage { get; set; }

        public PagedResponse(T data, int pageNumber, int pageSize)
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
            this.Data = data;
            this.Success = true;
            this.Errors = new List<string>();
        }
    }
}
