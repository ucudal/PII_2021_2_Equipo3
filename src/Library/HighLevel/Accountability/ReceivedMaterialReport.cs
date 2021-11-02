using System.Collections.ObjectModel;

namespace Library.HighLevel.Accountability
{
    /// <summary>
    /// This class represents a reports of all material purchased by the entrepreneur
    /// </summary>
    public class ReceivedMaterialReport
    {
        /// <summary>
        /// Is the list of purchased materials
        /// </summary>
        public readonly ReadOnlyCollection<MaterialBoughtLine> Materials;
        
        /// <summary>
        /// Is the constructor of ReceivedMaterialReport
        /// </summary>
        /// <param name="materials">Is the collection of purchased materials</param>
        public ReceivedMaterialReport(ReadOnlyCollection<MaterialBoughtLine> materials)
        {
            this.Materials = materials;
        }
    }
}