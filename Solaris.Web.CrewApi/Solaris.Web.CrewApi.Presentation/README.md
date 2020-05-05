# Solaris.Web.CrewApi

## Description

**Solaris.Web.CrewApi** is the back-end API which exposes features that manage Explorers Teams and their actions.
The API is managing root type and persisting the data.
A GraphQL schema provides a root type for each kind of operation.
Also, CrewApi is initiating and finalising flows that involve other micro services. The main functionality right now is to send
robots to a planet to explore it.

## Used Technologies

- .Net Core Console App
- [GraphQL for .NET] (https://github.com/graphql-dotnet/graphql-dotnet) This is an implementation of Facebook's GraphQL in .NET.
- RabbitMQ Client
- Pomelo EF
- MySql Server
- NLog logging


## Installing
Hosting machine requires:
- [.Net Core v3.1] (https://dotnet.microsoft.com/download).
- Install an RabbitMQ server
- [MySQL] (https://www.mysql.com/downloads/) for ease of use you should also get workbanch, or some IDE to visual connect to MySQL server.

      
RabbitMQ Configurations
In order to support the background running tasks that send messages to RabbitMQ, we need to install it on the local environment, it should be already configured for the other environments (staging/integration).
You can install it and configure it by following these steps:
   - Download & Install RabbitMQ server from  https://www.rabbitmq.com/install-windows.html (This will also install Erlang)
   - Open RabbitMQ Command Prompt and run the following command: ```rabbitmq-plugins enable rabbitmq_management```. This will enable the UI management tools.
   - Download the x-delayed-message RabbitMQ plugin from: http://www.rabbitmq.com/community-plugins.html , unpack it into ```Drive:\{{Instal_Dir}}\plugins```
   - Access the http://localhost:15672 with the default user.

      
## Deployment
Mandatory Environment variables are:
- APPSETTINGS__RABBITMQ__HOST 
- APPSETTINGS__RABBITMQ__PASSWORD
- APPSETTINGS__RABBITMQ__PORT
- APPSETTINGS__RABBITMQ__USERNAME
- CONNECTIONSTRINGS__CREW_API

Other optional Env Variables can be added to override the fields in the appSettings.json file.

## Input and Output
GraphQL schema can be discovered using [GraphiQL](https://electronjs.org/apps/graphiql)
http://localhost:5001/graphql

## Entity Framework Migrations
Run the following command from **Solaris.Web.CrewApi.Presentation** folder:

**dotnet ef migrations add {{name}} -c DataContext**


## Versioning

GitHub for versioning.