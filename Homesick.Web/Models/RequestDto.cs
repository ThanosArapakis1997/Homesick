using Homesick.Web.Utility;
using static Homesick.Web.Utility.SD;

namespace Homesick.Web.Models
{
    public class RequestDto
    {
        public ApiType ApiType { get; set; } = ApiType.GET;
        public string Url { get; set; }
        public object Data { get; set; }
        public string AccessToken { get; set; }
    }
}
