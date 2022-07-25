# CI Pipelines

Build the solution with `msbuild` and execute tests with `vstest.console.exe`. For example:

* `msbuild App1.sln /p:Configuration=Debug /p:Platform=x64`
* `. "C:\Program Files\Microsoft Visual Studio\2022\Enterprise\Common7\IDE\CommonExtensions\Microsoft\TestWindow\vstest.console.exe" .\App1.Tests.MSTest\bin\x64\Debug\net6.0-windows10.0.19041.0\App1.Tests.MSTest.dll`

The .NET CLI commands `dotnet build` and `dotnet test` are not yet fully supported in all scenarios.

## GitHub Actions

Below is a sample GitHub Actions Workflow that will build and test a Template Studio for WinUI (C#) project.

```yml
name: Template Studio for WinUI (C#)

on:
  push:
    branches: [ "master" ]
  pull_request:
    branches: [ "master" ]

jobs:
  build:
    strategy:
      matrix:
        configuration: [Debug, Release]
        platform: [x64, x86]

    runs-on: windows-latest

    # TODO: Update environment variables.
    env:
      Solution_Name: App1.sln
      Project_Name: App1
      Test_Project_Name: App1.Tests.MSTest

    steps:
    - name: Checkout
      uses: actions/checkout@v3
      with:
        fetch-depth: 0

    - name: Install .NET Core
      uses: actions/setup-dotnet@v2
      with:
        dotnet-version: 6.0.x

    - name: Setup MSBuild.exe
      uses: microsoft/setup-msbuild@v1.0.2
  
    - name: Restore
      run: dotnet restore $env:Solution_Name
  
    - name: Build
      run: msbuild $env:Solution_Name /p:Configuration=${{ matrix.configuration }} /p:Platform=${{ matrix.platform }}

    - name: Test
      run: . "C:\Program Files\Microsoft Visual Studio\2022\Enterprise\Common7\IDE\CommonExtensions\Microsoft\TestWindow\vstest.console.exe" ${env:Test_Project_Name}\bin\${{ matrix.platform }}\${{ matrix.configuration }}\**\${env:Test_Project_Name}.dll
```