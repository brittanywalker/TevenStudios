# TevenStudios
Repository for 761 agile project, working on Employee Budget Tracking app.

### Project Goal

The project goal is to create an internal training budget management application to allow employees to submit training budget requests and manage their training budget. Managers can then accept employee requests, with an internal admin able to add and modify employees and their budgets. The outcome of this project will simplify the management of training budgets, replacing the current approach using an excel spreadsheet.

### Features

The capabilities of this system depend on whether the user is an Administrator, Manager or Employee. These are discussed below respectively.

1. Administrators have the ability to add, delete and edit users of the system, as well as assigning their role (admin, manager or employee), budget (annual and start) and manager.
2. Managers are able to view the employees that they manage, along with relevant information about each employee’s budget and previous transactions. They will be able to see requests that they have received from their employees for spending, and choose whether to approve or decline these requests. Managers are also employee, so they have access to their own information as an Employee. This is discussed below in (3).
3. Employees are able to visualise their budget and what they have spent it on. They can then make requests to their managers to use a portion of their budget and describe what they are using it for. This request's status will change once the Manager accepts or declines this request. The employees budget shown includes pending requests as deducted, to ensure the employee doesn't spend more then their maximum allocated annual budget.

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

## Users in the database

There exist admins with the following emails in the database:
* r.hoda@auckland.ac.nz
* k.blincoe@auckland.ac.nz
* ysha962@aucklanduni.ac.nz
* m.mehrabi@auckland.ac.nz

The user's email must be in the database in order for them to be able to log in.

If your email is different to this you can add yourself as an admin by following the steps below.

1. Open the _TevenStudios_ connection in MySQL Workbench.
2. In the **Schema** section, click **tevenstudios** -> **Tables** -> **user**. If you hover over this user table, click the table icon on the far right. This will open up the table in both SQL script format and show the **Result Grid**.
3. Add the user into the table by adding values in the appropriate columns in the Result Grid. All values must be entered. Make sure the email is one you can log in with. To make the user an admin, add an ID of _0_ in the column _RoleId_.
4. Click **Apply**.
5. Follow the wizard using the default parameters. This should successfully add the user into the database.

You can now login to the application using the email that was provided.

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

You can log in using your Google Account email. This email must be in the database. To check this follow the instructions in the section above regarding _Users in the database_.

If your role in the database is an **Administrator**, following login you will be redirected to the Admin page. Here you can add, edit and delete users of the system. You can assign them roles, budgets, and managers. You can edit their emails.

If you login with the email of an **Employee**, you will be redirected to a page where you can view your budget, see your past requests and pending requests. You can also lodge a request here to your manager for the expenditure.

If you login with the email of a **Manager**, you will also have access to the **Employee** page described above. On top of this, there is a tab that redirects you to Manager page. Here you can view your employees along with their respective budget and requests. The manager can approve and decline these requests here.
