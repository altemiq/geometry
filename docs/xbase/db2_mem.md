## Xbase: dBASE II Memory Files (*.mem)

```
    _______________________  _______
 0 |                       |  ^
 1 | Variable name         |  |
   : terminated by 00h     : Header
   :                       :  |
 9 |                       |  |
10 |                       |  |
   |-----------------------|  |
11 | Variable type     *1  |  |
   |-----------------------|  |
12 | Length of stored value|  |
   |-----------------------|  |
13 | ( Reserved )          |  |
14 |                       |  |
   |-----------------------|  |
15 | Field length  (binary)|  | *2
   |-----------------------|  |
16 | Decimal count (binary)|  |
   |-----------------------|  |
17 | ( Reserved )   00h    |  |
18 | ( Reserved )   00h    |  |
   |-----------------------|  |
19 | Value of variable     |  |
   :                    *3 :  |
   :                       :  |
   :                       :  |
   :                       :__V_____
N  |=======================|
```

1. Variable type
   - C3h
     - Character variable
   - CEh
     - Numerical variable
   - CCh
     - Logical variable
2. Header: "E" marks the start of a definition
3. Variable type
   - Text
     - entries might have leading 00h, if text is shorter than field
   - Logical
     - 17 bytes are reserved, but only the last byte is used for 00h (false) or 01h (true).
   - End-of-File
     - Valid end of file (i.e. end of data) is indicated by 1Ah.
   - Numeric
     - are encoded in internal dBASEII format (no further description)