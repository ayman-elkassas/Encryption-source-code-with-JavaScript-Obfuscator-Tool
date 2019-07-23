@echo off
call :treeProcess
goto :eof

:treeProcess
rem Do whatever you want here over the files of this subdir, for example:

FOR /R "example" /D %%d in (*) do (

	D:\WORK\js_obfuscator\executableWinx64\jsobfs.exe -d %%d -e ".js"
	cd %%d
	call :treeProcess
	cd ..

)
exit /b