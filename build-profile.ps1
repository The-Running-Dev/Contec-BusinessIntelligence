$pathToPSake = 'C:\ProgramData\chocolatey\lib\PowerShell-Psake\tools\psake.psm1'
$chocolateyProfile = "$env:chocolateyInstall\helpers\chocolateyProfile.psm1"
$chocolateyInstallProfile = "$env:chocolateyInstall\helpers\chocolateyInstaller.psm1"

if (Test-Path $pathToPSake) {
    Import-Module $pathToPSake
}

if (Test-Path $chocolateyProfile) {
    Import-Module $chocolateyProfile
    Import-Module $chocolateyInstallProfile
}

if (-not (choco list -lo -r 'Chocolatey-Dev.extension')) {
}