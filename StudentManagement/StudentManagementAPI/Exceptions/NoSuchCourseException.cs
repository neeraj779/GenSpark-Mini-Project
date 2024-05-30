namespace StudentManagementAPI.Exceptions
{
    [Serializable]
    public class NoSuchCourseException : Exception
    {
        string _message;
        public NoSuchCourseException()
        {
            _message = "No such course found!";
        }
        public override string Message => _message;

    }
}