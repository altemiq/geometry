## Xbase: Print Character Set (dbprint.ptb)

![img](images/chock.png)This file type is not very well documented.

There is no header, but just at sequence of records, terminated by 00h 00h. The record types are both record type indicators and record separators at the same time.

```
           _______________________         
00h /    0|                       |                    Used block             ____
          |                       |           ____   |=======================| ^
          |=======================|          /      0|Record type         00h| |
          |                       | ________/        |-----------------------| | Converted
          |                       | ________        1|dBASE character value  | | character
          |                       |         \        |-----------------------| |
          |=======================|          \____  2|Print character value  |_v__
          :                       |                  |=======================|
          :                       :                  :                       :____
          |                       |           ____   |=======================| ^
          |=======================|          /      0|Record type         00h| | Not printet
          |                       |_________/        |-----------------------| | character
          |                       |_________        1|dBASE character value  |_V__
          |=======================|         \        |=======================|
          |                       |          \____   :                       :____
          |                       |           ____   |=======================| ^
          |                       |          /      0|Record type         08h| |
          |=======================|_________/        |-----------------------| | Comment
          |                       |                 1|Comment                | |
          |                       |_________         :                       : :
          |                       |         \       N|                       |_V__
          |                       |          \_____  |=======================|
          |=======================|
          |End of File (00h 00h)  |        
       N  |=======================|
```

![img](images/fatal.png)The Comment record is identified by the header value 08h, followed by "a string" that can contain any character except 00h or 08h. There is no indication of length!
![img](images/chock.png)Note that 08 in record header can be used to comment out a mapping simply by replacing the 00h with 08h.
The conversion record is identified by 00h in the header, followed by the byte representing the character in the dBASE file.
**IF** this byte is followed by another byte,this second byte represents the character that the byte should be converted to when printing. If **NOT** followed by a second byte this character is not printed at all. Unmasked characters are printed "as is".