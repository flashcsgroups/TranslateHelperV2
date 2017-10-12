﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TranslateHelper.Models
{
    public class THdbcontext : DbContext
    {
        public DbSet<IdiomCategory> IdiomCategory { get; set; }
        public DbSet<Idiom> Idiom { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(@"Data Source=dbhelper.mysql.database.azure.com; Database=translatehelper; User Id=sa@dbhelper; Password=Th10102017004;");
        }
    }
}
