using WebServer.ControllersAttributes;

namespace WebServer.Controllers;

[Route("test1")]
public class TestController1()
{

    [Route("dosth")]
    public void DoSth()
    {

    }

    [Route("dosthelse")]
    public void DoSthElse()
    {

    }
}