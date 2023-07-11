ASP.NET Movies RESTful API
==========================

This is an ASP.NET RESTful API that allows you to manage movies and genres. You can perform CRUD operations on movies and genres, including creating, retrieving, updating, and deleting movies and genres. You can also retrieve all movies, all movies in a certain genre, and a single movie by ID.

Getting Started
---------------

### Prerequisites

-   .NET 5.0
-   Visual Studio 2019 or later
-   SQL Server 2019 or later

### Installing

1.  Clone the repository to your local machine
2.  Update the `DefaultConnection` string in the `appsettings.json` file to point to your SQL Server instance
3.  Run `Update-Database` command in the `Package Manager Console` to create the database
4.  Build and run the solution

API Endpoints
-------------

### Movies

-   `GET /api/movies`: Retrieve all movies
-   `GET /api/movies/{id}`: Retrieve a single movie by ID
-   `GET /api/movies/GetAllInGenre?genreId={genreId}`: Retrieve all movies in a certain genre
-   `POST /api/movies`: Create a new movie
-   `PUT /api/movies/{id}`: Update an existing movie by ID
-   `DELETE /api/movies/{id}`: Delete a movie by ID

### Genres

-   `GET /api/genres`: Retrieve all genres
-   `POST /api/genres`: Create a new genre
-   `PUT /api/genres/{id}`: Update an existing genre by ID
-   `DELETE /api/genres/{id}`: Delete a genre by ID

Authentication Endpoints
------------------------

### Auth

-   `POST /api/auth/register`: Register a new user
-   `POST /api/auth/login`: Login and get a token
-   `POST /api/auth/addusertorole`: Add a user to a role

### Roles

-   `GET /api/roles`: Retrieve all roles
-   `POST /api/roles`: Create a new role
-   `PUT /api/roles/{id}`: Update an existing role by ID
-   `DELETE /api/roles/{id}`: Delete a role by ID

Authentication and Authorization
--------------------------------

Authentication and authorization are implemented using JWT tokens. The API endpoints are protected by the `[Authorize]` attribute, which requires a valid JWT token to access. The API uses `Microsoft.AspNetCore.Authentication.JwtBearer` and `Microsoft.IdentityModel.Tokens` libraries for authentication and authorization.

Swagger Documentation
---------------------

The API documentation is generated using Swagger. You can access the Swagger UI by navigating to `/swagger` in your browser. The Swagger UI provides an interactive documentation for the API, including all endpoints, request/response schemas, and authentication information.