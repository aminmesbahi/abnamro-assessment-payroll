using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Assessment.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Assessment.Infrastructure.Data
{
    public class PayrollContextSeed
    {
        public static async Task SeedAsync(PayrollContext payrollContext,
    ILoggerFactory loggerFactory, int? retry = 0)
        {
            int retryForAvailability = retry.Value;
            try
            {
                if (!await payrollContext.TaxReferences.AnyAsync())
                {
                    await payrollContext.TaxReferences.AddRangeAsync(
                        GetSampleTaxReferenceData());

                    await payrollContext.SaveChangesAsync();
                }

                if (!await payrollContext.TaxBrackets.AnyAsync())
                {
                    await payrollContext.TaxBrackets.AddRangeAsync(
                        GetSampleTaxReferenceBrackets());

                    await payrollContext.SaveChangesAsync();
                }
                

                // TODO: Only run this if using a real database
                // payrollContext.Database.Migrate();
                if (!await payrollContext.Employees.AnyAsync())
                {
                    await payrollContext.Employees.AddRangeAsync(
                        GetSampleEmployees());

                    await payrollContext.SaveChangesAsync();
                }

                if (!await payrollContext.Contracts.AnyAsync())
                {
                    await payrollContext.Contracts.AddRangeAsync(
                        GetSampleContracts());

                    await payrollContext.SaveChangesAsync();
                }

                if (!await payrollContext.Timesheets.AnyAsync())
                {
                    await payrollContext.Timesheets.AddRangeAsync(
                        GetSampleTimeSheets());

                    await payrollContext.SaveChangesAsync();
                }
                
                if (!await payrollContext.PaymentHistories.AnyAsync())
                {
                    var t = payrollContext.Timesheets.ToList();
                    foreach (var ts in t)
                    {
                        var ph = new PaymentHistory(ts.ContractId, ts.ToDate.AddDays(1), ts.ToDate.AddDays(2), -1, -1, -1);
                        payrollContext.PaymentHistories.Add(ph);
                        payrollContext.SaveChanges();
                        int ph_id = ph.Id;
                        var pfs = new List<PaymentFactor>();
                        if (ts.Contract.PayMethod == PayMethod.Hourly)
                        {
                            var totalDays = (int)(ts.ToDate - ts.FromDate).TotalDays;
                            var remainingDaysFromWeek = totalDays % 7;
                            var numberOfWeeks = totalDays / 7;


                            var diff = ts.WorkingHours / numberOfWeeks - ts.Contract.WorkingWeekHours;
                            if (diff > 0)
                            {
                                var OverTimeBenefit = new PaymentFactor(ph_id, PaymentFactorType.Benefit, "Over Time", diff * ts.Contract.Wage * 1.5M);
                                pfs.Add(OverTimeBenefit);
                            }
                            else
                            {
                                var BelowTimeDeduction = new PaymentFactor(ph_id, PaymentFactorType.Deduction, "Below Time", diff * ts.Contract.Wage * -.8M);
                                pfs.Add(BelowTimeDeduction);
                            }
                            pfs.Add(new PaymentFactor(ph_id, PaymentFactorType.Benefit, "Pension Contribution", ts.Contract.Wage * numberOfWeeks * 8M * .2M));
                            pfs.Add(new PaymentFactor(ph_id, PaymentFactorType.Benefit, "Team Building", 20M));
                            decimal payment = ts.Contract.Wage * ts.WorkingHours;
                            foreach (var item in pfs)
                            {
                                payment += item.Amount;
                            }
                            var tr = payrollContext.TaxReferences.FirstOrDefault(t => t.Year == ts.FromDate.Year).TaxBrackets.ToList();
                            var tax = CalculateTax(payment, tr);
                            pfs.Add(new PaymentFactor(ph_id, PaymentFactorType.Deduction, "Tax", tax));
                            ph.GrossIncome = payment;
                            ph.NetIncome = payment - tax;
                            ph.Tax = tax;
                            payrollContext.PaymentHistories.Update(ph);
                            payrollContext.SaveChanges();
                            payrollContext.PaymentFactors.AddRange(pfs);
                            payrollContext.SaveChanges();




                        }
                        else if (ts.Contract.PayMethod == PayMethod.Monthly)
                        {
                            var totalDays = (int)(ts.ToDate - ts.FromDate).TotalDays;
                            var remainingDaysFromWeek = totalDays % 7;
                            var numberOfWeeks = totalDays / 7;


                            var diff = ts.WorkingHours / numberOfWeeks - ts.Contract.WorkingWeekHours;
                            if (diff > 0)
                            {
                                var OverTimeBenefit = new PaymentFactor(ph_id, PaymentFactorType.Benefit, "Over Time", diff * ts.Contract.Wage / 12 / 173 * 1.5M);
                                pfs.Add(OverTimeBenefit);
                            }
                            else
                            {
                                var BelowTimeDeduction = new PaymentFactor(ph_id, PaymentFactorType.Deduction, "Below Time", diff * ts.Contract.Wage / 12 / 173 * -.8M);
                                pfs.Add(BelowTimeDeduction);
                            }
                            pfs.Add(new PaymentFactor(ph_id, PaymentFactorType.Benefit, "Pension Contribution", ts.Contract.Wage / 12 * .1M));
                            pfs.Add(new PaymentFactor(ph_id, PaymentFactorType.Benefit, "Team Building", 25M));
                            decimal payment = ts.Contract.Wage / 12;
                            foreach (var item in pfs)
                            {
                                payment += item.Amount;
                            }
                            var tr = payrollContext.TaxReferences.FirstOrDefault(t => t.Year == ts.FromDate.Year).TaxBrackets.ToList();
                            var tax = CalculateTax(payment, tr);
                            pfs.Add(new PaymentFactor(ph_id, PaymentFactorType.Deduction, "Tax", tax));
                            ph.GrossIncome = payment;
                            ph.NetIncome = payment - tax;
                            ph.Tax = tax;
                            payrollContext.PaymentHistories.Update(ph);
                            payrollContext.SaveChanges();
                            payrollContext.PaymentFactors.AddRange(pfs);
                            payrollContext.SaveChanges();




                        }
                    }
                }
                
                
            }
            catch (Exception ex)
            {
                if (retryForAvailability < 10)
                {
                    retryForAvailability++;
                    var log = loggerFactory.CreateLogger<PayrollContextSeed>();
                    log.LogError(ex.Message);
                    await SeedAsync(payrollContext, loggerFactory, retryForAvailability);
                }
                throw;
            }
        }

        private static decimal CalculateTax(decimal grossIncome, List<TaxBracket> reference)
        {
            
        var _taxBrackets= reference;

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

        private static IEnumerable<TaxReference> GetSampleTaxReferenceData()
        {
            return new List<TaxReference>()
            {
                new TaxReference(2014),
                new TaxReference(2015),
                new TaxReference(2016),
                new TaxReference(2017),
                new TaxReference(2018),
                new TaxReference(2019),
                new TaxReference(2020),
                new TaxReference(2021)
            };
        }
        private static IEnumerable<TaxBracket> GetSampleTaxReferenceBrackets()
        {
            return new List<TaxBracket>()
            {
                new TaxBracket(1,0M,20384M,.3665M),
                new TaxBracket(1,20385M,34300M,.3810M),
                new TaxBracket(1,34301M,68507M,.3810M),
                new TaxBracket(1,68508M,Int32.MaxValue,.5175M),

                new TaxBracket(2,0M,20384M,.3665M),
                new TaxBracket(2,20385M,34300M,.3810M),
                new TaxBracket(2,34301M,68507M,.3810M),
                new TaxBracket(2,68508M,Int32.MaxValue,.5175M),

                new TaxBracket(3,0M,20384M,.3665M),
                new TaxBracket(3,20385M,34300M,.3810M),
                new TaxBracket(3,34301M,68507M,.3810M),
                new TaxBracket(3,68508M,Int32.MaxValue,.5175M),

                new TaxBracket(4,0M,20384M,.3665M),
                new TaxBracket(4,20385M,34300M,.3810M),
                new TaxBracket(4,34301M,68507M,.3810M),
                new TaxBracket(4,68508M,Int32.MaxValue,.5175M),

                new TaxBracket(5,0M,20384M,.3665M),
                new TaxBracket(5,20385M,34300M,.3810M),
                new TaxBracket(5,34301M,68507M,.3810M),
                new TaxBracket(5,68508M,Int32.MaxValue,.5175M),

                new TaxBracket(6,0M,20384M,.3665M),
                new TaxBracket(6,20385M,34300M,.3810M),
                new TaxBracket(6,34301M,68507M,.3810M),
                new TaxBracket(6,68508M,Int32.MaxValue,.5175M),

                new TaxBracket(7,0M,20384M,.3665M),
                new TaxBracket(7,20385M,34300M,.3810M),
                new TaxBracket(7,34301M,68507M,.3810M),
                new TaxBracket(7,68508M,Int32.MaxValue,.5175M),

                new TaxBracket(8,0M,20384M,.3665M),
                new TaxBracket(8,20385M,34300M,.3810M),
                new TaxBracket(8,34301M,68507M,.3810M),
                new TaxBracket(8,68508M,Int32.MaxValue,.5175M),
            };
        }


        static IEnumerable<Employee> GetSampleEmployees()
        {
            return new List<Employee>()
            {
                new Employee("Gary", "Lange", new DateTime(1984,01,01)),
                new Employee("Anastasia", "Shirokova", new DateTime(1998,02,01)),
                new Employee("Deepak", "Goldwyn Livingston", new DateTime(1987,03,01)),
                new Employee("Amin", "Mesbahi", new DateTime(1985,09,21))
            };
        }

        static IEnumerable<Contract> GetSampleContracts()
        {
            return new List<Contract>()
            {
                new Contract(1,new DateTime(2018,11,01),new DateTime(2019,11,01),PayMethod.Monthly,90000,40,100,200),
                new Contract(1,new DateTime(2019,11,01),new DateTime(2020,11,01),PayMethod.Monthly,91000,40,100,200),
                new Contract(1,new DateTime(2020,11,01),new DateTime(2021,11,30),PayMethod.Monthly,92000,40,100,200),
                
                new Contract(2,new DateTime(2019,04,01),new DateTime(2020,04,01),PayMethod.Monthly,85000,40,100,200),
                new Contract(2,new DateTime(2020,04,01),new DateTime(2021,04,01),PayMethod.Monthly,85000,40,100,200),
                new Contract(2,new DateTime(2021,04,01),new DateTime(2022,04,01),PayMethod.Monthly,85000,40,100,200),

                new Contract(3,new DateTime(2014,06,01),new DateTime(2015,06,01),PayMethod.Hourly,35,40,100,200),
                new Contract(3,new DateTime(2015,06,01),new DateTime(2016,06,01),PayMethod.Hourly,40,40,100,200),
                new Contract(3,new DateTime(2016,06,01),new DateTime(2017,06,01),PayMethod.Hourly,41,40,100,200),
                new Contract(3,new DateTime(2017,06,01),new DateTime(2018,06,01),PayMethod.Hourly,42,40,100,200),
                new Contract(3,new DateTime(2018,06,01),new DateTime(2018,10,30),PayMethod.Hourly,42,40,100,200),
                new Contract(3,new DateTime(2019,05,01),new DateTime(2020,12,30),PayMethod.Monthly,80000,40,100,200),
                new Contract(3,new DateTime(2021,01,01),new DateTime(2022,01,01),PayMethod.Hourly,40,40,100,200),

                new Contract(4,new DateTime(2021,10,22),new DateTime(2022,10,22),PayMethod.Monthly,90000,40,100,200),
            };
        }

        static IEnumerable<Timesheet> GetSampleTimeSheets()
        {
            return new List<Timesheet>()
            {
                new Timesheet(1,new DateTime(2018,11,01),new DateTime(2018,12,01),196,0,0,true),
                new Timesheet(1,new DateTime(2018,12,01),new DateTime(2019,01,01),194,0,0,true),
                new Timesheet(1,new DateTime(2019,01,01),new DateTime(2019,02,01),192,0,0,true),
                new Timesheet(1,new DateTime(2019,02,01),new DateTime(2019,03,01),190,0,0,true),
                new Timesheet(1,new DateTime(2019,03,01),new DateTime(2019,04,01),188,0,0,true),
                new Timesheet(1,new DateTime(2019,04,01),new DateTime(2019,05,01),186,0,0,true),
                new Timesheet(1,new DateTime(2019,05,01),new DateTime(2019,06,01),184,0,0,true),
                new Timesheet(1,new DateTime(2019,06,01),new DateTime(2019,07,01),182,0,0,true),
                new Timesheet(1,new DateTime(2019,07,01),new DateTime(2019,08,01),180,0,0,true),
                new Timesheet(1,new DateTime(2019,08,01),new DateTime(2019,09,01),178,0,0,true),
                new Timesheet(1,new DateTime(2019,09,01),new DateTime(2019,10,01),176,0,0,true),
                new Timesheet(1,new DateTime(2019,10,01),new DateTime(2019,11,01),174,0,0,true),

                new Timesheet(2,new DateTime(2019,11,01),new DateTime(2019,12,01),180,0,0,true),
                new Timesheet(2,new DateTime(2019,12,01),new DateTime(2020,01,01),194,0,0,true),
                new Timesheet(2,new DateTime(2020,01,01),new DateTime(2020,02,01),180,0,0,true),
                new Timesheet(2,new DateTime(2020,02,01),new DateTime(2020,03,01),190,0,0,true),
                new Timesheet(2,new DateTime(2020,03,01),new DateTime(2020,04,01),180,0,0,true),
                new Timesheet(2,new DateTime(2020,04,01),new DateTime(2020,05,01),186,0,0,true),
                new Timesheet(2,new DateTime(2020,05,01),new DateTime(2020,06,01),180,0,0,true),
                new Timesheet(2,new DateTime(2020,06,01),new DateTime(2020,07,01),182,0,0,true),
                new Timesheet(2,new DateTime(2020,07,01),new DateTime(2020,08,01),180,0,0,true),
                new Timesheet(2,new DateTime(2020,08,01),new DateTime(2020,09,01),178,0,0,true),
                new Timesheet(2,new DateTime(2020,09,01),new DateTime(2020,10,01),180,0,0,true),
                new Timesheet(2,new DateTime(2020,10,01),new DateTime(2020,11,01),174,0,0,true),

                new Timesheet(3,new DateTime(2020,11,01),new DateTime(2020,12,01),196,0,0,true),
                new Timesheet(3,new DateTime(2020,12,01),new DateTime(2021,01,01),194,0,0,true),
                new Timesheet(3,new DateTime(2021,01,01),new DateTime(2021,02,01),192,0,0,true),
                new Timesheet(3,new DateTime(2021,02,01),new DateTime(2021,03,01),190,0,0,true),
                new Timesheet(3,new DateTime(2021,03,01),new DateTime(2021,04,01),188,0,0,true),
                new Timesheet(3,new DateTime(2021,04,01),new DateTime(2021,05,01),186,0,0,true),
                new Timesheet(3,new DateTime(2021,05,01),new DateTime(2021,06,01),184,0,0,true),
                new Timesheet(3,new DateTime(2021,06,01),new DateTime(2021,07,01),182,0,0,true),
                new Timesheet(3,new DateTime(2021,07,01),new DateTime(2021,08,01),180,0,0,true),
                new Timesheet(3,new DateTime(2021,08,01),new DateTime(2021,09,01),178,0,0,true),
                new Timesheet(3,new DateTime(2021,09,01),new DateTime(2021,10,01),176,0,0,true),

                new Timesheet(4,new DateTime(2019,04,01),new DateTime(2019,05,01),196,0,0,true),
                new Timesheet(4,new DateTime(2019,05,01),new DateTime(2019,06,01),194,0,0,true),
                new Timesheet(4,new DateTime(2019,06,01),new DateTime(2019,07,01),192,0,0,true),
                new Timesheet(4,new DateTime(2019,07,01),new DateTime(2019,08,01),190,0,0,true),
                new Timesheet(4,new DateTime(2019,08,01),new DateTime(2019,09,01),188,0,0,true),
                new Timesheet(4,new DateTime(2019,09,01),new DateTime(2019,10,01),186,0,0,true),
                new Timesheet(4,new DateTime(2019,10,01),new DateTime(2019,11,01),184,0,0,true),
                new Timesheet(4,new DateTime(2019,11,01),new DateTime(2019,12,01),182,0,0,true),
                new Timesheet(4,new DateTime(2019,12,01),new DateTime(2020,01,01),180,0,0,true),
                new Timesheet(4,new DateTime(2020,01,01),new DateTime(2020,02,01),178,0,0,true),
                new Timesheet(4,new DateTime(2020,02,01),new DateTime(2020,03,01),176,0,0,true),
                new Timesheet(4,new DateTime(2020,03,01),new DateTime(2020,04,01),174,0,0,true),

                new Timesheet(5,new DateTime(2020,04,01),new DateTime(2020,05,01),196,0,0,true),
                new Timesheet(5,new DateTime(2020,05,01),new DateTime(2020,06,01),194,0,0,true),
                new Timesheet(5,new DateTime(2020,06,01),new DateTime(2020,07,01),192,0,0,true),
                new Timesheet(5,new DateTime(2020,07,01),new DateTime(2020,08,01),190,0,0,true),
                new Timesheet(5,new DateTime(2020,08,01),new DateTime(2020,09,01),188,0,0,true),
                new Timesheet(5,new DateTime(2020,09,01),new DateTime(2020,10,01),186,0,0,true),
                new Timesheet(5,new DateTime(2020,10,01),new DateTime(2020,11,01),184,0,0,true),
                new Timesheet(5,new DateTime(2020,11,01),new DateTime(2020,12,01),182,0,0,true),
                new Timesheet(5,new DateTime(2020,12,01),new DateTime(2021,01,01),180,0,0,true),
                new Timesheet(5,new DateTime(2021,01,01),new DateTime(2021,02,01),178,0,0,true),
                new Timesheet(5,new DateTime(2021,02,01),new DateTime(2021,03,01),176,0,0,true),
                new Timesheet(5,new DateTime(2021,03,01),new DateTime(2021,04,01),174,0,0,true),

                new Timesheet(6,new DateTime(2021,04,01),new DateTime(2021,05,01),196,0,0,true),
                new Timesheet(6,new DateTime(2021,05,01),new DateTime(2021,06,01),194,0,0,true),
                new Timesheet(6,new DateTime(2021,06,01),new DateTime(2021,07,01),192,0,0,true),
                new Timesheet(6,new DateTime(2021,07,01),new DateTime(2021,08,01),190,0,0,true),
                new Timesheet(6,new DateTime(2021,08,01),new DateTime(2021,09,01),188,0,0,true),
                new Timesheet(6,new DateTime(2021,09,01),new DateTime(2021,10,01),186,0,0,true),


                new Timesheet(7,new DateTime(2014,06,01),new DateTime(2014,07,01),196,0,0,true),
                new Timesheet(7,new DateTime(2014,07,01),new DateTime(2014,08,01),194,0,0,true),
                new Timesheet(7,new DateTime(2014,08,01),new DateTime(2014,09,01),192,0,0,true),
                new Timesheet(7,new DateTime(2014,09,01),new DateTime(2014,10,01),190,0,0,true),
                new Timesheet(7,new DateTime(2014,10,01),new DateTime(2014,11,01),188,0,0,true),
                new Timesheet(7,new DateTime(2014,11,01),new DateTime(2014,12,01),186,0,0,true),
                new Timesheet(7,new DateTime(2014,12,01),new DateTime(2015,01,01),184,0,0,true),
                new Timesheet(7,new DateTime(2015,01,01),new DateTime(2015,02,01),182,0,0,true),
                new Timesheet(7,new DateTime(2015,02,01),new DateTime(2015,03,01),180,0,0,true),
                new Timesheet(7,new DateTime(2015,03,01),new DateTime(2015,04,01),178,0,0,true),
                new Timesheet(7,new DateTime(2015,04,01),new DateTime(2015,05,01),176,0,0,true),
                new Timesheet(7,new DateTime(2015,05,01),new DateTime(2015,06,01),174,0,0,true),

                new Timesheet(8,new DateTime(2015,06,01),new DateTime(2015,07,01),196,0,0,true),
                new Timesheet(8,new DateTime(2015,07,01),new DateTime(2015,08,01),194,0,0,true),
                new Timesheet(8,new DateTime(2015,08,01),new DateTime(2015,09,01),192,0,0,true),
                new Timesheet(8,new DateTime(2015,09,01),new DateTime(2015,10,01),190,0,0,true),
                new Timesheet(8,new DateTime(2015,10,01),new DateTime(2015,11,01),188,0,0,true),
                new Timesheet(8,new DateTime(2015,11,01),new DateTime(2015,12,01),186,0,0,true),
                new Timesheet(8,new DateTime(2015,12,01),new DateTime(2016,01,01),184,0,0,true),
                new Timesheet(8,new DateTime(2016,01,01),new DateTime(2016,02,01),182,0,0,true),
                new Timesheet(8,new DateTime(2016,02,01),new DateTime(2016,03,01),180,0,0,true),
                new Timesheet(8,new DateTime(2016,03,01),new DateTime(2016,04,01),178,0,0,true),
                new Timesheet(8,new DateTime(2016,04,01),new DateTime(2016,05,01),176,0,0,true),
                new Timesheet(8,new DateTime(2016,05,01),new DateTime(2016,06,01),174,0,0,true),

                new Timesheet(9,new DateTime(2016,06,01),new DateTime(2016,07,01),196,0,0,true),
                new Timesheet(9,new DateTime(2016,07,01),new DateTime(2016,08,01),194,0,0,true),
                new Timesheet(9,new DateTime(2016,08,01),new DateTime(2016,09,01),192,0,0,true),
                new Timesheet(9,new DateTime(2016,09,01),new DateTime(2016,10,01),190,0,0,true),
                new Timesheet(9,new DateTime(2016,10,01),new DateTime(2016,11,01),188,0,0,true),
                new Timesheet(9,new DateTime(2016,11,01),new DateTime(2016,12,01),186,0,0,true),
                new Timesheet(9,new DateTime(2016,12,01),new DateTime(2017,01,01),184,0,0,true),
                new Timesheet(9,new DateTime(2017,01,01),new DateTime(2017,02,01),182,0,0,true),
                new Timesheet(9,new DateTime(2017,02,01),new DateTime(2017,03,01),180,0,0,true),
                new Timesheet(9,new DateTime(2017,03,01),new DateTime(2017,04,01),178,0,0,true),
                new Timesheet(9,new DateTime(2017,04,01),new DateTime(2017,05,01),176,0,0,true),
                new Timesheet(9,new DateTime(2017,05,01),new DateTime(2017,06,01),174,0,0,true),

                new Timesheet(10,new DateTime(2017,06,01),new DateTime(2017,07,01),196,0,0,true),
                new Timesheet(10,new DateTime(2017,07,01),new DateTime(2017,08,01),194,0,0,true),
                new Timesheet(10,new DateTime(2017,08,01),new DateTime(2017,09,01),192,0,0,true),
                new Timesheet(10,new DateTime(2017,09,01),new DateTime(2017,10,01),190,0,0,true),
                new Timesheet(10,new DateTime(2017,10,01),new DateTime(2017,11,01),188,0,0,true),
                new Timesheet(10,new DateTime(2017,11,01),new DateTime(2017,12,01),186,0,0,true),
                new Timesheet(10,new DateTime(2017,12,01),new DateTime(2018,01,01),184,0,0,true),
                new Timesheet(10,new DateTime(2018,01,01),new DateTime(2018,02,01),182,0,0,true),
                new Timesheet(10,new DateTime(2018,02,01),new DateTime(2018,03,01),180,0,0,true),
                new Timesheet(10,new DateTime(2018,03,01),new DateTime(2018,04,01),178,0,0,true),
                new Timesheet(10,new DateTime(2018,04,01),new DateTime(2018,05,01),176,0,0,true),
                new Timesheet(10,new DateTime(2018,05,01),new DateTime(2018,06,01),174,0,0,true),

                new Timesheet(11,new DateTime(2018,06,01),new DateTime(2018,07,01),196,0,0,true),
                new Timesheet(11,new DateTime(2018,07,01),new DateTime(2018,08,01),194,0,0,true),
                new Timesheet(11,new DateTime(2018,08,01),new DateTime(2018,09,01),192,0,0,true),
                new Timesheet(11,new DateTime(2018,09,01),new DateTime(2018,10,01),190,0,0,true),
                new Timesheet(11,new DateTime(2018,10,01),new DateTime(2018,11,01),188,0,0,true),

                new Timesheet(12,new DateTime(2019,05,01),new DateTime(2019,06,01),196,0,0,true),
                new Timesheet(12,new DateTime(2019,06,01),new DateTime(2019,07,01),194,0,0,true),
                new Timesheet(12,new DateTime(2019,07,01),new DateTime(2019,08,01),192,0,0,true),
                new Timesheet(12,new DateTime(2019,08,01),new DateTime(2019,09,01),190,0,0,true),
                new Timesheet(12,new DateTime(2019,09,01),new DateTime(2019,10,01),188,0,0,true),
                new Timesheet(12,new DateTime(2019,10,01),new DateTime(2019,11,01),186,0,0,true),
                new Timesheet(12,new DateTime(2019,11,01),new DateTime(2019,12,01),184,0,0,true),
                new Timesheet(12,new DateTime(2019,12,01),new DateTime(2020,01,01),182,0,0,true),
                new Timesheet(12,new DateTime(2020,01,01),new DateTime(2020,02,01),180,0,0,true),
                new Timesheet(12,new DateTime(2020,02,01),new DateTime(2020,03,01),178,0,0,true),
                new Timesheet(12,new DateTime(2020,03,01),new DateTime(2020,04,01),176,0,0,true),
                new Timesheet(12,new DateTime(2020,04,01),new DateTime(2020,05,01),174,0,0,true),

                new Timesheet(13,new DateTime(2020,05,01),new DateTime(2020,06,01),196,0,0,true),
                new Timesheet(13,new DateTime(2020,06,01),new DateTime(2020,07,01),194,0,0,true),
                new Timesheet(13,new DateTime(2020,07,01),new DateTime(2020,08,01),192,0,0,true),
                new Timesheet(13,new DateTime(2020,08,01),new DateTime(2020,09,01),190,0,0,true),
                new Timesheet(13,new DateTime(2020,09,01),new DateTime(2020,10,01),188,0,0,true),
                new Timesheet(13,new DateTime(2020,10,01),new DateTime(2020,11,01),186,0,0,true),
                new Timesheet(13,new DateTime(2020,11,01),new DateTime(2020,12,01),184,0,0,true),
                new Timesheet(13,new DateTime(2020,12,01),new DateTime(2021,01,01),182,0,0,true),
                new Timesheet(13,new DateTime(2021,01,01),new DateTime(2021,02,01),180,0,0,true),
                new Timesheet(13,new DateTime(2021,02,01),new DateTime(2021,03,01),178,0,0,true),
                new Timesheet(13,new DateTime(2021,03,01),new DateTime(2021,04,01),176,0,0,true),
                new Timesheet(13,new DateTime(2021,04,01),new DateTime(2021,05,01),174,0,0,true),

                new Timesheet(14,new DateTime(2021,05,01),new DateTime(2021,06,01),196,0,0,true),
                new Timesheet(14,new DateTime(2021,06,01),new DateTime(2021,07,01),194,0,0,true),
                new Timesheet(14,new DateTime(2021,07,01),new DateTime(2021,08,01),192,0,0,true),
                new Timesheet(14,new DateTime(2021,08,01),new DateTime(2021,09,01),190,0,0,true),
                new Timesheet(14,new DateTime(2021,09,01),new DateTime(2021,10,01),188,0,0,true)
            };
        }


        static IEnumerable<PaymentHistory> GetSamplePaymentHistories()
        {
            return new List<PaymentHistory>()
            {

            };
        }
    }
}