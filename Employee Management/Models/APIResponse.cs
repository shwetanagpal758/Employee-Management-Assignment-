using System.Net;

namespace Employee_Management.Models
{
    public class APIResponse
    {

        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; }

        public List<string> ErrorMessages { get; set; }

        public object Result { get; set; }
    }
}
