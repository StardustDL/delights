properties {
    $NUGET_AUTH_TOKEN = $env:NUGET_AUTH_TOKEN
    $build_version = $env:build_version
}

Task default -depends Restore, Build

Task CI -depends Restore, Build, Pack, Test, Benchmark, Report

Task CD -depends Restore, Build, Pack, Deploy

Task Deploy -depends publish-packages

Task Restore {
    Exec { dotnet nuget add source https://sparkshine.pkgs.visualstudio.com/StardustDL/_packaging/feed/nuget/v3/index.json -n ownpkgs }
    Exec { dotnet tool restore }
    Exec { dotnet restore }
}

Function GenerateGraphQL($moduleName) {
    Assert ($null -ne $moduleName) '$moduleName should not be null'
    $lowerName = ($moduleName).tolower()
    Write-Output "Generating GraphQL for module: $moduleName"

    Set-Location src/modules/$lowerName

    Set-Location Delights.Modules.$moduleName

    $apiname = $moduleName + "GraphQL"
    
    Exec { dotnet graphql init https://localhost:5001/graphql -n $apiname -p "GraphQL" }

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

Task publish-packages {
    Exec { dotnet nuget update source ownpkgs -u sparkshine -p $NUGET_AUTH_TOKEN --store-password-in-clear-text }
    Exec { dotnet nuget push ./packages/Delights.Modules.Core.$build_version.nupkg -s ownpkgs -k az --skip-duplicate }
    Exec { dotnet nuget push ./packages/Delights.Modules.Client.$build_version.nupkg -s ownpkgs -k az --skip-duplicate }
    Exec { dotnet nuget push ./packages/Delights.Modules.Server.GraphQL.$build_version.nupkg -s ownpkgs -k az --skip-duplicate }
}

Task publish-packages-release {
    Exec { dotnet nuget push ./packages/Delights.Modules.Core.$build_version.nupkg  -s https://api.nuget.org/v3/index.json -k $NUGET_AUTH_TOKEN --skip-duplicate }
    Exec { dotnet nuget push ./packages/Delights.Modules.Client.$build_version.nupkg  -s https://api.nuget.org/v3/index.json -k $NUGET_AUTH_TOKEN --skip-duplicate }
    Exec { dotnet nuget push ./packages/Delights.Modules.Server.GraphQL.$build_version.nupkg  -s https://api.nuget.org/v3/index.json -k $NUGET_AUTH_TOKEN --skip-duplicate }
}


Task Api {
    Exec { dotnet run -p ./src/Delights.Api }
}

Task Client {
    Exec { dotnet run -p ./src/Delights.Client }
}

Task new-module {
    Assert ($name -ne $null) '$name should not be null'
    $lowerName = ($name).tolower()
    Write-Output "Generating module: $name, route path: $lowerName"

    Copy-Item src/modules/hello src/modules/$lowerName -Recurse
    Set-Location src/modules/$lowerName

    Function ReplaceContent($fileName) {
        $raw = Get-Content $fileName
        $result = $raw.Replace("Hello", $name).Replace("hello", $lowerName)
        Write-Output $result | Out-File $fileName
    }

    ReplaceContent Delights.Modules.Hello/Module.cs
    ReplaceContent Delights.Modules.Hello/ModuleOption.cs
    ReplaceContent Delights.Modules.Hello/Delights.Modules.Hello.csproj
    ReplaceContent Delights.Modules.Hello.Server/Module.cs
    ReplaceContent Delights.Modules.Hello.Server/ModuleOption.cs
    ReplaceContent Delights.Modules.Hello.Server/Delights.Modules.Hello.Server.csproj
    ReplaceContent Delights.Modules.Hello.UI/_Imports.razor
    ReplaceContent Delights.Modules.Hello.UI/Pages/Index.razor
    ReplaceContent Delights.Modules.Hello.UI/Delights.Modules.Hello.UI.csproj
    ReplaceContent Delights.Modules.Hello.Core/Delights.Modules.Hello.Core.csproj
    ReplaceContent Delights.Modules.Hello.Core/SharedMetadata.cs
    ReplaceContent Delights.Modules.Hello/GraphQL/berry.json


    mv Delights.Modules.Hello/GraphQL/HelloGraphQL.graphql Delights.Modules.Hello/GraphQL/${name}GraphQL.graphql
    mv Delights.Modules.Hello/Delights.Modules.Hello.csproj Delights.Modules.Hello/Delights.Modules.$name.csproj
    mv Delights.Modules.Hello.Core/Delights.Modules.Hello.Core.csproj Delights.Modules.Hello.Core/Delights.Modules.$name.Core.csproj
    mv Delights.Modules.Hello.Server/Delights.Modules.Hello.Server.csproj Delights.Modules.Hello.Server/Delights.Modules.$name.Server.csproj
    mv Delights.Modules.Hello.UI/Delights.Modules.Hello.UI.csproj Delights.Modules.Hello.UI/Delights.Modules.$name.UI.csproj

    mv Delights.Modules.Hello Delights.Modules.$name
    mv Delights.Modules.Hello.Server Delights.Modules.$name.Server
    mv Delights.Modules.Hello.UI Delights.Modules.$name.UI
    mv Delights.Modules.Hello.Core Delights.Modules.$name.Core

    Set-Location ../../..
}

Task gen-gql {
    Assert ($name -ne $null) '$name should not be null'
    GenerateGraphQL $name
}

# invoke-psake new-module -parameters @{"name"="Hello"}

Task update-gql {
    Start-Job -Name "api" -ScriptBlock { dotnet run -p ./src/Delights.Api  }
    Start-Sleep -Seconds 5

    Exec -maxRetries 10 { GenerateGraphQL Hello }
    Exec -maxRetries 10 { GenerateGraphQL ModuleManager }

    Stop-Job -Name "api"
}

Task update-gql-d {

    GenerateGraphQL Hello
    GenerateGraphQL ModuleManager

}