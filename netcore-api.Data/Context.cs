using Microsoft.EntityFrameworkCore;

namespace netcore_api.Data
{
  public class Context : DbContext
  {
    public Context(DbContextOptions options) : base(options) 
    { }

    public DbSet<Entities.User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      base.OnConfiguring(optionsBuilder);
    }

    public static int GetSqlCompatLevel(string? connectionString)
    {
      int minCompatLevel = 100;

      if (string.IsNullOrEmpty(connectionString))
        return minCompatLevel;

      try
      {
        var builder = new Microsoft.Data.SqlClient.SqlConnectionStringBuilder(connectionString);

        using (var conn = new Microsoft.Data.SqlClient.SqlConnection(builder.ConnectionString))
        {
          conn.Open();

          string query = "SELECT compatibility_level FROM sys.databases WHERE name = @Name";

          using (var command = conn.CreateCommand())
          {
            command.CommandTimeout = 1000;
            command.CommandText = query;
            command.Parameters.AddWithValue("@Name", builder.InitialCatalog);

            if (int.TryParse(command.ExecuteScalar().ToString(), out int level))
              return level;
          }
        }
      }
      catch (Exception ex) 
      {
        System.Diagnostics.Debug.WriteLine(ex.Message);
      }

      return minCompatLevel;
    }
  }
}
