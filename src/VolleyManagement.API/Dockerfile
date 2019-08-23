FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY . .
RUN dotnet build "VolleyManagement.sln" -c Release -o /artifacts

FROM build AS publish
RUN mkdir /app \
    && cp /artifacts/VolleyM.Domain.* /app/ \
    && cp /artifacts/VolleyM.Infrastructure.Hardcoded.* /app/
RUN dotnet publish "VolleyManagement.API/VolleyManagement.API.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "VolleyManagement.API.dll"]