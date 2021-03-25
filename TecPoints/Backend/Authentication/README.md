# ASP.NET Core认证

ASP.NET Core关于认证分为两部分
1. 身份验证（Authentication）
2. 授权 (Authorization)
## 知识点小结：
1. 证验什么？
 - 身份验证是确定用户身份的过程。 进而负责了一个叫做`ClaimsPrincipal`的身份对象，然后在管道上交给下游的authorization机制去进一步细分；授权是确定用户是否有权访问资源的过程。 在 ASP.NET Core 中，身份验证由中间件IAuthenticationService 负责，而它供身份验证中间件使用。 身份验证服务会使用已注册的身份验证处理程序来完成与身份验证相关的操作。 与身份验证相关的操作示例包括：
    - 对用户进行身份验证。
    - 在未经身份验证的用户试图访问受限资源时作出响应。
2. 怎么验证？
- 身份验证方案(scheme)由`Startup.ConfigureService`中注册的身份验证服务指定,直白的说用户提供了用户名和密码，然后你是让他愉快进来了，接下来你要和他之间建立一个通信证明，在有效期内用户拿着这个证明，就不需要再做一次登陆操作，这些方案比如是围绕JWT，Cookie等来做文章。
- 现在微软直接帮我们build-in了一些认证的模版和逻辑，利用了[Razor类库]（https://docs.microsoft.com/en-us/aspnet/core/razor-pages/ui-class?view=aspnetcore-5.0&tabs=visual-studio）可以参考visual studio关于带认证的webapp模版。
**如果我们自己去自定义认证，则需要[重写那些默认的模块](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/scaffold-identity?view=aspnetcore-5.0&tabs=visual-studio#create-full-identity-ui-source)** 
## DEMO
1. 这次的DEMO要演示的是
    - 用户可以默认进主页，但是他要看一些有权限(Autherization -> Authorication)的板块他需要登陆(建立他的ClaimsPrincipal)，如果没有注册过，那么他需要注册
    - ToDo:对于Authorication,可以围绕roles,claims,policy做进一步更详细的自定义，这里先不讨论。

2. repo steps:
    1. 我们从一个最简单的已经带有Razor基本视图的webapp开始，为啥还得带个Razor，用React不行吗？行！当然可以，不过引入React就增加了复杂度，这边先用Razor视图简单演示。
        ```bash
        dotnet new webapp -o TestAuthWebapp
        ```
    2. 安装和使用Scaffold工具
        ```bash
        dotnet tool install -g dotnet-aspnet-codegenerator

        dotnet restore
        ```
        然后在项目文件目录下继续执行：
        ```bash
        dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design &&
        dotnet add package Microsoft.EntityFrameworkCore.Design &&
        dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore &&
        dotnet add package Microsoft.AspNetCore.Identity.UI &&
        dotnet add package Microsoft.EntityFrameworkCore.Sqlite &&
        dotnet add package Microsoft.EntityFrameworkCore.Tools 
        ```
        使用Scaffold：
        ```bash
        dotnet aspnet-codegenerator identity -sqlite -u TestAuthWebappUser -fi "Account.Register;Account.Manage.Index" --force
        ```
    3. 数据库生成   
        - `dotnet tool install -g dotnet-ef` 
        - `dotnet ef migrations add CreateIdentitySchema`
        -`dotnet ef database update`
    4. 加上Authentication
        - 在startup.cs 中，`configure`方法里面在`app.UseAuthorization();`之前加上`app.UseAuthentication();`
    5. 修改主页模版，在layout.cshtml中加上：`<partial name="_LoginPartial" />`
        ```html
           <div class="navbar-collapse collapse d-sm-inline-flex flex-sm-row-reverse">
                    <partial name="_LoginPartial" />
                    <ul class="navbar-nav flex-grow-1">
                        <li class="nav-item">
                            <a class="nav-link text-dark" asp-area="" asp-page="/Index">Home</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link text-dark" asp-area="" asp-page="/Privacy">Privacy</a>
                        </li>
                    </ul>
                </div>
        ```
    6. 在Privacy.cshtml.cs中简单添加attrubute `[Authorize]`, 要求进入privacy页面需要认证.
