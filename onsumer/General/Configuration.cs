namespace Consumer.General
{
    public class Configuration
    {
        // RabbitMQ configuration.

        public static readonly string HostName = "localhost";

        public static readonly string UserName = "guest";

        public static readonly string Password = "guest";

        public static readonly int Port = 5672;

        public static readonly string BookQueue = "BookQueue";

        // Source link.

        public static readonly string SourceEndPoint = "https://localhost:44358/";

    }
}
