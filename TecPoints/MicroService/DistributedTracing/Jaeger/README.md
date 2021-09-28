# 背景

分布式追踪微服务进化的一个产物，微服务的架构引入了新的混乱，业务的分析和跟踪遇到了前所未有的挑战。

分布式追踪的元年是谷歌发布的Dapper论文，奠定了基本的思路和方法，后续陆续的有各种方案出现

## 分布式追踪开源的方案

1. Apache[Zipkin](https://zipkin.io/)
2. Twitter [Jaeger](https://www.jaegertracing.io/)
3. ...

## 分布式追踪规范和标准

1. [opentelemetry](https://opentelemetry.io/)
2. [W3 Trace Context](https://www.w3.org/TR/trace-context/)

本文介绍的是CNCF Jaeger一个分布式追踪平台，源自于Twitter，云原生基金会成员

## 分布式追踪各语言SDK体验
1. https://github.com/yurishkuro/opentracing-tutorial/tree/master/csharp
2. https://github.com/yurishkuro/opentracing-tutorial/tree/master/java
3. https://github.com/yurishkuro/opentracing-tutorial/tree/master/python
4. https://github.com/yurishkuro/opentracing-tutorial/tree/master/nodejs

# Jaeger快速体验

## 运行服务器和微服务示例程序

```bash
# 1. 运行jaegertracing平台
# 6831 信息收集器
# 16686 Web UI
# 14268 Http 收集器
docker run -d --name jaeger -p 6831:6831/udp -p 16686:16686 -p 14268:14268 jaegertracing/all-in-one

# 2. 运行example-hotrod示例微服务程序
docker run --rm -it --link jaeger --env JAEGER_AGENT_HOST=jaeger --env JAEGER_AGENT_PORT=6831 -p8080-8083:8080-8083 jaegertracing/example-hotrod all -j http://127.0.0.1:16686

# 3. 操作并分析
# 3.1 打开http://127.0.0.1:8080
# 3.2 操作 Rachel's Floral Designs 
# 3.3 点击生成的Trace分析链接进入分析

# 4. 分布式分析
```

## 分析示意图

1. (组件关系图-力导向图)[System Architecture-DAG.pdf]
2. (组件关系图-有向无环图)[System Architecture-Force Directed Graph.pdf]
3. (链路分析)[./trace_demo.png]

# 参考
0. https://research.google/pubs/pub36356/
1. https://opentelemetry.io/
2. https://www.jaegertracing.io/
3. https://zipkin.io/
4. https://www.w3.org/TR/trace-context/
5. https://github.com/jaegertracing/jaeger/tree/master/examples/hotrod
6. https://github.com/yurishkuro/opentracing-tutorial/
7. https://medium.com/opentracing/take-opentracing-for-a-hotrod-ride-f6e3141f7941
8. https://cloud.redhat.com/blog/openshift-commons-briefing-82-distributed-tracing-with-jaeger-prometheus-on-kubernetes
