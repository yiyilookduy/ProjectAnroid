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
public static class SessionExtensions
{
    public static void SetObjectAsJson(this ISession session, string key, object value)
    {
        session.SetString(key, JsonConvert.SerializeObject(value));
    }

    public static T GetObjectFromJson<T>(this ISession session, string key)
    {
        var value = session.GetString(key);
        return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
    }
    public static void SetCollectionAsJson(this ISession session, string key, object value)
    {
        session.SetString(key, JsonConvert.SerializeObject(value, Formatting.Indented));
    }

    public static T GetCollectionFromJson<T>(this ISession session, string key)
    {
        var value = session.GetString(key);
        return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
    }
}
