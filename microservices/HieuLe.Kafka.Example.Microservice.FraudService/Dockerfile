#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["HieuLe.Kafka.Example.Microservice.FraudService/HieuLe.Kafka.Example.Microservice.FraudService.csproj", "HieuLe.Kafka.Example.Microservice.FraudService/"]
RUN dotnet restore "HieuLe.Kafka.Example.Microservice.FraudService/HieuLe.Kafka.Example.Microservice.FraudService.csproj"
COPY . .
WORKDIR "/src/HieuLe.Kafka.Example.Microservice.FraudService"
RUN dotnet build "HieuLe.Kafka.Example.Microservice.FraudService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HieuLe.Kafka.Example.Microservice.FraudService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HieuLe.Kafka.Example.Microservice.FraudService.dll"]