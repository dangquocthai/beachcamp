@echo off

set folder="../../../Releases/CMapp-%date:~10,4%-%date:~7,2%-%date:~4,2%"
mkdir %folder%
copy setup.exe %folder% /Y
pause