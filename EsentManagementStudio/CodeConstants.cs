namespace EsentManagementStudio
{
    public static class CodeConstants
    {
        public const string NewQueryScript =
@"new Func<object>(() =>
	{
        throw new NotImplementedException();
    }).Invoke()";

        #region Table
        public const string NewTableScript =
@"new Func<string>(() =>
	{
        using (db.Session.BeginTransaction())
        {
            // Create table
            var table = db.CreateTable(
                ""{table.Name}"",
                0);  // pages
            // Add column   
            table.AddColumn<{datatype}>(
                ""{column.Name}"",
                option:
                    EsentColumn.Option.Key | 
                    EsentColumn.Option.Autoincrement);
        }
		return ""Success!"";
    }).Invoke()";

        public const string ShowTableScript =
@"db.Tables.Values
	.Select(i => new EsentRow
	{
		new EsentCell(""Table Name"", i.Name)
    })";

        public const string DropTableScript =
@"new Func<string>(() =>
	{
        using (db.Session.BeginTransaction())
        {
            db.Tables[""{table.Name}""].Close();
            db.DropTable(""{table.Name}"");
        }
		return ""Success!"";
    }).Invoke()";
        #endregion

        #region Data
        public const string SelectTopScript =
@"db.Tables[""{table.Name}""].SelectFirstRows({count})";

        public const string SelectExactScript =
@"new Func<object>(() =>
	{
        db.Tables[""{table.Name}""].SetCurrentIndex(""{index.Name}"");
        return db.Tables[""{table.Name}""]
            .SelectRow({key});
    }).Invoke()";

        public const string SelectConditionScript =
@"db.Tables[""{table.Name}""]
    .Where(r => r[""{column.Name}""].AsString.StartsWith(""{value}""))";

        public const string CountRowsScript =
@"db.Tables[""{table.Name}""].Count";

        public const string InsertDataScript =
@"new Func<string>(() =>
	{
        using (db.Session.BeginTransaction())
        {
            db.Tables[""{table.Name}""]
                .Insert(
                    new EsentCell(""{column.Name}"", ""{value}"")
                );
        }
		return ""Success!"";
    }).Invoke()";

        public const string UpdateDataScript =
@"new Func<object>(() =>
	{
        using (db.Session.BeginTransaction())
        {
            db.Tables[""{table.Name}""].SetCurrentIndex(""{index.Name}"");
            db.Tables[""{table.Name}""].Update(
                {key},
                {column.name},
                {value}
                );
        }
        return db.Tables[""{table.Name}""]
            .SelectRow({key});
    }).Invoke()";

        public const string DeleteDataScript =
@"new Func<string>(() =>
	{
        using (db.Session.BeginTransaction())
        {
            db.Tables[""{table.Name}""].SetCurrentIndex(""{index.Name}"");
            db.Tables[""{table.Name}""].Delete({key});
        }
		return ""Success!"";
    }).Invoke()";
        #endregion

        #region Column
        public const string NewColumnScript =
@"new Func<string>(() =>
	{
        db.Tables[""{table.Name}""].AddColumn<{datatype}>(
                ""{column.Name}"",
                option:
                    EsentColumn.Option.Key | 
                    EsentColumn.Option.Autoincrement);
		return ""Success!"";
    }).Invoke()";

        public const string ShowColumnScript =
@"db.Tables[""{table.Name}""].Columns.Values
	.Select(i => new EsentRow
	{
		new EsentCell(""Column Name"", i.Name),
		new EsentCell(""ColumnType"", i.ColumnType.Name),
		new EsentCell(""Max"", i.Max),
		new EsentCell(""Encoding"", i.Encoding),
		new EsentCell(""DefaultValue"", i.DefaultValue),
		new EsentCell(""Options"", i.Options)
    })";

        public const string DropColumnScript =
@"new Func<string>(() =>
	{
        db.Tables[""{table.Name}""].DropColumn(""{column.Name}"");
		return ""Success!"";
    }).Invoke()";
        #endregion

        #region Index
        public const string NewIndexScript =
@"new Func<string>(() =>
	{
        db.Tables[""{table.Name}""].CreateIndex(
            ""{index.Name}"",
            ""{column.Name}"",
            true,   // ascending
            100,    // density
            true,   // primary
            true);  // unique
		return ""Success!"";
    }).Invoke()";

        public const string ShowIndexScript =
@"db.Tables[""{table.Name}""].Indexes.Values
	.Select(i => new EsentRow
	{
		new EsentCell(""Index Name"", i.Name),
		new EsentCell(""Primary"", i.Primary),
		new EsentCell(""Unique"", i.Unique)
    })";

        public const string DropIndexScript =
@"new Func<string>(() =>
	{
        db.Tables[""{table.Name}""].DropIndex(""{index.Name}"");
		return ""Success!"";
    }).Invoke()";
        #endregion

    }
}
