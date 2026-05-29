## Xbase: What to check when opening a .DBF File

Records:

- Length of record must be > 1 and < max length. (max length = 4000 B in dBASE III and IV, can be 32KB in other systems).
- The number of records must be >= 0.

Fields:

- The .DBF file must have at least one field.
- The number of fields must be <= the maximum allowable number of fields.

File size:

- File size reported by the operating system must match the logical file size. Logical file size = ( Length of header + ( Number of records * Length of each record ) )