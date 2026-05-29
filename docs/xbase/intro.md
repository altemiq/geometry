# Xbase: What is Xbase

[Xbase](http://foldoc.doc.ic.ac.uk/foldoc/foldoc.cgi?query=xbase) "Generic term for the dBASE family of database languages. Coined in response to threatened litigation over use of the copyrighted trademark 'dBASE'."
: [FOLDOC](http://wombat.doc.ic.ac.uk/) Free On-Line Dictionary Of Computing

[xBase](http://en.wikipedia.org/wiki/XBase) is the generic term for all programming languages that derive from the original dBase (Aston-Tate) programming language. There are indicators that there was a non-commercial predecessor.
: [Wikipedia](http://www.wikipedia.org/), the free encyclopedia

Xbase is a complex of data files (.DBF), indexes (NDX, MDX, CDX etc.) and eventually note files (DBT) for storing large amounts of formatted data in a structured form.

The Xbase family of databases is covering the dBase, Clipper, FoxPro, and their Windows equivalents Visual dBase, Visual Objects, and Visual FoxPro, plus some older products. All are based on the .DBF file format.

Basically Xbase has four different types of files:

The Data File
: (NN.dbf) The core data file arround which all other system files are grouped. Without the data file - no database!

The Index file(s)
: (NN.idx, NN.ndx, NN.cdx) Indexes pointing to a field in the *.dbf

The Full text file(s)
: (NN.dbt) or in Xbase lingo: Memo files. i.e. a separate file containing full text fields)

Descriptive files
: Various files decriping labels, variables, stored expressions etc.

In this list NN reprecents the database name. The name and type of index files depends on the database name.

Xbase is almost compatible with dBASE and is actually a clone of dBASE. The creator of dBASE Ashton-Tate (and later Borland) has the copyright for the name dBASE, but NOT the structure.
Therefor the name Xbase (It smells like - but it's not :-) has been widely used for look-alike data structures.
There has been an attempt to create a standard for Xbase. I have not been able to determine the status of the Xbase commity’s work.

Xbase has - like most dBASE clones - it's offspring in the dBASE III+ file structure. I've tried to add all later enhancements as notes to the structure of the dBASE III files.