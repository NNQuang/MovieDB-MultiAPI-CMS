## MovieDB-MultiAPI-CMS

MovieDB-MultiAPI-CMS is a Imdb-like movie database website which is created with .NET5.

![MovieDB-MultiAPI-CMS Project Diagram!](https://github.com/ilovepilav/MovieDB-MultiAPI-CMS/blob/main/ClientService/wwwroot/img/moviedb-microservice-diagram.png)

In this repository i developed a movie catalog website using .NET5. When creating the project my main goal was becoming familiar with WebAPIs and Distributed Applications. You can check project live [cagrisakaoglu.com/movie](https://cagrisakaoglu.com/movie)

---

### Project Summary

#### Servers

I used 3 Google Cloud Micro instances and 1 Azure Cloud instance for my services. All servers are free and running Debian 10. I used NGINX in front of Kestrel as proxy server. Nginx is easy to use and familiar to me.

#### Databases

I used 2 PostgreSQL with [Heroku](https://heroku.com) add-ons and 1 GraphQL database from [Fauna](https://fauna.com). Both of them are free at least some degree.

#### Language

Project written by C# .NET 5 but also includes some JavaScript(Ajax, jquery) and for scraping movie data from web i used my Python Scraper which i built earlier, you can check that script [here](https://github.com/ilovepilav/IMDB-Scrapper-v1.0).

---

## [Services](#)

### [Client Service](https://github.com/ilovepilav/MovieDB-MultiAPI-CMS/tree/main/MovieHome)

- **UI Area**
  This is project's frontend. Basicly it is a MVC application. In here i used mostly Twitter's Bootstrap. Added some extra CSS & JavaScript when needed. API calls for rendering views of movies, actors etc.

- **Admin Area**
  This is restricted area for standart users. JWT based authentification for role claiming. All API calls for CRUD operations working from here. This section adding CMS feature to the project.

### - [ApiGateway Service](https://github.com/ilovepilav/MovieDB-MultiAPI-CMS/tree/main/ApiGateway)

This is the only service that client talks. Basicly getting requests from client service and forwarding them which endpoint they need to go. I used Ocelot library for creating a simple gateway. Ocelot has a lot of features like load balancing etc.

### - [Auth Service](https://github.com/ilovepilav/MovieDB-MultiAPI-CMS/tree/main/MovieUser)

This service is responsible for Token (JWT) generation.With 'Code First Approach' this service is linked to an external PostgreSQL database from Heroku which is storing Users' data. Running CRUD operations for User entities and managing authentification. Creating password hashes for Users. Responsing to login/register requests with Token as JSON.

### - [MovieDB Service](https://github.com/ilovepilav/MovieDB-MultiAPI-CMS/tree/main/MovieDB)

This service is managing CRUD operations and Business logic. With 'Code First Approach' this service is linked to an external PostgreSQL database from Heroku which is storing Movie related entities(Movie, Director, Genre, Actor).

---

## Admin Panel

Project has its own content management system. You can add/update/delete any movie related data also you can manage users and user comments.

<iframe width="560" height="315" src="https://www.youtube.com/embed/mkkgSOabAJo" title="YouTube video player" frameborder="0" allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture" allowfullscreen></iframe>

---

### [Autofac](https://autofac.org/)

<img src="https://camo.githubusercontent.com/77b2b9441be3bce3bf4658ecf79147121bc30aefb65dd1ae151a44c5954cf7d8/68747470733a2f2f6175746f6661632e6f72672f696d672f6175746f6661635f6c6f676f2d747970652e737667" width="400" alt="autofac">

Autofac is an IOC container for .NET applications. I used Autofac in my Auth service for registering my services. .NET already has its own IOC container which i used it project's other services.

```csharp
using Autofac;

namespace MovieUser.Business.DependencyResolvers.Autofac
{
    public class AutofacBusinessModule:Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UserManager>().As<IUserService>();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();
            builder.RegisterType<AuthManager>().As<IAuthService>();
            builder.RegisterType<JwtHelper>().As<ITokenHelper>();
        }
    }
}
```

---

### [AutoMapper](https://automapper.org/)

![automapper](https://camo.githubusercontent.com/603a9fdf1c6578e4df423ecdb784cb5d634e016850c10ba0798970fd48c55d41/68747470733a2f2f73332e616d617a6f6e6177732e636f6d2f6175746f6d61707065722f6c6f676f2e706e67)

AutoMapper is a simple little library built to solve a deceptively complex problem - getting rid of code that mapped one object to another. I used AutoMapper whenever i need to map my models/dtos/entites. It is very useful.

```csharp

using AutoMapper;
using MovieDB.Entities.Concrete;
using MovieDB.Entities.Dtos;

namespace MovieDB.Business.AutoMapper
{
    public class ActorProfile : Profile
    {
        public ActorProfile()
        {
            CreateMap<ActorAddDto, Actor>().ReverseMap();
            CreateMap<ActorAutoCreateDto, Actor>().ReverseMap();
            CreateMap<ActorUpdateDto, Actor>().ReverseMap();
        }
    }
}

```

---

### [Ocelot](https://github.com/ThreeMammals/Ocelot)

![ocelot](https://camo.githubusercontent.com/8b08ab2cc03f69ce7928453809ec98d487fd5698389218b88aa6adf0e42faa6d/68747470733a2f2f74687265656d616d6d616c732e636f6d2f696d616765732f6f63656c6f745f6c6f676f2e706e67)

> Ocelot is a .NET API Gateway. This project is aimed at people using .NET running a micro services / service oriented architecture that need a unified point of entry into their system. However it will work with anything that speaks HTTP and run on any platform that ASP.NET Core supports.

Check my [ocelot.json](https://github.com/ilovepilav/MovieDB-MultiAPI-CMS/blob/main/ApiGateway/ocelot.json) file previewed below.

```json

{
  "Routes": [
    {
      "DownstreamPathTemplate": "/Api/Auth/{url}",
      "DownstreamScheme": "http",
      "DownstreamHostAndPorts": [
        {
          "Host": "Insert Auth Service IP",
          "Port": 80
        }
      ],
      "UpstreamPathTemplate": "/Auth/{url}",
      "UpstreamHttpMethod": ["POST"]
    },

    ....

```

---

### [GraphQL](https://graphql.org/)

<img src="https://graphql.org/img/logo.svg" width="300" alt="graphql">

> GraphQL is an open-source data query and manipulation language for APIs, and a runtime for fulfilling queries with existing data. GraphQL was developed internally by Facebook in 2012 before being publicly released in 2015.

I used GraphQL for querying User comments on the web application.

```javascript
type Comment {
  user: String!
  content: String!
  date: String!
  like: Int!
  movieTitle: String!
  isActive: Boolean!
}

type Query {
  activeCommentsByMovieTitle(movieTitle: String!, isActive:Boolean!): [Comment!]
  allComments: [Comment!]!
  allActiveComments(isActive: Boolean!): [Comment!]
}
```

---

### [Fauna](https://fauna.com/)

<img src="https://images.ctfassets.net/po4qc9xpmpuh/7itYmeRxmVGIXwwGWHrQU3/e4ea73c2bebc64bd65d84964576515b9/fauna-logo-new-v2.svg" width="400" alt="faunadb">

> The data API for modern applications Fauna is a flexible, developer-friendly, transactional database available as a secure, cloud API with native GraphQL. Never again worry about database provisioning, maintenance, scaling, sharding, replication, or correctness.

Fauna has very user friendly interface especially for GraphQL newbies. They have their own query language plus GraphQL support. I used fauna for my Comment entity.

---

**Developer's Note:** I used lots of concepts, patterns (Generic Repository, Unit of Work, Factory etc.), tested lots of new stuff and gained huge experience. I know it seems overkill for using many resources(basicly free) for this kind of CMS project but my main goal is achieved which is experiencing distributed applications, using APIs, building a fun project with new technologies, writing code like working an enterprise level project etc.

---

##### All Dependencies

"Ocelot"

"Npgsql"

"Autofac"

"GraphQL"

"AutoMapper"

"GraphQL.Client"

"Newtonsoft.Json"

"GraphQL.Client.Abstractions"

"Microsoft.AspNetCore.Session"

"Microsoft.IdentityModel.Tokens"

"Microsoft.EntityFrameworkCore"

"Microsoft.AspNet.WebApi.Client"

"Microsoft.AspNetCore.JsonPatch"

"System.IdentityModel.Tokens.Jwt"

"Microsoft.Extensions.Configuration"

"GraphQL.Client.Serializer.Newtonsoft"

"Microsoft.EntityFrameworkCore.Tools"

"Microsoft.EntityFrameworkCore.Design"

"Autofac.Extensions.DependencyInjection"

"Microsoft.Extensions.Configuration.Binder"

"Npgsql.EntityFrameworkCore.PostgreSQL"

"Microsoft.Extensions.DependencyInjection"

"Microsoft.AspNetCore.Authentication.JwtBearer"

"Microsoft.Extensions.Configuration.Abstractions"

"Microsoft.VisualStudio.Web.CodeGeneration.Design"

"Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation"

"AutoMapper.Extensions.Microsoft.DependencyInjection"
