FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["project1.csproj", "./"]
RUN dotnet restore "./project1.csproj"
COPY . .
RUN dotnet build "project1.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "project1.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "project1.dll"]
