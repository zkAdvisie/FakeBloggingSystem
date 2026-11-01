#Build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

Copy . .
RUN dotnet restore "FakeBloggingSystem/FakeBloggingSystem.csproj"
RUN dotnet publish "FakeBloggingSystem/FakeBloggingSystem.csproj" -c Release -o /app/publish --no-restore

#Runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "FakeBloggingSystem.dll"]
