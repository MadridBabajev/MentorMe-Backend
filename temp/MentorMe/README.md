# icd0021-22-23-s

# MentorMe WebApp

# 1. Generate db migration

~~~bash
# environment variables on my machine: 
# C:\Program Files\Docker\Docker\resources\bin  
# C:\Program Files\dotnet 
# cd to the main directory: 
# cd AllProjects/RiderProjects/icd0021-22-23-s/temp/MentorMe

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

# 3. Run Docker Compose

~~~bash
# Start the DB service 
cd icd0021-22-23-s
docker-compose build
docker-compose up #-d to run the service in the background
# End the DB service
docker-compose down
~~~

# 4. Generate Identity UI

~~~bash
cd WebApp
dotnet aspnet-codegenerator identity -dc DAL.EF.App.ApplicationDbContext --userClass AppUser -f 
~~~

# 5. Required Nuget packages
* Microsoft.AspNetCore.Authentication.JwtBearer
* Microsoft.IdentityModel.Tokens
* ...

# 6. Run the live server

~~~bash
dotnet watch run --project WebApp
~~~

# Student Data

## Madrid Babajev

## mababa

## 213325IADB
