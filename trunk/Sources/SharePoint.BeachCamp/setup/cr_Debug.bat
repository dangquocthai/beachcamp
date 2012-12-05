del Setup_debug.7z
del Setup_Debug.exe
7z\7zr a Setup.7z Debug\ -m0=BCJ2 -m1=LZMA:d25:fb255 -m2=LZMA:d19 -m3=LZMA:d19 -mb0:1 -mb0s1:2 -mb0s2:3 -mx
copy /b 7zSD1.sfx + configd.txt + Setup_Debug.7z IOFFICEDEBUG.exe
