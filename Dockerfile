FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["WeatherCord.csproj", "./"]
RUN dotnet restore "WeatherCord.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "WeatherCord.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WeatherCord.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WeatherCord.dll"]
