using Library.Core.Processing;
using Library.HighLevel.Accountability;
using Library.InputHandlers.Abstractions;
using Library.Utils;

namespace Library.InputHandlers
{
    /// <summary>
    /// This class has the responsibility of create an instance of amount for the publication, with the input data of the user.
    /// </summary>
    public class AmountProcessor : FormProcessor<Amount>
    {
        private float? quantity;
        private Unit? unit;

        /// <summary>
        /// Initializes an instance of <see cref="AmountProcessor" />
        /// </summary>
        public AmountProcessor()
        {
            this.inputHandlers = new InputHandler[]
            {
                ProcessorHandler.CreateInfallibleInstance<string>(
                    q => this.quantity = float.Parse(q),
                    new BasicStringProcessor(() => "Por favor ingresa la cantidad de unidades del material que deseas publicar.")
                ),
                ProcessorHandler.CreateInfallibleInstance<Unit>(
                    u => this.unit = u,
                    new UnitProcessor(() => "Por favor ingresa la abreviatura de la unidad del material. (Por ejemplo \"cm\").")
                )
            };
        }

        protected override Result<Amount, string> getResult() =>
            Result<Amount, string>.Ok(new Amount(this.quantity.Unwrap(), this.unit.Unwrap()));
    }
}