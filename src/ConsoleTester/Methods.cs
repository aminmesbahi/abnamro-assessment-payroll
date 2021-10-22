using Assessment.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTester
{
    public static class Methods
    {
        public static decimal SumBenefits(PaymentHistory p)
        {
            //var b = p.Benefits.Sum(x => x.Value);
            //var d = p.Deductions.Sum(x => x.Value);
            decimal payment = -1;
            switch (p.Timesheet.Contract.PayMethod)
            {
                case PayMethod.Hourly:
                    Console.WriteLine("paymentHistory11");
                    Console.WriteLine($"Wage: {p.Timesheet.Contract.Wage}");
                    Console.WriteLine($"WorkingHours: {p.Timesheet.WorkingHours}");
                    Console.WriteLine("=======");
                    payment = p.Timesheet.Contract.Wage * p.Timesheet.WorkingHours;
                    break;
                case PayMethod.Monthly:
                    Console.WriteLine("paymentHistory22");
                    Console.WriteLine($"Wage: {p.Timesheet.Contract.Wage}");
                    Console.WriteLine($"WorkingHours: {p.Timesheet.WorkingHours}");
                    Console.WriteLine("=======");
                    payment = p.Timesheet.Contract.Wage / 12;
                    break;
                default:
                    payment = -1;
                    break;
            }
            return payment;
        }
    }
}
