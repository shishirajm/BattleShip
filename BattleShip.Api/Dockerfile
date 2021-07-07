# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/aspnet:3.1
ENV ASPNETCORE_URLS="http://+:55201"
COPY out/ App/
WORKDIR /App
EXPOSE 55201
ENTRYPOINT ["dotnet", "BattleShip.Api.dll"]