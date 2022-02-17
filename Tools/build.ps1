$version="1.0.1"
$name="scov"


$today = Get-Date
$date=$today.ToString('yyyy-MM-dd')
$fileData=@"
namespace Scorpio.Conversion {
    public static class Version {
        public const string version = "$version";
        public const string date = "$date";
    }
}
"@
$fileData | Out-File -Encoding utf8 ../ScorpioConversion/Scorpio.Conversion.Engine/src/Version.cs

Remove-Item ../bin/* -Force -Recurse
# Write-Host "正在生成Scorpio.Conversion.Engine.nupkg..."
# dotnet pack ../ScorpioConversion/Scorpio.Conversion.Engine/Scorpio.Conversion.Engine.csproj -p:PackageVersion=$version -o ../bin/ /p:AssemblyVersion=$version | Out-Null

$platforms = @("win-x86", "win-x64", "win-arm", "win-arm64", "linux-x64", "linux-musl-x64", "linux-arm", "linux-arm64", "osx-x64", "osx-arm64")
# $platforms = @()
$aipPath = ".\Install.aip"
foreach ($platform in $platforms) {
    Write-Host "正在打包 $platform 版本..."
    $pathName = "$name-$platform"
    dotnet publish ../ScorpioConversion/ScorpioConversion/Scorpio.Conversion.csproj -c release -o ../bin/$pathName -r $platform --self-contained -p:AssemblyVersion=$version | Out-Null
    Write-Host "正在压缩 $platform ..."
    $fileName = "$name-$version-$platform"
    Compress-Archive ../bin/$pathName/* ../bin/$fileName.zip -Force
    if ($IsWindows -and (($platform -eq "win-x86") -or ($platform -eq "win-x64"))) {
        Write-Host "正在生成安装包 $platform ..."
        git checkout $aipPath
        Get-ChildItem ..\bin\$pathName\ | ForEach-Object -Process{
            if($_ -is [System.IO.FileInfo]) {
                AdvancedInstaller.com /edit $aipPath /AddFile APPDIR $_.FullName
            }
        }
        AdvancedInstaller.com /edit $aipPath /SetVersion $version
        AdvancedInstaller.com /edit $aipPath /SetPackageName ..\bin\$fileName.msi -buildname DefaultBuild
        if ($platform -eq "win-x86") {
            AdvancedInstaller.com /edit $aipPath /SetPackageType x86 -buildname DefaultBuild
        } elseif ($platform -eq "win-x64") {
            AdvancedInstaller.com /edit $aipPath /SetPackageType x64 -buildname DefaultBuild
        }
        AdvancedInstaller.com /build $aipPath -buildslist DefaultBuild
        git checkout $aipPath
    }
}
$cur = Get-Location
Write-Host "正在生成Scorpio.Conversion.Runtime.nupkg..."
dotnet pack ../ScorpioProto/CSharp/Scorpio.Conversion.Runtime/Scorpio.Conversion.Runtime.csproj -p:PackageVersion=$version -o ../bin/ /p:AssemblyVersion=$version | Out-Null
Write-Host "正在生成Scorpio.Conversion.Runtime.jar..."

Set-Location ../ScorpioProto/Java/Scorpio.Conversion.Runtime
./gradlew release "-PVERSION=$version"
Copy-Item -Path app/build/libs/*.jar -Destination ../../../bin/
Set-Location $cur

$packageJson = "../ScorpioProto/Javascript/Scorpio.Conversion.Runtime/package.json"
$packageData = Get-Content -Path $packageJson -Raw | ConvertFrom-Json
$packageData.version = $version
$packageData | ConvertTo-Json | Set-Content -Path $packageJson

Set-Location ../ScorpioProto/Python/Scorpio.Conversion.Runtime
Remove-Item dist/* -Force -Recurse
$version | Set-Content ./version -NoNewline -Encoding utf8
python setup.py sdist
Set-Location $cur

