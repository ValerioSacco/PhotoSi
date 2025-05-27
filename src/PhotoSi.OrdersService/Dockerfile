FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/PhotoSi.OrdersService/PhotoSi.OrdersService.csproj", "src/PhotoSi.OrdersService/"]
RUN dotnet restore "./src/PhotoSi.OrdersService/PhotoSi.OrdersService.csproj"
COPY . .
WORKDIR "/src/src/PhotoSi.OrdersService"
RUN dotnet build "./PhotoSi.OrdersService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./PhotoSi.OrdersService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PhotoSi.OrdersService.dll"]