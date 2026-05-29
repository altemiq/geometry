# Xbase: dBASE specifications

|                                    | dBASE II | dBASE III       | dBASE IV      |
| ---------------------------------- | -------- | --------------- | ------------- |
| Max. no. of records                | 65,535   | 1,000,000,000   | 1,000,000,000 |
| Max. fields per record             | 32       | 128             | 255           |
| Max. data files open               |          | 10              | 10            |
| Max. index files per open database |          | 7               | 47            |
| Max. record size in bytes          | 1,000    | 4,000           | 4,000         |
| Max. records per file              |          | 65,535          |               |
| Max. bytes per file                |          | 8,000,000       |               |
| Max. memory variables              | -        | 256             |               |
| Max. memory for variables          | -        | 6,000 bytes \*1 |               |
| No. of data types                  |          | 5               | 6             |
| Max. size of MEMO file             |          | 5000 bytes      | 64000 bytes   |
| Max. character field size in bytes | 254      | 254             | 254           |
| Max. numerical field size in bytes |          | 19              | 20            |
| Size of logical field              | 1 byte   | 1 byte          | 1 byte        |
| Size of date field                 |          | 8 bytes         | 8 bytes       |
| Size of memo pointer in DBF        | -        | 10 bytes        |               |
| Size of floating point             |          | -               | 1-2           |
| Size of decimal values in numeric  | 10       | 15              |               |

1. Max. memory for variables. Can be expanded to 31 KB using CONFIG, 6 KB is mentioned as default.