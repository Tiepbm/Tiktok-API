FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

# MUST: listen on Render port
ENV ASPNETCORE_URLS=http://0.0.0.0:10000

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet publish -c Release -o /publish

FROM base AS final
WORKDIR /app
COPY --from=build /publish .
ENTRYPOINT ["dotnet", "TikTokAPI.dll"]
