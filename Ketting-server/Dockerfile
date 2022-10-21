FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Ketting-server/Ketting-server.csproj", "Ketting-server/"]
RUN dotnet restore "Ketting-server/Ketting-server.csproj"
COPY . .
WORKDIR "/src/Ketting-server"
RUN dotnet build "Ketting-server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Ketting-server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ketting-server.dll"]
