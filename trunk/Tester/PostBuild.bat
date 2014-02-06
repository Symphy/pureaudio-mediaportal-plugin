rem // Copyright (C) 2005-2010 Team MediaPortal
rem // http://www.team-mediaportal.com
rem // 
rem // MediaPortal is free software: you can redistribute it and/or modify
rem // it under the terms of the GNU General Public License as published by
rem // the Free Software Foundation, either version 2 of the License, or
rem // (at your option) any later version.
rem // 
rem // MediaPortal is distributed in the hope that it will be useful,
rem // but WITHOUT ANY WARRANTY; without even the implied warranty of
rem // MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
rem // GNU General Public License for more details.
rem // 
rem // You should have received a copy of the GNU General Public License
rem // along with MediaPortal. If not, see <http://www.gnu.org/licenses/>.

rem ------------------------------------------------------------------------------------------------------
rem Usage: call as project post-build event: "$(ProjectDir)PostBuild.bat" "$(SolutionDir)" "$(TargetDir)"
rem Remark: Visual Studio must be run as administrator to allow the files to be copied.
rem ------------------------------------------------------------------------------------------------------

set MPDIR=
call "%~1\Dependencies\SetDirectories.bat"

copy "%MPDIR%\Bass.Net.dll"         "%~2"
copy "%MPDIR%\BassRegistration.dll" "%~2"
copy "%MPDIR%\Core.dll"             "%~2"
copy "%MPDIR%\Utils.dll"            "%~2"
copy "%MPDIR%\Common.Utils.dll"     "%~2"

copy "%MPDIR%\bass.dll"       "%~2"
copy "%MPDIR%\bassmix.dll"    "%~2"
copy "%MPDIR%\bass_vst.dll"   "%~2"
copy "%MPDIR%\basswasapi.dll" "%~2"
copy "%MPDIR%\bass_wadsp.dll" "%~2"

md "%~2\MusicPlayer\plugins\audio decoders"
copy "%MPDIR%\MusicPlayer\plugins\audio decoders\*.*" "%~2\MusicPlayer\plugins\audio decoders"

