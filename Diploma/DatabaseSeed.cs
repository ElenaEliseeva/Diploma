using System.Data;
using Diploma.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Diploma;

public static class DatabaseSeed {
    public static void SetupTestData(DiplomDbContext _dbContext) {

        if (_dbContext.Database.EnsureCreated())
        {
            var sql = File.ReadAllText("db_seed.sql");
            var conn = _dbContext.Database.GetDbConnection();
            var initialConnectionState = conn.State;
            try
            {
                if (initialConnectionState != ConnectionState.Open)
                {
                    conn.Open();
                }

                _dbContext.Database.ExecuteSqlRaw(sql);
            }
            finally
            {
                if (initialConnectionState != ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }
    }
}