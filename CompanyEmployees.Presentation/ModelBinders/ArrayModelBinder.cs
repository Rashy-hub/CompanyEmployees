using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CompanyEmployees.Presentation.ModelBinders
{
    public sealed class ArrayModelBinder:IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            // Vérifie qu’on veut binder un type “enumerable”
            if (!bindingContext.ModelMetadata.IsEnumerableType)
            {
                            
            bindingContext.Result = ModelBindingResult.Failed();
                return Task.CompletedTask;
            }
            //Récupère la valeur envoyée par l’utilisateur dans l'URL via le binding  context
            var providedValue = bindingContext.ValueProvider.GetValue(bindingContext.ModelName).ToString();
            //Si la valeur est vide → retourne null
            if (string.IsNullOrEmpty(providedValue))
            {
                bindingContext.Result = ModelBindingResult.Success(null);
                return Task.CompletedTask;
            }
            //Récupère le type des éléments du tableau (Reflection)
            var genericType = bindingContext.ModelType.GetTypeInfo().GenericTypeArguments[0];
            //Prend un convertisseur pour ce type
            var converter = TypeDescriptor.GetConverter(genericType);
            //Coupe la string en morceaux et convertit chaque morceau
            var objectArray = providedValue.Split(new[] { "," },StringSplitOptions.RemoveEmptyEntries).Select(x => converter.ConvertFromString(x.Trim())).ToArray();
            // Créé le tableau final du bon type et renvoyez le resultat au controlleur (binding success)
            var guidArray = Array.CreateInstance(genericType, objectArray.Length);
            objectArray.CopyTo(guidArray, 0);
            bindingContext.Model = guidArray;
            bindingContext.Result = ModelBindingResult.Success(bindingContext.Model);
            return Task.CompletedTask;
        }
    }
}
