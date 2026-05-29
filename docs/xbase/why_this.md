# Xbase: Why this document

I've been looking for a simple database tool for my work with C programming and bibliographical database systems. During my investigation of a lot of BBS's, FTP sites, toolboxes etc. I've discovered at lot of almost-ready-to-fly Xbase clones. This made me wonder why no one has collected a description of Xbase files AND made a simple package of C functions to handle this type of data files.  
Unfortunately I never got beyond the point of describing the structure (sic!). But a lot of brave people has done a lot of hard work to make the libraries available for you like (see the [reference list](references.md)).

------

WARNING!!! 
![fatal](images/fatal.png)

DO *NOT*
: attempt to manipulate any critical database files *WITHOUT* making a proper and validated backup first. 

DO *NOT*
:  access any database files in a multi-user environment! 

DO *NOT*
: access the files in text mode. Use binary mode only ! 
