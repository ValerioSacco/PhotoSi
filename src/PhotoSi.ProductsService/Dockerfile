FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/PhotoSi.ProductsService/PhotoSi.ProductsService.csproj", "src/PhotoSi.ProductsService/"]
RUN dotnet restore "./src/PhotoSi.ProductsService/PhotoSi.ProductsService.csproj"
COPY . .
WORKDIR "/src/src/PhotoSi.ProductsService"
RUN dotnet build "./PhotoSi.ProductsService.csproj" -c $BUILD_CONFIGURATION -o /app/build


FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./PhotoSi.ProductsService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false


FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PhotoSi.ProductsService.dll"]