#!/bin/sh

# Yes this is a shell script on windows.  I think any sane person
# choosing between a batch file and installing msys would take the 
latter yeah?

rm *.nupkg
msbuild /t:Rebuild /p:Configuration=Release
nuget pack FStomp/FStomp.fsproj -IncludeReferencedProjects -Prop Configuration=Release
NuGet push *.nupkg
