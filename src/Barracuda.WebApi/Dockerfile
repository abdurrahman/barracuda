#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /app
COPY . .
#COPY ["src/Barracuda.WebApi/Barracuda.WebApi.csproj", "./Barracuda.WebApi/"]
#RUN dotnet restore "./Barracuda.WebApi/Barracuda.WebApi.csproj"
#COPY . .
WORKDIR "/app/src/Barracuda.WebApi"
RUN dotnet restore

RUN dotnet build "./Barracuda.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "./Barracuda.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Barracuda.WebApi.dll"]