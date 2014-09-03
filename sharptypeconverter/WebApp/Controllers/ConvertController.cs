using System.Web.Http;

namespace WebApp.Controllers
{
    public class ConvertController : ApiController
    {
        [AllowCrossSiteJson]
        public string Post([FromBody] string value)
        {
            return Converter.Converter.Convert(value);
        }
    }
}
