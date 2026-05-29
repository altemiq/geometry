## Xbase: Memory Files (*.mem)

```
    _______________________  _______
 0 |                       |  ^
 1 | Variable name         |  |
   : terminated by 00h     : Header
 9 |                       |  |
10 |                       |  |
   |-----------------------|  |
11 | Variable type       *1|  |
   |-----------------------|  |
12 | ( Reserved )          |  |
   :                       :  |
15 |                       |  |
   |-----------------------|  | <-
16 | Field length  (binary)|  |   |
   |-----------------------|  |   | *2
17 | Decimal count (binary)|  |   |
   |-----------------------|  | <-
18 | ( Reserved )          |  |
   :                       :  |
   :                       :  |
31 |                       | _v_____
   |-----------------------|  |
32 | Variable            *3|  |
   :                       :  |
   |-----------------------|  |
nn | Variable              |  |
   :                       :  |
   :                       :__V_____
N  |=======================|
```

1. Data type
   
   Note that the high bit is always set on this variable! i.e. you must add 80h to the normal character type.
   
   Example: Character (C) should read: "C" + high bit: 43h + 80h = C3h
   
   | Byte value | Description              |
   | ---------- | ------------------------ |
   | C3h        | (Char) ASCII text string |
   | C4h        | Date                     |
   | CCh        | Logical                  |
   | CEh        | Numeric                  |

2. Field length for non-numerical fields. Byte 16 is normally field length (0-255) and byte 17 represents the high byte for field length (256-65535). (FoxPro, Clipper)

3. Plus byte: dBASE III+ and later only
   
   | Three bit flags |                                |
   | --------------- | ------------------------------ |
   | 0               | Form feed PRIOR TO report      |
   | 1               | Form feed AFTER report         |
   | 2               | Plain report without form feed |