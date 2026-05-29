# Xbase: Index files (*.ndx)

The index file is a B+ tree (at least according to most description) - but it's more clearly decribed as a paged B-tree.
The essential structure is in an inverted tree, with an anchor note, multiple root nodes, and leaf nodes. The header is called the anchor node.

![img](images/chock.png)There is no indication in either the data nor index file of the related filenames.
**YOU** have to sort this out.

![img](images/fatal.png) Normally the header is read once only by the application, when the file is opened.But keep an eye on starting page no since this is updated durring reindex and can be modified durring index modifications.

The size of a page is always 512 bytes.

```
    _______________________  _______
 0 | Starting page no      |  ^
 1 |                     *1|  |
 2 |                       |  |
 3 |                       |  |
   |-----------------------|  |
 4 | Total no of pages   *2| File
 5 |                       | header
 6 |                       |  |
 7 |                       | (page 0)
   |-----------------------|  |
 8 | (Reserved)            |  |
 9 |                       |  |
10 |                       |  |
11 |                       |  |
   |-----------------------|  |
12 | Key length            |  |
13 |                       |  |
   |-----------------------|  |
14 | No of keys per page   |  |
15 |                       |  |
   |-----------------------|  |
16 | Key type: 0 = char    |  |
17 |           1 = Num     |  |
   |-----------------------|  |
18 | Size of key record  *3|  |
19 |                       |  |
20 |                       |  |
21 |                       |  |
   |-----------------------|  |
22 | (Reserved)            |  |
   |-----------------------|  |
23 | Unique flag         *4|  |
   |-----------------------|  |
24 | String defining the   |  |
   | key                   |  |
   :                       :  |
   :                       :  |
   :                       :  |
511|                       |  |
   |=======================| _v____            Link to lover level
  0| No of valid entries   |  ^           __  |=======================|
  1| on current page     *5|  |          /   0| Pointer to lower level|
  2|                       |  |         /    1| (next page)           |
  3|                       |  |        /     2|                       |
   |-----------------------|  |       /      3|                       |
  4|                       |  |      /        |-----------------------|
   | Array of key entries  | _|_____/        4| Record number in      |
   |                     *6|  |              5| data file             |
   |                       | Page            6|                       |
   |                       |  |              7|                       |
   |                       |  |               |-----------------------|
   |                       | _|_____         8| Key data            *7|
   :.......................:  |     \         :                       :
   :.......................:  |      \       N|                       |
   :.......................:  |       \_____  |=======================|
511|                       |  |      
   |=======================| _v_____           Link to DBF
  0| No of valid entries   |  ^           __  |=======================|
  1| on current page     *5|  |          /   0| No of keys on page    |
  2|                       |  |         /    1|                       |
  3|                       |  |        /     2|                       |
   |-----------------------|  |       /      3|                       |
  4|                       |  |      /        |-----------------------|
   | Array of key entries  |  |     /        4| Left page pointer     |
   |                     *6|  |              5|                       |
   |                       |__|___/          6|                       |
   |                       |  |              7|                       |
   |                       |__|___            |-----------------------|
   :.......................:  |   \          8| DBF record num      *8|
   |                       |  |    \          :                       :
   :.......................:  |     \       11|                       |
   |                       |  |      \        |-----------------------|
   :.......................:  |       \      8| Key data              |
   |                       |  |        \      :                       :
   :.......................:  |         \    N|                       |
511|                       |  |          \__  |=======================|
   |=======================| _v_____
```

1. Root page number's offset is: page number x 512 bytes

2. Total no of pages Max = 7FFFFFh = 8,388,607

3. Size of key record is a multiplum of 4. Record size is 4 (Pointer to next page) + 4 (record number i dbf) + key size ( as a multiplum of 4 ). i.e. if the key size is 10, the record size is 20 (4+4+12)

4. Unique flag
   
   | Value | Description |
   | ----- | ----------- |
   | 0     | Off         |
   | 1     | On          |

5. No of valid entries 0000h if empty

6. ??

7. Key data. Numbers are stored as little endians (binary values).

8. DBF record number refered directly to the physical record in the data file.
    0h = reference to another page

## Search algorithm for index files

1. The anchor node always resides in memory, while the file is open and determines which root node to access for at given key.
2. The root node is read and sequentially scanned until a key is found that is >= to the desired key.
3. A second-level node is accessed and scanned in a manner similar to the root node.
4. The process continues until the pointer to the next-lower-level node has a value of zero. If the key matches the leaf key at this level, the record number for the key is returned.
5. That's a discipline NOT to be discussed here :-) Way out of the scope fore this document and way too complex.

One of the advances of the B+ tree is that the higher levels of the tree does not have to be updated when a certain node is changed - unless the tree has become unbalanced.

![img](images/fatal.png) If the data or index files are corrupt you may get a "DISK FULL" error. The program can not read the EOF marker and therefore assumes that the disk is full. Check you file and make sure there is no garbage in it, also re-index your files.