using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Js;
using System.Diagnostics;

namespace ConsoleApp1;

internal class Program
{
    static async Task Main(string[] args)
    {
        var configWithJs = Configuration.Default
            .WithJs()
    //.Without<JsNavigationHandler>()
    .WithDefaultLoader()
        //.With(service)
    .WithConsoleLogger(ctx => new MyConsoleLogger(ctx));

        var contextWithJs = BrowsingContext.New(configWithJs);
        var source = File.ReadAllText("Redirect.Html");
        var docOrg = await contextWithJs.OpenAsync(req =>
        {
            req.Address(@"http://localhost:8080//");
            req.Content(source);
        }).WaitUntilAvailable();

        var docEl = docOrg.DocumentElement;
        var docHtml = docEl.OuterHtml;
        var docScript = docEl.InnerHtml;

        var actDoc = contextWithJs.Active;

        var actDocUrl = actDoc.Url;
        var actDocLocation = actDoc.Location;
        var docUrl = docOrg.Url;
        var docLocation = docOrg.Location;

        var locationFromDocScript = docOrg.ExecuteScript("location;");
        var locationFromActDocScrip = actDoc.ExecuteScript("location;");

        //i hope any of (actDocUrl,actDocLocation,docUrl,docLocation,locationFromDocScript,locationFromActDocScrip)'s value should be is thread-189739-1-1.html?_dsign=9ffd9d54
        //because if open Redirect.Html in the broswer will redirect to  thread-189739-1-1.html?_dsign=9ffd9d54

        // But now AngleSharp.Js and browsers seems have different behaviors.

        Console.WriteLine("Hello, World!");
        Console.ReadLine();
    }
}




public class MyConsoleLogger : IConsoleLogger
{
    public MyConsoleLogger(IBrowsingContext browsingContext)
    {

    }

    public void Log(object[] values)
    {
        Trace.WriteLine("错误:");
        Trace.WriteLine(values);
    }
}