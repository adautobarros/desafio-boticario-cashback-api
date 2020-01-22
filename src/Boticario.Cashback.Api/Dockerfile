#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/Boticario.Cashback.Api/Boticario.Cashback.Api.csproj", "src/Boticario.Cashback.Api/"]
COPY ["src/Boticario.Cashback.IoC/Boticario.Cashback.IoC.csproj", "src/Boticario.Cashback.IoC/"]
COPY ["src/Boticario.Cashback.Infra/Boticario.Cashback.Infra.csproj", "src/Boticario.Cashback.Infra/"]
COPY ["src/Boticario.Cashback.Dominio/Boticario.Cashback.Dominio.csproj", "src/Boticario.Cashback.Dominio/"]
RUN dotnet restore "src/Boticario.Cashback.Api/Boticario.Cashback.Api.csproj"
COPY . .
WORKDIR "/src/src/Boticario.Cashback.Api"
RUN dotnet build "Boticario.Cashback.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Boticario.Cashback.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Boticario.Cashback.Api.dll"]