using Microsoft.AspNetCore.Http;

namespace NewsPlus.Data.ViewModel
{
    public class CreateNewsImageViewModel
    {
        public IFormFile? UrlImage { get; set; }
    }
}
