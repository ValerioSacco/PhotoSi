#!/bin/bash
dotnet run --project src/PhotoSi.UsersService/PhotoSi.UsersService.csproj &
dotnet run --project src/PhotoSi.OrdersService/PhotoSi.OrdersService.csproj &
dotnet run --project src/PhotoSi.ProductsService/PhotoSi.ProductsService.csproj &
dotnet run --project src/PhotoSi.AddressBookService/PhotoSi.AddressBookService.csproj &
dotnet run --project src/PhotoSi.GatewayService/PhotoSi.GatewayService.csproj