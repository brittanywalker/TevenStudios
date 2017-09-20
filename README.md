# TevenStudios
Repository for 761 agile project, working on Employee Budget Tracking app.

# Execution Instructions
To run the Teven Studios Budget Tracker, you will need to have Visual Studio, ASP .NET Core, MySQL Server and MySQL Workbench installed. 

* [Visual Studio 2017 Community Download link](https://www.visualstudio.com/downloads/)
* [MySQL Community for Windows](https://dev.mysql.com/downloads/windows/installer/)
* [MySQL Workbench](https://dev.mysql.com/downloads/workbench/)
* [ASP .NET Core](https://www.microsoft.com/net/core#windowscmd) <br />

Once the download has been completed you will need to make sure that the server is running. This can be done by starting the service. If you are unsure, go to your desktop service manager and find MySQL server. You will be able to start and stop the server from here as well.


## Steps

These steps take you through the process of setting up the database and running the source code as a local website.

1. Clone the source code from TevenStudios: `git clone https://github.com/brittanywalker/TevenStudios.git`
2. Navigate to the branch "prototype" using `git checkout prototype`
2. Once MySQL Workbench is downloaded, you will need to create a connection called TevenStudios, which can be done from the main dashboard. Make sure that the username and password that you have created is the same one you used for MySQL Server.
3. Next, click on the New Model option under the File tab.
4. Once the model opens, navigate to Import -> Reverse Engineer MySQL Create Script under the File tab. Navigate to where you have saved the source code and select the createDB.sql file. Select Execute and continue through the Wizard until the Reverse Engineer is complete.
5. Now select the TevenStudios schema and navigate to Forward Engineer in the Database tab. Select it, then make sure that you have selected the TevenStudios connection in the Stored Connection dropdown. Leave everything else as default and continue through the wizard until the Forward Engineering process is complete. It is likely you will be prompted to enter the password you have made for your connection. 
6. Close the wizard and once again navigate to the Database tab, but instead select Connect to a Database. Choose the TevenStudios connection and continue. A new connection tab will open in the Workbench called TevenStudios.
7. Finally, select the Run SQL script under the File tab and run the createDB.sql file that was used previously. Once this has been successful, the database is up and running with the appropriate imported data.
8. Open Visual Studio 2017 and 
9. In Visual Studio 2017 select File > Open, navigating to the directory of the local TevenStudios repository. Open the solution file TevenStudios/TevenStudiosBudgetTracker/TevenStudiosBudgetTracker.sln
10. To run the solution file select Debug > Start Debugging.

This will open up the localhost in your default browser. Here you can view and go about the functionality developed by TevenStudios.


