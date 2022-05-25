# 轻量级Web服务SIT测试方案:

1. 输入: 准备批量输入

2. 测试系统： WebService 系统

3. 测试环境准备
        
        a. 由测试自己发起、清理，如后续的C#代码所示
        b. 由测试发起方准备、清理
        
4. 测试编写
```c#
// 如果需要，改造成数据驱动方式，报表更优雅
foreach(var data in testDataDirs){
// 1. prepare input&expected
// 2. call system
// 3. do assert
}
```

5. 测试执行(以mstest2为例)
```bash
dotnet test 
```

# 测试运行(调度方负责服务启动和停止)

```bash  
#!/usr/bin/env bash
#do_test_bash.sh test_data_dir

[$# -eq 0]|| { echo "please provide the test data dir" ; exit -1 ;}

test_data_dir=$1

pushd .
image_name=local_service
docker_name=local_service_container

docker build . -t $image_name
docker stop $docker_name
docker rm $docker_name

docker run $image_name -t docker_name

popd
dotnet test $test_data_dir --test-report abx.html

docker stop $docker_name
docker rm $docker_name
```

# 测试运行(测试自己负责服务启动和停止)

```bash
dotnet test  --test-report abx.html
```
```c#
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProject2;

[TestClass]
public class Initialize
{
    private static Process serviceProcess;
    
    [AssemblyInitialize]
    public static void AssemblyInitialize(TestContext context)
    {
        var serviceStartInfo = new ProcessStartInfo
        {
            FileName    = "dotnet",
            Arguments = "run --urls \"http://localhost:5100;https://localhost:5101\" ",
            WorkingDirectory = Directory.GetCurrentDirectory(), //change if you need
            UseShellExecute = true,
            Environment = {  } //something you need
        };

        serviceProcess = Process.Start(serviceStartInfo);
        
        Console.WriteLine("AssemblyInitialize");
    }

    [AssemblyCleanup]
    public static void AssemblyCleanup()
    {
        if (!serviceProcess.HasExited)
        {
            serviceProcess.Kill();
            serviceProcess.WaitForExit();
        }
        
        Console.WriteLine("AssemblyCleanup");        
    }
}

[TestClass]
public class UnitTest1
{
    [TestMethod]
    public void TestMethod1()
    {
        Console.WriteLine("do something http service test?");
    }
    
#region data driver test
    [DataTestMethod]
    [DataRow(1, 1, 2)]
    [DataRow(2, 2, 4)]
    [DataRow(3, 3, 6)]
    [DataRow(0, 0, 1)] // The test run with this row fails
    public void AddTests(int x, int y, int expected)
    {
      Assert.AreEqual(expected, x + y);
    }
    

    [DataTestMethod]
    [DynamicData(nameof(Data), DynamicDataSourceType.Property)] //DynamicDataSourceType.Method should works as well
    public void Test_Add_DynamicData_Property(int a, int b, int expected)
    {
        var actual = a + b;
        Assert.AreEqual(expected, actual);
    }

    public static IEnumerable<object[]> Data
    {
        get
        {
            yield return new object[] { 1, 1, 2 };
            yield return new object[] { 12, 30, 42 };
            yield return new object[] { 14, 1, 15 };
        }
    }
    

//     [DataTestMethod]
//     [GetAllFiles("/data/package")]
//     public void AddTests(string filePath)
//     {
//         //do some test
//     }
    #endregion
}
```

# 关于代码改造

     重构WebService业务系统以支持Mock或者关闭链路服务，如 S3、认证、Callback等

# 关于工程创建

```bash
mkdir ATestProject
cd ATestProject
dotnet new mstest
dotnet test 
```

## 关于工程依赖

```bash
#TBD: like dotnet add package Microsoft.EntityFrameworkCore
```

# 参考
1. https://www.meziantou.net/mstest-v2-test-lifecycle-attributes.htm
2. https://www.meziantou.net/mstest-v2-data-tests.htm
3. https://andrewlock.net/5-ways-to-set-the-urls-for-an-aspnetcore-app/
4. dotnet new --help
5. dotnet test --help
6. dotnet test --logger "html;logfilename=testResults.html"
7. https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-test
8. https://docs.microsoft.com/en-us/visualstudio/test/how-to-create-a-data-driven-unit-test?view=vs-2022
9. https://docs.microsoft.com/en-us/dotnet/core/tools/dependencies