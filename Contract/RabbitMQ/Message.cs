using ENUM;

namespace Contract.RabbitMQ
{
    public class Message
    {
        public long Id;

        public OperationType OperationType;

        public DirtyEntityType DirtyEntityType;

    }
}
