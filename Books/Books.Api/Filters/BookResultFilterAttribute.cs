using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Books.Api.Models;
using System.Collections;

namespace Books.Api.Filters
{
    public class BookResultFilterAttribute : ResultFilterAttribute
    {
        public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            var resultFromAction = context.Result as ObjectResult;

            if (resultFromAction?.Value == null
                || resultFromAction.StatusCode < 200
                || resultFromAction.StatusCode >= 300)
            {
                await next();
                return;
            }

            //if (typeof(IEnumerable).IsAssignableFrom(resultFromAction.Value.GetType()))
            //{

            //}

            resultFromAction.Value = Mapper.Map<BookDto>(resultFromAction.Value);

            await next();
        }
    }
}
