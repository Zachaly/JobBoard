#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 5000
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["JobBoard.Api/JobBoard.Api.csproj", "JobBoard.Api/"]
COPY ["JobBoard.Application/JobBoard.Application.csproj", "JobBoard.Application/"]
COPY ["JobBoard.Database/JobBoard.Database.csproj", "JobBoard.Database/"]
COPY ["JobBoard.Domain/JobBoard.Domain.csproj", "JobBoard.Domain/"]
COPY ["JobBoard.Expressions/JobBoard.Expressions.csproj", "JobBoard.Expressions/"]
COPY ["JobBoard.Model/JobBoard.Model.csproj", "JobBoard.Model/"]
RUN dotnet restore "./JobBoard.Api/JobBoard.Api.csproj"
COPY . .
WORKDIR "/src/JobBoard.Api"
RUN dotnet build "./JobBoard.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./JobBoard.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "JobBoard.Api.dll"]