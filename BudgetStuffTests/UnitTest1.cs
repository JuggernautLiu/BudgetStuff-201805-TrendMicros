using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace BudgetStuffTests
{
    [TestClass]
    public class UnitTest1
    {
        private IRepository<Budget> _repository = Substitute.For<IRepository<Budget>>();
        private BudgetManager _budgetmanager;

        [TestInitializeAttribute]
        public void TestInit()
        {
            _budgetmanager = new BudgetManager(_repository);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidException))]
        public void InvalidDate()
        {
            // arrange
            var startdate = DateTime.Today;
            var enddate = DateTime.Now.AddDays(-1);
            //var budgetmanager = new BudgetManager();
            decimal expected = 0;
            
            //act
            var actual = _budgetmanager.TotalAmount(startdate, enddate);
        }

        [TestMethod]
        public void OneMonthNoBudget()
        {
            // arrange
            var startdate = new DateTime(2017, 03, 01);
            var enddate = new DateTime(2017, 03, 31);

            // mock
            GivenBudget(startdate, enddate, new Dictionary<DateTime, Budget>()
            {
                {
                    startdate,
                    new Budget() {amount = 0}
                }
            });
            
            AmountShouldBe(startdate, enddate, 0);
        }

        [TestMethod]
        public void OneMonthHasBudget()
        {
            // arrange
            var startdate = new DateTime(2017, 03, 01);
            var enddate = new DateTime(2017, 03, 31);

            // mock
            GivenBudget(startdate, enddate, new Dictionary<DateTime, Budget>()
            {
                {
                    startdate,
                    new Budget() {amount = 3100}
                }
            });

            AmountShouldBe(startdate, enddate, 3100);
        }

        [TestMethod]
        public void OnedayHasBudget()
        {
            // arrange
            var startdate = new DateTime(2017, 03, 01);
            var enddate = new DateTime(2017, 03, 01);

            // mock
            GivenBudget(startdate, enddate, new Dictionary<DateTime, Budget>()
            {
                {
                    startdate,
                    new Budget() {amount = 3100}
                }
            });

            AmountShouldBe(startdate, enddate, 100);
        }

        [TestMethod]
        public void TwodaysHasBudget()
        {
            // arrange
            var startdate = new DateTime(2017, 03, 01);
            var enddate = new DateTime(2017, 03, 02);

            // mock
            GivenBudget(startdate, enddate, new Dictionary<DateTime, Budget>()
            {
                {
                    startdate,
                    new Budget() {amount = 3100}
                }
            });

            AmountShouldBe(startdate, enddate, 200);
        }

        [TestMethod]
        public void LeapYearFebHasBudget()
        {
            // arrange
            var startdate = new DateTime(2016, 02, 01);
            var enddate = new DateTime(2016, 02, 15);

            // mock
            GivenBudget(startdate, enddate, new Dictionary<DateTime, Budget>()
            {
                {
                    startdate,
                    new Budget() {amount = 29}
                }
            });

            AmountShouldBe(startdate, enddate, 15);
        }

        [TestMethod]
        public void MultiMonth_NoBudget()
        {
            // arrange
            var startdate = new DateTime(2017, 03, 01);
            var enddate = new DateTime(2017, 04, 30);

            // mock
            GivenBudget(startdate, enddate, new Dictionary<DateTime, Budget>()
            {
                {
                    startdate,
                    new Budget() {amount = 0}
                },
                {
                    new DateTime(2017,04,01), 
                    new Budget() {amount = 0}
                }
            });

            AmountShouldBe(startdate, enddate, 0);
        }

        [TestMethod]
        public void MultiMonth_Budget()
        {
            // arrange
            var startdate = new DateTime(2017, 03, 01);
            var enddate = new DateTime(2017, 04, 30);

            // mock
            GivenBudget(startdate, enddate, new Dictionary<DateTime, Budget>()
            {
                {
                    startdate,
                    new Budget() {amount = 3100}
                },
                {
                    new DateTime(2017,04,01),
                    new Budget() {amount = 30}
                }
            });

            AmountShouldBe(startdate, enddate, 3130);
        }

        private void AmountShouldBe(DateTime startdate, DateTime enddate, decimal expected)
        {
            Assert.AreEqual(expected,_budgetmanager.TotalAmount(startdate, enddate));
        }

        private void GivenBudget(DateTime starDateTime, DateTime endDateTime, Dictionary<DateTime, Budget> mockBudget)
        {
            _repository.GetBudget(starDateTime, endDateTime).ReturnsForAnyArgs(mockBudget);
        }
    }


}