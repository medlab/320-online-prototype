# 背景

OAuth中的客户端授权模式，解决服务到服务或者机器到机器之类的认证场景

本文定义三个参与方：
服务A（Client）， 服务B（Resource Server）， 服务C（Authorization Server）

验证方案：
1. 通过客户端id验证
    a. 服务A通过客户端ID、密码向服务C发起授权申请, 结果是获得access token
    b. [资源获取步骤]服务A通过ACCES TOKEN向服务B发起请求, 结果是获得资源
    c. [资源获取步骤中]中服务B从服务C获得公共钥匙
    d. [资源获取步骤中]中服务B通过公钥验证A的签名
    e. [资源获取步骤中]服务B通过公钥验证A的签名
    f. [资源获取步骤中]服务B通过CliendID等Claim断言客户端合法性 
2. 通过scope验证?
    a. 服务A通过客户端ID、密码、api scop向服务C发起授权申请, 结果是获得access token
    b. [资源获取步骤]服务A通过ACCES TOKEN向服务B发起请求, 结果是获得资源
    c. [资源获取步骤中]中服务B从服务C获得公共钥匙
    d. [资源获取步骤中]中服务B通过公钥验证A的签名
    e. [资源获取步骤中]服务B通过公钥验证A的签名
    f. [资源获取步骤中]服务B通过Scope等Claim断言api访问合法性 

# 环境准备工作(INSPECT模式)

准备一个oidc server服务器(hydra), 完成基本的验证

```bash

git clone https://github.com/ory/hydra.git

# docker-compose -f quickstart.yml \
#     -f quickstart-postgres.yml \
#     -f quickstart-tracing.yml \
#     up --build

# 创建一个服务器
docker-compose -f quickstart.yml \
    -f quickstart-postgres.yml \
    -f quickstart-tracing.yml \
    -f quickstart-prometheus.yml  \
    up --build

# 检查公钥
curl http://127.0.0.1:4444/.well-known/jwks.json

# 创建一个客户端
docker-compose -f quickstart.yml exec hydra \
    hydra clients create \
    --endpoint http://127.0.0.1:4445/ \
    --id my-client \
    --secret secret \
    --grant-types client_credentials

# 创建另一个客户端
docker-compose -f quickstart.yml exec hydra \
    hydra clients create \
    --endpoint http://127.0.0.1:4445/ \
    --id client \
    --secret secret \
    --grant-types client_credentials

# 1. 获取一个客户端认证token
docker-compose -f quickstart.yml exec hydra \
    hydra token client \
    --endpoint http://127.0.0.1:4444/ \
    --client-id my-client \
    --client-secret secret

# 2. 获取一个客户端认证并解码
echo --begin-- opaque token test

OpaqueToken=`docker-compose -f quickstart.yml exec -T hydra \
    hydra token client \
    --endpoint http://127.0.0.1:4444/ \
    --client-id my-client \
    --client-secret secret`

docker-compose -f quickstart.yml exec -T hydra \
    hydra token introspect \
    --endpoint http://127.0.0.1:4445/ \
    ${OpaqueToken}

echo --end-- opaque token test

# 3. 或者合并成一个命令
docker-compose -f quickstart.yml exec -T hydra \
    hydra token introspect \
    --endpoint http://127.0.0.1:4445/ \
    `docker-compose -f quickstart.yml exec -T hydra \
    hydra token client \
    --endpoint http://127.0.0.1:4444/ \
    --client-id my-client \
    --client-secret secret`
```

结果：
```json
{
        "active": true,
        "aud": [],
        "client_id": "my-client",
        "exp": 1631464115,
        "iat": 1631460514,
        "iss": "http://127.0.0.1:4444/",
        "nbf": 1631460514,
        "sub": "my-client",
        "token_type": "Bearer",
        "token_use": "access_token"
}
```
# 环境准备工作(jwt模式)

这个主要通过quickstart-jwt.yml来启用

服务器配置：
```bash
docker-compose -f quickstart.yml     -f quickstart-postgres.yml     -f quickstart-tracing.yml     -f quickstart-prometheus.yml  -f quickstart-jwt.yml    up --build

# 创建一个客户端
docker-compose -f quickstart.yml exec hydra \
    hydra clients create \
    --endpoint http://127.0.0.1:4445/ \
    --id my-client \
    --secret secret \
    --grant-types client_credentials

# 创建另一个客户端
docker-compose -f quickstart.yml exec hydra \
    hydra clients create \
    --endpoint http://127.0.0.1:4445/ \
    --id client \
    --secret secret \
    --grant-types client_credentials
    
```

Token获取以及解析：
```bash
OpaqueToken=`docker-compose -f quickstart.yml exec -T hydra \
    hydra token client \
    --endpoint http://127.0.0.1:4444/ \
    --client-id my-client \
    --client-secret secret`
echo ${OpaqueToken}

docker-compose -f quickstart.yml exec -T hydra \
    hydra token introspect \
    --endpoint http://127.0.0.1:4445/ \
    ${OpaqueToken}
```

结果：
```json
{
        "active": true,
        "aud": [],
        "client_id": "my-client",
        "exp": 1631612206,
        "iat": 1631608606,
        "iss": "http://127.0.0.1:4444/",
        "nbf": 1631608606,
        "sub": "my-client",
        "token_type": "Bearer",
        "token_use": "access_token"
}
```

# 服务器实现--通过客户端ID验证

决策权在API实现方面，可以通过客户端ID来验证

# 服务器实现--通过scope验证

这会涉及到API管理平台，API管理平台定义Client和ApiScope的关联关系，API实现方面可以通过scope来验证访问合法性

1. 管理平台为API起名
2. 管理平台定义客户端和API的关系
3. 管理平台为客户端颁发对应scope Claim
4. 资源提供方通过scope验证请求合法性

# 验证服务器实现：

```bash
# 1. public api
curl -X GET "http://localhost:5000/HelloWorldApi/SayHelloPublic" -H  "accept: */*"

# 2. auth api
curl -X GET "http://localhost:5000/HelloWorldApi/SayHelloSecured" -H  "accept: */*"
curl -X GET "http://localhost:5000/HelloWorldApi/SayHelloSecured" -H  "accept: */*" -H  "Authorization: Bearer ${OpaqueToken}"

# 3. auth by client_id api
curl -X GET "http://localhost:5000/HelloWorldApi/SayHelloSecuredByClientIDCheck" -H  "accept: */*"
curl -X GET "http://localhost:5000/HelloWorldApi/SayHelloSecuredByClientIDCheck" -H  "accept: */*" -H  "Authorization: Bearer ${OpaqueToken}"

# 4. auth by scope api
curl -X GET "http://localhost:5000/HelloWorldApi/SayHelloSecuredByApiScope" -H  "accept: */*"
curl -X GET "http://localhost:5000/HelloWorldApi/SayHelloSecuredByApiScope" -H  "accept: */*" -H  "Authorization: Bearer ${OpaqueToken}"

# 4. auth by custom asseration
curl -X GET "http://localhost:5000/HelloWorldApi/SayHelloSecuredByCustomFuncCheck" -H  "accept: */*" -H  "Authorization: Bearer ${OpaqueToken}"

```

# 核心代码参考
0. https://docs.identityserver.io/en/latest/quickstarts/1_client_credentials.html
1. https://github.com/IdentityServer/IdentityServer4/tree/main/samples/Quickstarts/1_ClientCredentials
2. https://devblogs.microsoft.com/aspnet/jwt-validation-and-authorization-in-asp-net-core/
3. https://docs.microsoft.com/en-us/aspnet/core/security/authorization/claims?view=aspnetcore-5.0
4. https://docs.microsoft.com/en-us/aspnet/core/security/authorization/policies?view=aspnetcore-5.0#use-a-handler-for-one-requirement
5. https://developer.okta.com/blog/2018/02/01/secure-aspnetcore-webapi-token-auth#create-the-token-service 

# 其他参考
0. https://www.ory.sh/hydra/docs/5min-tutorial/
1. https://github.com/ory/hydra
2. https://developer.okta.com/blog/2018/06/06/node-api-oauth-client-credentials
3. https://docs.microsoft.com/en-us/azure/active-directory/develop/v2-oauth2-client-creds-grant-flow
4. https://datatracker.ietf.org/doc/html/rfc6749#section-4.4
5. https://auth0.com/blog/using-m2m-authorization/
6. https://blog.mia-platform.eu/en/m2m-authentication-and-authorization-with-oauth-2.0-and-openid-connect
7. https://devblogs.microsoft.com/aspnet/jwt-validation-and-authorization-in-asp-net-core/
8. https://docs.microsoft.com/en-us/aspnet/core/security/authorization/claims?view=aspnetcore-5.0
9. https://docs.microsoft.com/en-us/aspnet/core/security/authorization/policies?view=aspnetcore-5.0#use-a-handler-for-one-requirement
10. https://docs.identityserver.io/en/latest/quickstarts/1_client_credentials.html
11. https://darutk.medium.com/oauth-2-0-client-authentication-4b5f929305d4
12. https://docs.microsoft.com/en-us/azure/active-directory/develop/v2-protocols-oidc
13. https://swagger.io/docs/specification/authentication/openid-connect-discovery/
14. https://developer.okta.com/blog/2018/03/23/token-authentication-aspnetcore-complete-guide