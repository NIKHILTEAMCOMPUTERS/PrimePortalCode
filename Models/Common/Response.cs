using System.Net;

namespace RMS.Client.Models.Common
{
    public class Response
    {
        public int responseCode { get; set; }
        public string responseMessage { get; set; }
    }
    public class GenericResponse<T> : Response
    {
        public T DataObject { get; set; }

        public static implicit operator GenericResponse<T>(Tuple<HttpStatusCode, string> v)
        {
            throw new NotImplementedException();
        }
    }
}
