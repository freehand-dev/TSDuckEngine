# TSDuckEngine
> TSDuckEngine

## Dependence
- [TSDuck](https://github.com/tsduck/tsduck)
- [.NET](https://dotnet.microsoft.com/en-us/download) 


## Docker
 ```bash
git clone https://github.com/freehand-dev/TSDuckEngine.git
cd TSDuckEngine
docker build . --file Dockerfile --tag tsduck-engine
```

 ```bash
docker volume create TSDuckEngine_data
docker run --detach --name TSDuckEngineM1 --restart=always -v TSDuckEngine_data:/opt/TSDuckEngine/etc/TSDuckEngine --env DOTNET_ENVIRONMENT=Production --env DOTNET_NAME=M1 --network host tsduck-engine
```

## Windows (Compile and install)
Once you have installed all the dependencies, get the code:
 ```powershell
git clone https://github.com/freehand-dev/TSDuckEngine.git
cd TSDuckEngine
```

Then just use:
```powershell
New-Item -Path "%ProgramData%\FreeHand\TSDuckEngine\bin\" -ItemType "directory"
dotnet restore
dotnet build
dotnet publish --runtime win-x64 --output %ProgramData%\FreeHand\TSDuckEngine\bin\ -p:PublishSingleFile=true -p:PublishTrimmed=true -p:PublishReadyToRun=true .\src\TSDuckEngine
```

Install as Windows Service (Default instance)
 ```powershell
$params = @{
  Name = "FreeHandTSDuckEngineSvc_Default"
  BinaryPathName = '"%ProgramData%\FreeHand\TSDuckEngine\bin\TSDuckEngine.exe"'
  DisplayName = "FreeHand TSDuckEngine (Default)"
  StartupType = "Auto"
  Description = "FreeHand TSDuckEngine (Default)"
}
New-Service @params
```

Install as Windows Service (M1 name instance)
 ```powershell 
$params = @{
  Name = "FreeHandTSDuckEngineSvc_M1"
  BinaryPathName = '"%ProgramData%\FreeHand\TSDuckEngine\bin\TSDuckEngine.exe" -name "M1" --local-config "C:/ProgramData/FreeHand/TSDuckEngine/custom_config_name.json"'
  DisplayName = "FreeHand TSDuckEngine (M1)"
  StartupType = "Auto"
  Description = "FreeHand TSDuckEngine (M1)"
}
New-Service @params
```

Install as Windows Service (M1 name instance)
 ```powershell 
$params = @{
  Name = "FreeHandTSDuckEngineSvc_M1"
  BinaryPathName = '"%ProgramData%\FreeHand\TSDuckEngine\bin\TSDuckEngine.exe" --name "M1"'
  DisplayName = "FreeHand TSDuckEngine (M1)"
  StartupType = "Auto"
  Description = "FreeHand TSDuckEngine (M1)"
}
New-Service @params
```

## Configure and start
To start the service, you can use the `TSDuckEngine` executable as the application or `Start-Service -Name "FreeHandTSDuckEngineSvc_Default"` as a Windows service. For configuration you can edit a configuration file:

	notepad.exe %ProgramData%\FreeHand\TSDuckEngine\[instance_name].json

  
