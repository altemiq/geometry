# Xbase: Multiple Index files (*.mdx)

------

## The Structure of Multiple Index files (*.mdx)

```
    _______________________  _______
 0 | Version number      *1|  ^
   |-----------------------|  |
 1 | Date of creation      |  |
 2 |      YYMMDD           |  |
 3 |                       |  |
   |-----------------------|  |
 4 | Data file name        | File
 5 | (no extension)        | Header
   :                       :  |
   :                       :  |
19 |                       |  |
   |-----------------------|  |
20 | Block size            |  |
   |                       |  |
   |-----------------------|  |
22 | Block size adder N    |  |
   |                       |  |
   |-----------------------|  |
24 | Production index flag |  |
   |-----------------------|  |
25 | No. of entries in tag |  | *2
   |-----------------------|  |
26 | Length of tag         |  | *3
   |-----------------------|  |
27 | (Reserved)            |  |
   |-----------------------|  |
28 | No.of tags in use     |  |
   |                       |  |
   |-----------------------|  |
30 | (Reserved)            |  |
   |                       |  |
   |-----------------------|  |
32 | No.of pages in tagfile|  |
   |                       |  |
   |                       |  |
35 |                       |  |
   |-----------------------|  |
36 | Pointer to first free |  |
   | page                  |  |
   |                       |  |
39 |                       |  |
   |-----------------------|  |
40 | No.of block available |  |
   |                       |  |
   |                       |  |
43 |                       |  |
   |-----------------------|  |
44 | Date of last update   |  |
   |      YYMMDD           |  |
46 |                       |  |
   |-----------------------|  |
47 | (Reserved)            |  |
   |-----------------------|  |
48 | (Garbage)             |  |
   :                       :  |
   :                       :  |
   |                       |  |        ___|=======================|
543|                       | _V___    / 0 | Tag header page no.   |
   |-----------------------|  |      /    |                       |
544| Tag table entries     | Tag    /     |                       |
   |                       | Table |    3 |                       |
   :.......................:  |    |      |-----------------------| Tag
   :                       :  |    |    4 | Tag name              | table
   :.......................:  |    |      :                       :
   :                       :  |   /       :                       :
   :                       :  |  /        |                       |
   :.......................:__|_/      14 |                       |
   :                       :  |           |-----------------------|
   :                       :  |        15 | Key format         *4 |
   :                       :  |           |-----------------------|
   :.......................:__|_       16 | Forward tag thread (<)|
   :                       :  | \         |-----------------------|
   :                       :  |  \     17 | Forward tag thread (>)|
   :                       :  |   \       |-----------------------|
   :                       :  |    |   18 | Backward tag thread *5|
   |                       |  |    |      |-----------------------|
   |                       |  |    |   19 | (Reserved)            |
M*N|                       |__V__  |      |-----------------------|
   |=======================|  ^    |   20 | Key type           *6 |
  0| Pointer to root page  |  |    |      |-----------------------|
   |                       |  |    |   21 | (Reserved)            |
   |                       |  |    |      :                       :
  3|                       |  |    |      :                       :
   |-----------------------|  |    |   31 |                       |
  4| File size in pages    | Tag   |      |-----------------------|
   |                       | header|   32 | (Garbage)             |
   |                       |  |    |      :                       :
  7|                       |  |    |      |                       |
   |-----------------------|  |     \  N  |                       |
  8| Key format         *7 |  |      \____|=======================|
   |-----------------------|  |
  9| Key type           *8 |  |
   |-----------------------|  |
 10| (Reserved)            |  |
   |                       |  |
   |-----------------------|  |
 12| Index key length   *9 |  |
   |                       |  |
   |-----------------------|  |
 14| Max.no.of keys/page   |  |
   |                       |  |
   |-----------------------|  |
 16| Secondary key type *10|  |
   |                       |  |
   |-----------------------|  |
 18| Index key item length |  |
   |                       |  |
   |-----------------------|  |
 20| (Reserved)            |  |
   |                       |  |
   |                       |  |
   |-----------------------|  |
 23| Unique flag           |  |
   |-----------------------|  |
   |                       |  |
   :                       :  |
   :                       :__V__
N*M|=======================|
```

1. 

2. Number of entries in tag table. Max no. is 48 (30h)

3. Length of each tag table entry. Max is 32 (20h).

4. |     |            |
   | --- | ---------- |
   | 00h | Calculated |
   | 10h | Data field |

5. Previous tag

6. |     |           |
   | --- | --------- |
   | C   | Character |
   | N   | Numerical |
   | D   | Date      |

7. |     |                   |
   | --- | ----------------- |
   | 00h | Right, left, dtoc |
   | 08h | Decending order   |
   | 10h | Fields , string   |
   | 40h | Unique keys       |

8. |     |           |
   | --- | --------- |
   | C   | Character |
   | N   | Numerical |
   | D   | Date      |

9. |           |                                                |
   | --------- | ---------------------------------------------- |
   | Numeric   | Length is 12                                   |
   | Date      | Length is 8                                    |
   | Character | Length is <= 100 bytes and not NULL terminated |

10. |     |                                            |
    | --- | ------------------------------------------ |
    | 0   | Character/numerical (db4), Character (db3) |
    | 1   | Date (db4) Numerical/date (db3)            |