using Domain;
using Domain.Interfaces;

namespace MeuContexto.EntityRepositories
{
    public class TrackingRepository : ITrackingRepository
    {
        private readonly IRepository repository;

        public TrackingRepository(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task DeleteTrackingCodeAsync(TrackingCode trackingCode)
        {
            await repository.RemoveEntityAsync(trackingCode);
        }

        public async Task DeleteTrackingCodeByIdAsync(long trackingCodeId)
        {
            TrackingCode trackingCode = await repository.GetEntityAsync<TrackingCode>(x=> x.TrackingCodeId == trackingCodeId);
            await repository.RemoveEntityAsync(trackingCode);
        }

        public async Task<List<TrackingCode>> GetTrackingCodeActiveAsync()
        {
            return await repository.GetEntityByProcedure<TrackingCode>(proc: "select * from TrackingCode where status = 0 and NextSearch <= getdate()", parameters: null);
        }

        public async Task<TrackingCode> GetTrackingCodeByCodeAsync(string code)
        {
            return await repository.GetEntityAsync<TrackingCode>(x => x.Code.Equals(code));
        }

        public async Task<List<TrackingCode>> GetTrackingCodeBySubPersonAsync(int subPersonId)
        {
            return await repository.GetEntitys<TrackingCode>(x => x.SubPersonId == subPersonId && x.Status == TrackingCodeStatus.Active && x.NextSearch <= DateTime.Now);
        }

        public async Task<List<TrackingCode>> GetTrackingCodeBySubPersonEventsAsync(int subPersonId)
        {
            var sql = $@"EXEC GetTrackingCodeBySubPersonId 
            @subPersonId = {(subPersonId != null ? $"N'{subPersonId}'" : "NULL")}";

            sql = sql.Replace("\r\n", "");

            return  await repository.GetEntityByProcedure<TrackingCode>(sql);
        }

        public async Task NewTrackingCodeAsync(TrackingCode trackingCode)
        {
            await repository.SaveEntityAsync<TrackingCode>(trackingCode);
        }


        public async Task UpdateTrackingCode(TrackingCode trackingCode)
        {
            await repository.UpdateEntityAsync<TrackingCode>(trackingCode);
        }
    }
}
