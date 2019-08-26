docker build -t volleymanagement/api:local .
docker run -d --rm --name vm-api -p 5000:80 --env ASPNETCORE_ENVIRONMENT=Development volleymanagement/api:local