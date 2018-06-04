"%~dp0nuget.exe" restore "%~dp0..\Photon.Sample.sln"
"%~dp0msbuild.cmd" "%~dp0..\PhotonTasks\PhotonTasks.csproj" /t:Rebuild /p:Configuration="Debug" /p:Platform="Any CPU" /p:OutputPath="bin\Debug" /v:m
