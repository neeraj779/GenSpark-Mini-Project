using System.Runtime.Serialization;

namespace StudentManagementAPI.Exceptions
{
    [Serializable]
    internal class NoCourseOfferingException : Exception
    {
        string _message;
        public NoCourseOfferingException()
        {
            _message = "Uh oh! No course offering found!";
        }

        public override string Message => _message;
    }
}