namespace StudentManagementAPI.Exceptions
{
    [Serializable]
    public class NoAssignmentFoundException : Exception
    {
        string _message;
        public NoAssignmentFoundException()
        {
            _message = "No assignment found!";
        }

        public override string Message => _message;
    }
}