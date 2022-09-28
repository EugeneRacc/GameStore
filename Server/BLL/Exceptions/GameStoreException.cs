using System.Runtime.Serialization;

namespace BLL.Exceptions
{
    [Serializable]
    public class GameStoreException : Exception
    {
        public GameStoreException()
        {
        }

        public GameStoreException(string message)
            : base(message)
        {
        }

        public GameStoreException(string message, Exception inner)
            : base(message, inner)
        {
        }
        protected GameStoreException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
