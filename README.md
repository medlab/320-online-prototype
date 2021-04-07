# 研发手册/大纲

## 技术方案整体概述 
- 目前采用MVVM web 架构，原型技术栈主要使用.net 5 + React + EF 
- 首先先实现用户常用场景下的一些操作做出技术点例子，如用户注册，登陆，文件上传，下载，看教学视频，能对自己的信息进行管理等，然后在进一步做详细规划

## 技术点研究
1. 后端例子
    - 功能相关
        - 日志系统
            - 日志规范，等级，分类
            - 内置日志，自定义日志，第三方日志
        - 认证授权
            - 认证
            - 授权
        - 数据库ORM
            - 数据模型定义，初始化,创建
            - 数据库迁移（Migration）
        - WebAPI 
            - 大文件包
                - 上传
                - 下载
            - 其他数据传输(json)
        - 中间件
            - 管道
            - 宿主
        - 服务器路由简单演示
    - 工程相关
        - 编译，打包发布，部署
            - 本地
            - docker
        - 测试
            - 单元测试
            - 半集成测试
            - 测试报告输出

2. 前端例子
    - 功能相关
        - 表单
	    - 上传
	    - 下载
        - 状态管理
		- 用户登陆 TODO
		- 表格控件
		- 界面组件库
		- 路由
		- css 布局，设计实现 TODO
        - 工程化 TODO
		- 过渡和动画 TODO
    - 工程相关
        - 编译部署
        - 单元测试
        - Mock数据
        - E2E测试 TODO
        - 代码规范
        - babel  TODO
        - webpack 设置

## WebApp原型（结合所有技术点做出的原型站点）
- Todo 

## 参考资料
- [ASP.NET Core Docs](https://docs.microsoft.com/zh-cn/aspnet/core/getting-started/?view=aspnetcore-5.0&tabs=macos)

## 目录结构
```
.
├── LICENSE
├── Notes
│   └── DataFormat.md
├── PrototypeWebApp
│   └── README.md
├── README.md
├── Resources
│   ├── doc
│   │   └── Microsoft.Press.CLR.Via.C.Sharp.4th.Edition.pdf
│   └── video
│       └── react.md
└── TecPoints
    ├── Backend
    │   ├── Authentication
    │   │   ├── Authen
    │   │   └── README.md
    │   ├── CustomRouting
    │   │   ├── 01_Initial
    │   │   ├── 02_Finally
    │   │   └── CustomRouting.md
    │   ├── DBMigration
    │   │   ├── Finally
    │   │   └── Initial
    │   ├── Logging
    │   │   ├── Finally
    │   │   ├── Initial
    │   │   └── README.md
    │   └── Middleware
    │       ├── Finally
    │       ├── Initial
    │       ├── README.md
    │       ├── middleware-pipeline.svg
    │       ├── mvc-endpoint.svg
    │       ├── request-delegate-pipeline.png
    │       └── test2
    └── Frontend
        └── Form
            └── form.md
```

