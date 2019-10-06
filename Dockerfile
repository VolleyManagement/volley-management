FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /build-dir
COPY ./src ./src
COPY ./tests ./tests
COPY ./tools ./tools
RUN dotnet build "./src/VolleyManagement.sln" -c Release -o /artifacts

FROM build AS publish
RUN dotnet publish "src/Domain/VolleyM.Domain.Contracts/VolleyM.Domain.Contracts.csproj" -c Release -o /app \
    && dotnet publish "src/Domain/VolleyM.Domain.Contributors/VolleyM.Domain.Contributors.csproj" -c Release -o /app \
    && dotnet publish "src/Domain/VolleyM.Domain.IdentityAndAccess/VolleyM.Domain.IdentityAndAccess.csproj" -c Release -o /app \
    && dotnet publish "src/Infrastructure/VolleyM.Infrastructure.Bootstrap/VolleyM.Infrastructure.Bootstrap.csproj" -c Release -o /app \
    && dotnet publish "src/Infrastructure/VolleyM.Infrastructure.Hardcoded/VolleyM.Infrastructure.Hardcoded.csproj" -c Release -o /app\
    && dotnet publish "src/Infrastructure/VolleyM.Infrastructure.IdentityAndAccess.AzureStorage/VolleyM.Infrastructure.IdentityAndAccess.AzureStorage.csproj" -c Release -o /app\
    && dotnet publish "src/Client/VolleyM.API.Contributors/VolleyM.API.Contributors.csproj" -c Release -o /app\
    && dotnet publish "src/Client/VolleyM.API/VolleyM.API.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "VolleyM.API.dll"]