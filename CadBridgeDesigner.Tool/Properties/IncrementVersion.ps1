$ErrorActionPreference = 'Stop'

$path = Join-Path $PSScriptRoot "AssemblyInfo.cs"
$content = [System.IO.File]::ReadAllText($path)
$versionPattern = '\[assembly:\s*AssemblyVersion\("(\d+)\.(\d+)\.(\d+)\.(\d+)"\)\]'
$matchResult = [regex]::Match($content, $versionPattern)

$major = [int]$matchResult.Groups[1].Value
$minor = [int]$matchResult.Groups[2].Value
$build = [int]$matchResult.Groups[3].Value
$revision = [int]$matchResult.Groups[4].Value + 1
$newVersion = '{0}.{1}.{2}.{3}' -f $major, $minor, $build, $revision

$content = [regex]::Replace($content, $versionPattern, ('[assembly: AssemblyVersion("{0}")]' -f $newVersion))

[System.IO.File]::WriteAllText($path, $content, (New-Object System.Text.UTF8Encoding($true)))
