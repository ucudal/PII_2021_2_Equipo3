using NUnit.Framework;
using System.Collections.Generic;
using Library.HighLevel.Materials;
using Library.HighLevel.Accountability;

namespace ProgramTests
{
    /// <summary>
    /// 
    /// </summary>
    public class PublishOfferTest
    {
        /// <summary>
        /// 
        /// </summary>
        [SetUp]
        public void Setup()
        {
           
        }

        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void PublishOffer()
        {
            MaterialCategory category = new MaterialCategory("Impermeable");
            Unit unit = new Unit("Centimeters", "cm", 0.1, Measure.Length);
            Amount amount = new Amount(10, unit);
            Price price = new Price(100, Currency.Peso, unit);
            Location location = new Location();
            List<string> keyword = new List<string>();
            keyword.Add("cámara");
            Material material = Material.CreateInstance("Cámara de cubierta", Measure.Length, category, keyword);

            MaterialPublication publication = MaterialPublication.CreateInstance(material, amount, price, location);   
        }
    }
}