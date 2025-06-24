using System;
using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AStar.Dev.Versioning.Demo.Controllers;

[ApiVersion("2.0")]
[ApiVersion("2.1")]
[ApiController]
[Route("HelloWorld")]
public class HelloWorld2Controller : ControllerBase
{
    // Common to v2.0 and v2.1 You can use HttpContext.GetRequestedApiVersion to get the matched version
    [HttpPost]
    public string Post()
    {
        return "v" + HttpContext.GetRequestedApiVersion();
    }

    // 👇 Map to v2.0
    [HttpGet]
    [MapToApiVersion("2.0")]
    [Obsolete("Please change to V2.1. Thanks.")]
    public string Get()
    {
        return "v2.0";
    }

    // 👇 Map to v2.1
    [HttpGet]
    [MapToApiVersion("2.1")]
    public string Get2_1()
    {
        return "v2.1";
    }
}
