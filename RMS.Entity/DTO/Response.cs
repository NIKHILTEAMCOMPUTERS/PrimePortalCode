using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Entity.DTO
{
    //public  class Response
    //{
    //    public int responseCode { get; set; }
    //    public string responseMessage { get; set; }
    //    public object? data { get; set; }
    //}
    //public class GenericResponse<T> : Response
    //{
    //    public T DataObject { get; set; }
    //}
    public class Response
    {
        public int responseCode { get; set; }
        public string responseMessage { get; set; }
        public object? data { get; set; }
        //public object? AdditionalData { get; set; } // New property to hold additional data
    }

    public class GenericResponse<T> : Response
    {
        public T DataObject { get; set; }
    }

}
