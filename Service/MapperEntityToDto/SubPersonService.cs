using Entities;
using Entities.Interfaces;
using Service.Mapper;
using Service.Models;

namespace Service.MapperEntityToDto
{
    public class SubPersonService
    {
        private readonly ISubPersonRepository _subPersonRepository;

        public SubPersonService(ISubPersonRepository subPersonRepository)
        {
            _subPersonRepository = subPersonRepository;
        }

        public void AddNewPerson(SubPersonModel subPersonModel)
        {
             _subPersonRepository.AddNewSubPersonAsync(Mappers.MapToViewModel<SubPersonModel,SubPerson>(subPersonModel));
        }
        public void DeleteSubPerson(SubPersonModel subPersonModel)
        {
             _subPersonRepository.DeleteSubPersonAsync(Mappers.MapToViewModel<SubPersonModel, SubPerson>(subPersonModel));
        }

        public List<SubPersonModel> GetAllSubPersonByPerson(long personId)
        {
            List<SubPersonModel>subPersons = new List<SubPersonModel>();
            List<SubPerson> subPeople = _subPersonRepository.GetAllSubPersonByPersonAsync(personId).Result;
            subPeople.ForEach(x => subPersons.Add(Mappers.MapToViewModel<SubPerson, SubPersonModel>(x)));

            return subPersons;
        }

        public SubPersonModel GetSubPersonById(int subPersonId)
        {
            return Mappers.MapToViewModel<SubPerson, SubPersonModel>(_subPersonRepository.GetSubPersonByIdAsync(subPersonId).Result);
        }
        public SubPersonModel GetSubPersonByPersonAndSubPersonId(int subPersonId, int personId)
        {
            return Mappers.MapToViewModel<SubPerson, SubPersonModel>(_subPersonRepository.GetSubPersonByPersonAndSubPersonId(subPersonId,personId).Result);
        }
    }
}

