Copy-Item ".\ZetCmd\bin\Debug\ZetCmd.exe" -Destination ".\OutPutBuild"
Copy-Item ".\ZetCmd\bin\Debug\CommandLine.dll" -Destination ".\OutPutBuild"
# http://www.generatedata.com/
Copy-Item ".\TestFiles\A_TestFile.csv" -Destination ".\Build"
Copy-Item ".\TestFiles\B_TestFile.csv" -Destination ".\Build"
Push-Location
cd ".\Build"
ls
Write-host	'-v -aA_TextFile.csv  -bB_TestFile.csv -k1 2 3 -s;'
cmd.exe /c start cmd 
Pop-Location