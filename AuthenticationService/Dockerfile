FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY AuthenticationService/AuthenticationService.csproj AuthenticationService/
RUN dotnet restore AuthenticationService/AuthenticationService.csproj
COPY . .
WORKDIR /src/AuthenticationService
RUN dotnet build AuthenticationService.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish AuthenticationService.csproj -c Release -o /app

FROM node:11.1-alpine as client-dev-stage
WORKDIR /src/vue-ui
COPY vue-ui/package*.json ./
RUN npm install
COPY vue-ui ./

FROM client-dev-stage as client-build-stage
RUN npm run build

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
COPY --from=client-build-stage /src/AuthenticationService/wwwroot ./wwwroot/
ENTRYPOINT ["dotnet", "AuthenticationService.dll"]
