# 写在前面
  
  由于历史原因，K8S环境的安装在EFK章节，如需要准备环境，请参考EFK章节中的前置软件安装段落。

  TODO: SVG无法显示，替换为PNG？

# 快速开始

运行 Asp.Net Sample

```bash
#set -ex
cat > kubernetes.yaml <<EOF
apiVersion: apps/v1
kind: Deployment
metadata:
  name: webapp
  labels:
    app: weather-forecast
spec:
  replicas: 1
  selector:
    matchLabels:
      service: webapp
  template:
    metadata:
      labels:
        app: weather-forecast
        service: webapp
    spec:
      containers:
        - name: webapp
          image:  mcr.microsoft.com/dotnet/samples:aspnetapp
          ports:
            - containerPort: 80
              protocol: TCP
          env:
            - name: ASPNETCORE_URLS
              value: http://+:80
            - name: WebApiBaseAddress
              value: http://webapi
            - name: TTT
              value: Hello_TTT
---
apiVersion: v1
kind: Service
metadata:
  name: webapp
  labels:
    app: weather-forecast
    service: webapp
spec:
  type: LoadBalancer
  ports:
    - port: 80
      targetPort: 80
      protocol: TCP
  selector:
    service: webapp
EOF
minikube kubectl -- apply -f kubernetes.yaml
#start minikube tunnel in background
minikube tunnel &
tunnel_pid=$!

#get external ip
minikube kubectl -- get svc

external_ip=$(minikube kubectl -- get svc | tail -1 | awk '{print $4}')

curl $external_ip
#kill minikube tunnel
kill $tunnel_pid

```

# 关键提示(理解k8s Cluster)
    ![overview](https://d33wubrfki0l68.cloudfront.net/8700a7f5f0008913aa6c25a1b26c08461e4947c7/cfc2c/docs/tutorials/kubernetes-basics/public/images/module_02_first_app.svg)

# 关键提示(理解Pod和Node)
    ![overview](https://d33wubrfki0l68.cloudfront.net/fe03f68d8ede9815184852ca2a4fd30325e5d15a/98064/docs/tutorials/kubernetes-basics/public/images/module_03_pods.svg)

    ![overview](https://d33wubrfki0l68.cloudfront.net/5cb72d407cbe2755e581b6de757e0d81760d5b86/a9df9/docs/tutorials/kubernetes-basics/public/images/module_03_nodes.svg)

# 关键提示(Label、服务、部署)

    1. 从服务角度整体理解；
    2. 从部署角度理解；
    3. 理解Label的重要性

    ![overview](https://d33wubrfki0l68.cloudfront.net/7a13fe12acc9ea0728460c482c67e0eb31ff5303/2c8a7/docs/tutorials/kubernetes-basics/public/images/module_04_labels.svg overview)

# 部署一个自己的asp.net core程序

    假设在dotnet_aspnet中建立了一个简单的asp.net core程序，这个程序的名字是dotnet_aspnet, 并增加了Docker支持。

    下面代码编译Docker镜像，并部署。

```bash
# 切换docker 上下文为minikube
eval $(minikube -p minikube docker-env)
# 构建镜像
docker build -t dotnet_aspnet dotnet_aspnet
cat > k8s_dotnet.yaml <<EOF
apiVersion: apps/v1
kind: Deployment
metadata:
  name: webapp
  labels:
    app: weather-forecast
spec:
  replicas: 1
  selector:
    matchLabels:
      service: webapp
  template:
    metadata:
      labels:
        app: weather-forecast
        service: webapp
    spec:
      containers:
        - name: webapp
          image:  mcr.microsoft.com/dotnet/samples:aspnetapp
          imagePullPolicy: Never
          ports:
            - containerPort: 80
              protocol: TCP
          env:
            - name: ASPNETCORE_URLS
              value: http://+:80
            - name: WebApiBaseAddress
              value: http://webapi
            - name: TTT
              value: Hello_TTT
---
apiVersion: v1
kind: Service
metadata:
  name: webapp
  labels:
    app: weather-forecast
    service: webapp
spec:
  type: LoadBalancer
  ports:
    - port: 80
      targetPort: 80
      protocol: TCP
  selector:
    service: webapp
                    
EOF
# 运行k8s
minikube kubectl -- apply -f k8s_dotnet.yaml
#start minikube tunnel in background
minikube tunnel &
tunnel_pid=$!

#get external ip
minikube kubectl -- get svc

external_ip=$(minikube kubectl -- get svc | tail -1 | awk '{print $4}')

curl $external_ip
#kill minikube tunnel
kill $tunnel_pid
```

# ref
0. https://d33wubrfki0l68.cloudfront.net/8700a7f5f0008913aa6c25a1b26c08461e4947c7/cfc2c/docs/tutorials/kubernetes-basics/public/images/module_02_first_app.svg
1. https://d33wubrfki0l68.cloudfront.net/fe03f68d8ede9815184852ca2a4fd30325e5d15a/98064/docs/tutorials/kubernetes-basics/public/images/module_03_pods.svg
2. https://d33wubrfki0l68.cloudfront.net/7a13fe12acc9ea0728460c482c67e0eb31ff5303/2c8a7/docs/tutorials/kubernetes-basics/public/images/module_04_labels.svg
3. https://kubernetes.io/docs/tutorials/kubernetes-basics/
4. https://docs.microsoft.com/en-us/dotnet/architecture/containerized-lifecycle/design-develop-containerized-apps/build-aspnet-core-applications-linux-containers-aks-kubernetes
5. https://hub.docker.com/_/microsoft-dotnet-samples
6. https://github.com/dotnet/dotnet-docker/tree/main/samples/
