# If you upgrade version, you need to update package in artifacts. See publish-karate.azcli

$url = "https://dl.bintray.com/ptrthomas/karate/karate-0.9.4.jar";

Invoke-WebRequest -Uri $url -OutFile .\karate.jar -Headers @{'Host' = 'dl.bintray.com'; 'Referer' = 'https://dl.bintray.com/ptrthomas/karate/'} -UserAgent 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/76.0.3809.132 Safari/537.36'