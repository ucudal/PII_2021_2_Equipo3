using System;
using System.Collections.Generic;
using Library.Core;
using Library.HighLevel.Companies;
using Library.HighLevel.Accountability;
using Library.HighLevel.Materials;
using Ucu.Poo.Locations.Client;
using Library.Core.Processing;
using Library.InputHandlers;
using Library.InputHandlers.Abstractions;

namespace Library.States.Companies
{
    public class CompanyPublishMaterialState : InputHandlerState
    {
        public CompanyPublishMaterialState(string id) : base(
            exitState: () => new CompanyInitialMenuState(),
            nextState: () => new CompanyInitialMenuState(),
            inputHandler: null! // Replace with real value
        )
        {
        }

        public override string GetDefaultResponse()
        {
            return base.GetDefaultResponse();
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override (State?, string?) ProcessMessage(string id, ref UserData data, string msg)
        {
            return base.ProcessMessage(id, ref data, msg);
        }

        private class CollectDataProcessor : FormProcessor<(Material, Amount, Price, Location, MaterialPublicationTypeData, IList<string>)>
        {
            private Material material;
            private Amount amount;
            private Price price;
            private Location location;
            private MaterialPublicationTypeData materialPublicationTypeData;
            private IList<string> keywords;

            public CollectDataProcessor()
            {
                this.inputHandlers = new InputHandler[]
                {
                    ProcessorHandler.CreateInfallibleInstance<Material>(

                        m => this.material = m,
                        new MaterialProcessor()
                    ),
                    ProcessorHandler.CreateInfallibleInstance<Amount>(
                        a => this.amount = a,
                        new AmountProcessor()
                    )
                };
            }
            protected override Result<(Material, Amount, Price, Location, MaterialPublicationTypeData, IList<string>), string> getResult()
            {
                throw new NotImplementedException();
            }
        }

    }
}
