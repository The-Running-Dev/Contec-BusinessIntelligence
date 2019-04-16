# PSake Build file for Business Intelligence application
#
# ConfigFile = The path to the CRS and CRS Services config file
#
# This build requires the following settings to be configured inside the Config.json file
# All settings should be paths relative to this script
#
# ArtifactsPath - The path where the artifacts will be stored
# DatabaseScriptsPath - The path where the database scripts are stored
# PackageId - The package ID that will be used in the NuGet spec file
# SolutionPath - The Visual Studio solution which will be built
# SpecPath - The Chocolatey NuGet spec file
# WebArtifactsPath - The path to compiled web application artifacts
# WebConfigPath - The path to the web application Web.config file

# OcotpusDeployPackageFeedUrl = The URL to the OctopusDeploy package feed
# OctopusDeployAPIKey = The OctopusDeploy API key
# ReleaseNotesFile = The path to file that holds the release notes
#
# Version = The version number of the build
# Branch = The branch name coming from the Git repository
# Publish = Should the created pacakges be published
# Release = Should the build run in release mode, default is true

Properties `
{
    $ConfigFile = '.\Config.json'

    $Version = 0
    $Branch = ''
    $BuildId = 0
    $Publish = $true
    $Release = $true
}

Framework('4.6')

Task Default `
    -Depends Setup, Compile, Package, Publish, Release, Clean

Task Compile `
{
    Write-BuildProgress 'Building Solution'

    # Execute msbuild /clean
    msbuild $script:Config.SolutionPath /t:Clean /v:quiet

    # Remove everything from the artifacts
    Remove-Item "$($script:Config.ArtifactsPath)\**" -Recurse -Force

    # Restore NuGet packages
    & nuget restore $script:Config.SolutionPath

    msbuild $script:Config.SolutionPath /t:Build /p:Configuration=Release /v:quiet /p:OutDir="$($script:Config.ArtifactsPath)"
}

Task Package `
{
    Write-BuildProgress 'Creating Choco Package'

    Get-TeamCityChangeLog `
        $script:Config.TeamCityUrl `
        $script:Config.TeamCityUser `
        $script:Config.TeamCityPassword `
        $script:BuildId > $script:Config.ReleaseNotesPath

    $releaseNotes = Get-Content $script:Config.ReleaseNotesPath -ErrorAction SilentlyContinue

    # Save the release notes in the spec file
    Set-XmlValue $script:Config.SpecPath "//ns:releaseNotes" "`n$releaseNotes"

    # Set the ID in the spec file
    Set-XmlValue $script:Config.SpecPath "//ns:id" $script:Config.PackageId

    # Set the version in the spec file
    Set-XmlValue $script:Config.SpecPath "//ns:version" $script:Version

    # Set the version number in the Web.config
    Set-XmlValue $script:Config.WebConfigPath `
        "//ns:configuration/appSettings/add[@key=""ApplicationVersion""]/@value" `
        $script:Version

    Remove-Item "$($script:Config.PackagePath)\Web" -Recurse -ErrorAction SilentlyContinue
    Remove-Item "$($script:Config.PackagePath)\Database" -Recurse -ErrorAction SilentlyContinue

    New-Item -ItemType Directory "$($script:Config.PackagePath)\Web" | Out-Null
    New-Item -ItemType Directory "$($script:Config.PackagePath)\Database" | Out-Null

    Copy-Item "$($script:Config.WebArtifactsPath)\**" "$($script:Config.PackagePath)\Web" -Recurse
    Copy-Item "$($script:Config.DatabaseScriptsPath)\**" "$($script:Config.PackagePath)\Database" -Recurse

    & choco pack $script:Config.SpecPath -OutputDirectory $script:Config.ArtifactsPath

    Remove-Item "$($script:Config.PackagePath)\Web" -Recurse
    Remove-Item "$($script:Config.PackagePath)\Database" -Recurse
}

Task Publish -Depends Package `
{
    if ($script:Publish) {
        Write-BuildProgress 'Executing Choco Push'

        & choco push (Get-ChildItem "$($script:Config.ArtifactsPath)\*.nupkg" | Select-Object -First 1 -ExpandProperty FullName) `
            -ApiKey $script:Config.ContecFeedAPIKey `
            -Source $script:Config.ContecFeedUrl -f
    }
    else {
        Write-BuildProgress 'Skipping Choco Push'
    }
}

Task Release -Depends Setup -PreCondition { $script:Config.OctopusDeployProject -ne ''} `
{
    if ($script:Publish) {
        Write-BuildProgress 'Executing Octo Create-Release'

        & octo create-release `
            --ApiKey $script:Config.OctopusDeployAPIKey `
            --Server $script:Config.OctopusDeployServerUrl `
            --ReleaseNotesFile $script:Config.ReleaseNotesPath `
            --Version $script:Version `
            --Project $script:Config.OctopusDeployProject
    }
    else {
        Write-BuildProgress "Skipping Octo Create-Release"
    }
}

Task Clean `
{
    Write-BuildProgress 'Executing Clean'

    # Execute msbuild /clean
    msbuild $script:Config.SolutionPath /t:Clean /v:quiet

    # Remove everything from the artifacts path but the .nupkg and .md files
    Remove-Item "$($script:Config.ArtifactsPath)\**" -exclude *.nupkg, *.md -Recurse -Force
}

Task Setup `
{
    $script:Version = @{$true = [DateTime]::Now.ToString("yyyy.MM.dd"); $false = $Version}[$Version -match '']

    $script:BuildId = @{$true = 0; $false = $BuildId}[$BuildId -eq 0]

    # The version should end with the build ID
    $script:Version = "$($script:Version).$($script:BuildId)"

    # Release mode is 'true' unless explicitly set to 'false'
    $script:Release = @{$true = $true; $false = $false}["1,true,yes" -Match $Release]

    # Publish is 'true' unless explicitly set to 'false'
    $script:Publish = @{$true = $true; $false = $false}["1,true,yes" -Match $Publish]

    # Branch is empty unless explicitly set
    $script:Branch = @{$true = ""; $false = $Branch.replace("refs/heads/", "")}["" -Match $Branch]

    $script:Config = Get-Config (Join-Path . $ConfigFile -Resolve)

    # Set the package ID based on the branch name
    if ($script:Branch) {
        $packageId = $script:Config.PackageId
        $script:Config.PackageId = "$script:Branch.$packageId"
    }

    New-Item -Force -ItemType directory -Path $script:Config.ArtifactsPath | Out-Null

    Write-BuildProgress "Artifacts: $($script:Config.ArtifactsPath)"
    Write-BuildProgress "Branch: $($script:Branch)"
    Write-BuildProgress "Package Dir: $($script:Config.PackagePath)"
    Write-BuildProgress "Package Id: $($script:Config.PackageId)"
    Write-BuildProgress "Publish: $($script:Publish)"
    Write-BuildProgress "Release: $($script:Release)"
    Write-BuildProgress "Solution: $($script:Config.SolutionPath)"
    Write-BuildProgress "Spec: $($script:Config.SpecPath)"
    Write-BuildProgress "Version: $($script:Version)"
    Write-BuildProgress "Web Artifacts: $($script:Config.WebArtifactsPath)"
    Write-BuildProgress "Web Config: $($script:Config.WebConfigPath)"
}