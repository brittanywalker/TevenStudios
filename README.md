# TevenStudios
Repository for 761 agile project, working on Employee Budget Tracking app.

# Execution Instructions
To run the Teven Studios Budget Tracker, you will need to have Visual Studio, MySQL Server and MySQL Workbench installed. 
1. Download the source code.
2. Once MySQL Workbench is downloaded, you will need to create a connection called TevenStudios, which can be done from the main dashboard.
3. Next, click on the New Model option under the File tab.
4. Once the model opens, navigate to Import -> Reverse Engineer MySQL Create Script under the File tab. Navigate to where you have saved the source code and select the createDB.sql file. Select Execute and continue through the Wizard until the Reverse Engineer is complete.
5. Now select the TevenStudios schema and navigate to Forward Engineer in the Database tab. Select it, then make sure that you have selected the TevenStudios connection in the Stored Connection dropdown. Leave everything else as default and continue through the wizard until the Forward Engineering process is complete. It is likely you will be prompted to enter the password you have made for your connection. 
6. Close the wizard and once again navigate to the Database tab, but instead select Connect to a Database. Choose the TevenStudios connection and continue. A new connection tab will open in the Workbench called TevenStudios.
7. Finally, select the Run SQL script under the File tab and run the createDB.sql file that was used previously. Once this has been successful, the database is up and running. 
