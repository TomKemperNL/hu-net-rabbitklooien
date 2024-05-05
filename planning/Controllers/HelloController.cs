using Microsoft.AspNetCore.Mvc;

namespace Planning.Inno.HU.Controllers;

[ApiController]
[Route("/hello")]
public class HelloController : ControllerBase
{
    [HttpGet]
    public ActionResult<Message> Test()
    {
        return new Message("Hello World");
    }
}

public class Message
{
    public Message(string value)
    {
        Value = value;
    }

    public String Value { get; private set; }
}