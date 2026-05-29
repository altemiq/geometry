# Xbase File Format Description

```
#### #### ######
 ##   ##  ##   ##
  ## ##   ##   ##  ######    ######   #####
   ###    ######        ##  ##       ##   ##
  ## ##   ##   ##  #######  ######## #######
 ##   ##  ##   ## ##    ##        ## ##
#### #### ######   ######## #######   #####
```

Xbase ( & dBASE ) File Format Description  
by  
[Erik Bachmann](index.md#CONTACT)  
Clickety Click Software 2010-02-27

## [Index](index.md#INDEX)

- [What Is Xbase](intro.md#WHAT_IS)

- [Why This Document](why_this.md#WHY_THIS)

- [Notation](notation.md#WARNINGS)

- [What sort of data can be handled](data_types.md#DATA_TYPES)

- Structure diagrams:
  
  - Data files:
    
    - [Data files (*.dbf)](dbf.md#DBF_STRUCT)
      - [What to check when opening a .DBF File](dbf_check.md#CHECK_DBF)
  
  - Full text files (Memo):
    
    - [Memo Field Files (*.dbt)](dbt.md#DBT_STRUCT)
    - [FoxPro Object and Memo Field Files (*.fpt)](fpt.md#FPT_STRUCT)
  
  - Index files:
    
    - [Index files (*.ndx)](ndx.md#NDX_STRUCT)
      
      - [Search algorithm for index files](ndx.md#NDX_ALGORITH)
    
    - [Index file example (*.ndx)](ndx_example.md#NDX_STRUCT)
    
    - [Multiple Index files (*.mdx)](mdx.md#MDX_STRUCT)
    
    - [Compound Index files (*.cdx)](cdx.md#CDX_STRUCT)
    
    - [Uncompressed Index files (*.idx)](idx.md#IDX_STRUCT)
    
    - [Compressed Index files (*.idx)](idx_comp.md#IDX_COMP_STRUCT)
    
    - [Clipper Index Files (*.ntx)](ntx.md#NTX_STRUCT)
  
  - Miscellaneous files:
    
    - [Memmory Files (*.mem)](mem.md#MEM_STRUCT)
    - [Report Files (*.frm)](frm.md)
    - [Label Format Instruction Files (*.lbl)](lbl.md)
    - [Foxpro v1.0 Label Format Instruction Files (*.lbx)](lbx.md)
    - [Print Character Set (dbprint.ptb)](ptb.md)

- Features:
  
  - [The Record Lock Field on a dBASE IV Table](rec_lock.md#REC_LOCK)
  - [Read/write locks using share](rec_lock.md#REC_LOCK_SHARE)
  - [Year 2000 problems](y2k.md#YEAR_2000)

- dBASE II:
  
  - [dBASE II FILES](db2_dbf.md#DBII)
  - [The structure of dBASE II data files (*.dbf)](db2_dbf.md#DBII_DBF_STRUCT)
  - [The Structure of dBASE II index file](db2_ndx.md#DBII_NDX_STRUCT)

- Examples:
  
  - [Description of test.dbf](examples.md#DESC_DBF)
  - [Description of test.ndx](examples.md#DESC_NDX)
  - [Description of test.dbt](examples.md#DESC_DBT)
  - [Hex dump of test.dbf](examples.md#HEX_DBF)
  - [Hex dump of test.ndx](examples.md#HEX_NDX)
  - [Hex dump of test.dbt](examples.md#HEX_DBT)
  - [dBASE Specifications](dbase_spec.md#DBASE_SPEC)
  - [dBASE Versions](dbase_ver.md#DBASE_VERSIONS)

- Appendixes
  
  - [List of File extensions](file_ext.md#FILEEXT)
  - [FAQ (Frequently asked questions)](faq.md)
  - [HISTORY](history.md#HISTORY)
  - [REFERENCE LIST](references.md#REF_LIST)

------

Any comments, corrections, additions etc. are welcome. You can reach me at: [e_bachmann@hotmail.com](mailto:e_bachmann@hotmail.com?subject=Comment on Xbase&body=index.html)
or by snail mail:

Erik Bachmann  
Grydehøjvej 62  
DK-4000 Roskilde  
Denmark  
Europe

Third stone from the Sun - and turn left :-)

Note! All *mispelling an tybingerors* are for *freee* :-)