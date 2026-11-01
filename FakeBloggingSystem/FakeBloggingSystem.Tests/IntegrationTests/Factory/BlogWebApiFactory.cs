using FakeBloggingSystem.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace FakeBloggingSystem.Tests.IntegrationTests.Factory
    {
    internal class BlogWebApiFactory : WebApplicationFactory<Program>
        {
        private readonly SqliteConnection connection = new("Data Source=:memory:");

        public BlogWebApiFactory()
            {
            connection.Open();
            }

        
        protected override void ConfigureWebHost(IWebHostBuilder builder)
            {
            builder.UseEnvironment("Testing");
            builder.ConfigureServices(services =>
                {
                    services.RemoveAll<DbContextOptions<BlogDbContext>>();
                    services.AddDbContext<BlogDbContext>(options =>
                        {
                            options.UseSqlite(connection);
                        });

                    using var provider = services.BuildServiceProvider();
                    using var scope = provider.CreateScope();
                    var dbContext = scope.ServiceProvider.GetRequiredService<BlogDbContext>();
                    dbContext.Database.EnsureCreated();
                });
            }

        protected override void Dispose(bool disposing)
            {
            base.Dispose(disposing);

            if (disposing)
                {
                connection.Close();
                connection.Dispose();
                }
            }
        }
    }