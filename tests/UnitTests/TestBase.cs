using Assessment.ApplicationCore.Entities;
using Assessment.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace UnitTests
{

    public abstract class TestBase
    {
        protected PayrollContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<PayrollContext>()
                                  .UseInMemoryDatabase("Payroll")
                                  .Options;
            return new PayrollContext(options);
        }

        protected List<TaxBracket> GetTaxtBrackets()
        {
            return new List<TaxBracket>() { new TaxBracket(8,0M,20384M,.3665M),
                new TaxBracket(8,20385M,34300M,.3810M),
                new TaxBracket(8,34301M,68507M,.3810M),
                new TaxBracket(8,68508M,Int32.MaxValue,.5175M)};
        }
    }
}
