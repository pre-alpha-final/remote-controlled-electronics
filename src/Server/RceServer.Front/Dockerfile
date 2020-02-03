#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["RceServer.Front/RceServer.Front.csproj", "RceServer.Front/"]
COPY ["RceServer.Data.Identity/RceServer.Data.Identity.csproj", "RceServer.Data.Identity/"]
COPY ["RceServer.Core/RceServer.Core.csproj", "RceServer.Core/"]
COPY ["RceServer.Domain/RceServer.Domain.csproj", "RceServer.Domain/"]
COPY ["RceServer.Data/RceServer.Data.csproj", "RceServer.Data/"]
RUN dotnet restore "RceServer.Front/RceServer.Front.csproj"
COPY . .
WORKDIR "/src/RceServer.Front"
RUN dotnet build "RceServer.Front.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RceServer.Front.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RceServer.Front.dll"]