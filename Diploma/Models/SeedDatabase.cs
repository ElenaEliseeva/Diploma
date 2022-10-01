using System.Text;
using Diploma.DataAccess;

namespace Diploma.Models;

public class SeedDatabase
{
    private readonly DiplomaContext dbContext;
    public SeedDatabase(DiplomaContext dbContext)
    {
        this.dbContext = dbContext;
    }
    public async Task Initialize()
    {
        if (await dbContext.Database.EnsureCreatedAsync())
        {
            var transaction = await dbContext.Database.BeginTransactionAsync();
            //if (!context.Genre.Any())
            //{
            //    context.Genre.AddRange(
            //        new Genre
            //        {
            //            GenreName = "Зарубежная литература"
            //        },
            //        new Genre
            //        {
            //            GenreName = "История"
            //        },
            //        new Genre
            //        {
            //            GenreName = "Классика"
            //        });
            //}
            await dbContext.SaveChangesAsync();
            await transaction.CommitAsync();
        }
    }
}