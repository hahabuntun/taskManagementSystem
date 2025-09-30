using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Vkr.Domain.Enums.Web;

namespace Vkr.API.Utils;

public static class Response
{
    private static readonly JsonSerializerSettings Settings = new()
    {
        NullValueHandling = NullValueHandling.Ignore,
        ObjectCreationHandling = ObjectCreationHandling.Replace,
        MissingMemberHandling = MissingMemberHandling.Ignore,
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
    };

    /// <summary>
    /// Успешный ответ
    /// </summary>
    public static ActionResult Success() =>
        new JsonResult(null, Settings) { StatusCode = (int)StatusCode.Ok };

    /// <summary>
    /// Успешный ответ с данными
    /// </summary>
    public static ActionResult Success<TData>(TData data) =>
        new JsonResult(data, Settings) { StatusCode = (int)StatusCode.Ok };
}