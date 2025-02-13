using WebServer.ControllersAttributes;

namespace WebServer.Controllers;

[Route("test")]
public class TestController
{

    [Route("abc/def")]
    public void DoSomething()
    {

    }

    [Route("efg/hci")]
    public void DoSthElse()
    {

    }

}