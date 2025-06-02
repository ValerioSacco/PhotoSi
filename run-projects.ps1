# Start-Process powershell -ArgumentList "dotnet run --project src/PhotoSi.UsersService/PhotoSi.UsersService.csproj"
# Start-Process powershell -ArgumentList "dotnet run --project src/PhotoSi.OrdersService/PhotoSi.OrdersService.csproj"
# Start-Process powershell -ArgumentList "dotnet run --project src/PhotoSi.ProductsService/PhotoSi.ProductsService.csproj"
# Start-Process powershell -ArgumentList "dotnet run --project src/PhotoSi.AddressBookService/PhotoSi.AddressBookService.csproj"
# Start-Process powershell -ArgumentList "dotnet run --project src/PhotoSi.GatewayService/PhotoSi.GatewayService.csproj"

dotnet run --project src/PhotoSi.UsersService/PhotoSi.UsersService.csproj &
dotnet run --project src/PhotoSi.OrdersService/PhotoSi.OrdersService.csproj &
dotnet run --project src/PhotoSi.ProductsService/PhotoSi.ProductsService.csproj &
dotnet run --project src/PhotoSi.AddressBookService/PhotoSi.AddressBookService.csproj &
dotnet run --project src/PhotoSi.GatewayService/PhotoSi.GatewayService.csproj