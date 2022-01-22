$version="0.0.1"
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
$fileData | Out-File -Encoding utf8 ../ScorpioConversion/src/Version.cs

Remove-Item ../bin/* -Force -Recurse

$platforms = @("win-x86", "win-x64", "win-arm", "win-arm64", "linux-x64", "linux-musl-x64", "linux-arm", "linux-arm64", "osx-x64", "osx-arm64")
foreach ($platform in $platforms) {
    Write-Host "正在打包 $platform 版本..."
    $pathName = "$name-$platform"
    dotnet publish ../ScorpioConversion/ScorpioConversion.csproj -c release -o ../bin/$pathName -r $platform --self-contained -p:AssemblyVersion=$version | Out-Null
    Write-Host "正在压缩 $platform ..."
    $fileName = "$name-$version-$platform"
    Compress-Archive ../bin/$pathName/* ../bin/$fileName.zip -Force
}
