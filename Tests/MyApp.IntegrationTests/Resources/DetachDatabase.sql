ALTER DATABASE [MyAppTest] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
EXEC sp_detach_db
     'MyAppTest',
     'true';