#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BuildConfiguration=Release
RUN --mount=type=secret,id=nuget,dst=/pat cp /pat ~/.nuget/NuGet/NuGet.Config
WORKDIR /src
COPY ["nuget.config", "."]
COPY ["Tgs.GameRecord.JDBService/Tgs.GameRecord.JDBService.csproj", "Tgs.GameRecord.JDBService/"]
COPY ["Tgs.GameRecord.JDBService.Core/Tgs.GameRecord.JDBService.Core.csproj", "Tgs.GameRecord.JDBService.Core/"]
RUN dotnet restore "Tgs.GameRecord.JDBService/Tgs.GameRecord.JDBService.csproj"
COPY . .
WORKDIR "/src/Tgs.GameRecord.JDBService"
RUN dotnet build "Tgs.GameRecord.JDBService.csproj" -c $BuildConfiguration -o /app/build

FROM build AS publish
ARG BuildConfiguration
RUN dotnet publish "Tgs.GameRecord.JDBService.csproj" -c $BuildConfiguration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tgs.GameRecord.JDBService.dll"]