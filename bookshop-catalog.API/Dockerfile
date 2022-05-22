#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["bookshop-catalog.API/bookshop-catalog.API.csproj", "bookshop-catalog.API/"]
RUN dotnet restore "bookshop-catalog.API/bookshop-catalog.API.csproj"
COPY . .
WORKDIR "/src/bookshop-catalog.API"
RUN dotnet build "bookshop-catalog.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "bookshop-catalog.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "bookshop-catalog.API.dll"]