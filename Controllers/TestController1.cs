using WebServer.ControllersAttributes;

namespace WebServer.Controllers;

[Route("test1")]
public class TestController1()
{

    [Verb("delete")]
    [Route("dosth")]
    public void DoSth()
    {

    }

    [Verb("put")]
    [Route("dosthelse")]
    public void DoSthElse()
    {

    }
}