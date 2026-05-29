# Xbase: Year 2000 problems

## How do I cope with the Year 2000 problem?

In the header the date has the format YYMMDD which gives us the wonderful Year 2000 problem. Or does it ?

The 1st of February 1997 is written in the header as: 61h 02h 01h. If your computer can handle date *after* the year 1999 you'll find that, the 1st of February 2001 is written in the header as: 64h 02h 01h.

This means that the valid range for the year in the "last updated at" field in the dbf header is year 1900 - 2155 (value 00h to FFh + base year 1900).

All other fields containing dates has the format YYYYMMDD, which can handle all valid dates (so far :-)

Conclusion: Unless you care about data handling *after* the year 2155, the Xbase file format should not impose any problems.

------

Check out: ["New Year 2000 page on The Oasis" by Phil Barnett:](http://www.the-oasis.net/clipy2k.htm) or [Fixing dBASE DOS Applications for Year 2000 Compliance](http://members.aol.com/adechert/y2kscan.htm)