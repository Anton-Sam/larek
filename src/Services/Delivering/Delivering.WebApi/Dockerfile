#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src



COPY ["src/Services/Delivering/Delivering.WebApi/Delivering.WebApi.csproj", "src/Services/Delivering/Delivering.WebApi/"]
COPY ["src/Services/Delivering/Delivering.Application/Delivering.Application.csproj", "src/Services/Delivering/Delivering.Application/"]
COPY ["src/BuildingBlocks/Application/BuildingBlocks.Application/BuildingBlocks.Application.csproj", "src/BuildingBlocks/Application/BuildingBlocks.Application/"]
COPY ["src/Services/Delivering/Delivering.Domain/Delivering.Domain.csproj", "src/Services/Delivering/Delivering.Domain/"]
COPY ["src/BuildingBlocks/Domain/BuildingBlocks.Domain/BuildingBlocks.Domain.csproj", "src/BuildingBlocks/Domain/BuildingBlocks.Domain/"]
COPY ["src/Services/Delivering/Delivering.Infrastructure/Delivering.Infrastructure.csproj", "src/Services/Delivering/Delivering.Infrastructure/"]
RUN dotnet restore "src/Services/Delivering/Delivering.WebApi/Delivering.WebApi.csproj"
COPY . .
WORKDIR "/src/src/Services/Delivering/Delivering.WebApi"
RUN dotnet build "Delivering.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Delivering.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Delivering.WebApi.dll"]