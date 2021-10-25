using Assessment.ApplicationCore.Entities;
using System.Collections.Generic;
using System.Linq;

namespace UnitTests
{
    internal class Calculator
    {
        public decimal CalculateTax(decimal grossIncome, List<TaxBracket> reference)
        {

            var _taxBrackets = reference;

            var fullPayTax =
                _taxBrackets.Where(t => t.High < grossIncome)
                    .Select(t => t)
                    .ToArray()
                    .Sum(taxBracket => (taxBracket.High - taxBracket.Low) * taxBracket.Rate);

            var partialTax =
                _taxBrackets.Where(t => t.Low <= grossIncome && t.High >= grossIncome)
                    .Select(t => (grossIncome - t.Low) * t.Rate)
                    .Single();

            return fullPayTax + partialTax;

        }
    }
}
