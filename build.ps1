param (
	[string]$BuildVersionNumber=$(throw "-BuildVersionNumber is required."),
	[string]$TagVersionNumber
)

if ($TagVersionNumber){
	Write-Host "TagVersion: $TagVersionNumber"
}
else{
	Write-Host "Version: $BuildVersionNumber"
}

Get-ChildItem -Path $PSScriptRoot\src -Filter *.csproj -Recurse | ForEach-Object{ 
    $ProjectJsonPath =  $_.FullName
	$csproj = [xml](Get-Content $ProjectJsonPath)
    if ($TagVersionNumber){
       $csproj.Project.PropertyGroup.Version = $TagVersionNumber
	   $csproj.Save($ProjectJsonPath)
    }
    else{
       $csproj.Project.PropertyGroup.Version = $BuildVersionNumber
	   $csproj.Save($ProjectJsonPath)
    }
}
dotnet build "$PSScriptRoot\Hexiron.Azure.ActiveDirectory.sln"