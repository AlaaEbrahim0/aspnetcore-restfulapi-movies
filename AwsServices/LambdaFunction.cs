using System.IO;
using Microsoft.AspNetCore.Hosting;
using MoviesApi;
using WebApplication1;

namespace AwsServices;

public class LambdaFunction : Amazon.Lambda.AspNetCoreServer.APIGatewayProxyFunction
{
    protected override void Init(IWebHostBuilder builder)
    {
        builder
            .UseContentRoot(Directory.GetCurrentDirectory())
            .UseStartup<Startup>()
            .UseLambdaServer();
    }
}