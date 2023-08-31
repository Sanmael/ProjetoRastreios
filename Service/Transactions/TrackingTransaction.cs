using Domain;
using MeuContexto.UOW;
using FrontEnd.Models;
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
        public async Task<bool> TrackingCodeExist(string code)
        {
            TrackingCode trackingCode = await _unitOfWork.TrackingService.GetTrackingCodeByCodeAsync(code);

            return trackingCode != null;
        }
        public void AddNewTrackingCode(int subPersonId, string code)
        {

            TrackingCode trackingCode = new TrackingCode(code, subPersonId);

            _unitOfWork.TrackingService.NewTrackingCodeAsync(trackingCode);
        }
        public void ValidateTrackingCode(string code)
        {
            ValidateUtils validateUtils = new ValidateUtils(MessageService.CodeLengthError);

            validateUtils.ValidateLength(code, size: 13);

            validateUtils.ValidateSegment(code, index: 0, size: 2, shouldBeNumber: false);

            validateUtils.ValidateSegment(code, index: 11, size: 2, shouldBeNumber: false);

            validateUtils.ValidateSegment(code, index: 2, size: 9, shouldBeNumber: true);
        }
        public async Task<List<TrackingCodeModel>> GetTrackingCodesByPersonAsync(long subPersonId)
        {
            return _unitOfWork.TrackingService.GetTrackingCodeBySubPersonAsync((int)subPersonId).Result.Select(x => Mappers.MapToViewModel<TrackingCode, TrackingCodeModel>(x)).ToList();
        }
        public List<TrackingCodeModel> GetTrackingCodesByPersonAndTrackingCodeEventsNotNull(long subPersonId)
        {
            return _unitOfWork.TrackingService.GetTrackingCodeBySubPersonEventsAsync((int)subPersonId).Result.Select(x => Mappers.MapToViewModel<TrackingCode, TrackingCodeModel>(x)).ToList();
        }
        public async Task<List<TrackingCodeEventsModel>> GetTrackinEventsByIdAsync(int trackingCodeEvents)
        {
            return _unitOfWork.TrackingCodeEvents.GetTrackingEventsByTrackingCodeIdAsync(trackingCodeEvents).Result.Select(x => Mappers.MapToViewModel<TrackingCodeEvents, TrackingCodeEventsModel>(x)).ToList();
        }
        public async Task RemoveAllTrackingCodeEventsBySubPersonAsync(long personId)
        {
            List<TrackingCode> trackingCodes = await _unitOfWork.TrackingService.GetTrackingCodeBySubPersonEventsAsync((int)personId);

            foreach (TrackingCode code in trackingCodes)
            {
                List<TrackingCodeEvents> trackingCodeEvents = await _unitOfWork.TrackingCodeEvents.GetTrackingEventsByTrackingCodeIdAsync(code.TrackingCodeId);

                foreach (TrackingCodeEvents events in trackingCodeEvents)
                {
                    _unitOfWork.TrackingCodeEvents.DeleteTrackingEventsAsync(events);
                }

                _unitOfWork.TrackingService.DeleteTrackingCodeAsync(code);
            }
        }
        public void DeleteTrackingCodeById(long trackingCodeId)
        {
            _unitOfWork.TrackingService.DeleteTrackingCodeByIdAsync(trackingCodeId);
        }
    }
}
