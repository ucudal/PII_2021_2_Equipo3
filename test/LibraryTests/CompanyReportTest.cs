using System;
using System.Collections.Generic;
using Library;
using Library.HighLevel.Accountability;
using Library.HighLevel.Materials;
using Library.Utils;
using NUnit.Framework;

namespace ProgramTests
{
    /// <summary>
    /// Test if a company can get a report of all sent materials.
    /// </summary>
    [TestFixture]
    public class CompanyReportTest
    {
        private MaterialCategory? category;
        private MaterialCategory? category2;
        private Unit? unit;
        private Unit? unit2;
        private Price price;
        private Price price2;
        private Amount? amount;
        private Amount? amount2;
        private Material? soldMaterial;
        private Material? soldMaterial2;
        private DateTime sold;
        private DateTime sold2;

        /// <summary>
        /// Necessary configuration.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            this.category = MaterialCategory.GetByName("Plásticos").Unwrap();
            this.unit = Unit.GetByAbbr("cm")!;
            this.price = new Price(300, Currency.Peso, this.unit);
            this.amount = new Amount(3, this.unit);
            this.soldMaterial = Material.CreateInstance("Palet Plástico", Measure.Length, this.category);
            this.sold = new DateTime(2021, 10, 3, 15, 30, 16);
            this.category2 = MaterialCategory.GetByName("Celulósicos").Unwrap();
            this.unit2 = Unit.GetByAbbr("cm")!;
            this.price2 = new Price(10, Currency.Dollar, this.unit2);
            this.amount2 = new Amount(40, this.unit2);
            this.soldMaterial2 = Material.CreateInstance("Bujes de cartón", Measure.Length, this.category2);
            this.sold2 = new DateTime(2021, 11, 1, 16, 21, 15);
        }

        /// <summary>
        /// Tests if the company can see the report of the sealed materials.
        /// </summary>
        [Test]
        public void CompanyReport()
        {
            MaterialSalesLine materialSale = new MaterialSalesLine(this.soldMaterial!, this.amount!, this.price, this.sold, string.Empty);
            MaterialSalesLine materialSale2 = new MaterialSalesLine(this.soldMaterial2!, this.amount2!, this.price2, this.sold2, string.Empty);
            IList<MaterialSalesLine> sales = new List<MaterialSalesLine> { materialSale, materialSale2 };
            IList<MaterialSalesLine> expected = SentMaterialReport.GetSentReport(sales, 3);
            Assert.AreEqual(expected, sales);
        }

        /// <summary>
        /// Test if the materials sold are out of time.
        /// </summary>
        [Test]
        public void CompanyReportOutOfTime()
        {
            MaterialCategory category3 = MaterialCategory.GetByName("Plásticos").Unwrap();
            Unit unit3 = Unit.GetByAbbr("cm")!;
            Price price3 = new Price(300, Currency.Peso, unit3);
            Amount amount3 = new Amount(3, unit3);
            Material soldMaterial3 = Material.CreateInstance("Palet Plástico", Measure.Length, category3);
            DateTime sold3 = new DateTime(2021, 3, 10, 13, 45, 12);
            MaterialCategory category4 = MaterialCategory.GetByName("Celulósicos").Unwrap();
            Unit unit4 = Unit.GetByAbbr("g")!;
            Price price4 = new Price(10, Currency.Dollar, unit4);
            Amount amount4 = new Amount(40, unit4);
            Material soldMaterial4 = Material.CreateInstance("Bujes de cartón", Measure.Length, category4);
            DateTime sold4 = new DateTime(2021, 2, 15, 17, 45, 02);

            MaterialSalesLine materialSale3 = new MaterialSalesLine(soldMaterial3, amount3, price3, sold3, string.Empty);
            MaterialSalesLine materialSale4 = new MaterialSalesLine(soldMaterial4, amount4, price4, sold4, string.Empty);
            IList<MaterialSalesLine> sales2 = new List<MaterialSalesLine> { materialSale3, materialSale4 };
            IList<MaterialSalesLine> report = SentMaterialReport.GetSentReport(sales2, 3);
            IList<MaterialSalesLine> expected = new List<MaterialSalesLine>();

            Assert.AreEqual(expected, report);
        }

        /// <summary>
        /// Tests the user story of creating a company report.
        /// </summary>
        [Test]
        public void CreateCompanyReportTest()
        {
            RuntimeTest.BasicRuntimeTest("create-company-report", () =>
            {
                List<(string, string)> messages = Singleton<ProgramaticMultipleUserPlatform>.Instance.ReceiveMessages(
                    "Company1",
                    "/companyreport",
                    "23/11/2021");

                Assert.AreEqual("Reporte vacío.", messages[messages.Count - 1].Item2.Split('\n')[0]);
            });
        }

        /// <summary>
        /// Tests the user story of making a company report after selling materials.
        /// </summary>
        [Test]
        public void CreateCompanyReportTest2()
        {
            RuntimeTest.BasicRuntimeTest("create-company-report-2", () =>
            {
                List<(string, string)> messages = Singleton<ProgramaticMultipleUserPlatform>.Instance.ReceiveMessages(
                    "Company1",
                    "/companyreport",
                    "23/11/2021");

                Assert.AreEqual(
                    "(0) 10.00 cm de Bujes de cartón vendido(s) al emprendedor Santiago a precio de 15 U$/cm el día 28/11/2021",
                    messages[messages.Count - 1].Item2.Split('\n')[0]);
            });
        }
    }
}