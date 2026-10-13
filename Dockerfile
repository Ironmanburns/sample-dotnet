FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /src
COPY *.csproj ./
RUN dotnet restore
COPY . ./
RUN dotnet publish -c Release -o /out --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine
RUN adduser -D -u 10001 app
WORKDIR /app
COPY --from=build /out ./
USER app
EXPOSE 8080
ENV PORT=8080 ASPNETCORE_URLS=http://0.0.0.0:8080
ENTRYPOINT ["dotnet", "sample-dotnet.dll"]
