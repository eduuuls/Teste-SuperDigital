#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["SuperDigital.ContaCorrente.API.csproj", ""]
COPY ["../SuperDigital.ContaCorrente.Infra.CrossCutting.IoC/SuperDigital.ContaCorrente.Infra.CrossCutting.IoC.csproj", "../SuperDigital.ContaCorrente.Infra.CrossCutting.IoC/"]
COPY ["../SuperDigital.ContaCorrente.Domain/SuperDigital.ContaCorrente.Domain.csproj", "../SuperDigital.ContaCorrente.Domain/"]
COPY ["../SuperDigital.ContaCorrente.Infra.Data/SuperDigital.ContaCorrente.Infra.Data.csproj", "../SuperDigital.ContaCorrente.Infra.Data/"]
RUN dotnet restore "./SuperDigital.ContaCorrente.API.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "SuperDigital.ContaCorrente.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SuperDigital.ContaCorrente.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SuperDigital.ContaCorrente.API.dll"]