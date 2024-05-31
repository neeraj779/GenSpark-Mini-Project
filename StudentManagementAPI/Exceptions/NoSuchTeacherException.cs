namespace StudentManagementAPI.Exceptions
{
    [Serializable]
    public class NoSuchTeacherException : Exception
    {
        string _message;
        public NoSuchTeacherException()
        {
            _message = "Uh oh! No such teacher found!";
        }

        public override string Message => _message;
    }
}