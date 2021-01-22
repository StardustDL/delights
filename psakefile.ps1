properties {
    $NUGET_AUTH_TOKEN = $env:NUGET_AUTH_TOKEN
    $build_version = $env:build_version
}

Task default -depends Restore, Build

Task CI -depends Restore, Build, Pack, Test, Benchmark, Report

Task CD -depends Restore, Build, Pack

Task Deploy -depends publish-packages

Task Restore {
    Exec { dotnet tool restore }
    Exec { dotnet restore }
}

Function GenerateGraphQL($moduleName) {
    Assert ($null -ne $moduleName) '$moduleName should not be null'
    $lowerName = ($moduleName).tolower()
    Write-Output "Generating GraphQL for module: $moduleName"

    Set-Location src/modules/$lowerName

    Set-Location client

    $apiname = $moduleName + "GraphQL"
    
    Exec { dotnet graphql init https://localhost:5001/graphql/${moduleName}Server -n $apiname -p "GraphQL" }

    Set-Location "GraphQL"

    # $namespace = $moduleName + ".GraphQL"
    # Exec { dotnet graphql generate -n Delights.Modules.$namespace -d }
    
    Set-Location ../..

    Set-Location ../../..
}

Task Build {
    # Start-Job -Name "api" -ScriptBlock { dotnet run -p ./src/Delights.Api  }
    # Start-Sleep -Seconds 1
    # GenerateGraphQL Hello
    # Stop-Job -Name "api"
    Exec -maxRetries 3 { dotnet build -c Release /p:Version=$build_version }
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

Task Pack {
    if (-not (Test-Path -Path "packages")) {
        New-Item -Path "packages" -ItemType Directory
    }

    Exec -maxRetries 10 { dotnet pack -c Release /p:Version=$build_version -o ./packages }
}

Function PublishPackages($source, $key) {
    Exec { dotnet nuget push ./packages/Modulight.Modules.Core.$build_version.nupkg -s $source -k $key --skip-duplicate }
    Exec { dotnet nuget push ./packages/Modulight.Modules.Client.RazorComponents.$build_version.nupkg -s $source -k $key --skip-duplicate }
    Exec { dotnet nuget push ./packages/Modulight.Modules.Server.AspNet.$build_version.nupkg -s $source -k $key --skip-duplicate }
    Exec { dotnet nuget push ./packages/Modulight.Modules.Server.GraphQL.$build_version.nupkg -s $source -k $key --skip-duplicate }
    Exec { dotnet nuget push ./packages/StardustDL.AspNet.ObjectStorage.$build_version.nupkg -s $source -k $key --skip-duplicate }
}

Task publish-packages {
    Exec { dotnet nuget add source https://sparkshine.pkgs.visualstudio.com/StardustDL/_packaging/feed/nuget/v3/index.json -n ownpkgs }
    Exec { dotnet nuget update source ownpkgs -u sparkshine -p $NUGET_AUTH_TOKEN --store-password-in-clear-text }
    PublishPackages ownpkgs az
}

Task publish-packages-release {
    PublishPackages https://api.nuget.org/v3/index.json $NUGET_AUTH_TOKEN
}

Task Api {
    Exec { dotnet run -p ./src/Delights.Api }
}

Task Api-prod {
    Exec { dotnet run -p ./src/Delights.Api -c Release --launch-profile "Delights.Api.Prod"}
}

Task Client {
    Exec { dotnet run -p ./src/Delights.Client }
}

Task gen-gql {
    Assert ($name -ne $null) '$name should not be null'
    GenerateGraphQL $name
}

# invoke-psake new-module -parameters @{"name"="Hello"}

Task update-gql {

    # GenerateGraphQL Hello
    GenerateGraphQL ModuleManager
    GenerateGraphQL Persons
    GenerateGraphQL Notes
    GenerateGraphQL Bookkeeping
}
