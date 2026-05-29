## Xbase: dBASE II index file (*.idx)

This was previously presented as the structure of dBASE III index files. Checking with the actual header of an index made it obvious, that it did NOT fit with dBASE III index files ( In that case several of the most essential key fields should be NULL values ).

```
    _______________________  _______
 0 | (Reserved)            |  ^
 1 |                       |  |
   |-----------------------|  |
 2 | Node number of        |  |
 3 | root node             |  |
   |-----------------------|  |
 4 | Node number of        | Anchor
 5 | next available node   |  node
   |-----------------------| description
 6 | Length of key       *1|  |
   |-----------------------|  |
 7 | Size of key entry   *2|  |
   |-----------------------|  |
 8 | Maximum keys/node   *3|  |
   |-----------------------|  |
 9 | Field type            |  |
   |-----------------------|  |
 10| Key expression in     |  |
   | ASCII                 |  |
   :                       :  |
   :                       :  |
   |                       |  |
109| (00h terminated)      |  |
   |-----------------------|  |
110| (Unused)              |  |
   |                       |  |            _    Key node                      __ |=======================|
   :                       :  |           /   |=======================|      /  0| Pointer to next key   |
   :                       :  |          /   0| No of keys          *5|     /   1|                       |
   |                       |  |         /     |-----------------------|    /     |-----------------------|
511| ( 16 bits )           |  |        /      | Array of Keys to data |   /     2| Record no in DBF      |
   |=======================| _v_____  /       |- - - - - - - - - - - -|__/      3|                       |
  0| No.of keys in node    |  ^      /        |                       |__        |-----------------------|
   |-----------------------| _|_____/         |- - - - - - - - - - - -|  \      4| Key expression        |
  1| Array of key entries  |  |               |                       |   \      |                       |
   |                       | Index            |- - - - - - - - - - - -|    \     :                       :
   |                       | record           :                       :     \   N|                       |
   |-----------------------| _|_____          |                       |      \__ |=======================|
   :                       :  |     \      511|                       |     
   :                       :  |      \______  |=======================|
   :                       :  |
   :                       :  |            _    Data node
   :                       :  |           /   |=======================|
   :                       :  |          /   0| Pointer to lower level|
   |                       |  |         /    1| (next page)           |
511| ( 16 bits )           |  |        /      |-----------------------|
   |=======================| _v_____  /      2| Record number in DBF  |
  0| No.of keys in node    |  ^      /       3|                       |
   |-----------------------| _|_____/         |-----------------------|
  1| Array of key entries  |  |              4| Key expression      *4|
   |                       | Index            |                       |
   |                       | record           :                       :
   |-----------------------| _|_____          :                       :
   :                       :  |     \        N|                       |
511|                       |  |      \______  |=======================|
   |=======================| _v_____
```

1. Length of key = Key length in bytes + 2

2. Size of key entry = Bytes in key expression + 2

3. Field type:
   
   |        |                 |
   | ------ | --------------- |
   | 00h    | Character field |
   | (else) | Numeric field   |

4. No of keys: Max 2 x size of key entry

5. Key expression: There can be several keys in each node.

### Hieracy

Root key -> keynode : Key -> Datanode : Record no -> DBF