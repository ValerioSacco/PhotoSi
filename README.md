# PhotoSi Prova Tecnica
The project is a simplified order management platform which enables users to place or modify orders for a given product catalog and users. The system is built using a microservices architecture, with each service responsible for a specific domain: users, orders, products, and address book. The services communicate with each other using asynchronous messaging via MassTransit and RabbitMQ.
Each service owns its own SqlLite database and the data is persisted using Entity Framework Core.

<u>The project is not designed to be a production system and it is meant to be run locally for development and testing purposes.</u>

Here's a brief overview of the services:
### Orders Service
The Orders Service is responsible for managing orders, including creating, updating, and retrieving orders. It receives from the products service and users service the events related to the product and users CRUD operations and stores the data in its own database.<br>
The service enables the user to get the list of orders, get a specific order by ID, create a new order, update the existing order lines of a given order, and delete an order. For each operation the system checks the validity of the user and product associated to the order, ensuring that the user exists and the product is available in the catalog..<br>

### Users Service
The Users Service provides user management functionality, allowing all the CRUD operations on the user data. When a user is modified a synchronous check on the address book service to check the validity of the shipment address associated to the user is performed.<br>
All the updates on the user are sent asynchronously to the Orders Service using RabbitMQ as a message broker.

### Products Service
The Products Service manages the product catalog, allowing for CRUD operations on products. Whenever a product is updated a check on the category is performed assuring that every product is associated to a valid category. The service makes available checking what the available categories are with dedicated GET endpoints. <br>
It also sends events to the Orders Service when a product is created, updated, or deleted, allowing the Orders Service to keep its product data in sync.

### Address Book Service
The service expose only GET endpoints to retrieve the list of available shipment addresses and the details of a specific address. It is used by the Users Service to validate the shipment address associated to a user when the user is modified.<br>

### Gateway Service
The interaction with the system is centralized using a gateway service, built with Yarp, which exposes a single entry point for all client requests. It exposes also the Swagger UI for each service, allowing users to interact with the APIs.

## How to run

1. **Prerequisites**<br>
   In order to run the project, you need to have the following installed on your machine:
   - [.NET SDK 8.0 or later](https://dotnet.microsoft.com/download)
   - [Docker](https://www.docker.com/get-started)

   Check that you have the .NET8 SDK installed by running:
   ```sh
   dotnet --list-sdks
   ```
   Check that you have Docker installed and running by executing:
   ```sh
   docker -v
   ```
   Docker is needed to run run the rabbit MQ container which enables the communication between services.

2. **Run RabbitMQ container locally**<br>
   Open a terminal and run the following command to start a RabbitMQ container with the management plugin enabled:
   ```sh
   docker run -d --name photosi.rabbitmq -p 5672:5672 -p 15672:15672 -e RABBITMQ_DEFAULT_USER=photosi -e RABBITMQ_DEFAULT_PASS=photosi rabbitmq:3-management
   ```
   If you don't already have the docker image locally available, the command will download it from the Docker Hub and it could take a few minutes.<br>
   Check that the RabbitMQ container is running by executing:
   ```sh
   docker ps
   ```
   The result should be like this:
   
![RabbitMQ container running](assets/docker-check.png)

2. **Test and Build the solution**
   Open a terminal on the root directory of the project, which contains the `PhotoSi.sln` file, and run the following command to build and test the solution:

   ```sh
   dotnet build
   dotnet test
   ```

3. **Run the project**
   In the root directory there are two scripts run-projects.ps1 and run-projects.sh that can be used to run the services in development mode. These scripts will start each service in a separate terminal window, allowing you to see the logs and interact with the services.<br>

   To start the services run the script for your operating system, in the root directory of the project.<br>

   Is it also possible to run each service individually by navigating to the service directory and running the following command:

   ```sh
   dotnet run --project src/PhotoSi.UsersService/PhotoSi.UsersService.csproj
   dotnet run --project src/PhotoSi.OrdersService/PhotoSi.OrdersService.csproj
   dotnet run --project src/PhotoSi.ProductsService/PhotoSi.ProductsService.csproj
   dotnet run --project src/PhotoSi.AddressBookService/PhotoSi.AddressBookService.csproj
   dotnet run --project src/PhotoSi.Gateway/PhotoSi.Gateway.csproj
   ```

   The services will start on different ports, as defined in the `appsettings.Development.json` files of each service. The gateway service will be available at `http://localhost:8080` by default, and it will route requests to the appropriate service based on the URL path.<br>
   At the start of each services the migrations on the databases are applied automatically, creating the necessary tables and seeding the databases with some initial data.<br>

   The services are set to run on the flowwing ports, make sure that the ports are avilable on your machine:
   | Service Name          | Port  |
   |-----------------------|-------|
   | Address Book Service  | 6781  |
   | Users Service         | 6782  |
   | Products Service      | 6783  |
   | Orders Service        | 6784  |
   | Gateway               | 8080  |


5. **Access the APIs**
   To access the APIs navigate to `http://localhost:8080/swagger` in your web browser. This will open the Swagger UI, where you can interact with the APIs of each service, choosing the service in the top-right dropdown. If you don't see all the services available try refreshing the browser cache with Ctrl+F5.<br>
   For each endpoint a brief description is provided, along with the request and response models. You can also test the endpoints directly from the Swagger UI by clicking on the "Try it out" button.<br>

![Swagger docs](assets/swagger.png)

