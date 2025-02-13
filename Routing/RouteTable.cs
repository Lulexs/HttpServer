using System.Reflection;
using System.Text;
using WebServer.ControllersAttributes;
using WebServer.RequestHandlers;

namespace WebServer.Routing;

public sealed class RouteTable
{
    private static Dictionary<string, MethodInfo> Routes = null!;
    private static readonly Lock _lock = new();

    public static Dictionary<string, MethodInfo> GetRoutes()
    {
        if (Routes == null)
        {
            lock (_lock)
            {
                if (Routes == null)
                {
                    Routes = [];
                    Discover();
                }
            }
        }
        return Routes;
    }

    private RouteTable()
    {

    }

    private static void Discover()
    {
        IEnumerable<Type> classes = Assembly.GetExecutingAssembly().GetTypes().Where(x => x.Namespace == "WebServer.Controllers");
        foreach (var c in classes)
        {

            if (c.GetCustomAttribute<Route>() is not null)
            {
                string baseRoute = c.GetCustomAttribute<Route>()!.TheRoute;

                var methods = c.GetMethods(BindingFlags.Public | BindingFlags.Instance);
                foreach (var method in methods)
                {
                    if (method.GetCustomAttribute<Route>() is not null)
                    {
                        string route = baseRoute + "/" + method.GetCustomAttribute<Route>()!.TheRoute;
                        Routes.Add(route, method);
                    }

                }

            }

        }
    }
}