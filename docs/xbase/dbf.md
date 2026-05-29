# Xbase Data file (*.dbf)

The data file with suffix DBF is the central table in an Xbase database. All other data files are related to this one file.
The Data File is a mix of binary and ASCII data. Header contains binary data. The records are all in ASCII (except ofcause the binary objects like pictures).

![img](images/chock.png) Several sources claim that dBASE clears the header on creation with blanks (20h). But I've seen data in the reserved areas.

Some documents states that deleted records are overwritten by new valid records. My experience is that new records are **appended** to the data file - not inserted.
A deleted record can only be deleted physically using the PACK command. Even after PACK the deleted record exists after the EOF mark. The file is not truncated in dBASE III (But don't count on it ;-)

![img](images/destructive.png) Note that this structure is valid for Xbase - and dBASE v. III - 5. Later versions of dBASE has a different layout, like dBASE 7 (see http://www.dbase.com/KnowledgeBase/int/db7_file_fmt.htm)

```
00h /   0│ Version number      *1│  ^
         ├───────────────────────┤  │
01h /   1│ Date of last update   │  │
02h /   2│      YYMMDD        *21│  │
03h /   3│                    *14│  │
         ├───────────────────────┤  │
04h /   4│ Number of records     │ Record
05h /   5│ in data file          │ header
06h /   6│ ( 32 bits )        *14│  │
07h /   7│                       │  │
         ├───────────────────────┤  │
08h /   8│ Length of header   *14│  │
09h /   9│ structure ( 16 bits ) │  │
         ├───────────────────────┤  │
0Ah /  10│ Length of each record │  │
0Bh /  11│ ( 16 bits )     *2 *14│  │
         ├───────────────────────┤  │
0Ch /  12│ ( Reserved )        *3│  │
0Dh /  13│                       │  │
         ├───────────────────────┤  │
0Eh /  14│ Incomplete transac.*12│  │
         ├───────────────────────┤  │
0Fh /  15│ Encryption flag    *13│  │
         ├───────────────────────┤  │
10h /  16│ Free record thread    │  │
11h /  17│ (reserved for LAN     │  │
12h /  18│  only )               │  │
13h /  19│                       │  │
         ├───────────────────────┤  │
14h /  20│ ( Reserved for        │  │            _        ╒═══════════════════════╕ ______
         │   multi─user dBASE )  │  │           / 00h /  0│ Field name in ASCII   │  ^
         : ( dBASE III+ ─ )      :  │          /          : (terminated by 00h)   :  │
         :                       :  │         │           │                       │  │
1Bh /  27│                       │  │         │   0Ah / 10│                       │  │
         ├───────────────────────┤  │         │           ├───────────────────────┤ For
1Ch /  28│ MDX flag (dBASE IV)*14│  │         │   0Bh / 11│ Field type (ASCII) *20│ each
         ├───────────────────────┤  │         │           ├───────────────────────┤ field
1Dh /  29│ Language driver     *5│  │        /    0Ch / 12│ Field data address    │  │
         ├───────────────────────┤  │       /             │                     *6│  │
1Eh /  30│ ( Reserved )          │  │      /              │ (in memory !!!)       │  │
1Fh /  31│                     *3│  │     /       0Fh / 15│ (dBASE III+)          │  │
         ╞═══════════════════════╡__│____/                ├───────────────────────┤  │ <─┐
20h /  32│                       │  │  ^          10h / 16│ Field length       *22│  │   │
         ├ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ┤  │  │                  ├───────────────────────┤  │   │ *7
         │                    *19│  │  │          11h / 17│ Decimal count      *23│  │   │
         ├ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ┤  │  Field              ├───────────────────────┤  │ <─┘
         │                       │  │ Descriptor  12h / 18│ ( Reserved for        │  │
         :. . . . . . . . . . . .:  │  │array     13h / 19│   multi─user dBASE)*18│  │
         :                       :  │  │                  ├───────────────────────┤  │
      n  │                       │__│__v_         14h / 20│ Work area ID       *16│  │
         ├───────────────────────┤  │    \                ├───────────────────────┤  │
      n+1│ Terminator (0Dh)      │  │     \       15h / 21│ ( Reserved for        │  │
         ╞═══════════════════════╡  │      \      16h / 22│   multi─user dBASE )  │  │
      m  │ Database Container    │  │       \             ├───────────────────────┤  │
         :                    *15:  │        \    17h / 23│ Flag for SET FIELDS   │  │
         :                       :  │         │           ├───────────────────────┤  │
    / m+263                      │  │         │   18h / 24│ ( Reserved )          │  │
         ╞═══════════════════════╡__v_ ___    │           :                       :  │
         :                       :    ^       │           :                       :  │
         :                       :    │       │           :                       :  │
         :                       :    │       │   1Eh / 30│                       │  │
         │ Record structure      │    │       │           ├───────────────────────┤  │
         │                       │    │        \  1Fh / 31│ Index field flag    *8│  │
         │                       │    │         \_        ╘═══════════════════════╛ _v_____
         │                       │ Records
         ├───────────────────────┤    │
         │                       │    │          _        ╒═══════════════════════╕ _______
         │                       │    │         / 00h /  0│ Record deleted flag *9│  ^
         │                       │    │        /          ├───────────────────────┤  │
         │                       │    │       /           │ Data               *10│  One
         │                       │    │      /            : (ASCII)            *17: record
         │                       │____│_____/             │                       │  │
         :                       :    │                   │                       │ _v_____
         :                       :____│_____              ╘═══════════════════════╛
         :                       :    │
         │                       │    │
         │                       │    │
         │                       │    │
         │                       │    │
         │                       │    │
         ╞═══════════════════════╡    │
         │__End_of_File__________│ ___v____  End of file ( 1Ah )  *11
```

1. Also called signature.
   
   | Value | Bit mask  | Description                                                                                                                                                                         |
   | ----- | --------- | ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
   | 02    | h00000010 | FoxBase                                                                                                                                                                             |
   | 03    | h00000011 | File without DBT                                                                                                                                                                    |
   | 04    | h00000100 | dBASE IV w/o memo file                                                                                                                                                              |
   | 05    | h00000101 | dBASE V w/o memo file                                                                                                                                                               |
   | 07    | h00000111 | VISUAL OBJECTS (first 1.0 versions) for the Dbase III files w/o memo file                                                                                                           |
   | 30    | h00110000 | Visual FoxPro                                                                                                                                                                       |
   | 30    | h00110000 | Visual FoxPro w. DBC                                                                                                                                                                |
   | 31    | h00110001 | Visual FoxPro w. AutoIncrement field                                                                                                                                                |
   | 43    | h01000011 | .dbv memo var size (Flagship)                                                                                                                                                       |
   | 7B    | h01111011 | dBASE IV with memo                                                                                                                                                                  |
   | 83    | h10000011 | File with DBT                                                                                                                                                                       |
   | 83    | h10000011 | dBASE III+ with memo file                                                                                                                                                           |
   | 87    | h10000111 | VISUAL OBJECTS (first 1.0 versions) for the Dbase III files (NTX clipper driver) with memo file                                                                                     |
   | 8B    | h10001011 | dBASE IV w. memo                                                                                                                                                                    |
   | 8E    | h10001110 | dBASE IV w. SQL table                                                                                                                                                               |
   | B3    | h10110011 | .dbv and .dbt memo (Flagship)                                                                                                                                                       |
   | E5    | h11100101 | Clipper SIX driver w. SMT memo file.<br /><br />**Note!** Clipper SIX driver sets lowest 3 bytes to 110 in descriptor of crypted databases. So, 3->6, 83h->86h, F5->F6, E5->E6 etc. |
   | F5    | h11110101 | FoxPro w. memo file                                                                                                                                                                 |
   | FB    | h11111011 | FoxPro ???                                                                                                                                                                          |
   
   dBASE IV bit flags:
   
   | Bit | Description           |
   | --- | --------------------- |
   | 0-2 | Version no. i.e. 0-7  |
   | 3   | Presence of memo file |
   | 4-6 | Presence of SQL table |
   | 7   | DBT flag              |
   
    `MSSSFVVV`
   
   |     |                     |
   | --- | ------------------- |
   | M   | dBASE III memo file |
   | S   | SQL mask            |
   | F   | Memo field flag     |
   | V   | Version             |

2. Sum of lengths of all fields  + 1 (deletion flag)

3. Filled with 00h (dBASE IV)

4. Production index / Multiple index file
   
   (dBASE IV)
   
   | Value | Description                      |
   | ----- | -------------------------------- |
   | 01h   | MDX file present                 |
   | 00h   | no MDX file (index upon demand). |
   
   (FoxBase)
   
   | Value | Description                      |
   | ----- | -------------------------------- |
   | 01h   | CDX compound index file present, |
   | 00h   | no CDX file.                     |
   
   (Visual FoxPro)
   
   | Value | Description                |
   | ----- | -------------------------- |
   | 02h   | With memo                  |
   | 04h   | Database Container (DBC)   |
   | 07h   | DBC (incl. memo & indexes) |

5. (Foxpro) Code page : These values follow the DOS / Windows Code Page values.
   
   | Value | Description                | Code page      |
   | ----- | -------------------------- | -------------- |
   | 01h   | DOS USA                    | code page 437  |
   | 02h   | DOS Multilingual           | code page 850  |
   | 03h   | Windows ANSI               | code page 1252 |
   | 04h   | Standard Macintosh         |                |
   | 64h   | EE MS-DOS                  | code page 852  |
   | 65h   | Nordic MS-DOS              | code page 865  |
   | 66h   | Russian MS-DOS             | code page 866  |
   | 67h   | Icelandic MS-DOS           |                |
   | 68h   | Kamenicky (Czech) MS-DOS   |                |
   | 69h   | Mazovia (Polish) MS-DOS    |                |
   | 6Ah   | Greek MS-DOS (437G)        |                |
   | 6Bh   | Turkish MS-DOS             |                |
   | 96h   | Russian Macintosh          |                |
   | 97h   | Eastern European Macintosh |                |
   | 98h   | Greek Macintosh            |                |
   | C8h   | Windows EE                 | code page 1250 |
   | C9h   | Russian Windows            |                |
   | CAh   | Turkish Windows            |                |
   | CBh   | Greek Windows              |                |

6. Field data address
   
   ![img](images/chock.png)Note that this field has two very different interpretations.
   
   | Version | Offset | Description                               |
   | ------- | ------ | ----------------------------------------- |
   | dBASE   | 12-15  | Address in memory.                        |
   | FoxPro  | 12-13  | Offset of field from beginning of record. |
   
   The field addresss is irellevant for other applications.

7. Field length for non-numerical fields. (FoxPro, Clipper)
   Byte 16 is normally field length (0-255) and byte 17 represents the high byte for field length (256-65535).

8. Index field flag  (dBASE IV):
   
   | Value | Description                        |
   | ----- | ---------------------------------- |
   | 00h   | No key for this field (ignored)    |
   | 01h   | Key exists for this field (in MDX) |

9. Deleted flag:
   
   | Value       | Description       |
   | ----------- | ----------------- |
   | 2Ah (*)     | Record is deleted |
   | 20h (blank) | Record is valid   |

10. There are no field separators for record terminators.

11. End-of-file
    dBASE II regards any End-of-File 1Ah value as the end of the file. dBASE III regard an End-of-File as an ordinary character, however it appends an extra End-of-File character at the physical end of the file.
    If the file is packed the physical size of the file may be larger than the logical i.e. there may be garbage after the EOF mark.

12. (dBASE IV) Incomplete transaction:
    
    | Value | Description                        |
    | ----- | ---------------------------------- |
    | 00h   | Transaction ended (or rolled back) |
    | 01h   | Transaction started                |

13. Encryption flag (dBASE IV):
    
    ![img](images/destructive.png)Be very careful NOT to modify this flag!
      This is the only indication that the content is enctrypted.
    
    | Value | Description    |
    | ----- | -------------- |
    | 00h   | Not encrypted  |
    | 01h   | Data encrypted |

14. Stored at binary. Unsigned.

15. Database Container (DBC) 263 bytes for backlist. (Visual FoxPro)
    Included in header structure.

16. Work area ID  is 01h in all dBASE III files

17. An empty memo field has a reference filled with 10 blanks.

18. Field Flags
    
    (FoxPro/FoxBase):
    
    | Value | Description                            |
    | ----- | -------------------------------------- |
    | 01h   | System column (not visible to user)    |
    | 02h   | Column can store null values           |
    | 04h   | Binary column (for CHAR and MEMO only) |

19. Max 128 fields

20. ?? See [Data types](data_types.md)

21. Date in header is without century YYMMDD and date in records are with century YYYYMMDD. Valid interval is 00h - FFh. Add base year 1900 and you'll have the interval 1900 - 2155.

22. Field length Max = 255. Valid types (See [Data types](data_types.md))

23. Decimal count Numeric <= 15
