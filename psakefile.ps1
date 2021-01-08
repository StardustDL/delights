properties {
    $build_version = $env:build_version
}

Task default -depends Restore, Build

Task CI -depends Install-deps, Restore, Gen-Build-Status, Build, Test, Benchmark, Report

Task CD -depends Gen-Build-Status

Task Restore {
    # Exec { dotnet nuget add source https://sparkshine.pkgs.visualstudio.com/StardustDL/_packaging/feed/nuget/v3/index.json -n ownpkgs }
    Exec { dotnet restore }
}

Task Build {
    Exec { dotnet build -c Release /p:Version=$build_version }
}

Task Install-deps {
    # Exec { npm install --global gulp }
    # Exec { dotnet tool install --global Microsoft.Web.LibraryManager.Cli }
    # Exec { dotnet tool install dotnet-reportgenerator-globaltool --tool-path ./tools }
}

Task Test {
    # if (-not (Test-Path -Path "reports/test")) {
    #     New-Item -Path "reports/test" -ItemType Directory
    # }
    # Exec { dotnet test -c Release --logger GitHubActions /p:CollectCoverage=true /p:CoverletOutput=../../reports/test/coverage.json /p:MergeWith=../../reports/test/coverage.json /maxcpucount:1 }
    # Exec { dotnet test -c Release ./test/Test.Base --logger GitHubActions /p:CollectCoverage=true /p:CoverletOutputFormat=opencover /p:CoverletOutput=../../reports/test/coverage.xml /p:MergeWith=../../reports/test/coverage.json }
}

Task Benchmark {
    # Exec { dotnet run -c Release --project ./test/Benchmark.Base }
}

Task Report {
    # Exec { ./tools/reportgenerator -reports:./reports/test/coverage.xml -targetdir:./reports/test }
    # if (-not (Test-Path -Path "reports/benchmark")) {
    #     New-Item -Path "reports/benchmark" -ItemType Directory
    # }
    # Copy-Item ./BenchmarkDotNet.Artifacts/* ./reports/benchmark -Recurse
}

Task Gen-Build-Status {
    # Set-Location src/Delights.Client
    # Write-Output "{ ""Build"": { ""Commit"": ""$env:GITHUB_SHA"", ""Branch"": ""$env:GITHUB_REF"", ""BuildDate"": ""$(Get-date)"", ""Repository"": ""$env:GITHUB_REPOSITORY"", ""Version"": ""$env:build_version"" } }" > ./build.json
    # Set-Location ../..
}

Task Api {
    Exec { dotnet run -p ./src/Delights.Api }
}

Task Client {
    Exec { dotnet run -p ./src/Delights.Client }
}