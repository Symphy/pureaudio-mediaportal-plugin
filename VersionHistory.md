## Version 0.2.8.0 beta ##
### Changes: ###
  * Updated to work with MediaPortal 1.6.0 final.
  * Updated to target the .NET 4.0 framework.
  * Added WASAPI support.

## Version 0.2.2.0 stable ##
### Changes: ###
  * Available in MPEInstaller as "known extension".

## Version 0.2.2.0 alpha 4 ##
### Changes: ###
  * Fixed problem when used in combination with the MiniDisplay plugin

## Version 0.2.2.0 alpha 3 ##
### Changes: ###
  * Upgraded to work with MediaPortal 1.1.0 RC2
  * Fixed playback of .xm and .mod files
  * Added basic WMP Visualization support
  * Fixed problem with audiodevice selection dropdowns
  * Settings are now stored in PureAudio.xml, no longer in MediaPortal.xml

## Version 0.2.2.0 alpha 2 ##
### Changes: ###
  * Problem when reaching the end of the playlist is solved. Visualizations still not functional.

## Version 0.2.2.0 alpha ##
### Changes: ###
  * Upgraded to work with MediaPortal 1.1.0.0.
  * Fixed display of trackinfo embedded in webstreams.
  * Visualisations can now be configured from within the plugin's configuration applet

### Known issues: ###
  * Visualizations do not work yet, and enabling them will cause MediaPortal to crash

## Version 0.2.1.0 ##
### Changes: ###
  * Upgraded to BASS library 2.4, so now works with SVN builds 21688 and up.
  * Should be bitperfect now on DirectSound in combination with the custom c-media drivers.
  * Should work now without .NET 2.0 SP1.
  * Added an extra setting "Enable garbage collector fix": If you are using ASIO and having problems with the sound "breaking" just before each songchange,  turning this on will probably fix it for you. You can find it on the "Advanced" tab of the plugin configuration.

## Version 0.2.0.3 ##
### Changes: ###
  * Should work now without .NET 2.0 SP1.

## Version 0.2.0.2 ##
### Changes: ###
  * Should work properly now with the OneButtonMusic plugin.
  * No longer handles last.fm radio by default. The default player will be used when playing last.fm.
  * The plugin currently is not working properly with last.fm, but i haven't been able to find the cause yet. For the time being there's an extra setting on the Extensions page in the plugin setup to disable last.fm radio handling.