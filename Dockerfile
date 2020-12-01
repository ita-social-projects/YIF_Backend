#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["YIF_Backend/YIF_Backend.csproj", "YIF_Backend/"]
COPY ["YIF.Core.Domain/YIF.Core.Domain.csproj", "YIF.Core.Domain/"]
COPY ["YIF.Core.Service/YIF.Core.Service.csproj", "YIF.Core.Service/"]
COPY ["YIF.Core.Data/YIF.Core.Data.csproj", "YIF.Core.Data/"]
RUN dotnet restore "YIF_Backend/YIF_Backend.csproj"
COPY . .
WORKDIR "/src/YIF_Backend"
RUN dotnet build "YIF_Backend.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "YIF_Backend.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "YIF_Backend.dll"]