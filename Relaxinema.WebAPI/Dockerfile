#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
COPY ["Relaxinema.Infrastructure/Seed/UserSeedData.json", "/app/"]
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Relaxinema.WebAPI/Relaxinema.WebAPI.csproj", "Relaxinema.WebAPI/"]
COPY ["Relaxinema.Core/Relaxinema.Core.csproj", "Relaxinema.Core/"]
COPY ["Relaxinema.Infrastructure/Relaxinema.Infrastructure.csproj", "Relaxinema.Infrastructure/"]
RUN dotnet restore "Relaxinema.WebAPI/Relaxinema.WebAPI.csproj"
COPY . .
WORKDIR "/src/Relaxinema.WebAPI"
RUN dotnet build "Relaxinema.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Relaxinema.WebAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false


FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Relaxinema.WebAPI.dll"]