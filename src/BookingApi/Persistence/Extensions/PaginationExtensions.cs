using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookingApi.Application.Common.Pagination;
using Microsoft.EntityFrameworkCore;

namespace BookingApi.Persistence.Extensions
{
    // <summary>
    /// Extension methods for IQueryable to support pagination.
    /// </summary>
    public static class PaginationExtensions
    {
        public static async Task<PagedResponse<T>> ToPageResponseAsync<T>
            (this IQueryable<T> query,
            int page,
            int pageSize,
            CancellationToken cancellationToken = default)
        {
            var totalCount = await query.CountAsync(cancellationToken);
            var data = await query.Skip((page - 1) * pageSize)
                                  .Take(pageSize)
                                  .ToListAsync(cancellationToken);

            return new PagedResponse<T>
            {
                Data = data,
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }
        
    }
}