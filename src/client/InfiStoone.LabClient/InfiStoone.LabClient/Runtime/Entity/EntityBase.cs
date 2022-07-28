using System; 

namespace InfiStoone.LabClient.Runtime.Entity
{
    public abstract class EntityBase<T>
    {
        public T Id { get; set; }
    }
    public abstract class EntityBase:EntityBase<int>
    { 
    }
}
