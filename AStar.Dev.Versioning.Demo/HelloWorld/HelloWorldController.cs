using System;
using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AStar.Dev.Versioning.Demo.HelloWorld;

[ApiVersion("2.0")]
[ApiVersion("2.1")]
[ApiController]
[Route("HelloWorld")]
public class HelloWorldController : ControllerBase
{
    [HttpPost]
    public string Post() => "v" + HttpContext.GetRequestedApiVersion();

    [HttpGet]
    [MapToApiVersion("2.0")]
    [Obsolete("Please change to V2.1. Thanks.")]
    public string Get() => "v2.0";

    [HttpGet]
    [MapToApiVersion("2.1")]
    public string Get2_1() => "v2.1";
}