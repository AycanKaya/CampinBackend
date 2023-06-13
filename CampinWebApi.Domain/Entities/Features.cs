namespace CampinWebApi.Domain.Entities;

public class Features
{ 
        public int Id { get; set; }
        public bool HasElectricity { get; set; }
        public bool HasWater { get; set; }
        public bool HasToilet { get; set; }
        public bool HasShower { get; set; }
        public bool HasWiFi { get; set; }
        public bool HasTrees { get; set; }
        public bool HasParking { get; set; }
        public bool HasSecurity { get; set; }
        public bool HasBusinessServices { get; set; }
        public bool HasActivities { get; set; }
        public bool HasFirePit { get; set; }
        public bool HasSignal { get; set; }
        public bool IsNearSea { get; set; }
}