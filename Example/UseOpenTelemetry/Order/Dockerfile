﻿FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Service1/Service1.csproj", "Service1/"]
RUN dotnet restore "Service1/Service1.csproj"
COPY . .
WORKDIR "/src/Service1"
RUN dotnet build "Service1.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Service1.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Service1.dll"]
