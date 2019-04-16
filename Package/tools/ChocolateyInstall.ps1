$packageParameters = Get-Parameters

$configFile = Join-Path $env:ChocolateyPackageFolder 'config.json' -Resolve
$config = Get-Config -Config $configFile -Environment $packageParameters.environment -BaseDir $env:ChocolateyPackageFolder

Write-Host "App Pool: $($config.AppPool)"
Write-Host "App Pool Identity: $($config.AppPoolIdentity)"
Write-Host "Branch: $($config.Branch)"
Write-Host "Database Name: $($config.DatabaseName)"
Write-Host "Database Scripts: $($config.DatabaseScriptsPath)"
Write-Host "Database Server: $($config.DatabaseServer)"
Write-Host "Name: $($config.Name)"
Write-Host "Physical Path: $($config.PhysicalPath)"
Write-Host "Release Notes: $($config.ReleaseNotesPath)"
Write-Host "Url: $($config.Url)"
Write-Host "Version: $($config.Version)"
Write-Host "Web App Directory: $($config.WebAppPath)"

$arguments = @{
    Name         = $config.Name
    PhysicalPath = $config.PhysicalPath
    WebAppPath   = $config.WebAppPath
    Bindings     = $config.Bindings
    Environment  = $packageArgs.environment
    AppPool      = $config.AppPool
    Config       = $config
}

Install-WebSite @arguments -Clean -ExcludeFromCleaning 'Logs', '*.log'