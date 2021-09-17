# 背景

本文介绍了Asp.Net Core 如何通过OpenTelemetry框架实现和ZipKin的对接

# ZipKin准备

ZipKin对部署非常友好，直接Java -jar执行或者Docker部署即可

```bash
docker run -d -p 9411:9411 openzipkin/zipkin
```

或者
```bash
curl -sSL https://zipkin.io/quickstart.sh | bash -s
java -jar zipkin.jar
```

# 程序思路

使用OpenTelemetry框架，实现ZipKin的访问日志记录, 并将记录的数据发送到ZipKin服务器

使用到的nuget包:

1. OpenTelemetry.Extensions.Hosting
2. OpenTelemetry.Instrumentation.AspNetCore
3. OpenTelemetry.Instrumentation.SqlClient
4. OpenTelemetry.Instrumentation.Http
5. OpenTelemetry.Exporter.Zipkin
6. OpenTelemetry.Exporter.Jaeger
7. OpenTelemetry.Exporter.Console

# 核心代码块

1. 配置Host
2. 创建OpenTelemetry服务
3. 配置OpenTelemetry服务
    1. 配置控制台导出模块
    2. 配置Zipkin导出模块

# 示例模拟测试
    1. Api入栈拦截
    2. Api出栈拦截
    3. Async 执行上下文验证
