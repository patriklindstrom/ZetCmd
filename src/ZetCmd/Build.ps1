Copy-Item ".\ZetCmd\bin\Debug\ZetCmd.exe" -Destination ".\Build"
Copy-Item ".\ZetCmd\bin\Debug\CommandLine.dll" -Destination ".\Build"
# http://www.generatedata.com/
# add ilmerge /target:exe /out:MyApp.exe zetcmd.exe CommandLine.dll
Copy-Item ".\TestFiles\A_TestFile.csv" -Destination ".\Build"
Copy-Item ".\TestFiles\B_TestFile.csv" -Destination ".\Build"
# ilmerge /target:exe /out:.\Build\zetcmd.exe .\Build\zetcmd.exe .\Build\CommandLine.dll
# ilmerge /target:exe /out:.\zetcmd.exe .\zetcmd.exe .\CommandLine.dll
#remove-item .\Build\CommandLine.dll
Push-Location
cd ".\Build"
ls
Write-host	'-v -aA_TextFile.csv  -bB_TestFile.csv -k1 2 3 -s;'
cmd.exe /c start cmd 
# Pop-Location