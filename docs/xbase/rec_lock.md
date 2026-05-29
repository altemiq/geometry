## Xbase: The Record Lock Field on a dBASE IV Table

In a DOS multiuser environment, each user can place record locks on a shared table. For example, if user JSMITH is editing record number 12 of Stock, user MBROWN cannot access that record until it is unlocked. This prohibits one user from unintentionally overwriting another user's work.

The dBASE table type gives you the Record Lock option to show you information about a locked record. If you check Record Lock, Database Desktop adds a hidden character field named _DBASELOCK to the table. This field shows you when a record was locked and by whom.

![img](images/chock.png) Although Database Desktop adds the Record Lock field to the table, you will not see it when you view the table. You see a record's Record Lock field only if you are locked out of that record.

Use the Create dBASE Table dialogue box to create the Record Lock field for a dBASE table. Record Lock is not available for dBASE III+ tables.

The information you see when you find a locked field depends on the Info Size you specify. The Record Lock field can be from 8 to 24 characters. The default is 16.

- 0-1
  
  The first two bytes (binary integer) tell whether a user has changed the record. Every committed change is counted encreasing the count by one.

- 2-4
  
  The next three characters tell the time a user placed the lock. (10h 09h 07h i.e. 16:09:07)

- 5-7
  
  The next three characters tell the date a user placed the lock. ( 60h 09h 0Bh i.e. (19)96-09-11 )

- 8-24
  
  The remaining 16 characters are optional. They tell the name of the user that placed the lock.

The default size of 16 displays the changed status of the record, the time and date of the lock, and the first 8 characters of the user who placed the lock.

![img](images/danger.png) You cannot see from the locking info if the user releases the record again. You have to test the record locking. On DOS systems SHARE has to be loaded. Turbo C, DJGPP and compatible C compilers have functions like: lock()  _dos_lock()  locking() for setting locks on a region of a file and unlock()  _dos_unlock()  unlocking for removing the lock again. It's important that YOU remove the locks BEFORE closing the file.

------

### Read/write locks using share

by **Phil Barnett** ([midnight@the-oasis.net](mailto:midnight@the-oasis.net))

Share holds the locks, but it doesn't know anything about the structure or program that is requesting the lock. All share does is record the lock offset, type (record or file) and file name with path and where (station) it came from, and remove it when the correct station requests removal.

Lets make a scenario.

You have a 1000 byte header on your dbf file, and each record is 99 bytes + one for the delete flag. (this is to make the math simple)

If you locked record one at the actual offset it would lock a range from byte 1001 to 1100.

Any other station requesting READ or WRITE would be denied. Of course, we can read through locks in xBase, so how do they do it?

The xBase languages add an arbitrary number to the actual offset. Originally, Clipper used 1,000,000,000 + record offset. Fox used another offset, and I am not sure but I think it was 4,000,000,000.

We'll call this huge number the Lock Offset.

Later, Clipper changed the Lock Offset to the same as Fox for certain RDD's. (.CDX and .IDX)

For years, even though Clipper could create .NDX indexes, it was not Lock Offset compatible with dBase, and using both platforms on the same data caused incorrect locking.

The Lock Offset numbers are arbitrarily large to keep actual data files from crossing the limit. (hopefully, we aren't creating any 1 trillion byte files) (or 4 trillion for newer schemes)

So, in an xBase file, record locks and writes are at Lock Offset + File Offset and reads are at File Offset, so they do not run into a lock. This allows "read through"

Locks between xBase systems are only able to be used concurrently when the arbitrary Lock Offset is the same in both systems.

So, in the above example, to lock the first record the Lock Offset would be 4,000,001,001 to 4,000,001,100.

If both languages used the same offset, then the write lock would be correctly enforced.

------

These offsets are mentioned in online documentation (for CA-Clipper) (read 52eint.txt or clip53a.txt). The 1,000,000,000+RECNO() is used as default. If you want to use offset 4,000,000,000 you have to link the ntxlock2.obj file. For CDX indexes you should link cdxlock.obj to be compatible with Fox locking scheme (but only for Clipper != 5.2e)  
Confirmed by: r(ycho)GLAB, VIP Software (rglab@waw.pdi.net)

SoftC uses 0x4,000,000L for lock offset in FoxPro files

------

![img](images/read_manual.png)For more information on record locking read :
"Transactional Locking, Part 1" by Thomas Wang (wang@cup.hp.com) on
http://www.concentric.net/~Ttwang/tech/locks1.htm