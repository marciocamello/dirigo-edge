==============================================
       Welcome to Dirigo Edge!
==============================================

Please follow the installation instructions below before running the program.

I. Setting up your website

	1. Edit your web.config file with your appopriate database connections.
	
	2. Set Up Membership provider on your SQL Database :
		- http://runtingsproper.blogspot.com/2009/08/using-aspnetregsql-via-command-line-to.html
	
	3. Run the application and create a new user.
		- Running the application for the first time will seed the database and provide you with some initial content to play around with.
		- When you register a new user, you will be added as an Administrator.
		- After you create a user and have successfully logged in, you may modify the registration page to restrict sign ups. We recommend using an additional registration code for sign up.

	4. You may need to set permissions on the folders 'CSS' and 'Scripts' so that the 'IISUSR' has full permission
		- Edge will automatically combine and minify your CSS and Javascript files in this directory when you publish your solution or run in Release mode
		- More informatino on SquishIt can be found at https://github.com/jetheredge/SquishIt


II. Using Automatic Migrations
    
	1. Edge uses CodeFirst to handle database interaction.
	2. Automatic Migrations are enabled by default.
	3.Below you will find some information regarding updating your database if you decide to add new fields.
		- See : http://blogs.msdn.com/b/adonet/archive/2012/02/09/ef-4-3-automatic-migrations-walkthrough.aspx
		In Package Manage console, run : 
			'Enable-Migrations -EnableAutomaticMigrations'
		Then to update your model, run :
			'Update-Database -Verbose'