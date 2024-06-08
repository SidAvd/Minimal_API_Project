# RESTapi Project
Minimal API that manages a database.

## About the Project
In this project a minimal API manages information in a Database. The project is created with the ASP.NET Framework. The DB tables
are connected with a many-to-many relationship. The managing of the DB information is realized with the use of Entity Framework
ORM. The RDBMS used is Microsoft SQL Server.

## Functionality
With the API running the user can use Postman or Endpoints Explorer in Visual Studio Community and test the endpoints. They
can post JSON formated information to the DB, can get info from the DB, can update DB's already existing records and can 
delete them (CRUD).

In the DB there are three tables: Workers, Errands and the Join Table that coresponds to the many-to-many relationship that the
first two tables have, meaning which workers run which errands:

![database](<Minimal_API_Project/Images/Database.png>)

The user can get all Workers:

![get all workers](<Minimal_API_Project/Images/getallworkers_image.png>)

can get one Worker:

![get one worker](<Minimal_API_Project/Images/getoneworker.png>)

can post a Worker:

![post a worker](<Minimal_API_Project/Images/postaworker.png>)

can update a Worker:

![update worker](<Minimal_API_Project/Images/updateworker.png>)

and can delete a Worker:

![delete worker](<Minimal_API_Project/Images/deleteworker.png>)

The same actions can be done for the Errands table, for example the user can post an errand:

![post an errand](<Minimal_API_Project/Images/postanerrand.png>)

The user also can create relations between a Worker and an Errand by adding their respective ids to the third DB table (Join Table):

![post errand worker](<Minimal_API_Project/Images/posterrandworker.png>)

The user can ask all errand-worker realtionaships:

![get all errandworkers](<Minimal_API_Project/Images/getallerrandworkers.png>)

can ask a specified errand-worker relationship:

![get one errandworker](<Minimal_API_Project/Images/getanerrandworker.png>)

and finally can delete a relationship between a worker and an errand from the DB:

![delete errandworker](<Minimal_API_Project/Images/deleteerrandworker.png>)

## Technical Information
The project contains all the logic of an API that manages a DB. This means that it uses Models (classes) that corespond to the tables
of the DB, it uses the Entity Framework object-relational mapping to relate the DB's tables with the models and realize the many-to-many
relationship that these tables have. Because it is a minimal API the project doesn't use Controllers. It uses Map methods that each 
correspond to an endpoint and fetches the info from the DB that matches to the URI. DTO logic is also used for practicing more safe methods.
