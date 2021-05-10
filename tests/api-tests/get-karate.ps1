$karateVersion = 'karate-0.9.9.RC3'

$url = "https://dl.bintray.com/ptrthomas/karate/${karateVersion}.jar";

$karateHome = '~\.karate'
If (!(test-path $karateHome)) {
	New-Item -ItemType Directory -Force -Path $karateHome
}

$karateFile = "${karateHome}\karate.jar";

Invoke-WebRequest -Uri $url -OutFile $karateFile -Headers @{'Host' = 'dl.bintray.com'; 'Referer' = 'https://dl.bintray.com/ptrthomas/karate/' } -UserAgent 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/76.0.3809.132 Safari/537.36'

$karatePath = Resolve-Path -Path $karateFile

$env:KARATE = $karatePath
[System.Environment]::SetEnvironmentVariable('KARATE', $karatePath, [System.EnvironmentVariableTarget]::User)