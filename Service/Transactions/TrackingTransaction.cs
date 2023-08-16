using Entities;
using MeuContexto.UOW;
using ProjetoJqueryEstudos.Models;
using Service.Mapper;
using Service.Models;
using Service.Utils;

namespace Service.Transactions
{
    public class TrackingTransaction
    {
        private readonly IUnitOfWork _unitOfWork;
        public TrackingTransaction(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public bool TrackingCodeExist(string code)
        {
            TrackingCode trackingCode = _unitOfWork.TrackingService.GetTrackingCodeByCode(code);

            return trackingCode != null;
        }
        public void AddNewTrackingCode(int subPersonId, string code)
        {

            TrackingCode trackingCode = new TrackingCode(code, subPersonId);

            _unitOfWork.TrackingService.NewTrackingCode(trackingCode);
        }
        public void ValidateTrackingCode(string code)
        {
            ValidateUtils validateUtils = new ValidateUtils(MessageService.CodeLengthError);

            validateUtils.ValidateLength(code, size: 13);

            validateUtils.ValidateSegment(code, index: 0, size: 2, shouldBeNumber: false);

            validateUtils.ValidateSegment(code, index: 11, size: 2, shouldBeNumber: false);

            validateUtils.ValidateSegment(code, index: 2, size: 9, shouldBeNumber: true);
        }
        public List<TrackingCodeModel> GetTrackingCodesByPerson(long subPersonId)
        {
            return _unitOfWork.TrackingService.GetTrackingCodeBySubPerson((int)subPersonId).Select(x => Mappers.MapToViewModel<TrackingCode, TrackingCodeModel>(x)).ToList();
        }
        public List<TrackingCodeModel> GetTrackingCodesByPersonAndTrackingCodeEventsNotNull(long subPersonId)
        {
            return _unitOfWork.TrackingService.GetTrackingCodeBySubPersonEvents((int)subPersonId).Select(x => Mappers.MapToViewModel<TrackingCode, TrackingCodeModel>(x)).ToList();
        }
        public List<TrackingCodeEventsModel> GetTrackinEventsById(int trackingCodeEvents)
        {
            return _unitOfWork.TrackingCodeEvents.GetTrackingEventsByTrackingCodeId(trackingCodeEvents).Select(x => Mappers.MapToViewModel<TrackingCodeEvents, TrackingCodeEventsModel>(x)).ToList();
        }
        public void RemoveAllTrackingCodeEventsBySubPerson(long personId)
        {
            List<TrackingCode> trackingCodes = _unitOfWork.TrackingService.GetTrackingCodeBySubPersonEvents((int)personId);

            foreach (TrackingCode code in trackingCodes)
            {
                List<TrackingCodeEvents> trackingCodeEvents = _unitOfWork.TrackingCodeEvents.GetTrackingEventsByTrackingCodeId(code.TrackingCodeId);

                foreach (TrackingCodeEvents events in trackingCodeEvents)
                {
                    _unitOfWork.TrackingCodeEvents.DeleteTrackingEvents(events);
                }

                _unitOfWork.TrackingService.DeleteTrackingCode(code);
            }
        }
        public void DeleteTrackingCodeById(long trackingCodeId)
        {
            _unitOfWork.TrackingService.DeleteTrackingCodeById(trackingCodeId);
        }
    }
}
