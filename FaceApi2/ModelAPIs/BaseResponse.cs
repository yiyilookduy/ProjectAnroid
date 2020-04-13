using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FaceApi2.ModelAPIs
{
    public class BaseResponse
    {
        [JsonProperty("Data")]
        public Object Data { get; set; }
        [JsonProperty("Message")]
        public string Message { get; set; }
        [JsonProperty("Success")]
        public bool Success { get; set; }
        public BaseResponse()
        {

        }
        public BaseResponse(Object data, string message, bool success)
        {
            this.Data = data;
            this.Message = message;
            this.Success = success;
        }
    }
}
public class BaseResponse<T>
{
    [JsonProperty("Data")]
    public T Data { get; set; }
    [JsonProperty("Message")]
    public string Message { get; set; }
    [JsonProperty("Success")]
    public bool Success { get; set; }
    public BaseResponse()
    {

    }
    public BaseResponse(T data, string message, bool success)
    {
        this.Data = data;
        this.Message = message;
        this.Success = success;
    }
}

