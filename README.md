# 研发手册/大纲

## 技术方案整体概述 
- 目前采用MVVM web 架构，原型技术栈使用.net 5 + React + EF 
- 首先先实现用户常用场景下的一些操作做出技术点例子，如用户注册，登陆，文件上传，下载，看教学视频，能对自己的信息进行管理等，然后在进一步做详细规划

## 技术点研究
1. 后端例子
    - 功能相关
        - log 用法
        - 认证原理
        - 数据库ORM （定义，初始化）
        - 数据库迁移
        - WebAPI
        - 文件的IO操作（上传和下载）
    - 工程相关
        - 编译，测试（命令行）
        - 测试报表输出
        - 单元测试
        - 半集成测试
        - 打包发布
        - 部署
2. 前端例子
    - 功能相关
        - 表单
	    - 上传
	    - 下载
        - 状态管理
		- 用户登陆
		- 表格控件
		- 界面组件库
		- 路由
		- css 布局，设计实现，工程化
		- 过渡和动画
    - 工程相关
        - 编译部署
        - 单元测试
        - Mock数据
        - E2E测试
        - 代码规范
        - babel 
        - webpack 设置

## WebApp原型（结合所有技术点做出的原型站点）
    - Todo 

## 目录结构
    ```
    .
    ├── LICENSE
    ├── Notes
    │   └── DataFormat.md
    ├── PrototypeWebApp
    │   ├── README.md
    │   └── src
    ├── README.md
    ├── TecPoints
    │   ├── Backend
    │   │   ├── Log
    │   │   │   ├── CodeSample
    │   │   │   └── log.md
    │   │   └── Middleware
    │   │       ├── CodeSample
    │   │       └── Middleware.md
    │   └── Frontend
    │       └── Form
    │           ├── CodeSample
    │           └── form.md
    └── tree.md
    ```

