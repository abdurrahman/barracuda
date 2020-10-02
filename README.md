# Barracuda Message Service Tool

A simple message service tool

## Prerequisites

* [.NET Core SDK (3.1.4)](https://dotnet.microsoft.com/download/dotnet-core/3.1)

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

## Todo

* Add user block feature
* Exception handling
* Group or private chat feature
* Unit tests

## Built with

* C#, .NET Core
* EntityFramework Core
* FluentValidation
* Mapster
* Swagger

## License
[MIT](LICENSE.md)
