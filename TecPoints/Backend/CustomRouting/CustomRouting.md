### CustomRouting

    本文描述了如何简单定义一个路由和对应的处理逻辑

```c#
endpoints.MapGet("/hello", async context =>
{
    var displayUrl = context.Request.GetDisplayUrl();
    var rootFullPath = displayUrl.Replace("hello","");
    await context.Response.WriteAsync($"To access root, visitor {rootFullPath}");
});
```

#### 参考
1. https://docs.microsoft.com/en-us/aspnet/core/fundamentals/routing
2. https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.routing.linkgenerator
3. https://stackoverflow.com/questions/30755827/getting-absolute-urls-using-asp-net-core
4. https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/tokens/interpolated

