This is my first attempt.
The code of course could be better!

Comments on task
=====================

5) what if the server/service restarted?

The app is not perstatnt. You turn it off it starts again. In the real world you would store the data else wher i.e. in a database or document store
 
6) Keep in mind that another developer may want to extend the program later to also work with files located on FTP server, dropbox, etc. Also more formats for input/output files can be added in the future.  

To do this I would look at adding interfaces and making the code more generic.

Todo
===========
Unit Tests
Better error handling
Logging
Make scan configurable
Make type of files configurable
