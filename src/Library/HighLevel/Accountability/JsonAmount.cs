using Library.HighLevel;
using Library.Utils;

namespace Library.HighLevel.Accountability
{
    /// <summary>
    /// This class holds the JSON information of an <see cref="Amount" />.
    /// </summary>
    public class JsonAmount : IJsonHolder<Amount>
    {
        /// <summary>
        /// The quantity.
        /// </summary>
        public double Quantity { get; set; }

        /// <summary>
        /// The unit.
        /// </summary>
        public string? Unit { get; set; }

        /// <inheritdoc />
        public Amount ToValue() => 
            new Amount(this.Quantity, Accountability.Unit.GetByAbbr(this.Unit.Unwrap()).Unwrap());

        /// <inheritdoc />
        public void FromValue(Amount value)
        {
            this.Quantity = value.Quantity;
            this.Unit = value.Unit.Abbreviation;
        }
    }
}
