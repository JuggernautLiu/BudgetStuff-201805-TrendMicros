using System;
using System.Collections.Generic;

namespace BudgetStuffTests
{
    public class BudgetManager
    {
        private readonly IRepository<Budget> _repo;
        public BudgetManager(IRepository<Budget> repo)
        {
            _repo = repo;
        }

        public decimal TotalAmount(DateTime startdate, DateTime enddate)
        {
            if (startdate > enddate)
                throw new InvalidException();
            var BudgetMap = _repo.GetBudget(startdate, enddate);
            if (BudgetMap.Keys.Count > 1)
            {
                decimal amount = 0;
                int index = 0;
                foreach (var month in BudgetMap.Keys)
                {
                    int timeSpan = 0;
                    if (index == 0)
                    {
                         timeSpan = DateTime.DaysInMonth(month.Year, month.Month) - startdate.Day + 1;
                    }
                    else if (index == BudgetMap.Keys.Count - 1)
                    {
                        timeSpan = enddate.Day;
                    }
                    else
                    {
                        timeSpan = DateTime.DaysInMonth(month.Year, month.Month);
                    }
                    // var timeSpan2 = DateTime.DaysInMonth(month.Year, month.Month) - startdate.Day + 1;
                    amount += getAmount(DateTime.DaysInMonth(month.Year, month.Month), BudgetMap[month].amount, timeSpan);
                    //amount += BudgetMap[month].amount;
                    index++;
                }
                return amount;
            }
            var timeSpan2 = (enddate - startdate).Days+1;
            return getAmount(DateTime.DaysInMonth(startdate.Year, startdate.Month), BudgetMap[startdate].amount, timeSpan2);
        }

        private static decimal getAmount(int monthdays, int amount, int actualdays)
        {
            return amount / monthdays * actualdays;
            //return BudgetMap[startdate].amount / DateTime.DaysInMonth(startdate.Year, startdate.Month) * (timeSpan.Days + 1);
        }

        // private decimal GetBudget
    }
}