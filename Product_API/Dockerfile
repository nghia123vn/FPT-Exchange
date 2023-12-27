#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app


FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Product_API/Product_API.csproj", "Product_API/"]
COPY ["FPT_Exchange_Data/FPT_Exchange_Data.csproj", "FPT_Exchange_Data/"]
COPY ["FPT_Exchange_Utility/FPT_Exchange_Utility.csproj", "FPT_Exchange_Utility/"]
COPY ["FPT_Exchange_Service/FPT_Exchange_Service.csproj", "FPT_Exchange_Service/"]
RUN dotnet restore "Product_API/Product_API.csproj"
COPY . .
WORKDIR "/src/Product_API"
RUN dotnet build "Product_API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Product_API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 3000
ENTRYPOINT ["dotnet", "Product_API.dll"]