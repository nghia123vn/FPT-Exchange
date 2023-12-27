#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Service.OAuth/Service.OAuth.csproj", "Service.OAuth/"]
COPY ["FPT_Exchange_Data/FPT_Exchange_Data.csproj", "FPT_Exchange_Data/"]
COPY ["FPT_Exchange_Utility/FPT_Exchange_Utility.csproj", "FPT_Exchange_Utility/"]
COPY ["FPT_Exchange_Service/FPT_Exchange_Service.csproj", "FPT_Exchange_Service/"]
RUN dotnet restore "Service.OAuth/Service.OAuth.csproj"
COPY . .
WORKDIR "/src/Service.OAuth"
RUN dotnet build "Service.OAuth.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Service.OAuth.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Service.OAuth.dll"]