if ($args[0] -eq "ci") {
    dotnet test ./chia-dotnet.tests.csproj --filter TestCategory!=Integration --nologo
}
elseif ($args[0] -eq "integration") {
    dotnet test ./chia-dotnet.tests.csproj --filter TestCategory!=CAUTION --nologo
}