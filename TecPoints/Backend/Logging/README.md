# ASP.NET Core Web主机端Logging

ASP.Net Core 中已经内置（built-in）了日志系统，同时也提供了一个统一的日志接口`ILogger`，ASP.Net Core 自己的系统以及其它第三方类库等都使用这个日志接口来记录日志，而不关注日志的具体实现，这样便可以在我们的应用程序中进行统一的配置，并能很好的与第三方日志框架集成

## 规范 

### ILogger
- `Microsoft.Extensions.Logging.ILogger` 位于 `Microsoft.Extensions.Logging.Abstractions.dll`
	```C#
	public interface ILogger
	{
		IDisposable BeginScope<TState>(TState state);

		bool IsEnabled(LogLevel logLevel);

		void Log<TState> (Microsoft.Extensions.Logging.LogLevel logLevel, Microsoft.Extensions.Logging.EventId eventId, TState state, Exception exception, Func<TState,Exception,string> formatter);
	}
	```
- 第一个参数LogLevel指明了这条信息的级别，日志级别即其重要程度。ASP.NET Core 日志系统定义了 6 个级别
	| LogLevel | 值 | 方法 | 描述 |
	|  ----  | ----  | ----  | ----  |
	| 跟踪 | 0 | LogTrace |包含最详细的消息。 这些消息可能包含敏感的应用数据。 这些消息默认情况下处于禁用状态，并且不应在生产中启用。|
	| 调试 | 1 |LogDebug |	用于调试和开发。 由于量大，生产中小心使用。|
	|信息 |2 |LogInformation |	跟踪应用的常规流。 可能具有长期值。|
	|警告 |3 |LogWarning |	对于异常事件或意外事件。 通常包括不会导致应用失败的错误或情况。|
	|错误 |4| LogError |	表示无法处理的错误和异常。 这些消息表示当前操作或请求失败，而不是整个应用失败。|
	|严重 |5 |LogCritical	|需要立即关注的失败。 例如数据丢失、磁盘空间不足。|
- 第二个参数 EventId , 事件的ID，可以自定义，目的是为了分类和过滤
- 最后一个参数formatter，一个返回值类型为字符串的委托，该委托的意义在于根据指定的状态以及异常返回要输出的日志信息。

	直接使用 Log 方法来记录日志会**非常麻烦**。为此 ILogger 接口提供了若干个扩展方法，用来更方便地记录指定级别的日志，它们包括 LogTrace、LogDebug、LogInformation、LogWarning、LogError 和 LogCritical，这几个方法分别对应上面所提到的各个级别。

### ILoggerProvider 

- `Microsoft.Extensions.Logging.ILoggerProvider` 位于 `Microsoft.Extensions.Logging.Abstractions.dll`

- ILoggerProvider 是用来创建写日志的对象 ILogger ，即在应用程序启动时，把实现了 ILoggerProvider 接口的类型放到集合中，在应用程序运行期间，需要写日志，先去集合中取 ILoggerProvider ，使用其创建 ILogger 对象，然后就可以写日志了

	```C#
	public interface ILoggerProvider: IDisposable
	{
	     ILogger CreateLogger(string categoryName)
	}
	```

	ASP.NET Core 已经做好的了一些buildin的loggerProvider
	- Console (默认开启) Microsoft.Extensions.Logging.Debug
	- Debug （默认开启）
	- EventSource (dotnet trace......)
	- EventLog （看是否是windows环境）

	下面这两个也可以通过nuget安装
	- AzureAppServicesFile and AzureAppServicesBlob
	- ApplicationInsights

	ASP.NET Core 本身并**不包含写入文件系统**的logging provider，如果需要的话，我们可以说使用第三方的logging provider, 如 
	- [NLOG](https://github.com/NLog/NLog/wiki/Getting-started-with-ASP.NET-Core-5)
	- [Serilog](https://github.com/serilog/serilog-aspnetcore)


### Log Filtering

Log filter是用来规定什么样的LOG可以被provider知道并且各自输出(原理是ILoggerFactory做了控制，详情可以查ILoggerFactory)
- 日志等级，如warnning, error,等
- 日志的归类，一般来说，这个归类的名字就是我们注入ILOG到具体类的类名，

有两种方式去配置：

- 方式一、代码写：
```C#
logging.AddFilter("System", LogLevel.Debug).AddFilter<DebugLoggerProvider>("Microsoft", LogLevel.Information).AddFilter<ConsoleLoggerProvider>("Microsoft", LogLevel.Trace))
```
- 方式二、在appsetting.json里面写配置(MSDN推荐做法)
```
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    },
    "Console": {
      "IncludeScopes": true,
      "LogLevel": {
        "Microsoft.AspNetCore.Mvc.Razor": "Error",
        "Default": "Information"
      }
    }
  },
  "AllowedHosts": "*"
}
```

在上述 JSON 中：

1. 指定了 "Default"、"Microsoft" 和 "Microsoft.Hosting.Lifetime" 类别。

2. "Microsoft" 类别适用于以 "Microsoft" 开头的所有类别。 例如，此设置适用于 "Microsoft.AspNetCore.Routing.EndpointMiddleware" 类别。

3. "Microsoft" 类别在日志级别 Warning 或更高级别记录。

4. "Microsoft.Hosting.Lifetime" 类别比 "Microsoft" 类别更具体，因此 "Microsoft.Hosting.Lifetime" 类别在日志级别“Information”和更高级别记录。

未指定特定的日志提供程序，因此 LogLevel 适用于所有启用的日志记录提供程序，但 Windows EventLog 除外。


## DEMO实践步骤
1. 注册 Provider
	- 使用默认provider (commented in DEMO)
	- 使用第三方provider (currently DEMO use Serilog)
	- 自定义provider => Todo

2. 注册 Filter
	- use the appsetting.json 
	- use the 3rd party setting.json （optional）
	- use filter in code (DEMO)

3. 获取 ILogger
	- constructor (DEMO in "WeWeatherForecastController")


4. 写日志
	- DEMO使用Serilog把默认的系统日志过滤掉，只留下自己想要的，然后保存在`/logs/`文件夹下面


## 参考
[MSDN - ASPNET CORE LOG](https://docs.microsoft.com/zh-cn/aspnet/core/fundamentals/logging/?view=aspnetcore-5.0#log-message-template)

[MSDN - NET CORE GENERIC HOST](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/generic-host?view=aspnetcore-5.0)

[MSDN - ASPNET CORE WEB HOST](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/web-host?view=aspnetcore-5.0)

[Serilog GIT HUB](https://github.com/serilog/)

