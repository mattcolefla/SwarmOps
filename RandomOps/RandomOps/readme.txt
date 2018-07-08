RandomOps - (Pseudo) Random Number Generator For C#
Copyright (C) 2003-2010 Magnus Erik Hvass Pedersen.
Please see the file license.txt for license details.
RandomOps on the internet: http://www.Hvass-Labs.org/


Installation:

To install RandomOps follow these simple steps:
1. Unpack the RandomOps archive to a convenient
   directory.
2. In MS Visual Studio open the Solution in which you
   will use RandomOps.
3. Add the RandomOps project to the solution.
4. Add a reference to RandomOps in all the projects
   which must use it.


Compiler Compatibility:

RandomOps was developed in MS Visual C# 2010 with .NET
framework v4.


Update History:

Version 2.1:
- Changed software license to a simpler one.

Version 2.0:
- Renamed class ThreadSafe to ThreadSafe.Wrapper
- Added ThreadSafe.Independent
- Added ThreadSafe.MWC256 and ThreadSafe.CMWC4096

Version 1.2:
- Added Disk, uniform RNG of a disk.
- Added Circle, uniform RNG of a circle.
- Added Sphere, uniform RNG of a n-sphere.
- Added usage of Disk to Gauss.

Version 1.1:
- Added KISS, XorShift, MWC256, CMWC4096 by George Marsaglia.
- Added IndexDistribution
- Added SumUInt32

Version 1.0:
- First release.
