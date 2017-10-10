# TevenStudios
Repository for 761 agile project, working on Employee Budget Tracking app.

**Team: (Name | UPI | GitHub Username)** 
* Elizabeth Stevenson | este775 | @Libby713
* Christina Bell | cbel296 | @ChristinaBell
* Ida De Smet | ides542 | @idaknow
* Angad Nayyar | anay794 | @AngadNayyar
* Dan Kelly | dkel766 | @dankelly989
* Harry Jackson | hjac660 | @harryjacko
* Hannah Sampson | hsam938 | @hjadenz
* Brittany Walker | bwal566 | @brittanywalker

**Customer:** Elliot Whiley | PushPay

# Installation

## Dependencies
To run the Teven Studios Budget Tracker, you will need to have Visual Studio, ASP .NET Core, MySQL Server and MySQL Workbench installed. 

* [Visual Studio 2017 Community Download link](https://www.visualstudio.com/downloads/)
* [MySQL Community for Windows](https://dev.mysql.com/downloads/windows/installer/)
* [MySQL Workbench](https://dev.mysql.com/downloads/workbench/)
* [ASP .NET Core](https://www.microsoft.com/net/core#windowscmd) <br />
Once the download has been completed you will need to make sure that the server is running. This can be done by starting the service. If you are unsure, go to your desktop service manager and find MySQL server. You will be able to start and stop the server from here as well.

## Download the repository

Clone the source code from TevenStudios: `git clone https://github.com/brittanywalker/TevenStudios.git`

## Setup the database

1. Once MySQL Workbench is downloaded, you will need to create a connection called _TevenStudios_, which can be done from the main dashboard. Make sure that the username and password that you have created is the same one you use for MySQL Server.
2. Next, click on the tab **File** -> **New Model**.
3. Once the model opens, navigate to **File** -> **Import** -> **Reverse Engineer MySQL Create Script...**. 
4. Navigate to the source code repository and select the **createDB.sql** file. Select **Execute** and continue through the Wizard using the default parameters until the Reverse Engineer is complete.
5. Select the _TevenStudios_ schema and navigate to **Database** -> **Forward Engineer**. 
6. Select the _TevenStudios_ connection in the **Stored Connection** dropdown. Leave everything else as default and continue through the wizard until the Forward Engineering process is complete. It is likely you will be prompted to enter the password you have made for your connection. 
7. Close the wizard and once again navigate to the **Database** tab, but instead select **Connect to a Database**. Choose the _TevenStudios_ connection and continue. A new connection tab will open in the Workbench called _TevenStudios_.
6. Finally, select **File** -> **Run SQL script** and select the **createDB.sql** file that was used previously. Once this has been successful, the database is up and running with the appropriate imported data.

## Add an admin to the database

...

## Open the Solution

1. Open Visual Studio 2017 
2. In Visual Studio 2017 select **File** > **Open**, navigating to the directory of the local _TevenStudios_ repository. 
3. Open the solution file **TevenStudios/TevenStudiosBudgetTracker/TevenStudiosBudgetTracker.sln**

## Add secrets to the solution

1. Open the solution file in Visual Studio (using instructions above in Section _Open the Solution_)
2. Right-click the project in Visual Studio and select **Manage User Secrets**. This will open a **secrets.json** file.
3. Copy and paste the following into the secrets.json file: `{
   "Authentication:Google:ClientId": "636051315170-4svott714ete1jl8f8vtou7hrf484rdf.apps.googleusercontent.com",
   "Authentication:Google:ClientSecret": "yAuOm4jx2t1obL-jVodA6-89"
}`

## Execution

To run the solution file select **Debug** > **Start Without Debugging**.

This will open up the localhost in your default browser. Here you can view and go about the functionality developed by TevenStudios.


