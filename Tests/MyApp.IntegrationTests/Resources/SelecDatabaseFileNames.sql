SELECT [physical_name]
FROM [sys].[master_files]
WHERE [database_id] = DB_ID('MyAppTest');