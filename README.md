ASP.NET Movies RESTful API
==========================

This is an ASP.NET RESTful API that allows you to manage movies and genres. You can perform CRUD operations on movies and genres, including creating, retrieving, updating, and deleting movies and genres. You can also retrieve all movies, all movies in a certain genre, and a single movie by ID.

Getting Started
---------------

### Prerequisites

-   .NET 5.0
-   Visual Studio 2019 or later
-   SQL Server 2019 or later

### Installing

1.  Clone the repository to your local machine
2.  Open the solution file using Visual Studio
3.  In the `appsettings.json` file, update the `DefaultConnection` string to point to your SQL Server instance
4.  Open the `Package Manager Console` and run the following command to create the database:


```
Update-Database

```

1.  Build and run the solution

API Endpoints
-------------

### Movies

-   `GET /api/movies` : Retrieve all movies
-   `GET /api/movies/{id}` : Retrieve a single movie by ID
-   `GET /api/movies/GetAllInGenre?genreId={genreId}` : Retrieve all movies in a certain genre
-   `POST /api/movies` : Create a new movie
-   `PUT /api/movies/{id}` : Update an existing movie by ID
-   `DELETE /api/movies/{id}` : Delete a movie by ID

### Genres

-   `GET /api/genres` : Retrieve all genres
-   `POST /api/genres` : Create a new genre
-   `PUT /api/genres/{id}` : Update an existing genre by ID
-   `DELETE /api/genres/{id}` : Delete a genre by ID

Built With
----------

<<<<<<< HEAD
-   ASP.NET Core 6.0
-   Entity Framework�Core
-   AutoMapper
=======
-   ASP.NET Core 6.0 Web API 
-   Entity Framework Core
-   AutoMapper
>>>>>>> 4d4f97b47cbb20296a478fb91647cc2168fbfb23
