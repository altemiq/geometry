## Xbase: Label Format Instruction Files (*.lbl)

```
          _______________________  _______
00h /  0 |Signature         (02h)|  ^
         |-----------------------|  |
00h / 01 |Remarks                |  |
         :Padded with blanks     :  |
00h / 60 |                       |  |
         |-----------------------|  |
00h / 61 |Height/no of lines     |  |
00h / 62 |                       |  |
         |-----------------------|  |
00h / 63 |Width                  |  |
00h / 64 |                       |  |
         |-----------------------|  |
00h / 65 |Left margin            |  |
00h / 66 |                       |  |
         |-----------------------|  |
00h / 67 |Label line           *1|  |
00h / 68 |                       |  |
         |-----------------------|  |
00h / 69 |Label space          *2|  |
00h / 70 |                       |  |
         |-----------------------|  |
00h / 71 |Labels across          |  |
00h / 72 |(horizontal)           |  |
         |-----------------------|  |
00h / 73 |Label text             |  |
         |                       |  |
         :                       :  |
408h/1032|                       |  |
         |=======================|  |
409h/1033|End of File (02h)      |  |
??h / N  |=======================|__V_____
```

1. Line space
   Space in lines between labels horizontal

2. Label space. No of characters between labels vertical