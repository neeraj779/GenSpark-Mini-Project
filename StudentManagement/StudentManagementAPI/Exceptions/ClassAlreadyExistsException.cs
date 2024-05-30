namespace StudentManagementAPI.Exceptions
{
    [Serializable]
    public class ClassAlreadyExistsException : Exception
    {
        string _message;
        public ClassAlreadyExistsException()
        {
            _message = "Class already exists!";
        }

        public override string Message => _message;

    }
}