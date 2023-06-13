using CampinWebApi.Core.DTO.RezervationDTO;
using CampinWebApi.Core.Models.RezervationsModel;
using CampinWebApi.Domain.Entities;

namespace CampinWebApi.Contracts;

public interface IRezervationService
{

    Task<bool> MakeRezervations(MakeRezervationDTO makeRezervationDto, string userToken);
    Task<List<GetUserReservedModel>> GetUserRezervedCampsite(string userToken);
    
}