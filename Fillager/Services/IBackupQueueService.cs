namespace Fillager.Services
{
    public interface IBackupQueueService
    {
        void SendBackupRequest(string bucketName, string fileGuid);
    }
}