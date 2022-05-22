# Bookshop Catalog Tutorial

## Running The App

1. Download docker
2. In the root directory, there will be a `docker-compose.yaml` file
  - From a terminal run `docker-compose up -d` to spin up the SQL Server DB and the API together
  - If this is a first run, the DB will take some time to initiate and apply the Entity Framework Core migrations
  - Navigate to `http://localhost:5200` on the same machine and you should see the Swagger UI page

## Debugging The App

1. Using docker from the root directory, run `docker-compose up book-catalog-db`
  - This will run the SQL container in isolation, allowing you to launch the app from Visual Studio
  - In Visual Studio, select the `bookshop_catalog` launch  profile to debug the app as normal


## Application Assumptions

1. The traffic to the API will not need to scale in any significant way and as such, a simple CRUD API without any need for complex logic can be implemented
    - Design patterns like DDD are not required as a result of this.
2. Beyond simple CRUD operations, additional features would be beneficial, with the obvious one being pagination of the GET Books request
3. The App should be easy to develop and test

## Roadmap (and known limitations due to time scale)

1. Fix the Update request
    - The update request behaves unusually and doesn't take into account the supplied ID at present
2. Tidy up the Swagger interface to allow Enums to be represented as strings (for Sorting field)
3. Make the API more robust in terms of it's models, separating request from response models and updating required fields
  so they are more appropriate to the relevant requests
4. Add in a logger sink for debugging in production (i.e .Serilog)
5. Additional unit testing for the API endpoints
