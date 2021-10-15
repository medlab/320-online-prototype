# 背景

重要提醒：考虑到兼容模块开发可能比较困难，第一版本全部使用SkyWalking，后续再考虑框架混合

此处要设计一个.Net Core服务程序，程序会参与链路追踪，上游和下游使用SkyWalking这个APM框架。

要求程序使用OpenTelemetry完成链路追踪，这和APM不兼容，需要设计自定义的Extractor和Injector进行链路适配。


场景制造：

1. 一个Spring Boot程序 WebAPI程序，他会调用Asp.Net Core
2. 一个Node.js程序, 他会被Asp.Net Core调用
3. 一个SkyWalking服务器
    1. Spring Boot, Asp.Net Core, Node.js都会吧数据发送到这个服务器
    2. SkyWalking新增一个OpenTelemetry的差价，一直吃OpenTelemetry的数据

# SkyWalking 服务器准备
启动后端

```bash
#docker run --name oap --restart always -d apache/skywalking-oap-server:8.8.0
docker run -it --rm --name oap -p 11800:11800 -p 12800:12800 apache/skywalking-oap-server:8.8.0
```

启动前端：
```bash
#docker run --name oap --restart always -d -e SW_OAP_ADDRESS=http://oap:12800 apache/skywalking-ui:8.8.0
docker run --link oap  --name skywalking-ui  --rm -it  -e SW_OAP_ADDRESS=http://oap:12800 -p 8080:8080  apache/skywalking-ui:8.8.0
```

## 参考
1. https://skywalking.apache.org/docs/main/latest/en/setup/backend/docker/

# 程序开发

端口说明:
	1. Spring Boot 服务 9081
	2. Dotnet WebAPI 服务 9082
	3. Nodejs 服务	9083

## Spring Boot 程序

```bash
# 获取skywalking, 不知道怎么取，直接从官方镜像里面复制吧
docker pull ghcr.io/apache/skywalking-java/jdk-8:latest
# 复制镜像内容到本地
#docker run -it -v $PWD:/data --rm ghcr.io/apache/skywalking-java/jdk-8:latest cp -r /skywalking /data
docker run -i --rm ghcr.io/apache/skywalking-java/jdk-8:latest tar c /skywalking | tar xvf -

#create a basic project from https://start.spring.io/ , remeber add Dependencies of Spring Web, or from cmdline
# TODO how to do this in one line?
curl https://start.spring.io/starter.zip -d dependencies=web,devtools -d bootVersion=2.5.5.RELEASE -o demo.zip ; unzip demo.zip ; rm demo.zip

# ADD hello world

cat > src/main/java/com/example/demo/HelloController.java <<EOF
package com.example.springboot;

import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RestController;

@RestController
public class HelloController {

	@GetMapping("/")
	public String index() {
		return "Greetings from Spring Boot!";
	}

}
EOF

# 运行spring程序

export SW_AGENT_NAME=spring-boot-demo-application # 配置 Agent 名字。一般来说，我们直接使用 Spring Boot 项目的 `spring.application.name` 。
export SW_AGENT_COLLECTOR_BACKEND_SERVICES=127.0.0.1:11800 # 配置 Collector 地址。
export SW_AGENT_SPAN_LIMIT=2000 # 配置链路的最大 Span 数量。一般情况下，不需要配置，默认为 300 。主要考虑，有些新上 SkyWalking Agent 的项目，代码可能比较糟糕。
export JAVA_AGENT="-javaagent:$PWD/skywalking/agent/skywalking-agent.jar" # SkyWalking Agent jar 地址。
export SERVER_PORT=9081

./mvnw spring-boot:run  -Dspring-boot.run.jvmArguments="$JAVA_AGENT"

EOF
```
### 参考
1. https://spring.io/guides/gs/serving-web-content/
2. https://gist.github.com/fernandoabcampos/c380e4354e4443d36619
3. https://github.com/apache/skywalking/issues/7848

## Nodejs 程序

快速创建
```bash
mkdir nodejs
cd nodejs

npm install --save skywalking-backend-js express

cat > main.mjs <<'EOF'

import agent from 'skywalking-backend-js';

// //provide arguments if needed
agent.default.start();
//agent.start();

console.log({'agent':agent});

import express from 'express';
const app=express();
const port=9083

app.get('/', (req, res)=>{
	res.send('Hello World From Node Express');
})

app.listen(port, ()=>{
	console.log(`Example app listening at http://localhost:${port}`)
})

EOF


export SW_AGENT_NAME=nodejs-demo-application # 配置 Agent 名字。一般来说，我们直接使用 Spring Boot 项目的 `spring.application.name` 。
export SW_AGENT_COLLECTOR_BACKEND_SERVICES=127.0.0.1:11800 # 配置 Collector 地址。
export SW_AGENT_SPAN_LIMIT=2000 # 配置链路的最大 Span 数量。一般情况下，不需要配置，默认为 300 。主要考虑，有些新上 SkyWalking Agent 的项目，代码可能比较糟糕。

node main.mjs

```



### 参考
1. https://github.com/apache/skywalking-nodejs
2. https://expressjs.com/en/starter/installing.html
3. https://expressjs.com/en/starter/hello-world.html

## Asp.Net Core 程序

# 效果和演示

