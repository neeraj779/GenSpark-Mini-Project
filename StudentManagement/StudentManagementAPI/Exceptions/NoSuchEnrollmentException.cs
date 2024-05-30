namespace StudentManagementAPI.Exceptions
{
    [Serializable]
    public class NoSuchEnrollmentException : Exception
    {
        string _message;
        public NoSuchEnrollmentException()
        {
            _message = "No such enrollment found!";
        }
        public override string Message => _message;

    }
}