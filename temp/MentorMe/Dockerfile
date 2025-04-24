FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

EXPOSE 80

# copy csproj and restore as distinct layers
COPY *.props .
COPY *.sln .

# copy over all the projects from host to image
#Base
COPY Base.BLL/*.csproj ./Base.BLL/
COPY Base.BLL.Contracts/*.csproj ./Base.BLL.Contracts/
COPY Base.DAL/*.csproj ./Base.DAL/
COPY Base.DAL.Contracts/*.csproj ./Base.DAL.Contracts/
COPY Base.DAL.EF/*.csproj ./Base.DAL.EF/
COPY Base.Domain/*.csproj ./Base.Domain/
COPY Base.Domain.Contracts/*.csproj ./Base.Domain.Contracts/
COPY Base.Mapper.Contracts/*.csproj ./Base.Mapper.Contracts/

#App
COPY Helpers/*.csproj ./Helpers/
COPY ProjectTests/*.csproj ./ProjectTests/
COPY Public.DTO/*.csproj ./Public.DTO/
COPY App.BLL/*.csproj ./App.BLL/
COPY App.BLL.Contracts/*.csproj ./App.BLL.Contracts/
COPY App.DAL.Contracts/*.csproj ./App.DAL.Contracts/
COPY App.DAL.EF/*.csproj ./App.DAL.EF/
COPY App.Domain/*.csproj ./App.Domain/
COPY DAL.DTO/*.csproj ./DAL.DTO/
COPY BLL.DTO/*.csproj ./BLL.DTO/
COPY Public.DTO/*.csproj ./Public.DTO/
COPY WebApp/*.csproj ./WebApp/

RUN dotnet restore

# copy everything else (source files) and build app

#Base
COPY Base.BLL/. ./Base.BLL/
COPY Base.BLL.Contracts/. ./Base.BLL.Contracts/
COPY Base.DAL/. ./Base.DAL/
COPY Base.DAL.Contracts/. ./Base.DAL.Contracts/
COPY Base.DAL.EF/. ./Base.DAL.EF/
COPY Base.Domain/. ./Base.Domain/
COPY Base.Domain.Contracts/. ./Base.Domain.Contracts/
COPY Base.Mapper.Contracts/. ./Base.Mapper.Contracts/

#App
COPY Helpers/. ./Helpers/
COPY ProjectTests/. ./ProjectTests/
COPY Public.DTO/. ./Public.DTO/
COPY App.BLL/. ./App.BLL/
COPY App.BLL.Contracts/. ./App.BLL.Contracts/
COPY App.DAL.Contracts/. ./App.DAL.Contracts/
COPY App.DAL.EF/. ./App.DAL.EF/
COPY App.Domain/. ./App.Domain/
COPY DAL.DTO/. ./DAL.DTO/
COPY BLL.DTO/. ./BLL.DTO/
COPY Public.DTO/. ./Public.DTO/
COPY WebApp/. ./WebApp/

WORKDIR /src/WebApp
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

WORKDIR /app
EXPOSE 80

COPY --from=build /src/WebApp/out ./

ENTRYPOINT ["dotnet", "WebApp.dll"]