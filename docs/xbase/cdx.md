# Xbase: Compound Index files (*.cdx)

## The Structure of Compound Index files (*.cdx)

by **David Kuechler** ([71202.1315@CompuServe.COM](mailto:71202.1315@CompuServe.COM))

A CDX is a compact IDX. The initial IDX contains one key per tag. The key is 10 byte character string which is the tag name. The record number stored with the key is the offset to the root page for that tag.

![img](images/dont.png) There is no description on the compression algorithm

A compact IDX has the following structure:

```
    _______________________  _______
  0 | Pointer to root node  |  ^
  1 |                       |  |
  2 |                       |  |
  3 |                       |  |
    |-----------------------|  |
  4 | Pointer to free list  | File
  5 | (-1 if empty)         | header
  6 |                       |  |
  7 |                  *8   | (page 0)
    |-----------------------|  |
  8 | Version no.      *10  |  |
  9 |                       |  |
 10 |                       |  |
 11 |                       |  |
    |-----------------------|  |
 12 | Key length            |  |
 13 |                  *9   |  |
    |-----------------------|  |
 14 | Index options *1      |  |
    |-----------------------|  |
 15 | Index Signature       |  |
    |-----------------------|  |
 16 | (Reserved)            |  |
 17 |                       |  |
    : (Currently all NULL's):  |
    :                       :  |
    :                       :  |
 501|                       |  |
    |-----------------------|  |
 502| Sort order *2         |  |
 503|                       |  |
    |-----------------------|  |
 504| Total expression      |  |
 505| length (FoxPro 2)     |  |
    |-----------------------|  |
 506| FOR expression length |  |
 507| (binary)              |  |
    |-----------------------|  |
 508| (Reserved)            |  |
 509|                       |  |
    |-----------------------|  |
 510| Key expression length |  |
 511| (binary)              |  |
    |=======================| _v____
 512| Key & FOR expression  |  ^
 513|            *3         |  |
    :                       :  |
    :                       :  |
1023|                       |  |
    |=======================| _v____
  0 | Node attributes *4    |  ^
  1 |                       |  |
    |-----------------------|  |
  2 | Number of keys        |  |
  3 |                       |  |
    |-----------------------| Non
  4 | Pointer to left       | leaf
  5 | brother node          | page
  6 | (-1 if no left node)  |  |
  7 |                       | (compressed)
    |-----------------------|  |
  8 | Pointer to right      |  |
  9 | brother node          |  |
 10 | (-1 if no right node) |  |
 11 |                       |  |
    |-----------------------|  |
 12 |                       |  |
    |                       |  |           __  |=======================|
    |                       |  |          /    | Key data              |
    |                       | NON        /     :                       :
    |                       | leaf      /      :                       :
    |                       | page     /       |                       |
    |                       |  |      /        |-----------------------|
    | Array of key entries  | _|_____/        M| Record number in      |
    |                       |  |               | data file             |
    |                       |  |               | (high order byte      |
    |                       |  |              N|  first)               |
    |                       |  |               |-----------------------|
    |                       | _|_____         m| Pointer to child page |
    :.......................:  |     \         |                       |
    :.......................:  |      \        |                       |
    :.......................:  |       \      n|                       |
 511|                       |  |        \____  |=======================|
    |=======================| _v_____
  0 | Node attributes *4    |  ^
  1 |                       |  |
    |-----------------------|  |
  2 | Number of keys        |  |
  3 |                       |  |
    |-----------------------|  |
  4 | Pointer to left       | Leaf
  5 | brother node          | page
  6 | (-1 if no left node)  |  |
  7 |                       | (compressed)
    |-----------------------|  |
  8 | Pointer to right      |  |
  9 | brother node          |  |
 10 | (-1 if no right node) |  |
 11 |                       |  |
    |-----------------------|  |
 12 | Free space available  |  |
 13 | in page               |  |
    |-----------------------|  |
 14 | Record number mask    |  |
 15 |                       |  |
 16 |                       |  |
 17 |                       |  |
    |-----------------------|  |
 18 | Duplicate count mask  |  |  *11
    |-----------------------|  |
 19 | Trailing byte count mask |  *11
    |-----------------------|  |
 20 |*5 record no           |  |
    |-----------------------|  |
 21 |*5 duplicate count     |  |
    |-----------------------|  ^           __  |=======================|
 22 |*5 trailing count      |  |          /   0| Recno/supCount/       |
    |-----------------------| Leaf       /    1| TrailCount            |
 23 |*6 holding record no   | page      /     2| *7)                   |
    |-----------------------|  |       /      3|                       |
 24 |                       |  |      /        |-----------------------|
    | Array of key entries  | _|_____/        4| Record number in      |
    |                       |  |              5| data file             |
    |                       |  |              6|                       |
    |                       |  |              7|                       |
    |                       |  |               |-----------------------|
    |                       | _|_____         8| Key data              |
    :.......................:  |     \         :                       :
    :.......................:  |      \       N|                       |
 511|                       |  |       \_____  |=======================|
    |=======================| _v_____
```

1. Index options represented as the sum of the following values:
   
   |           |                                   |
   | --------- | --------------------------------- |
   | 01        | Unique index                      |
   | 08        | FOR clause                        |
   | 16 (10h)  | Bit vector (SoftC)                |
   | 32 (20h)  | Compact index format (FoxPro)     |
   | 64 (40h)  | Compounding index header (FoxPro) |
   | 128 (80h) | Structure index (FoxPro)          |

2. |     |            |
   | --- | ---------- |
   | 0   | Ascending  |
   | 1   | Descending |

3. Key first with null terminator, then FOR expression.

4. Node attributes represented as the sum of the following values:
   
   |     |                        |
   | --- | ---------------------- |
   | 0   | Interior node (branch) |
   | 1   | Root page              |
   | 2   | Leaf page              |

5. |          |                                    |
   | -------- | ---------------------------------- |
   | Byte 020 | Number of bits for record number   |
   | Byte 021 | Number of bits for duplicate count |
   | Byte 022 | Number of bits for trailing count  |

6. |           |                                                                                                                          |
   | --------- | ------------------------------------------------------------------------------------------------------------------------ |
   | Byte 023: | Number of bytes holding record number, duplicate count & trailing count (i.e. the total size of values in byte 20 - 22). |

7. At the start of this area, the recno/dupCount/trailCount is stored (bit compressed). Each entry requires the number of bytes as indicated by byte 023.
   
   The key values are placed at the end of this area (working backwards) and are stored by eliminating any duplicates with the previous key and any trailing blanks.

8. |     |                   |
   | --- | ----------------- |
   | -1  | if none (FoxPro)  |
   | 0   | if none (FoxBase) |

9. Number and date keys are 8 bits long . Character keys are <= 100 bytes long.
   
   Note! Character keys are NOT terminated with 00h
   
   |     |                   |
   | --- | ----------------- |
   | -1  | if none (FoxPro)  |
   | 0   | if none (FoxBase) |

10. (Foxbase, FoxPro 1.x) No. of pages in file. (FoxPro 2.x) Reserved.

11. The individual list must be ANDed with these masks in order to calculate the original index.

### Other Notes on CDX

Dates are stored as Julian dates and converted to numbers

Numbers are stored as IEEE doubles (binary values) with the following conversion:

- Convert to an IEEE double (8 bytes)
- Swap the order of the bytes
- IF the number was negative:
  - Invert all the bits
- ELSE
  - Invert only the highest order bit

The advantage of this storage format is it allows numbers to be compared directly using `memcmp()`. 