## Xbase: dBASE II data files (*.dbf)

```
    _______________________  _______
 0 | Version number      *1|  ^
   |-----------------------|  |
 1 | Number of records     | Record
 2 | in data file (16 bit) | header
   |-----------------------|  |            _  |=======================| _______
 3 | Date of last update   |  |           /  0| Field name in ASCII   |  ^
 4 |      YYMMDD           |  |          /    : (terminated by 00h)   :  |
 5 |                       |  |         |     |                       |  |
   |-----------------------|  |         |   10|                       |  |
 6 | Length of each record |  |         |     |-----------------------| For
 7 | ( 16 bits )         *2|  |         |   11| Field type in ASCII *4| each
   |-----------------------|--|---      |     |-----------------------| field
 8 |                       |  |  ^      |   12| Field length  (binary)|  |
 9 | Field descriptor      |  |  | ____/      |-----------------------|  |
   : Array             *5  :  |  |          13| Field adress in memory|  |
   : (Terminated by 0Dh)   :  |  |          14|                       |  |
   :                       :  |  | ______     |-----------------------|  |
   :                       :  |  |       \  15| Field decimal count   |  |
   :                       :  |  |        \_  |=======================| _v_____
   |                       |  |  |
519|                       |  |  |
   |-----------------------|  |  |
520| Terminator          *3|__v__v_
   |=======================|    |
521| Records               |    |
   :                       :    |
   :                       :    |
   :                       :    |
   :                       :    |
  n|                       |    |
   |__End_of_File__________| ___v____  End of file ( 1Ah )  *11
```

1. dBASE II version no: 02h

2. Sum of lengths of all fields + 1 (deletion flag). Max. length is 1,000 bytes

3. Terminator: `0Dh` if all 32 fields present, otherwise `00h`.

4. Field type: C, N or L

5. Field descriptor array. Max. 32 fields

6. Field length: `0` - `ffh`. Logical fields = 1, decimal no ( = no of digits)

7. 

8. Field name **can** be undefined (= 1 x 00h)

9. Field address in memory can be ignored.

10. Deleted flag. Blanks (20h) are valid! "*" = deleted.
    
    Note that records remain in the datafile when deleted (i.e. are marked with "*") The deleted record are moved **out of the valid range of records** by the pack command. But is still present in the data file!!! This means that the file size is unchanged!
    
    ```
     Deleted          Packed
     ===========      ===========
    |Header     |    |Header     |
    |-----------|    |-----------|
    |Valid 1    |    |Valid 1    |
    |-----------|    |-----------|
    |Deleted 1  |    |Valid 2    |
    |-----------|    |-----------|
    |Valid 2    |    |Valid 3    |
    |-----------|    |===========|
    |Deleted 2  |    |EOF        |
    |-----------|    |===========|
    |Deleted 3  |    |Deleted 1  |
    |-----------|    |-----------|
    |Valid 3    |    |Deleted 2  |
    |===========|    |-----------|
    |EOF        |    |Deleted 3  |
     ===========     ===========
    ```

![img](images/read_manual.png)The data file can be truncated by using the COPY command.