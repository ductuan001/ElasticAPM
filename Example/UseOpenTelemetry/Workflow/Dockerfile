﻿FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Service2/Service2.csproj", "Service2/"]
RUN dotnet restore "Service2/Service2.csproj"
COPY . .
WORKDIR "/src/Service2"
RUN dotnet build "Service2.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Service2.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Service2.dll"]
