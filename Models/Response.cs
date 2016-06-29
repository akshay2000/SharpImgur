using System;

namespace SharpImgur.Models
{
    public class Response<T> where T : new()
    {
        public Exception Error { get; set; }
        public bool IsError { get; set; }
        public string Message { get; set; }
        public T Content { get; set; } = new T();
    }
}
