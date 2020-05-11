FROM mcr.microsoft.com/dotnet/core/aspnet:5.0.0-preview-alpine AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:5.0.100-preview-alpine AS build
# Need to set this var for the build to avoid Specflow issue
# https://github.com/SpecFlowOSS/SpecFlow/issues/1912
ENV MSBUILDSINGLELOADCONTEXT=1
WORKDIR /build-dir
COPY ./src ./src
COPY ./tests ./tests
COPY ./tools ./tools
RUN dotnet build "./src/VolleyManagement.sln" -c Release -o /artifacts

FROM build AS publish
RUN dotnet publish "src/Domain/VolleyM.Domain.Contracts/VolleyM.Domain.Contracts.csproj" -c Release -o /app \
    && dotnet publish "src/Domain/VolleyM.Domain.Framework/VolleyM.Domain.Framework.csproj" -c Release -o /app \
    && dotnet publish "src/Domain/VolleyM.Domain.Contributors/VolleyM.Domain.Contributors.csproj" -c Release -o /app \
    && dotnet publish "src/Domain/VolleyM.Domain.IdentityAndAccess/VolleyM.Domain.IdentityAndAccess.csproj" -c Release -o /app \
    && dotnet publish "src/Domain/VolleyM.Domain.Players/VolleyM.Domain.Players.csproj" -c Release -o /app \
    && dotnet publish "src/Infrastructure/VolleyM.Infrastructure.Bootstrap/VolleyM.Infrastructure.Bootstrap.csproj" -c Release -o /app \
    && dotnet publish "src/Infrastructure/VolleyM.Infrastructure.Hardcoded/VolleyM.Infrastructure.Hardcoded.csproj" -c Release -o /app\
    && dotnet publish "src/Infrastructure/VolleyM.Infrastructure.AzureStorage/VolleyM.Infrastructure.AzureStorage.csproj" -c Release -o /app\
    && dotnet publish "src/Infrastructure/VolleyM.Infrastructure.IdentityAndAccess.AzureStorage/VolleyM.Infrastructure.IdentityAndAccess.AzureStorage.csproj" -c Release -o /app\
    && dotnet publish "src/Infrastructure/VolleyM.Infrastructure.Players.AzureStorage/VolleyM.Infrastructure.Players.AzureStorage.csproj" -c Release -o /app\
    && dotnet publish "src/Infrastructure/VolleyM.Infrastructure.EventBroker/VolleyM.Infrastructure.EventBroker.csproj" -c Release -o /app\
    && dotnet publish "src/Client/VolleyM.API.Contributors/VolleyM.API.Contributors.csproj" -c Release -o /app\
    && dotnet publish "src/Client/VolleyM.API.Players/VolleyM.API.Players.csproj" -c Release -o /app\
    && dotnet publish "src/Client/VolleyM.API/VolleyM.API.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "VolleyM.API.dll"]