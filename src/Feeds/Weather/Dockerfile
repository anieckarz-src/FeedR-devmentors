FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app
COPY . .
RUN dotnet publish src/Feeds/Weather/FeedR.Feeds.Weather -c release -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0
COPY --from=build /app/out .
ENV ASPNETCORE_ENVIRONMENT docker
ENV ASPNETCORE_URLS http://*:80
ENTRYPOINT dotnet FeedR.Feeds.Weather.dll