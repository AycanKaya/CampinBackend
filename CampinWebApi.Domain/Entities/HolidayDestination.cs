namespace CampinWebApi.Domain.Entities;

//Olimpos , Çıralı falan gibi düşünülebilir.

public class HolidayDestination
{
        public int Id { get; set; }
        public int CityId { get; set; }
        public string HolidayDestinationName { get; set; }
        public string Information { get; set; }
}