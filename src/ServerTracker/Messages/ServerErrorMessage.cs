using ServerTracker.Data.Models;

namespace ServerTracker.Messages
{
    public class ServerErrorMessage
    {
        public static ServerErrorMessage FromServiceError(ServiceError error)
        {
            return new ServerErrorMessage
            {
                Message = error.Message,
            };
        }

        public string Type { get; set; } = "ERR_SERVER";

        public string Message { get; set; }
    }
}
