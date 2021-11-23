using System;
using Library.HighLevel.Materials;
using Library.Utils;

namespace Library.HighLevel.Accountability
{
    /// <summary>
    /// This class holds the JSON information of a <see cref="MaterialSalesLine" />.
    /// </summary>
    public class JsonMaterialSalesLine : IJsonHolder<MaterialSalesLine>
    {
        /// <summary>
        /// The sold material.
        /// </summary>
        public JsonMaterial Material { get; set; }

        /// <summary>
        /// The amount of sold material.
        /// </summary>
        public JsonAmount Amount { get; set; }

        /// <summary>
        /// The price of the sold material.
        /// </summary>
        public JsonPrice Price { get; set; }

        /// <summary>
        /// The moment the sale happened.
        /// </summary>
        public DateTime DateTime { get; set; }

        /// <inheritdoc />
        public void FromValue(MaterialSalesLine value)
        {
            this.Material = new JsonMaterial();
            this.Material.FromValue(value.Material);
            this.Amount = new JsonAmount();
            this.Amount.FromValue(value.Amount);
            this.Price = new JsonPrice();
            this.Price.FromValue(value.Price);
            this.DateTime = value.DateTime;
        }

        /// <inheritdoc />
        public MaterialSalesLine ToValue() =>
            new MaterialSalesLine(
                this.Material.ToValue(),
                this.Amount.ToValue(),
                this.Price.ToValue(),
                this.DateTime);
    }
}