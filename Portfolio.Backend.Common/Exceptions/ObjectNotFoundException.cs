namespace Portfolio.Backend.Common.Exceptions
{
    public class ObjectNotFoundException : Exception
    {
        public ObjectNotFoundException() : base()
        {
        }

        public ObjectNotFoundException(string msg) : base(msg)
        {

        }
    }
}

