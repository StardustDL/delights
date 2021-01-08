#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
# COPY ["src/Delights.Api/Delights.Api.csproj", "src/Delights.Api/"]
# RUN dotnet restore "src/Delights.Api/Delights.Api.csproj"
COPY . .
WORKDIR "/src/src/Delights.Api"
RUN dotnet build "Delights.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Delights.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Delights.Api.dll"]