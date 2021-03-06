# Barracuda Message Service [![Build Status](https://travis-ci.org/abdurrahman/barracuda.svg?branch=master)](https://travis-ci.org/abdurrahman/barracuda)

A simple messaging service tool with written .NET Core

> A barracuda is a large, predatory, ray-finned fish known for its fearsome appearance and ferocious behaviour. ~Wiki 
>
> I was looking for a new hdd for myself, so i was impressed the Segate Barracuda version name and named this project as Barracuda. I know its not similar with the service tool what i made but its kinda cool name i think. :)

## Prerequisites

* [.NET Core SDK (3.1.4)](https://dotnet.microsoft.com/download/dotnet-core/3.1)
* Postgresql
* Docker

## Installation

```shell
git clone https://github.com/abdurrahman/barracuda.git
cd barracuda
dotnet build
dotnet test
```

### Db Migration

You can run the migration by running these commands from the root folder (where the .sln file is located):

* Adding migration

```shell
dotnet ef migrations add InitialCreate --project src/Barracuda.Application/Barracuda.Application.csproj --startup-project src/Barracuda.WebApi/Barracuda.WebApi.csproj
```

* Applying migration

```shell
dotnet ef database update --project src/Barracuda.Application/Barracuda.Application.csproj --startup-project src/Barracuda.WebApi/Barracuda.WebApi.csproj
```

## Running the app using Docker

You can run the Barracuda service by running these commands from the root folder (where the .sln file is located):

```
docker-compose build
docker-compose up
```

## Api Documentation

### Account Management

> POST: api/account/register

Request Body:
```json
{
    "firstName": "Abdurrahman",
    "lastName": "Işık",
    "userName": "xJason21",
    "email": "xjason@idsoftware.com",
    "password": "123456"
}
```

> POST: api/account/login

Request Body:
```json
{
    "userName": "xJason21",
    "password": "123456",
    "rememberMe": false
}
```

### Message management

*Retrieving messages*

> GET: api/messages
Response Body:
```json
[
    {
        "text": "Merhaba",
        "recipientId": "recipientId",
        "senderId": "userId"
    },
    {
        "text": "Merhaba, nasılsın ?",
        "recipientId": "userId",
        "senderId": "recipientId"
    },
]
```

> GET: api/messages/1

```json
{
    "text": "Merhaba",
    "recipientId": "recipientId",
    "senderId": "userId"
}
```

*Send new message*

> POST api/messages

Request Body:
```json
{
    "text": "Merhaba",
    "recipientId": "recipientId",
    "senderId": "userId"
}
```

## Todo

* ~~Add user block feature~~
* Exception handling
* Group or private chat feature
* ~~Unit tests (% 30)~~

## Built with

* C#, .NET Core
* Posgresql
* EntityFramework Core
* FluentValidation
* Mapster
* Swagger
* Docker, Docker Compose

## License
[MIT](LICENSE.md)
