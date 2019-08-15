docker build -f ./src/VolleyManagement.API/Dockerfile -t volleymanagemnt/app:local ./src
docker run -d --rm --name vm-api -p 5000:80 --env ASPNETCORE_ENVIRONMENT=Development volleymanagemnt/app:local