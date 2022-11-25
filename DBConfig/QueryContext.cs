using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotChocolateExplorer.DBConfig
{
    public class QueryContext : SchoolContext
    {
        public QueryContext(DbContextOptions<SchoolContext> options) : base(options)
        {
        }
    }
}
