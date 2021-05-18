#build stage
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 as build

WORKDIR /app

#restore our project and deps 
COPY *.sln .
COPY ./IdentityServer/*.csproj ./IdentityServer/
COPY ./Domain/*.csproj ./Domain/
COPY ./DataAccess/*.csproj ./DataAccess/
COPY ./Persistence/*.csproj ./Persistence/

RUN dotnet restore

#copy rest and publish 
COPY ./IdentityServer/. ./IdentityServer/
COPY ./Domain/. ./Domain
COPY ./DataAccess/. ./DataAccess/
COPY ./Persistence/. ./Persistence/

RUN dotnet publish -c release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT [ "dotnet", "IdentityServer.dll" ]