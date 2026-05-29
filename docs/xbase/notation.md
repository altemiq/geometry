# Xbase: A few words of concern

### Definitions

Binary
: Means data is stored in blocks of paired bytes each 8 bits in size. The bytes are swaped in pairs the Intel way with the low byte value first and the high byte value last:

Garbage/Reserved
: ![img](images/chock.png) Some areas in the files are labeled garbage or reserved or not used. These areas **might** contain bits and pieces from old files. Check out these areas (and overwrite them with 00h).

### Notation

![img](images/chock.png) Beware!

![img](images/danger.png) You **must** follow this instruction.

![img](images/destructive.png) This will **destroy** your database.

![img](images/dont.png) **DON'T**!!!

![img](images/fatal.png) This could have a **fatal** influence on you database.

![img](images/read_manual.png) RYFM!!! (Read Ye' Folded Manuscript ;─). This is an excellent opportunity to consult your manual.

### Structure diagrams

How to read and interperate the diagrams.

```
 ┌─── Position (hexadecimal)
 │      ┌─── Position (decimal)
 │      │
 │      │  ┌─── Field entry
 │      │  │                   ┌─── Note. See details below diagram
vv      v  v                   │
         ┌─────────────────────v─┬─────── 
00h /   0│ Version number      *1│  ^ <─── Section start
         ├───────────────────────┤<──── Logical field separator
01h /   1│ Date of last update   │  │   only for enhancing the visibility
02h /   2│      YYMMDD           │  │
03h /   3│                       │  │
         ├───────────────────────┤ Header <─── Section title
04h /   4│                       │  │
05h /   5│                       │  │
         :                       :<──── Skipping space (i.e. rows NOT displayed)
         :                       :  │
0Ah /  10│                       │  │      ┼──── Record from array expanded
         │                       │  │      │
         │                       │  │      │              ┼─── Expanded record
0Dh /  13│                       │  │      │              │   
         ├───────────────────────┤  │      │              v
0Eh /  14│                       │  │      │   ─        ╒═══════════════════════╕ ───────
         │                       │  │      │  / 00h /  0│ Field deleted flag  *9│  ^
         │                       │  │      v /          ├───────────────────────┤  │
         │                       │  │       /           │ Data               *10│  One
         │                       │  │      /            :                    *17:  Record
         │                       ├──├─────/             │                       │  │
         :                       :  │                   │                       │ ─v─────
         :                       :──├─────              ╞═══════════════════════╡
         :                       :  │     \             │ Field deleted flag  *9│
         │                       │  │      \            ├───────────────────────┤
         │                       │  │       \           │                       │
         │                       │  │        \          │                       │
         │                       │  │         \─        └───────────────────────┘
1Fh /  31│                       │  │
         ╞═══════════════════════╡  │
20h /  32├──End─of─File──────────┤ ─v────  <─── Section end
```

1. Note that there are hyperlinks between indicator and text note. `<─── Note text with link to indicator`