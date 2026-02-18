using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Blog.Extentions
{
    public static class MosdelStateExtension
    {
        public static List<string> GetErrors(this ModelStateDictionary modelState) 
        {
            var result = new List<string>();

            foreach(var item in modelState.Values) 
            {
                //result.AddRange(item.Errors.Select(error => error.ErrorMessage));
                foreach (var erro in item.Errors) 
                {
                    result.Add(erro.ErrorMessage);
                }
            }
            
            return result;
        }
    }
}
