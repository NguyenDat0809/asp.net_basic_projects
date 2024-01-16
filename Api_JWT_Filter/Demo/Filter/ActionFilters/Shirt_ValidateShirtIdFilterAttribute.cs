using Demo.Models.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Demo.Filter.ActionFilters
{
    public class Shirt_ValidateShirtIdFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            var shirtId = context.ActionArguments["id"] as int?;
            if (shirtId.HasValue)
            {
                if (shirtId.Value <= 0)
                {
                    context.ModelState.AddModelError("ShirtId", "ShirtId is invalid");
                    //context.Result = new BadRequestObjectResult(context.ModelState);
                    //code như trên thì sẽ trả về 1 dữ liệu

                    //FIX -> thêm ValidationProblemDetails
                    var problemDetail = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status400BadRequest
                    };
                    context.Result = new BadRequestObjectResult(problemDetail);
                }
                else if (!ShirtRepository.ShirtExist(shirtId.Value))
                {
                    context.ModelState.AddModelError("ShirtId", "ShirtId no exists");
                    //context.Result = new NotFoundObjectResult(context.ModelState);
                    //nhưng code như này thì lại trả về 2 dữ liệu, gồm id đã tìm kiếm dc trả về là gì và model State của ShirtId


                    //FIX -> thêm ValidationProblemDetails
                    var problemDetail = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status404NotFound
                    };
                    context.Result = new BadRequestObjectResult(problemDetail);
                }
            }
        }
    }
}
