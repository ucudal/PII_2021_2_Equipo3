using System.Linq;

namespace Library.HighLevel.Accountability
{
    /// <summary>
    /// This class represents a measure through which a certain amount of material can be measured.
    /// We used the pattern Creator, we assigned the static attributes Length, Weight and Volume to Measure because
    /// it's the class that knows about it.
    /// </summary>
    public class Measure
    {
        /// <summary>
        /// The length measure.
        /// </summary>
        public static Measure Length = new Measure("Length", new (string, string, double)[]
        {
            ("millimeter", "mm",  0.001),
            ("centimeter", "cm",  0.01),
            ("decimeter",  "dm",  0.1),
            ("meter",      "m",   1),
            ("decameter",  "dam", 10),
            ("hectometer", "hm",  100),
            ("kilometer",  "km",  1000)
        });

        /// <summary>
        /// The weight measure.
        /// </summary>
        public static Measure Weight = new Measure("Weight", new (string, string, double)[]
        {
            ("gram",      "g",   0.001),
            ("decagram",  "dag", 0.01),
            ("hectogram", "hg",  0.1),
            ("kilogram",  "kg",  1),
            ("tonne",     "t",   1000)
        });

        /// <summary>
        /// The volume measure.
        /// </summary>
        public static Measure Volume = new Measure("Volume", new (string, string, double)[]
        {
            ("millilitre", "ml",  0.001),
            ("centilitre", "cl",  0.01),
            ("decilitre",  "dl",  0.1),
            ("litre",      "l",   1),
            ("decalitre",  "dal", 10),
            ("hectolitre", "hl",  100),
            ("kilolitre",  "kl",  1000)
        });

        /// <summary>
        /// Gets the list of available measures.
        /// </summary>
        public static readonly Measure[] Measures = new Measure[]
        {
            Measure.Length, Measure.Weight, Measure.Volume
        };

        /// <summary>
        /// Gets the measure's name.
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// The measure's available units.
        /// </summary>
        public readonly Unit[] Units;

        /// <summary>
        /// Initializes a new instance of the <see cref="Measure"/> class, assigning its units in the process.
        /// </summary>
        /// <param name="name">The measure's name.</param>
        /// <param name="unitsData">An array of tuples containing data about its units.</param>
        private Measure(string name, (string name, string abbr, double weight)[] unitsData)
        {
            this.Name = name;
            this.Units = unitsData.Select(data => new Unit(data.name, data.abbr, data.weight, this)).ToArray();
        }

        /// <summary>
        /// Gets the measure with a concrete name.
        /// </summary>
        /// <param name="name">The measure's name.</param>
        /// <returns>A measure.</returns>
        public static Measure? GetByName(string name) =>
            Measures.Where(measure => measure.Name.ToLowerInvariant() == name.ToLowerInvariant()).FirstOrDefault();
    }
}
