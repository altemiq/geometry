# Xbase: Uncompressed Index files (*.idx)

Uncompressed Index files (*.idx)

```
            _______________________  _______
 00h /   0 | Pointer to root node  |  ^
 01h /   1 |                       |  |
 02h /   2 |                       |  |
 03h /   3 |                       |  |
           |-----------------------|  |
 04h /   4 | Pointer to free list  | File
 05h /   5 | (-1 if empty)         | header
 06h /   6 |                       |  |
 07h /   7 |                     *3|  |
           |-----------------------|  |
 08h /   8 | Pointer to EOF        |  |
 09h /   9 |                       |  |
 0Ah /  10 |                       |  |
 0Bh /  11 |                       |  |
           |-----------------------|  |
 0Ch /  12 | Key length            |  |
 0Dh /  13 |                     *4|  |
           |-----------------------|  |
 0Eh /  14 | Index options       *1|  |
           |-----------------------|  |
 0Fh /  15 | Index Signature       |  |
           |-----------------------|  |
 10h /  16 | Key expression        |  |
 11h /  17 |                       |  |
           :                       :  |
           :                       :  |
 EBh /  235|                       |  |
           |-----------------------|  |
 ECh /  236| FOR expression        |  |
           |                       |  |
           :                       :  |
           :                       :  |
1C7h /  455|                       |  |
           |-----------------------|  |
1C8h /  456| (Reserved)            |  |
           |                       |  |
           :                       :  |
           :                       :  |
1FFh /  511|                       |  |
           |=======================| _v____
 00h /   0 | Node attributes     *2|  ^
 01h /   1 |                       |  |
           |-----------------------|  |
 02h /   2 | Number of keys      *6|  |
 03h /   3 |                       |  |
           |-----------------------| Non
 04h /   4 | Pointer to left       | leaf
 05h /   5 | brother node          | page
 06h /   6 | (-1 if no left node)  |  |
 07h /   7 |                       | (compressed)
           |-----------------------|  |
 08h /   8 | Pointer to right      |  |
 09h /   9 | brother node          |  |
 0Ah /  10 | (-1 if no right node) |  |
 0Bh /  11 |                       |  |
           |-----------------------|  |
 0Ch /  12 |                       |  |
           |                       |  |           __  |=======================|
           |                       |  |          /   M| Key data              |
           |                       | NON        /     :                       :
           :                       : leaf      /      :                       :
           :                       : page     /      N|                       |
           :                       :  |      /        |-----------------------|
           | Array of key entries  | _|_____/        O| Record number in    *5|
           |                       | _|______         | data file             |
           |                       |  |      \        | (Big endian binary)   |
           |                       |  |       \    O+4|                       |
1FFh /  511|                       |  |        \____  |=======================|
           |=======================| _v_____
```

1. Index options

   represented as the sum of the following values:

   | Value     | Description                       |
   | --------- | --------------------------------- |
   | 01        | Unique index                      |
   | 08        | FOR clause                        |
   | 16 (10h)  | Bit vector (SoftC)                |
   | 32 (20h)  | Compact index format (FoxPro)     |
   | 64 (40h)  | Compounding index header (FoxPro) |
   | 128 (80h) | Structure index (FoxPro)          |

2. Node attributes

   represented as the sum of the following values:

   | Value | Description |
   | ----- | ----------- |
   | 0 | Index node |
   | 1 | Start page |
   | 2 | End page |

3. Pointer value:

   | Value                  | Description       |
   | ---------------------- | ----------------- |
   | -1 (= FFh FFh FFh FFh) | if none (FoxPro) |
   | 0 | if none (FoxBase) |

4. Node attributes

   represented as the sum of the following values:

   | Value | Description |
   | ----- | ----------- |
   | 4     | Number and date keys are 8 bits long . Character keys are <= 100 bytes long. Note! Character keys are **NOT** terminated with 00h |
   | -1 | if none (FoxPro) |
   | 0  | if none (FoxBase) |

4. Pointer

   Interpretated by attrbute type

   | Value | Description |
   | ----- | ----------- |
   | 0-1 | Pointer to next lowest not containg section for tree |
   | 2-3 | Pointer to record in data file |

   

   

8. ?? Must be in the interval 0 - 100, where 0 is empty

## Schematic Tree structure in uncompresse FoxPro idx

```
                         ____ ________ ____ 
                        |    |        |    |
                        | -1 |  F, H  | -1 |
                        |____|__|__|__|____|
                                |  |
                        ________|  |__________
             _ __ _____|__ ____       ____ ___|____ ____ 
            |    |     V  |    |     |    |   V    |    |
            | -1 |  C, F  |  *-+-----+-*  |   H    | -1 |
            |____|__|__|__|____|     |____|___|____|____|
                    |  |                      |
            ________|  |____________          |______________
 ____ _____|__ ____       ____ _____|__ ____       ____ _____|__ ____ 
|    |     V  |    |     |    |     V  |    |     |    |     V  |    |
| -1 | A B C  |  *-+-----+-*  | D E F  |  *-+-----+-*  |  G, H  | -1 |
|____|________|____|     |____|________|____|     |____|________|____|
```