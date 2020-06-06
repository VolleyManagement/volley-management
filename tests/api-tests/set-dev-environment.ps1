# It's ok to have this secret here as it cannot be leveraged against production environment
$Auth0_Dev = "NKn65TQ-BhkYaIkPRG8iUruIMjwiTzN01xUc4CwM7ucNnyEeV4RXTOgLuxu97Emj"
[Environment]::SetEnvironmentVariable("VM_KARATE_AUTH0_CLIENT_SECRET", $Auth0_Dev, "User")
$Env:VM_KARATE_AUTH0_CLIENT_SECRET = $Auth0_Dev
