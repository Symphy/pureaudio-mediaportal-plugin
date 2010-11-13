1. Remarks regarding ASIO.
==========================

This project uses parts of the Steinberg ASIO SDK. Due to licensing restrictions these parts cannot be published in the SVN repository.

If you want to build this project you need to register at www.steinberg.net and obtain an ASIO SDK. It is available at no cost.

The build events of this solution will automatically copy the needed files into the solution structure so it can be build. See also "How to build this solution" below.

Note that regarding distributing the resulting dll you may be bound by licencing restrictions imposed by Steinberg. Check with them prior to distributing anything.

Note: the enduser needs to have the "Microsoft Visual C++ 2008 Redistributable Package (x86)" installed in order to run this dll. This package is available for download at Microsoft.

ASIO is a trademark and software of Steinberg Media Technologies GmbH.


2. How to build this solution.
==============================

In order to build this solution you need to go through the following steps first.

2.1. Install required SDK's
-------------------------
You need to have the following SDK's installed:
- Microsoft Windows SDK, available from www.microsoft.com
- Steinberg ASIO 2.2 SDK, available from www.steinberg.net after registration

2.2. Configure dependency paths
-----------------------------
Edit the batch file "Dependencies\SetDirectories.bat" en enter paths 
to a MediaPortal installation and the ASIO SDK.
