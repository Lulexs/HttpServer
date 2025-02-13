using WebServer.ControllersAttributes;

namespace WebServer.Controllers;

[Route("test")]
public class TestController
{

    [Verb("get")]
    [Route("abc/def")]
    public void DoSomething()
    {

    }

    [Verb("post")]
    [Route("efg/hci")]
    public void DoSthElse()
    {

    }

}