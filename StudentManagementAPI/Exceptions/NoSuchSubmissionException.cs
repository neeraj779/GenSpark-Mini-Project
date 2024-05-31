namespace StudentManagementAPI.Exceptions
{
    [Serializable]
    public class NoSuchSubmissionException : Exception
    {
        string _message;
        public NoSuchSubmissionException()
        {
            _message = "No such submission found!";
        }

        public override string Message => _message;

    }
}