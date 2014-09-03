using System.Web.Http;

namespace WebApp.Controllers
{
    public class ConvertController : ApiController
    {
        public string Post([FromBody] string value)
        {
            return Converter.Converter.Convert(value);
        }
    }
}
