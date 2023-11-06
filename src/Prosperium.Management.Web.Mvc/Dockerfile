FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

RUN dotnet tool install -g Microsoft.Web.LibraryManager.Cli
RUN ln -s ~/.dotnet/tools/libman /usr/local/bin/libman

WORKDIR /src
COPY ["src/Prosperium.Management.Web.Mvc/Prosperium.Management.Web.Mvc.csproj", "src/Prosperium.Management.Web.Mvc/"]
COPY ["src/Prosperium.Management.Web.Core/Prosperium.Management.Web.Core.csproj", "src/Prosperium.Management.Web.Core/"]
COPY ["src/Prosperium.Management.Application/Prosperium.Management.Application.csproj", "src/Prosperium.Management.Application/"]
COPY ["src/Prosperium.Management.Core/Prosperium.Management.Core.csproj", "src/Prosperium.Management.Core/"]
COPY ["src/Prosperium.Management.EntityFrameworkCore/Prosperium.Management.EntityFrameworkCore.csproj", "src/Prosperium.Management.EntityFrameworkCore/"]
WORKDIR "/src/src/Prosperium.Management.Web.Mvc"
RUN dotnet restore 

WORKDIR /src
COPY ["src/Prosperium.Management.Web.Mvc", "src/Prosperium.Management.Web.Mvc"]
COPY ["src/Prosperium.Management.Web.Core", "src/Prosperium.Management.Web.Core"]
COPY ["src/Prosperium.Management.Application", "src/Prosperium.Management.Application"]
COPY ["src/Prosperium.Management.Core", "src/Prosperium.Management.Core"]
COPY ["src/Prosperium.Management.EntityFrameworkCore", "src/Prosperium.Management.EntityFrameworkCore"]
WORKDIR "/src/src/Prosperium.Management.Web.Mvc"
RUN libman restore
RUN dotnet publish -c Release -o /publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:7.0
EXPOSE 80
WORKDIR /app
COPY --from=build /publish .
ENTRYPOINT ["dotnet", "Prosperium.Management.Web.Mvc.dll"]
