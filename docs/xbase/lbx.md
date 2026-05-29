## Xbase: Foxpro v1.0 Label Format Instruction Files (*.lbx)

This file type is used by FoxPro v. 1.0. only. V2.0 and later versions stores label data in a DBF-file.

```
           _______________________  _______
 00h /  0 |Version no.   03h      |  ^
          |-----------------------|  |
 01h /  1 | Comment in ASCII      |  |
          :                       : Header
          |                       |  |
 00h / 60 |                       |  |
          |-----------------------|  |
 00h / 61 |No of lines on label   |  |
 00h / 62 |                       |  |
          |-----------------------|  |
 00h / 63 |Width of left margin   |  |
 00h / 64 |                       |  |
          |-----------------------|  |
 00h / 65 |Label width            |  |
 00h / 66 |                       |  |
          |-----------------------|  |
 00h / 67 |No of labes per row    |  |
 00h / 68 |                       |  |
          |-----------------------|  |
 00h / 69 |Space between labels   |  |
 00h / 70 |                       |  |
          |-----------------------|  |
 00h / 71 |Blank lines between    |  |
 00h / 72 |rows                   |  |
          |-----------------------|  |
 00h / 73 |Length of label text in|  |
 00h / 74 |characters             |  |
          |-----------------------|  |
 00h / 75 |Label text             |  |
          :                       :  |
          :0Dh = new line         :  |
          :                       :  |
 ??h /  N |                       |__V_____
          |=======================|
```