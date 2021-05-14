## This is Your IT Future Backend

## About the project

This is a project that provides applicants with an opportunity to get acquainted with the list of educational institutions that provide education in our region. Applicants can get detailed information about the available specialties that are required for admission, training materials etc.

---

## Main parts of the project and technologies

This project contains 3 main parts: client frontend, backend and database. Frontend part of the project is at https://github.com/ita-social-projects/YIF_Frontend

Let's take a closer look at the technologies of each part.

Client Frontend - some kind of unicorn magic is used [here](https://github.com/ita-social-projects/YIF_Frontend)

Backend - ASP.NET Core 3.1, Doker, FluentValidation, Swagger, Automaper, Identity Server, Azure, Jenkins.

Database - MS SQL SERVER, Microsoft Azure.

Code quality - SonarCloud.

Testing - XUnit

SDLC - Scrum/Kanban

---

## How to start the project locally.


Clone or download the project from https://github.com/ita-social-projects/YIF_Backend

* Install ASP.NET Core 3.1

* Install Entity Framework

* Create local database from https://github.com/ita-social-projects/YIF_Backend

* Open project in Visual Studio or enter command "dotnet run 
    [-c|--configuration <CONFIGURATION>] [-f|--framework <FRAMEWORK>]
    [--force] [--interactive] 
    [--launch-profile <NAME>] [--no-build]
    [--no-dependencies] [--no-launch-profile] 
    [--no-restore] [-p|--project <PATH>] 
    [-r|--runtime <RUNTIME_IDENTIFIER>] [-v|--verbosity <LEVEL>] 
    [[--] [application arguments]]"

---

## How to run the project with docker-compose locally

### Installation Prerequisites:
Git
Docker for Mac, Docker for Windows, or Docker Engine

### Run application in containers:
To run this application, first git clone the project.
* Clone repository from GitHub with:
command:
 git clone https://github.com/ita-social-projects/YIF_Backend.git

You should add appsettings.json to project (folder YIF_Backend/YIF_Backend). 
Check: 
1)the connection string in appsettings.json
"ConnectionStrings": {
    "DefaultConnection": "Server=yifsql,1433;Database=master;User Id=SA;Password=YIF_Backend_DB_MyKeyOnlyInMyHeart;"
  }
  
2) Seeder in Startup.cs
#region Seeder
SeederDB.SeedData(app.ApplicationServices);
#endregion

Then run these commands to start the containers:
 cd YIF_Backend
 docker-compose up --build

Then you can access the web api at http://localhost:5000/swagger/index.html

### Stop containers:
Run command to stop the containers:
 docker-compose stop
 
### Delete containers:
Run command to delete the containers:
 docker-compose down

### Details
### docker-compose.yml file
Note that there are two images - one for the web api (yifbackend) and one for SQL Server (yifsql). The yifsql image uses mcr.microsoft.com/mssql/server:2017-latest as the base. It is the SQL Server image for running on Linux. Whenever you start a mcr.microsoft.com/mssql/server:2017-latest container, you need to pass in some environment variables:

ACCEPT_EULA: "Y"
SA_PASSWORD: "YIF_Backend_DB_MyKeyOnlyInMyHeart" 

For this demo the password is set to 'YIF_Backend_DB_MyKeyOnlyInMyHeart' but you can change it if you want. Make sure you also change it in appsettings.json configuration if you do though!

Note that the server name is 'yifsql'. This is the name that the SQL Server container is known by the web container as on the container network. 
Note the name of the DB is 'master'. You can change this to whatever you want. Entity Framework will attempt to create the DB if it doesnt already exist.
---

## How to run the project with docker-compose on Azure

### Prerequisites:
Azure account
Virtual machine Ubuntu18.04
Allow port: 5000 on Virtual machine
Git (https://git-scm.com/book/en/v2/Getting-Started-Installing-Git)
Docker Engine on Ubuntu (https://docs.docker.com/engine/install/ubuntu/)
Docker-compose (https://docs.docker.com/compose/install/)

### Run application in containers
Look above ## How to run the project with docker-compose locally

Then you can access the web api at http://ip:5000/swagger/index.html

## Team

![@team](https://avatars.githubusercontent.com/u/34924839?s=400&u=c698ded4b7aa4c34491d39b76fb0b7d2436d26e6&v=4)
![@team](https://avatars.githubusercontent.com/u/42476974?s=400&u=b49aa4ca49046de0c87c82da6d48cc37ac08a170&v=4)
![@team](https://avatars.githubusercontent.com/u/44744677?s=400&u=0b6a5ad0c6e7712a53c4ff2c42a24e2aeb0c34a3&v=4)
![@team](https://avatars.githubusercontent.com/u/52170310?s=400&v=4)
![@team](https://avatars.githubusercontent.com/u/55939463?s=400&v=4)
![@team](https://avatars.githubusercontent.com/u/16308549?s=400&u=48b55feed8dad680a02c1633efff050ccfb1ebb2&v=4)

![@team](https://avatars.githubusercontent.com/u/78746301?s=400&u=30423308506a96a6943b287113bb8f6ec3c76ded&v=4)
![@team](https://avatars.githubusercontent.com/u/56673817?s=400&u=f1324d56227074f2c38c314f5a316a2a827a7be4&v=4)
![@team](https://avatars.githubusercontent.com/u/48133795?s=400&u=2a5419941d325d551f95331c953b2ed5add3bf1e&v=4)
![@team](https://avatars.githubusercontent.com/u/62856840?s=400&u=3efd2de912e9adc9ed80036c0b2ded59628d0e90&v=4)
![@team](https://avatars.githubusercontent.com/u/31737653?s=400&v=4)
![@team](https://avatars.githubusercontent.com/u/61685799?v=4&s=400)
![@team](https://avatars.githubusercontent.com/u/73486410?v=4&s=400)

![@team](https://avatars.githubusercontent.com/u/54326631?v=4&s=400)
![@team](https://avatars.githubusercontent.com/u/49400214?v=4&s=400)
![@team](https://avatars.githubusercontent.com/u/51949505?v=4&s=400)
![@team](https://avatars.githubusercontent.com/u/35916945?v=4&s=400)
![@team](https://avatars.githubusercontent.com/u/51949879?v=4&s=400)
![@team](https://avatars.githubusercontent.com/u/58307006?v=4&s=400)
![@team](https://avatars.githubusercontent.com/u/83704219?v=4&s=400)

- https://github.com/shelkon
- https://github.com/Yura-Androshchuk
- https://github.com/Bandera1
- https://github.com/smetanskyy
- https://github.com/KoTuK3
- https://github.com/lyutko

- https://github.com/IvanBorovets
- https://github.com/ProfesorProst
- https://github.com/yuxima
- https://github.com/ArturSodolskyi
- https://github.com/Glebhawk
- https://github.com/stein4444
- https://github.com/DimaMoroziuk

- https://github.com/zm21
- https://github.com/pinkevmladchy
- https://github.com/Greendiax
- https://github.com/EugeneLiutko
- https://github.com/denyshchuk0
- https://github.com/MsLolita
- https://github.com/LukomskyiVitalii

---

### FAQ

- [API](https://drive.google.com/file/d/1f2wuhrevAdIz-Cs4wZ8GyCOvNELu0gXr/view?usp=sharing)

---

- Copyright 2020 © <a href="https://softserve.academy/" target="_blank"> SoftServe IT Academy</a>.
