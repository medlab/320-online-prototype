FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["dotnet-console-sample.csproj", "./"]
RUN dotnet restore "dotnet-console-sample.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "dotnet-console-sample.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "dotnet-console-sample.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "dotnet-console-sample.dll"]
