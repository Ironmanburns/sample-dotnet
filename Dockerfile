FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /src
COPY *.csproj ./
RUN dotnet restore
COPY . ./
RUN dotnet publish -c Release -o /out --no-restore

# aspnet:8.0-alpine already ships a non-root `app` user — do not adduser again.
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine
WORKDIR /app
COPY --from=build --chown=app:app /out ./
USER app
EXPOSE 8080
ENV PORT=8080 ASPNETCORE_URLS=http://0.0.0.0:8080
ENTRYPOINT ["dotnet", "sample-dotnet.dll"]
