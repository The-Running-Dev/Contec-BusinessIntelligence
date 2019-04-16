param (
    [Parameter(Position = 0)][String] $version,
    [Parameter(Position = 1)][String] $branch,
    [Parameter(Position = 2)][int] $buildId,
    [Parameter(Position = 3)][switch] $publish,
    [Parameter(Position = 4)][switch] $release
)

# Run the build profile script first
. (Join-Path -Resolve $PSScriptRoot '.\build-profile.ps1')

$buildScript = Join-Path -Resolve $PSScriptRoot 'build.ps1'

Invoke-Psake $buildScript -properties @{
    Version = $version
    Branch  = $branch
    BuildId = $buildId
    Publish = $publish
    Release = $release
}
if ($psake.build_success -eq $false) { exit 1 }