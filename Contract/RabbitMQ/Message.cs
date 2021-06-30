using ENUM;

namespace Contract.RabbitMQ
{
    public class Message
    {
        public long id;
        public OperationType operationType;
        public DirtyEntityType dirtyEntityType;
    }
}
