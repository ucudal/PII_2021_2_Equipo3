using System;
using System.Collections.Generic;
using System.Linq;
using Library.HighLevel.Materials;
using Library.HighLevel.Companies;
using Ucu.Poo.Locations.Client;

namespace Library.HighLevel.Entrepreneurs
{
    /// <summary>
    /// This class has the responsibility of searching material publication's
    /// according to a specified category, keyword or location.
    /// We created this class using expert, this class itself does all the possible
    /// filter searches). It also has a High Cohesion because of the same reason.
    /// </summary>
    public class Searcher
    {
        /// <summary>
        /// This method has the responsibility of searching all the publication's by a category.
        /// </summary>
        /// <param name="category"></param>
        public List<AssignedMaterialPublication> SearchByCategory(MaterialCategory category)
        {
           List<AssignedMaterialPublication> searchResultCategory = new List<AssignedMaterialPublication>();
           foreach (var item in Singleton<CompanyManager>.Instance.Publications)
           {
               if (item.Publication.Material.Category.Name == category.Name && !item.Publication.Sold)
               {
                   searchResultCategory.Add(item);
               }
           }
           return searchResultCategory;
        }

        /// <summary>
        /// This method has the responsibility of searching all the publication's by a keyword.
        /// </summary>
        /// <param name="keyword"></param>
        public List<AssignedMaterialPublication> SearchByKeyword(string keyword)
        {
           List<AssignedMaterialPublication> searchResultKeyword = new List<AssignedMaterialPublication>();
           foreach (var item in Singleton<CompanyManager>.Instance.Publications)
           {
               if (item.Publication.Keywords.Any(k => k.ToLowerInvariant() == keyword.ToLowerInvariant()) && !item.Publication.Sold)
               {
                   searchResultKeyword.Add(item);
               }
           }
           return searchResultKeyword;
        }

        /// <summary>
        /// This method has the responsibility of searching all the publication's by a location.
        /// </summary>
        /// <param name="locationSpecified"></param>
        /// <param name="distanceSpecified"></param>
        public List<AssignedMaterialPublication> SearchByLocation(Location locationSpecified, double distanceSpecified)
        {
           List<AssignedMaterialPublication> searchResultLocation = new List<AssignedMaterialPublication>();
           foreach (var item in Singleton<CompanyManager>.Instance.Publications)
           {
               Distance distance;
               distance = Singleton<LocationApiClient>.Instance.GetDistanceAsync(locationSpecified, item.Publication.PickupLocation).Result;
               if (distance.TravelDistance <= distanceSpecified && !item.Publication.Sold)
               {
                   searchResultLocation.Add(item);
               }
           }
           return searchResultLocation;
        }        
    }
}
