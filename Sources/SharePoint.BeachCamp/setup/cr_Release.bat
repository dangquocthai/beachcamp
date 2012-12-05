@echo Packing...
set version="1.0"
set folder="../../../Releases/CCIapp-%date:~10,4%-%date:~4,2%-%date:~7,2%"
set filename="BEACHCAMP-%version%-%date:~10,4%%date:~4,2%%date:~7,2%-setup.exe"

del Setup.7z
del %filename%
7z\7zr a Setup.7z Release\* -m0=BCJ2 -m1=LZMA:d25:fb255 -m2=LZMA:d19 -m3=LZMA:d19 -mb0:1 -mb0s1:2 -mb0s2:3 -mx
copy /b 7zSD1.sfx + config.txt + Setup.7z %filename%


-set folder="../../../Releases/CCIapp-%date:~10,4%-%date:~4,2%-%date:~7,2%"
-set filename ="SETUP%date:~10,4%-%date:~4,2%-%date:~7,2%.exe"
-mkdir %folder%
-copy %filename% %folder% /Y
pause