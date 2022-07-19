namespace Petroineos.Coding.Challenge.Service.Interfaces
{
    public interface IReportingService
    {
        Task GenerateReportAsync(DateTime dateTime);
    }
}