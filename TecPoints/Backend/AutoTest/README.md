# ASP.NET Core webapi testing

- 总体来说关于`webapi`的自动化测试分为：
    - 单元测试（UT），这些测试保证独立的方法或者模块能正常工作，类似于`react`的函数式组件，一切依赖项都需要Mock，不应该去调用其他的模块
    - 集成测试（IT），查看整个链路，或者链路的某一段的测试，只需要准备模拟的环境，调用`webapi`看整体最终能否达到结果

- 测试框架
    - MSTEST
    - XUnit
    - NUnit

- 关于DB
    - 针对于和产品相同的数据库系统,需要安装数据库运行时，一般来说这种可以放在一台统一安装测试环境的主机上，方便做最终的检测
        - `LocalDB`
        - `SqlServer`
    - 为了UT和IT制造的功能相同的能满足测试的简单数据库,一般来说不需要部署数据库环境，直接可以生成运行或者是加载于内存的数据库
        - `In Memory`
        - `Sqlite`
    - 即便是可以使用`In Memory`，`Sqlite`来模拟，但是效果终究达不到`sql server`的效果，有几点要注意：
        - `In Memory` 不能操作及联操作
        - `In Memory`，`Sqlite`是大小写敏感，而`sql server`不敏感

## Unit testing
考虑到某些业务逻辑需要使用数据库的数据，但是不去使用或者担忧上游的数据库体系，一般来说可以MOCK模拟数据结构（如Mq），但是在使用EF Core的时候去Mock DBContext是复杂而且成本很高的事情，而且不能保证正确，所以
我们尽量使用能跑在内存中的数据库来作为测试

## Intergration testing
目前看，我们的集成测试有这几方面：
- 使用`Client`,`Server`等`Microsoft.AspNetCore.TestHost`提供的模拟客户端和主机端的可以运行在内存中的实例
- 使用`Sqlite`去提供需要的测试数据

## Demo
Demo中创建了一个`ItemWebApi`工程和它相关的测试工程
- 关于数据库的切换，并没有放在主项目的配置文件里面，而是单独在Tests工程中做了切换
- 要完成以上目的，需要在写`xxxxController`的时候注意,对应的DBContext要使用依赖注入模式（即放在构造函数的参数上）
- 使用`Sqlite`的in-memory模式，就是即创造即使用即删除
- 在项目根目录下`cd Tests`
- 运行`dotnet test -l:trx`


## 参考：
[dotnet-test](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-test#:~:text=dotnet%20test%20%201%20Name%202%20Synopsis%203,6%20Examples.%20has%20the%20format%20%5B%7C.%20See%20More)

[Testing ASP.NET Core services and web apps](https://docs.microsoft.com/en-us/dotnet/architecture/microservices/multi-container-microservice-net-applications/test-aspnet-core-services-web-apps)

[UnitTesting With .NET Core](https://docs.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-dotnet-test)










