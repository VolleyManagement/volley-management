# Builds migration host and related task
.\migrations\publish-azure-storage-migration.ps1
cd .\migrations\azure-storage-migrations
dotnet VolleyM.Tools.MigrationTool.dll
cd ..\..