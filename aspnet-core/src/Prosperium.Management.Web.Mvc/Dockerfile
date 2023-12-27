FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

RUN dotnet tool install -g Microsoft.Web.LibraryManager.Cli
RUN ln -s ~/.dotnet/tools/libman /usr/local/bin/libman

WORKDIR /src

COPY . .

# Restaura os pacotes
RUN dotnet restore

# Executa a restauração do Libman
WORKDIR /src/src/Prosperium.Management.Web.Mvc
RUN libman restore

# Publicação da aplicação
WORKDIR /src/src/Prosperium.Management.Web.Mvc
RUN dotnet publish -c Release -o /app


FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final
EXPOSE 80
WORKDIR /app

COPY --from=build /app .

ENTRYPOINT ["dotnet", "Prosperium.Management.Web.Mvc.dll"]
