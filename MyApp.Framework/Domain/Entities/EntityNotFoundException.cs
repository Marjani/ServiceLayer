using System;
using System.Runtime.Serialization;

namespace MyApp.Framework.Domain.Entities
{
    [Serializable]
    public class EntityNotFoundException : Exception
    {
        #region Constructors

        public EntityNotFoundException()
        {
        }

        public EntityNotFoundException(string message)
            : base(message)
        {
        }

        public EntityNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public EntityNotFoundException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {
        }

        public EntityNotFoundException(string entityName, long id, Exception innerException = null)
            : base($"Entity: {entityName}, id: {id}", innerException)
        {
        }

        #endregion
    }
}