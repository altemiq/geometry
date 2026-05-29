## Xbase: FAQ - Frequently asked questions

or "Fake another Question". These are questions I've received and answer I've given.

1. **How do I access / read / export a dBASE database with memo fields without the original dBase product?**
   
   > **A** Any database system that can access dBASE IV files will do. I.e. Open Office, Microsoft Access etc. Open Office is free of charge and an excellent open source tool. And last but not least: It handles memo fields (free text fields). If you're thinking of a one time conversion. Try to make your new database system read the data. OR export the data from dBASE to comma separated files (CSV) and then import the data in the new database system. This will give you an opportunity to redesign your database as well.

2. **Offset for database lock**
   
   **Your document discusses the 4,000,000,000 offset for Fox and 1,000,000,000 for Clipper. Do you happen to know what the original offset is for the original dBase product?**
   
   > **A** The original (and only portable) dBASE IV solution is adding a hidden character field named _DBASELOCK to the table. If you use share to lock the records you will have to check all possible offsets. i.e. for record no: 1, header size 1000, record size=99 + delete flag
   > 
   > | Type               | Start offset  | End offset    |
   > | ------------------ | ------------- | ------------- |
   > | Logical            | 1,001         | 1,100         |
   > | Clipper compatible | 1,000,001,001 | 1,000,001,100 |
   > | FOX compatible     | 4,000,001,001 | 4,000,001,100 |
   > 
   > as well as any other interval used by various vendors.
   > 
   > Back in the not so good DOS days you could use SHARE.EXE to lock a part of a file. This does NOT work on Windows - or any other platform for that matter. I don't remember how to make a portable locking by offset in C. I left C years ago and uses mainly Perl

3. **The header terminator in TWO bytes**
   
   **On your Xbase page I found dbf file structure (http://www.e-bachmann.dk/computing/databases/xbase/xbase.htm#DBF_STRUCT). The header terminator is 0Dh as you wrote, but it is stored in two bytes not in one. Exactly, mentioned terminator, looks:**
   
   ```
   n+1  Terminator (0Dh)
   n+2  Terminator (00h)
   ```
   
   > **A** There has been quite some dispute on this matter. However only one 0Dh is required as you can see on the dump at: index.md#HEX_DBF
   > 
   > ```
   > 0000B0  08000000 01000000 00000000 00000000  ................
   > 0000C0  0D202020 20203152 65636F72 64206E6F  .     1Record no
   > 0000D0  20312020 20202020 20202020 20202020   1
   > ```
   > 
   > Note that there is only one 0Dh at 0000C0.It is true that there MAY be two terminators - but it's not required by dBASE III.

4. **2GB file limit**
   
   **We are currently using FoxPro and are exceeding the 2GB file limitation. Although we split our files to get around this problem, we are looking for an alternative**.
   
   Do you know of any XBase products that will provide us with larger file capabilities?
   
   > **A** Xbase has a definite size limit of 2GB.

5. **Convertion: Clipper 87 - Visual FoxPro**
   
   **I need to know if it is easy to convert an application written in Clipper 87, into Visual FoxPro. I have an opportunity where the least cost solution will win. As a VB / Access etc. solution will require a complete rewrite effectively, I was wondering if there may be a much quicker approach, if we could retain all, or most, of the existing Clipper code???**
   
   > **A** If it's not broke - don't fix it!
   > 
   > Every time you convert data or port applications you might introduce errors. When you go for a new application, always start with a proper analysis of data
   > 
   > * amount (a hand full, hundreds, thousands, billions of records)
   > * type (relational, free text)
   > * structure (tagged fields, comma separated, fixed size)
   > 
   > And choose the database and development platform that matches your data.

6. **forming DBF file from DBT file**
   
   **Is it possible forming DBF file from DBT file because I suppose i have lost DBF files. Can DBT files be used without DBF files?**
   
   > **A** There's really no sense in having a DBT file if you can't address the data inside. And you can't without the proper DBF containing a MEMO field.You can extract the data from the DBT in case you've lost the corresponding DBF. Simply load the file into a text editor and remove the headers. Do *NOT* write the data back into the DBT file!!! Store the data in a new file.

7. **Import: Foxpro to Oracle**
   **I am trying to import data from foxpro into oracle by using plsql ..**
   **I check some suggestions but it did not help , could you suggest me a simpler way ?**
   
   > **A** My best guess is following the procedures in: http://www.experts-exchange.com/Databases/Oracle/Q_20287456.html
   > 
   > If you haven't got the dbf2ora try to export the data from Foxpro as CSV files.
   
   **There is an application named *Genesis Pro* which is written in Foxpro , I don't know the release. All I want is reading its files ( .dbf extesion ) by using oracle plsql/oracle forms d2k.**
   
   **My applications are all based on windows 2000; the release of oracle forms d2k could be 2.1 or 6i and the database is 7.3.4 ;**
   
   **The question : Is possible reading those files with oracle forms ?**
   
   > **A** The answer is NO.
   > 
   > plsql reads formated data from a scheme - not binary data like a Xbase dbf.
   > 
   > Using Foxpro you should be able to dump a CVS file, which you can import into Oracle.

8. **Numerical values in MDX**
   
   **We use MDX index mostly. I expect soon somebody need to include a numerical value as an indexed field. So what is the exact format of numerical values inside the MDX node expressions?**
   
   > **A** The documentation doesn't say - and I haven't got a Foxpro or similar to create an example.
   > 
   > Try to build a simple table with one or two entries, and then debug the MDX file.

9. **Accessing XBASE from MySQL**
   
    **My website is programmed in PHP.XML on MySQL database on Apache Server**
   
    **How or can it interface both directions with XBASE?**
   
   >  **A** That's a MySQL question. Try: http://www.mysql.com
   > 
   >  I don't think that MySQL can access Xbase files interactively. But you can import Xbase files into mySQL (see http://www.mysql.com/doc/en/LOAD_DATA.html)
   > 
   >  However!
   > 
   >  Take a look at: **Max** and **Max Server Pages**. This solution might offer an integration.

10. **Editing Xbase files**
    
    **I need an application to edit my dBASE database**
    
    > **A** If you need shareware or freeware utilities for Xbase (or dBASE) please try searching at http://www.simtel.net
    > 
    > * Max DOS32/W, Version 2.03 : Xbase compiler for text-based applications
    >   Platform: ![Windows](images/windows.png) ![img](images/linux.png)
    >   Type: Freeware
    > 
    > * Dbftools: DBF util for dBase III/IV,FoxPro 2.X
    >   Platform: ![Windows](images/windows.png)
    >   Type: Freeware
    > 
    > * datamn21 : Manage dBase-compatible databases easily
    >   Platform: ![Windows](images/windows.png)
    >   Type: Freeware
    > 
    > * ldcbv30a.zip (Lodestar Database v3.0a: Borland C dbase lib)
    >   Platform: ![Windows](images/windows.png)
    >   Type: Shareware
    > 
    > * Data Quik, Version 2.0 : Find, Copy, Print, View, EMail Database Files
    >   Platform: ![Windows](images/windows.png)
    >   Type: Shareware
    > 
    > * BB DOALL, Version 6.06 : View/Edit/Manage .dbf files w/graphing
    >   
    >   Platform: ![Windows](images/windows.png)
    >   
    >   Type: Shareware
    > 
    > * **Max** and **Max Server Pages**

11. **Tool for creating IDX-files**
    
    **I wrote a program to create Dbase III files based upon your data structures. Thanks for doing the documentation. I now need to create an IDX file for it but wonder if you know of anyone who has already written this in C or something so I don't have to figure out the b-tree coding? Perhaps maybe you know of a freeware DLL that could do this?**
    
    > **A** If you need development tools try http://sourceforge.net/ as well:
    > 
    > Search for. "Xbase" or "dbase"
    > 
    > * [Xbase (formerly XDB)](http://sourceforge.net/projects/xdb/) - Xbase compatible
    >   
    >   Platform: ![Windows](images/windows.png) ![Linux](images/linux.png)
    >   
    >   Programming language: ![C++](images/cpp.png) C++ ![Perl](images/perl.png) Perl ![Python](images/python.png) Python or the hard way:
    >   
    >   [![Google](http://www.google.com/logos/Logo_25wht.gif)](http://www.google.com/search?safe=vss) 

12. **Preventing dBASE reating FoxPro fields**
    
    **We are wondering about "borrowing" one of the apparently dead fields in the header, eg bytes bytes 30-31 Note *3 (dBASE IV) Filled with 00h**
    
    **If anyone with dBase IV tried to read this file they would blow up anyway, because we are using the funny Foxpro fields. Presumably foxpro is ignoring whatever is in bytes 30-31.**
    
    > **A** Well - The version number (or signature) in byte 0 should prevent dBASE from reading a Foxpro data file.The reserved area (which I usually refere to as reserved_1) cannot be interpreted and should not be interpreted by any DBMS. The content is undefined. In dBASE IV this area is filled with 00h, but in dBASE III it is filled with "what ever is on the disc today". You might see some old data from your disc drive appear in the reserved areas in dBASE III.I actually once found transaction logs from a users bank account (sic!)

13. **Is ??? really Xbase / dBASE compatible ???**
    
    > **A** I can't analyze commercial or freeware products in detail, but I'll do my best to answer specific questions:
    > 
    > Make a backup!
    > 
    > Try for yourself!

14. **Can I read data from NDX files?**
    
    **I got some NDX files but I dont know which program can be used to open them. I can open only dbf files by using dbfview.**
    
    > **A** Index files like NDX are only relevant for searching in the DBF - you really don't need them (i.e. you CAN read the data file without the index file).Removing the index file might break you application though.

15. **Interpreting the Xbase version**
    
    **The primary question for programmers is: which header format can be assumed by reading the initial .dbf byte? (Ie, does the file use a version 2 header, a version 3 to 5 header, or a version 7 header?) It's not at all clear what the best logic would be...**
    
    > **A** The logic is simple and crude: You read the first byte and try to mask out which version and configuration you've got. Each vendor uses this format as they prefere. Interpret the byte like this:
    > 
    > MSSSFVVV
    > 
    > M dBASE III memo file
    > 
    > S SQL mask
    > 
    > F Memo field flag
    > 
    > V Version
    > 
    > The three lowest bits are version, then comes the MEMO field flag (but with no indication about the type of memo field) etc. It's trial and error all the way.

16. **Destinguins structure from conflicting version numbers**
    
    **if the masked version bits are 011, do you have FoxBASE+/dBASE III PLUS or dBASE IV?**
    
    **if the masked version bits are 101, do you have Clipper SIX driver, or FoxPro 2.x (or earlier)?**
    
    **if the masked version bits are 010, do you have FoxBASE or Visual FoxPro?**
    
    **Are the "conflicting" versions identical in their use of the remaining file structure so it doesn't matter, or are further checks necessary because two different versions with the same version bits don't generate the same file structure?**
    
    > **A** Feeling at bit depressed: There is no way of telling. No garantee that various vendors behave well and follow the rules of the game. There is only one way of doing this: Find out what software package that was used to create the data files - and stick to their rules ie. use their interface.

17. **Unique indexes in MDX**
    
    **I do not see any description of the differences in unique indexes for MDX files. I created a DBF with one 12 character field named GL. I create a unique index on this field using Delphi having the BDE setup to dBase file version 3. I This unique index does NOT allow duplicate field values in GL. When I add the index to the same file using Visual dBase version 5.0 (16 bit), the index is slightly different. This index allows duplicate values in the field. I would like to recreate the index in Delphi the same as it is created in Visual dBase.**
    **The differences are in bytes 2056, 2071, and 2528.**
    **Here is the output of the differences using DOS FC (with a /b switch):**
    
    ```
    00000808: 70 50
    00000817: 21 01
    000009E0: 00 01
    ```
    
    **This is the byte offset (in hex), and the byte value for file 1 followed by the byte value for file2. File 1 would be the MDX file created with Delphi and the other is the MDX file created with Visual dBase.**
    **Can you tell me where I can find a detailed description of the MDX file structure that would explain these differences? Do you know what the differences mean?**
    
    > **A** Well - it's not a lot that I can say about this.
    > First you'll have to find out whether this is a diff in the "Tag table entries" Or perhaps in an area marked "Reserved" or "Garbage", in which case the difference is irrelevant.
    > To calculate the layout of the "Tag table entries" use this formular
    > 
    > > Header size + ( No. of entries in tag * Length of tag )
    > 
    > This gives you the offset of the first index page after the "Tag table entries".
    > If this number is higher than 2528 then the diff is in the "Tag table entries". You will then have to calculate where in the table the diffs are:
    > 
    > > (2528 - Header size) % ( No. of entries in tag * Length of tag )
    > 
    > The reminder is the offset in the tag table entry. If the reminder is > 20 just ignore the diff - is in the "Reserved" / "Garbage" area.
    > In general: You are comparing files layout from two different vendors. There is no garantee that they will use the same structure!

18. **Is there life after death?**
    
    >  **A** Not sure - haven't been there - yet! Hoping for the best - expecting the worst }:-)
    > 
    >  *This IS a genuine question from a Xbase letter! (sic!)*

19. **Why is the sky blue?**
    
    >  **A** Believe it or not! [http://www.google.com/search?hl=en&ie=UTF-8&oe=UTF-8&q=%22Why+is+the+sky+blue%22](http://www.google.com/search?hl=en&ie=UTF-8&oe=UTF-8&q="Why+is+the+sky+blue")
    > 
    >  sends you to: http://www.why-is-the-sky-blue.org/why-is-the-sky-blue.html
    > 
    >  It's all there!!!
    > 
    >  *This IS a genuine question from a Xbase letter!*