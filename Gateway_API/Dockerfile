#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Gateway_API/Gateway_API.csproj", "Gateway_API/"]
RUN dotnet restore "Gateway_API/Gateway_API.csproj"
COPY . .
WORKDIR "/src/Gateway_API"
RUN dotnet build "Gateway_API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Gateway_API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 3000
ENTRYPOINT ["dotnet", "Gateway_API.dll"]