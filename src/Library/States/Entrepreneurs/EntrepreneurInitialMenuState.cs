using Library.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using Library.HighLevel.Materials;
using Library.HighLevel.Companies;
using Library.HighLevel.Entrepreneurs;

namespace Library.States.Entrepreneurs
{
    /// <summary>
    /// This class represents a <see cref="State" /> for an entrepreneur in the initial menu.
    /// </summary>
    public class EntrepreneurInitialMenuState : MultipleOptionState
    {
        private string? initialResponse;
        private string id;

        /// <summary>
        /// Initializes an instance of <see cref="EntrepreneurInitialMenuState" />.
        /// </summary>
        public EntrepreneurInitialMenuState(string id, string? initialResponse = null)
        {
            this.initialResponse=initialResponse;
            this.id = id;
            this.commands = new (string, string, Func<(State, string?)>)[]
            {
                ("/buy",           "Le permite al emprendedor comprar un material",                 this.buyMaterial),
                ("/searchFK",      "Busca materiales utilizando palabras claves.",                  this.searchByKeyword),
                ("/searchFC",      "Busca materiales por categorías.",                              this.searchByCategory),
                ("/searchFZ",      "Busca materiales por zona.",                                    this.searchByZone),
                ("/materialgen",   "Muestra qué materiales son constantemente generados.",          this.materialsgen),
                ("/materialspunt", "Muestra qué materiales son generados puntualmente.",            this.materialspunt),
                ("/ereport",       "Muestra los reportes de materiales recibidos en cierta fecha.", this.ereport)
            };
        }

        private (State, string?) buyMaterial()
        {
            return (new EntrepreneurBuyMaterialState(this.id), null);
        }

        private (State, string?) ereport()
        {
            if (Singleton<EntrepreneurManager>.Instance.GetById(this.id) is Entrepreneur entrepreneur)
            {
                return (new EntrepreneurCreateReportState(entrepreneur), null);
            }
            else
            {
                return (this, "Lo siento, no te reconozco como emprendedor.");
            }
        }

        private (State, string) materialsgen()
        {
            IEnumerable<AssignedMaterialPublication> publications = Singleton<CompanyManager>.Instance.Publications.Where(
                publications =>
                    publications.Publication.Type.PublicationType == MaterialPublicationTypeData.MaterialPublicationType.CONTINUOUS
            );

            return (this, string.Join('\n', publications));
        }

        private (State, string) materialspunt()
        {
            IEnumerable<AssignedMaterialPublication> publications = Singleton<CompanyManager>.Instance.Publications.Where(
                publication =>
                    publication.Publication.Type.PublicationType == MaterialPublicationTypeData.MaterialPublicationType.SCHEDULED
            );
            return (this, string.Join('\n', publications));
        }

        private (State, string?) searchByKeyword()
        {
            return (new EntrepreneurSearchByKeywordState(this.id), null);
        }

        private (State, string?) searchByCategory()
        {
            return (new EntrepreneurSearchByCategoryState(this.id), null);
        }

        private (State, string?) searchByZone()
        {
            return (new EntrepreneurSearchByZoneState(this.id), null);
        }
        
       /// <inheritdoc />
        protected override string getInitialResponse()
        {
            if(initialResponse is null) return "¿Qué quieres hacer?";
            string response = initialResponse;
            initialResponse = null;
            return $"{response}\n¿Qué quieres hacer?";
        }

        /// <inheritdoc />
        protected override string getErrorString() =>
            "Opción inválida.";
    }
}
