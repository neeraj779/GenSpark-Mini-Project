namespace StudentManagementAPI.Exceptions
{
    [Serializable]
    public class NoStudentFoundException : Exception
    {
        string _message;
        public NoStudentFoundException()
        {
            _message = "Uh oh! No student found!";
        }

        public override string Message => _message;
    }
}