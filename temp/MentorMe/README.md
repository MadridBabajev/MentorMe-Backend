# icd0021-22-23-s

# MentorMe WebApp

# 1. Generate db migration

~~~bash
# environment variables on my machine: 
# C:\Program Files\Docker\Docker\resources\bin  
# C:\Program Files\dotnet 
# cd to the main directory: 
# cd RiderProjects\icd0021-22-23-s\temp\MentorMe

# install or update
dotnet tool install --global dotnet-ef
dotnet tool update --global dotnet-ef

# create migration
dotnet ef migrations add Initial --project App.DAL.EF --startup-project WebApp --context ApplicationDbContext 

# reverse db changes
dotnet ef database update LastGoodMigration

# apply migration
dotnet ef database update --project App.DAL.EF --startup-project WebApp --context ApplicationDbContext 
~~~

# 2. generate rest controllers

Add nuget packages
- Microsoft.VisualStudio.Web.CodeGeneration.Design
- Microsoft.EntityFrameworkCore.SqlServer
~~~bash
# install tooling
dotnet tool install --global dotnet-aspnet-codegenerator
dotnet tool update --global dotnet-aspnet-codegenerator

cd WebApp
# MVC
dotnet aspnet-codegenerator controller -m Student -name StudentsController -outDir Controllers -dc ApplicationDbContext  -udl --referenceScriptLibraries -f
# Rest API
dotnet aspnet-codegenerator controller -m Student -name StudentsController -outDir ApiControllers -api -dc ApplicationDbContext -udl -f
~~~

# 3. Download required Nuget packages
* Microsoft.AspNetCore.Authentication.JwtBearer
* Microsoft.IdentityModel.Tokens
* AutoMapper.Extensions.Microsoft.DependencyInjection
* Swashbuckle.AspNetCore.SwaggerGen
* Asp.Versioning.ApiExplorer
* Microsoft.AspNetCore.Mvc.Versioning
* Asp.Versioning.Mvc.ApiExplorer

# 4. Run the live server (To see the swagger documentation)

~~~bash
dotnet watch run --project WebApp
~~~

# 7. Docker deployment

~~~bash
# Updating and running the containers (locally)
docker-compose up -d --build
# Deploying the container to the DockerHub
docker buildx build --progress=plain  -t webapp:latest .
docker tag webapp madridbabajev/mb-distributed-22-23-app:latest
docker push madridbabajev/mb-distributed-22-23-app:latest
~~~

# Student Data

## Madrid Babajev

## mababa

## 213325IADB
