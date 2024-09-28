using App.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace App.Services.Filter
{
    public class NotFoundFilter<T,TId>(IGenericRepository<T,TId> generic) : Attribute, IAsyncActionFilter where T : class where TId : struct
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var idValue = context.ActionArguments.Values.FirstOrDefault();

            if (idValue == null)
            {
                await next();
                return;
            }

            //if(!int.TryParse(idValue.ToString(), out int id))
            //{
            //    await next();
            //    return;
            //}

            if((idValue is not TId  id))
            {
                await next();
                return;
            }

            var hasEntity =await generic.AnyAsync(id);

            if(hasEntity)
            {
                await next();
                return;
            }

            var entityName = typeof(T).Name;

            //action method name

            var actionName = context.ActionDescriptor.RouteValues["action"];

            var result = ServiceResult.Fail($" Data Not Found!{entityName}{actionName}");
            context.Result = new NotFoundObjectResult(result);
            

            
        }
    }
}
