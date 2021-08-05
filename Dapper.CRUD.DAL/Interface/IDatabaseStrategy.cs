using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace Dapper.CRUD.DAL.Interface
{
    public interface IDatabaseStrategy
    {
        void SeedDatabase();
        DbConnection Connection { get; }
    }
}
