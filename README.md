# Barracuda Message Service Tool

A simple messaging service tool

## Prerequisites

* [.NET Core SDK (3.1.4)](https://dotnet.microsoft.com/download/dotnet-core/3.1)
* Postgresql

## Installation

```shell
git clone https://github.com/abdurrahman/barracuda.git
cd barracuda
dotnet build
dotnet test
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

* Add user block feature
* Exception handling
* Group or private chat feature
* Unit tests

## Built with

* C#, .NET Core
* Posgresql
* EntityFramework Core
* FluentValidation
* Mapster
* Swagger

## License
[MIT](LICENSE.md)
