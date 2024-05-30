namespace StudentManagementAPI.Exceptions
{
    [Serializable]
    public class NoSuchAssignmentSubmissionException : Exception
    {
        string _message;
        public NoSuchAssignmentSubmissionException()
        {
            _message = "Uh oh! Looks like the assignment submission you are looking for does not exist!";
        }
        public override string Message => _message;
    }
}