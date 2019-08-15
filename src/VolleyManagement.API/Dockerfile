FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["VolleyManagement.API/VolleyManagement.API.csproj", "VolleyManagement.API/"]
RUN dotnet restore "VolleyManagement.API/VolleyManagement.API.csproj"
COPY . .
WORKDIR "/src/VolleyManagement.API"
RUN dotnet build "VolleyManagement.API.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "VolleyManagement.API.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "VolleyManagement.API.dll"]