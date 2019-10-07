$projects = `
@{folder = 'tools'; name = 'VolleyM.Tools.MigrationTool' }, `
@{folder = 'src/Infrastructure'; name = 'VolleyM.Infrastructure.IdentityAndAccess.AzureStorage' } 

$outputFolder = "./migrations/azure-storage-migrations"

foreach ($project in $projects) {
    $projectPath = './{0}/{1}/{1}.csproj' -f ($project.folder, $project.name)

    dotnet build $projectPath -c Release
    dotnet publish $projectPath -c Release -o $outputFolder
}