using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections;

namespace CompanyEmployees.Presentation.ActionFilters
{
    public class MaxCollectionSizeAttribute : ActionFilterAttribute
    {
        private readonly int _maxSize;
        private readonly Type _expectedElementType;

        public MaxCollectionSizeAttribute(Type expectedElementType, int maxSize)
        {
            _expectedElementType = expectedElementType;
            _maxSize = maxSize;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // On cherche une collec dont les éléments sont du type attendu
            var collection = context.ActionArguments.Values
                .Where(v => v is IEnumerable)
                .Cast<IEnumerable>()
                .FirstOrDefault(col =>
                {
                    var elementType = col.GetType().GetGenericArguments().FirstOrDefault();
                    return elementType == _expectedElementType;
                });

            if (collection == null)
                return;

            
            int count = collection.Cast<object>().Count();

            if (count > _maxSize)
            {
                context.Result = new BadRequestObjectResult(
                    $"The collection of '{_expectedElementType.Name}' cannot exceed {_maxSize} items."
                );
            }
        }
    }
}
