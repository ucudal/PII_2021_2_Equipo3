using Library.Core;
using Library.Core.Processing;
using Library.HighLevel.Companies;

namespace Library.States.Companies
{
    public partial class IncompleteCompanyRepresentativeState
    {
        private class AssignExistingCompanyState : InputProcessor<Company>
        {
            private Company company;

            public AssignExistingCompanyState(Company company)
            {
                this.company = company;
            }

            /// <inheritdoc />
            public override string GetDefaultResponse() =>
                $"Ya existe una compañía llamada {this.company.Name}. ¿Es esta la empresa a la que pertenece?";

            /// <inheritdoc />
            protected override Result<Company, string> getResult() => Result<Company, string>.Ok(this.company);

            /// <inheritdoc />
            public override Result<bool, string> ProcessInput(string msg)
            {
                msg = msg.Trim().ToLowerInvariant();
                if (msg == "sí" || msg == "si" || msg == "s" || msg == "yes" || msg == "y")
                    return Result<bool, string>.Ok(true);
                else if (msg == "no" || msg == "n")
                    return Result<bool, string>.Ok(false);
                
                return Result<bool, string>.Err($"Por favor ingresa \"sí\" o \"no\".\n{this.GetDefaultResponse()}");
            }

            /// <inheritdoc />
            public override void Reset()
            {
            }
        }
    }
}
