dotnet new sln -n SQLCoverRunner
dotnet new console -n SQLCoverRunner -f net7.0
dotnet sln add SQLCoverRunner
dotnet build



dotnet tool install -g dotnet-reportgenerator-globaltool

reportgenerator -reports:SQLCoverRunner/bin/Debug/net7.0/coverage.xml -targetDir:coverageoutput

reportgenerator -reports:SQLCoverRunner/bin/Debug/net7.0/coverage.xml -targetDir:coverageoutput -sourcedirs:SQLCoverRunner/bin/Debug/net7.0

reportgenerator -reports:SQLCoverRunner/bin/Debug/net7.0/coverage.xml -targetDir:coverageoutput -sourcedirs:SQLCoverRunner/bin/Debug/net7.0 -filefilters:+*stringresources_get*

[["]-filefilters:<(+|-)filter>[;<(+|-)filter>][;<(+|-)filter>]["]]

file:///C:/Users/Employee/OneDrive%20-%20Good%20Sign%20Oy/Transfer/repos/azure-works/SQLCoverRunner/coverageoutput/index.html


-c "Data Source=localhost,1434;Persist Security Info=True;User ID=sa;Password=GoodSign2022;Application Name=SQLCoverRunner" -d jarmotest1 --include "stringresources_get" -e "exec tSQLt.RunAll" -r "Html"


dotnet run --project SQLCover.Runner/SQLCover.Runner.csproj -- -c "Data Source=localhost,1434;Persist Security Info=True;User ID=sa;Password=GoodSign2022;Application Name=SQLCoverRunner" -d jarmotest1 --include "stringresources_get" -e "exec tSQLt.RunAll" -r "Html" -o C:\Temp\SQLCoverRunner

