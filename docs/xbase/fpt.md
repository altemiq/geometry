## Xbase: FoxPro Object and Memo Field Files (*.fpt)

The file format is used by Fox Pro 2.x and later The size of the header is 512 bytes

```
           _______________________  _______
00h /   0 | Number of next        |  ^
00h /   1 | available block       |  |
00h /   2 | for appending data    | Header
00h /   3 | (binary)            *1|  |
          |-----------------------|  |
00h /   4 | ( Reserved )          |  |
00h /   5 |                       |  |
          |-----------------------|  |
00h /   6 | Size of blocks N    *1|  |
00h /   7 |                     *2|  |
          |-----------------------|  |
00h /   8 | ( Reserved )          |  |
          |                       |  |
          |                       |  |
          | (i.e. garbage)        |  |
          :                       :  |
          :                       :  |
00h /  511|                       |  |
          |=======================| _v_____
00h /    0|                       |  ^                 Used block
          |                       |  |           __  |=======================|
          |                       |  |          /   0| Record type         *3|
          :                       :  |         /    1|                     *1|
          :                       :  |        /     2|                       |
          |                       |  |       /      3|                       |
00h /    N|                       |  |      /        |-----------------------|
          |=======================| _|_____/        4| Length of memo field  |
00h /    0|                       |  |              5|                     *1|
          :                       :  |              6|                       |
          :                       :  |              7|                       |
          |                       |  |               |-----------------------|
00h /    N|                       | _|_____         8| Memo data             |
          |=======================|  |     \         :                       :
         0|                       |  |      \       N|                       |
          |                       |  |       \_____  |=======================|
          |                       |  |
          :                       :  |
00h /    N|                       | _v_____
          |=======================|
```

1. Big-endian. Binary value with **high** byte first.

2. Size of blocks in memo file (SET BLOCKSIZE). Default is 512 bytes.

3. Record type
   
   | Value | Description                                                                                                                      |
   | ----- | -------------------------------------------------------------------------------------------------------------------------------- |
   | 00h   | Picture. This normally indicates that file is produced on a MacIntosh, since pictures on the DOS/Windows platform are "objects". |
   | 01h   | Memo                                                                                                                             |
   | 02h   | Object                                                                                                                           |

4. A memo field can be longer than the 512 byte block. It simply continues through the next block. The field is logically terminated by two End-of-file marks in the field. The reminder of the block is unused.