
namespace TeachMe.Models
{
    /// <summary>
    /// City
    /// </summary>
    public class City
    {

        #region Fields

        public int Id { get; set; }

        public string Name { get; set; }

        public int GeoLocationId { get; set; }

        public virtual GeoLocation GeoLocation { get; set; }

        #endregion

        #region Methods

        public override string ToString()
        {
            string geo = GeoLocation != null ? GeoLocation.Latitude + " " + GeoLocation.Longitude : "";
            return Name + " " + geo;
        }
        #endregion

    }

    public class Street
    {
        public Street()
        {
        }

        public Street(string name)
        {
            this.Name = name;
        }

        public int Id { get; set; }

        public string Name { get; set; }

    }
}