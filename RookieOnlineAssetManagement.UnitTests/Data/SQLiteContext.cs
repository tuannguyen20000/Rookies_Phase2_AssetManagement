using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using RookieOnlineAssetManagement.Data;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RookieOnlineAssetManagement.UnitTests.Data
{
    public abstract class SQLiteContext
    {
        public DbConnection _connection;
        public DbContextOptions<ApplicationDbContext> _contextOptions;
        public SQLiteContext()
        {
            _connection = new SqliteConnection("Filename =:memory:");
            _connection.Open();
            _contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseSqlite(_connection)
               .Options;
            var dbContext = new ApplicationDbContext(_contextOptions);
            if (dbContext.Database.EnsureCreated())
            {
                dbContext.Users.AddRange(
                    FakeData.UserFakeData.ListUser());
                dbContext.Users.Add(
                    FakeData.UserFakeData.ResUser());
                dbContext.Roles.AddRange(
                    FakeData.UserFakeData.ListRoles());
                dbContext.UserRoles.AddRange(
                    FakeData.UserFakeData.ListUserInRoles());

                dbContext.Categories.AddRange(
                    FakeData.CategoryFakeData.ListCategories());

                dbContext.Assets.AddRange(
                    FakeData.AssetFakeData.ListAssets());

                dbContext.Assignments.AddRange(
                    FakeData.AssignmentFakeData.ListAssignments());

                dbContext.SaveChangesAsync();
            }
        }
        public ApplicationDbContext CreateContext() => new ApplicationDbContext(_contextOptions);
    }
}
