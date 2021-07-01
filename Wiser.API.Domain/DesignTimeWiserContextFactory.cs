using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Wiser.API.Entities
{
    class DesignTimeWiserContextFactory : IDesignTimeDbContextFactory<WiserContext>
    {
        public WiserContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder().
                SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile(@Directory.GetCurrentDirectory() + "/../Wiser_WEB_API/appsettings.json").Build();
            var builder = new DbContextOptionsBuilder<WiserContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            builder.UseSqlServer(connectionString);
            return new WiserContext(builder.Options);
        }
    }
}
