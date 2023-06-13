namespace CampinWebApi.Domain.Entities;

public class Card
{
    //Sepet gibi düşünün :) 
    
    public int Id { get; set; }
    public string UserInfoID { get; set; }
    public string CampsiteId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int NumOfAdult { get; set; }
    public int NumOfChilder { get; set; }
    public bool isPaid { get; set; }
    public bool isEnable { get; set; }
    public float TotalPrice { get; set; }
}