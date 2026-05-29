## Xbase: Compressed Index files (*.idx)

Compressed Index files (*.idx)

```
            _______________________  _______
 00h /   0 | Pointer to root node  |  ^
 01h /   1 |                     *1|  |
 02h /   2 |                       |  |
 03h /   3 |                       |  |
           |-----------------------|  |
 04h /   4 | Pointer to free list  | File
 05h /   5 | (-1 if empty)         | header
 06h /   6 |                       |  |
 07h /   7 |                     *1|  |
           |-----------------------|  |
 08h /   8 | ( Reserved )          |  |
 09h /   9 |                       |  |
 0Ah /  10 |                       |  |
 0Bh /  11 |                       |  |
           |-----------------------|  |
 0Ch /  12 | Key length            |  |
 0Dh /  13 |                     *1|  |
           |-----------------------|  |
 0Eh /  14 | Index options       *2|  |
           |-----------------------|  |
 0Fh /  15 | Index Signature     *3|  |
           |-----------------------|  |
 10h /  16 | ( Reserved )          |  |
           |                       |  |
           :                       :  |
           :                       :  |
1F4h /  500|                       |  |
           |-----------------------|  |
   h /  501| Sort direction        |  |
        502|                       |  |
           |-----------------------|  |
        503| ( Reserved )          |  |
        504|                       |  |
           |-----------------------|  |
        505| Length of all FOR     |  |
        506| exceptions            |  |
           |-----------------------|  |
        507| ( Reserved )          |  |
        508|                       |  |
           |-----------------------|  |
        509| Length of all FOR     |  |
        510| expressions           |  |
           |-----------------------|  |
1FFh /  511| Key index expression  |  |
           |                     *4|  |
           :                       :  |
3FFh / 1023|                       |  |
           |=======================|  |
           |                       |  |           __  |=======================|
           |                       |  |          /   M| Key data              |
           |                       | NON        /     :                       :
           :                       : leaf      /      :                       :
           :                       : page     /      N|                       |
           :                       :  |      /        |-----------------------|
           | Array of key entries  | _|_____/        O| Record number in      |
           |                       | _|______         | data file             |
           |                       |  |      \        | (high order byte      |
           |                       |  |       \    O+4|  first)               |
           |                       |  |        \____  |=======================|
           |                       |  |
           :                       :  |
           :                       :  |
 ??h /    N|                       |  |
           |=======================| _v_____
```

1. ittle endian (binary):
   -1 (FFh FFh FFh FFh) if empty.

2. Index option:
   
   | Bit flag | Description            |
   | -------- | ---------------------- |
   | 1        | Unique index           |
   | 8        | Index with FOR clauses |
   | 32       | Compact Index file     |
   | 64       | CDX index file         |

3. No documentation

4. Key index expression with multi indexes several keys can be stored here.