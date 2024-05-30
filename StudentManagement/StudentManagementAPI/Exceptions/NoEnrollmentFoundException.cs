namespace StudentManagementAPI.Exceptions
{
    [Serializable]
    public class NoEnrollmentFoundException : Exception
    {
        string _message;
        public NoEnrollmentFoundException()
        {
            _message = "No enrollment found!";
        }
        public override string Message => _message;
    }
}