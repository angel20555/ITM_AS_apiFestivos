# Etapa 1: Construcci�n
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
# Configura la URL en la que escuchar� la app
ENV ASPNETCORE_URLS=http://+:5235

# Expone el puerto 5235 al exterior
EXPOSE 5235

# Etapa de build con SDK de .NET
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o /app

# Etapa final para ejecutar el contenedor
FROM base AS final
WORKDIR /app
COPY --from=build /app .

ENTRYPOINT ["dotnet", "apiFestivos.Presentacion.dll"]
