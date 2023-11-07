FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

WORKDIR /src
COPY ["src/Prosperium.Management.Web.Host/Prosperium.Management.Web.Host.csproj", "src/Prosperium.Management.Web.Host/"]
COPY ["src/Prosperium.Management.Web.Core/Prosperium.Management.Web.Core.csproj", "src/Prosperium.Management.Web.Core/"]
COPY ["src/Prosperium.Management.Application/Prosperium.Management.Application.csproj", "src/Prosperium.Management.Application/"]
COPY ["src/Prosperium.Management.Core/Prosperium.Management.Core.csproj", "src/Prosperium.Management.Core/"]
COPY ["src/Prosperium.Management.EntityFrameworkCore/Prosperium.Management.EntityFrameworkCore.csproj", "src/Prosperium.Management.EntityFrameworkCore/"]
WORKDIR "/src/src/Prosperium.Management.Web.Host"
RUN dotnet restore 

WORKDIR /src
COPY ["src/Prosperium.Management.Web.Host", "src/Prosperium.Management.Web.Host"]
COPY ["src/Prosperium.Management.Web.Core", "src/Prosperium.Management.Web.Core"]
COPY ["src/Prosperium.Management.Application", "src/Prosperium.Management.Application"]
COPY ["src/Prosperium.Management.Core", "src/Prosperium.Management.Core"]
COPY ["src/Prosperium.Management.EntityFrameworkCore", "src/Prosperium.Management.EntityFrameworkCore"]
WORKDIR "/src/src/Prosperium.Management.Web.Host"
RUN dotnet publish -c Release -o /publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:7.0
EXPOSE 80
WORKDIR /app
COPY --from=build /publish .
ENTRYPOINT ["dotnet", "Prosperium.Management.Web.Host.dll"]
