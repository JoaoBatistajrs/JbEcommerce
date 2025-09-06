# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copia apenas o csproj primeiro (para cache de restore)
COPY *.csproj ./
RUN dotnet restore

# Copia o restante e publica
COPY . .
RUN dotnet publish -c Release -o /app/out

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .

ENV ASPNETCORE_URLS=http://+:5012
EXPOSE 5012

ENTRYPOINT ["dotnet", "UsersAPI.dll"]
