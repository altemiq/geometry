## Xbase: HISTORY

- 2012-05-24
  
  ['T' fields](data_types.md) reversed ingeneered
  
   An 8-byte field.
  
  * The first 4 bytes are a 32-bit little-endian integer representation of the Julian date, where Oct. 15, 1582 = 2299161 per http://www.nr.com/julian.html
  
  * The last 4 bytes are a 32-bit little-endian integer time of day represented as milliseconds since midnight.
    
    Thanks to David Burton

- 2010-02-27
  
  VISUAL OBJECTS (first 1.0 versions) for the Dbase III files (NTX clipper driver).
  
  * FILE WITHOUT DBT: 07h. (Normally 03 with DOS codepage)
  
  * FILE WITH DBT: 87h (Normally 83 with DOS codepage).
    
    Thanks to Walter Stubbs.

- 2009-13-03
  
  **typhos!**
  
  ​    A few "mispellings" fixed. Thanks to Xan Gregg

- 2007-03-03
  
  [FAQ](faq.md)
  
  ​    How to read data without dBASE

- 2006-02-16
  
  [Data types](data_types.md)
  
  ​    Length of NUMERIC values were wrong. Should read "up till 18 characters long (include sign and decimal point)"

- 2005-08-26
  
  [xBaseView](references.md#REF_XBASEVIEW) (v.6.06) - Universal Database Viewer (and Editor)
  
  ​    Adding new link to Windows based edior

- 2005-08-22
  
  [**Pazdziora, Jan**: *DBD::XBase : Module for dealing with XBase files*](references.md#REF_PAZDZIORA_JAN)
  
  ​    Adding new link to CPAN

- 2005-07-07
  
  [dbf.html](dbf.md#DBF_NOTE_20_TARGET)
  
  ​    Adding link to new data types.
  [Added new types](data_types.md)
  
  ​    @, O, +
  
  Reference list
  
  ​    [Habour project](references.html#REF_HABOUR_PROJECT)

- 2005-04-07
  
  [dbf.md](dbf.html#DBF_STRUCT)
  
    Bug fix: Missing leading 0 in: 7Bh 01111011 dBASE IV with memo
  [Logic in version number](faq.md)
  
    Reference to [Wikipedia](http://en.wikipedia.org/wiki/XBase)
  
  Reference list
  
    [Habour project](references.md#REF_HABOUR_PROJECT)

- 2005-02-05
  
  [dbf.md](dbf.md#DBF_NOTE_9_SOURCE)
  
     Updates after server crach
  [Intro: What is Xbase](intro.md)
  
     Reference to [Wikipedia](http://en.wikipedia.org/wiki/XBase)
  
  Reference list
     [Habour project](references.md#REF_HABOUR_PROJECT)

- 2005-01-25
  
  [dbf.md](dbf.html#DBF_NOTE_9_SOURCE)
  
     Minor correction: "Field deleted flag" -> "Record deleted flag"
  [ndx_example.md](ndx_example.html)
  
     The ENGEL link should read 4
  
     E-mail link in page footer corrected

- 2003-08-26
  
  Various
  
    Update DATE on front page ;-)
  
    Update FAQ!
  
    New signatures on SIx driver and CLIP

- 2003-04-26
  
  **Vital oops! The original link from www.e-bachmann.dk/docs/xbase.html did not work.**

- 2003-04-23
  
  World Book and Copyright Day (http://www.unesco.org)
  
    Major rewrite - probably THE final rewrite: Virtually all chapters updated and enhanced. Document now available as chapters.

- 2000-09-13
  
  Added reference to Turbo Pascal Toolbox

- 2000-07-05
  
  Updated links and references

- 2000-06-04
  
  Added a reference to tDbf.

- 2000-05-19
  
  Updated a few references.

- 2000-01-17
  
  Updated a few references. And a few misspellings corrected

- 1999-10-22
  
  References added. And a few misspellings corrected

- 1999-07-30
  
  dBASE version sceme.

- 1999-03-08
  
  Minor additions.

- 1998-11-28
  
  Updating some links.

- 1998-09-21
  
  The path to *CxBase : A Perl Library for manipulating Xbase files* fixed:

- 1998-08-26
  
  More references.

- 1998-08-24
  
  More references (Perl).
  
  List of File extentions.

- 1998-07-16
  
  FoxPro/FoxBase specific notes.

- 1998-04-07
  
  More references. Java and Rexx.

- 1998-03-26
  
  New references.

- 1998-03-03
  
  A reference to a Perl Xbase manipulation library added.

- 1997-09-29
  
  A reference to a Perl Xbase manipulation library added.

- 1997-09-17
  
  Fixing minor bug in reference list
  
  Reference list: Pratap Pereira moved to new host

- 1997-08-11
  
  Updating structure of:
  
    data file (\*.dbf): codepages etc.
  
    Memo files (\*.dbt): dBASE III & dBASE IV does *not* use the same structure
  
    Memory files (\*.mem): Full structure of file
  
    Clipper Index (\*.NTX): Page structure added. Header structure improvedd
  
    BASE II data and index file
  
  Adding structure of:
  
    Fox Pro object and memo files (\*.fpt)
  
    dBASEII Memory files (\*.mem)
  
    dBASE IV Multiple index files (\*.MDX)

- 1997-06-02
  
  Fixing bug in calculating dates for "The Year 2000": 1900 + 255 = 2155

- 1997-04-16
  
  Note on Year 2000 problems.
  
  Correcting page description in index file .ndx.

- 1997-04-18
  
  Added the chapter: 
  
  *The Structure of Compound Index files (\*.cdx)* by David Kuechler

- 1997-03-10
  
  Still more references and updates of references.
  
  Refining description of NTX file.

- 1997-01-27
  
  Description of .MEM (Memory index file) and .NTX (Clipper Index File).
  
  Description of Database Container in Visual FoxPro
  
  More references and various minor changes in text.

- 1997-01-09
  
  Description of .MDX (Multiple index file) - This seems to be a mistake :-(

- 1996-11-27
  
  Document converted to HTML

- 1996-11-19
  
  Minor fixes:
  
  Elements in Field Descriptor Array are exact 32 B long.
  
  Typing error in data type Float.
  
  "What to check on opening a .DBF file" added.

- 1996-10-18
  
  Chapter on read/write locks using share by Phil Barnett ([midnight@the-oasis](mailto:midnight@the-oasis.net))

- 1996-10-15
  
  Change in description of structure of index files.

  Major mistake! The previous version described the structure for dBASE II index files.

  Only excuse: This mistake was made by several of my sources :-(

- 1996-10-03
  
  First public version of this document (as far as I remember :-)