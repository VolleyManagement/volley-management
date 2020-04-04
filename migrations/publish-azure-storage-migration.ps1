$taskProjects = `
@{folder = 'src/Infrastructure'; name = 'VolleyM.Infrastructure.IdentityAndAccess.AzureStorage' },
@{folder = 'src/Infrastructure'; name = 'VolleyM.Infrastructure.Players.AzureStorage' } 

$outputFolder = "./migrations/azure-storage-migrations"

# deploy migration host
dotnet publish './tools/VolleyM.Tools.MigrationTool/VolleyM.Tools.MigrationTool.csproj' -c Release -o $outputFolder

foreach ($project in $taskProjects) {
    $projectPath = './{0}/{1}/{1}.csproj' -f ($project.folder, $project.name)
    $outFolder = '{0}/tasks/{1}' -f ($outputFolder, $project.name)

    dotnet publish $projectPath -c Release -o $outFolder
}