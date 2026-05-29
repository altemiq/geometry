## Xbase: Report Files (*.frm)

Each report file can hold up til 55 expresions witha total length less than 1440 bytes. Not created - yet

```
          _______________________  _______                           _______________________  _______
00h /  0 |Version       (binary) |  ^               -----  00h /  0 |Report width           |  ^
01h /  1 |                       |  |              /       01h /  1 |                       |  |
         |-----------------------| Header         /                 |-----------------------|  |
02h /  2 |End of Expression      |  |            /         02h /  2 |Pad1  (not used)       |  |
03h /  3 |              (binary) |  |           /          03h /  3 |                       |  |
         |-----------------------|  |           |                   |-----------------------|  |
04h /  4 |Length in bytes of each|<-+-+         |          04h /  4 |Pad2  (not used)       |  |
05h /  5 |Expression             |  | |         |                   |-----------------------|  |
         |- - - - - - - - - - - -|  | |         |          05h /  5 |Total                  |  |
         :                       :  | |         |                   |-----------------------|  |
         |- - - - - - - - - - - -|  | |         |          06h /  6 |Decimal                |  |
70h /112 |                       |  | |         |          07h /  7 |                       |  |
71h /113 |   55 entries x 2 byte |<-+-+         |                   |-----------------------|  |
         |-----------------------|  |           |          08h /  8 |Expression content     |  |
72h /114 |Expression index       |<-+-+         |          09h /  9 |                       |  |
         |Pointer to each exp.   |  | |         |                   |-----------------------|  |
         |- - - - - - - - - - - -|  | |         |          0Ah / 10 |Title expression no.   |  |
         :                       :  | |         |          0Bh / 11 |                       |  |
         |- - - - - - - - - - - -|  | |         |                   |-----------------------|  |
         |                       |  | |         |          0Ch / 12 |GROUP-ON number        |  |
DFh /223 |   55 entries x 2 byte |<-+-+         |          0Dh / 13 |                       |  |
         |-----------------------|  |           |                   |-----------------------|  |
E0h /224 |Expression strings     |  |           |          0Eh / 14 |SUB-GROUP-ON-NO no.    |  |
         |                       |  |           |          0Fh / 15 |                       |  |
         |                       |  |           |                   |-----------------------|  |
         :                       :  |           |          10h / 16 |GROUP-ON header text   |  |
         :                       :  |           |          11h / 17 |number                 |  |
         :                       :  |          /                    |-----------------------|  |
         :                       :  |         /            12h / 18 |SUB-ON header text no. |  |
         :                       :  |        /             13h / 19 |                       |  |
67Fh/1663|   1440 bytes          |  |       /                       |-----------------------|  |
         |-----------------------|  |      /               14h / 20 |Page width             |  |
680h/1664|25 data structures   *1|  |     /                15h / 21 |                       |  |
         |- - - - - - - - - - - -|--+-----                          |-----------------------|  |
         :                       :  |                      16h / 22 |Lines per page         |  |
         :                       :--+---                   17h / 23 |                       |  |
         |- - - - - - - - - - - -|  |   \                           |-----------------------|  |
         |                       |  |    \                 18h / 24 |Left margin            |  |
 ?h / N  |=======================|__v__   \------------    19h / 25 |                       |  |
                                                                    |-----------------------|  |
                                                           1Ah / 26 |Right margin           |  |
                                                           1Bh / 27 |                       |  |
                                                                    |-----------------------|  |
                                                           1Ch / 28 |No. of columns         |  |
                                                           1Dh / 29 |                       |  |
                                                                    |-----------------------|  |
                                                           1Eh / 30 |Double space flag (Y/N)|  |
                                                                    |-----------------------|  |
                                                           1Fh / 31 |Summary below flag(Y/N)|  |
                                                                    |-----------------------|  |
                                                           20h / 32 |Form feed      *2 (Y/N)|  |
                                                                    |-----------------------|  |
                                                           21h / 33 |Plus byte bitmap     *3|  |
                                                                    |-----------------------|  |
                                                           22h / 34 |End of Record          |  |
                                                           23h / 35 |              2 x (02h)|  |
                                                                    |=======================|__v__
```

1. Note that the first entry in data structure is always empty!

2. Form feed after GROUP

3. Plus byte: dBASE III+ and later only
   
   | Three bit flags |                                |
   | --------------- | ------------------------------ |
   | 0               | Form feed PRIOR TO report      |
   | 1               | Form feed AFTER report         |
   | 2               | Plain report without form feed |