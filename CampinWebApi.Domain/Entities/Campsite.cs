using System;
namespace CampinWebApi.Domain.Entities
{ 
	public class Campsite
    { 
        public string CampsiteId { get; set; }
        public int HolidayDestinationId { get; set; }
        public int FeatureId { get; set; }
        public string OwnerID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float Rate { get; set; }
        
        // convert float 
        public float AdultPrice { get; set; }
        public float ChildPrice { get; set; }
        public DateTime SeasonStartDate { get; set; }
        public DateTime SeasonCloseDate { get; set; }
        public string lat { get; set; }
        public string lng { get; set; }
        public int Capacity { get; set; }
        public bool isEnable { get; set; }
    }
}
