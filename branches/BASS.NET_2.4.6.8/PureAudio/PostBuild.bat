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

set MPDIR=
call "%1\Dependencies\SetDirectories.bat"

copy "%1\Bin\%2\PureAudio.dll" "%MPDIR%\plugins\ExternalPlayers"
copy "%1\Bin\%2\ASIO.Interop.dll" "%MPDIR%\plugins\ExternalPlayers"
copy "%1\Bin\%2\WMPEffects.Interop.dll" "%MPDIR%\plugins\ExternalPlayers"

