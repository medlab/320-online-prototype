# 背景

希望在CI流程中，覆盖尽可能多的测试场景，将测试左移，促进研发更好的思考。

包括两类场景：
1. API 的集成测试
2. UI 的集成测试

# 工程组成

1. .Net WebAPI 程序
    程序使用.Net WebAPI模版
        a. 新增Hello {UserName}接口，接口返回Hello {UserName}
        b. 新增文件上传接口 UploadFile(filename, file)，接口返回 Size: {file.Length}
    ```c#
    // Controllers/HelloApiController.cs
    [HttpGet]
    using Microsoft.AspNetCore.Mvc;

    namespace WebApi.Controllers;

    [ApiController]
    [Route("[controller]")]
    public class HelloApiController : ControllerBase
    {
        [HttpGet]
        [Route("[action]")]
        public string SayHello(string name)
        {
            return $"Hello {name}";
        }
        [HttpPost]
        [Route("[action]")]
        public string UploadFile(IFormFile file)
        {
            return $"Hello {file.Length}";
        }

    }

    ```
2. .Net WebUI 程序
    程序使用BlazorServer模版
        a. 提供交互场景， 文本框输入{UserName}, Response 区域输出 Hello {UserName}
    ```c#
    // Pages/SayHello.razor
    @page "/sayhello"

    <label for="input">Please input your name:</label>
    <input type="text" @bind="Name" id="input" > 
    <br>
    <span id="msg">
        Hello @Name
    </span>
    @code {
        private string Name;
    }

    ```
3. 测试数据管理
    测试数据是一系列文件集合, 此处人为制造一批数据
    ```bash
    cd TestDatas
    for i in {1..15} ; do echo $((2**$i) > $i.txt ; done
    ```
4. Jest 测试程序
    1. 工程建立
        ```bash
        cd SitByNodejs
        yarn add --dev puppeteer jest axios weak-napi form-data
        #add dummy test
        echo "it('should return hello world', ()=>{expect('hello world').toBe('hello world');});"> hello.test.js 
        #run it
        yarn jest
        # or if needed
        # yarn jest --detectOpenHandles --detect-leaks
        ```

    2. WebApi 测试程序
        程序使用Node.js+Jest编写，使用数据驱动思路，针对所有测试数据依次完成测试
        ```js
            
            //1. TODO 装备测试环境
                // 1. TODO 外部服务，或
                // 2. TODO 启动外部Container
            
            //2. TODO 执行测试
            it.each([
                ['zhangsan','Hello zhangsan'],
                ['lisi','Hello lisi'],
                ['wangmazi','Hello wangmazi']
                ])(' call sayhello to %s should get response %s', (name,response) => {
                //call webapi with name=name
                //expect response=response
            });
            
            //TODO how to do async/await and upload for data?
            //TODO 设法自动遍历文件系统以获得真正的测试数据列表和预期断言结果
            let data_and_assert_list=[['1.txt', 1], ['2.txt‘，1]，['3.txt', 1], ['4.txt', 2], ['5.txt', 2]];
            it.each(data_and_assert_list)('%s should get response %s', (name,response) => {
                //TODO upload file and get response
                //TODO do assert 
            });

            //3. TODO 清理测试环境
                //1. TODO 关闭外部服务，或
                //2. TODO 关闭外部Container
        ```
    3. WebUi 测试程序
        程序使用Puppeteer编写，使用数据驱动思路，针对一个用户名列表完成迭代测试

        ```js
            
            //1. TODO 装备测试环境
                // 1. TODO 外部服务，或启动外部Container
                // 2. TODO 通过puppeteer启动浏览器
            //2. TODO 执行测试
            it.each([
                ['zhangsan','Hello zhangsan'],
                ['lisi','Hello lisi'],
                ['wangmazi','Hello wangmazi']
                ])(' call sayhello to %s should get response %s', (name,response) => {
                //TODO set input with #name
                //TODO get msg from #msg
                //TODO do assert
            });
            
            //3. TODO 清理测试环境
                //1. TODO 通过puppeteer关闭浏览器
                //2. TODO 关闭外部服务，或 关闭外部Container
        ```

# 测试便利贴--理念
    1. 发布和环境的质量决定了测试的质量
        1. 产出物的发布
        2. 产出物的获取
        2. 产出物的的运行
        3. 运行现场的清理
    2. 可测性的设计决定测试的质量
    3. 测试数据的管理决定测试的质量
    4. 测试需求的管理决定测试的质量
    5. 开发、测试、运维的协作质量决定测试的质量
    6. 基础设施的质量决定测试的质量
    
# 测试便利贴--实用过程
1. 常见路径操作    
    a. 获取当前脚本路径
    ```js
    __filename
    ```
    b. 路径拼接
    ```js
    //ref: https://nodejs.org/api/path.html#path_path_join_paths
    import { join } from 'path';
    join('/foo', 'bar', 'baz/asdf', 'quux', '..'); // Returns: '/foo/bar/baz/asdf'
    ```
    c. 获取文件大小
    ```js
    //ref: https://nodejs.org/api/fs.html#fs_fspromises_stat_path_options
    let file_stat=await stat(path)
    console.log(file_stat.size)
    ```
    d. 文件夹内容枚举
    ```js
    //ref: https://nodejs.org/api/fs.html#fs_fspromises_readdir_path_options
    import { readdir } from 'fs/promises';

    try {
    const files = await readdir(path);
    for (const file of files)
        console.log(file);
    } catch (err) {
    console.error(err);
    }
    ```
2. nodejs 进程操作
    1. 启动进程
    ```js
    //start process
    import { spawn } from 'child_process';
    const ls = spawn('ls', ['-lh', '/usr']);
    ls.stdout.on('data', (data) => {
        console.log(`stdout: ${data}`);
    });
    ```
    2. 停止进程
    ```js
    //stop process
    ls.kill('SIGINT');
    ```
3. Docker Container 操作
    安全启动
    ```bash
    #--begin-- input control section
    image_name=someimagename # TODO your image name
    container_name=somecontainername # your container name
    publish_parts='-p 8000:8000' # TODO your publish part
    env_parts='-e MYVAR2=foo -e FOO=bar'
    #--end-- input control section

    #--begin-- work part
    docker container stop ${name}
    docker container rm ${name}
    docker start -it --rm --name ${container_name} ${env_parts} ${image_name}
    #--end-- work part

    ```
    安全停止:
    ```bash
    #--begin-- input control section
    image_name=someimagename # TODO your image name
    container_name=somecontainername # your container name
    publish_parts='-p 8000:8000' # TODO your publish part
    env_parts='-e MYVAR2=foo -e FOO=bar'
    #--end-- input control section

    #--begin-- work part
    docker container stop ${name}
    docker container rm ${name}
    #--end-- work part
    ```

4. puppeteer常见操作

    1. 启动浏览器
    ```js
    const puppeteer = require('puppeteer');
    const browser = await puppeteer.launch({url: 'http://localhost:8000'});
    ```
    2. 关闭浏览器
    ```js
    await browser.close();
    ```
    3. 元素操作
        a. 检索元素
        ```js
        const page = await browser.newPage();
        await page.goto('http://localhost:8000');
        const element = await page.$('#name');
        ```
        b. 点击元素
        ```js
        await element.click();
        ```
        b. 设置值
        ```js
        await element.type('some text');
        ```
        c. 获取值
        ```js
        await element.evaluate(element => element.value);
        ```
