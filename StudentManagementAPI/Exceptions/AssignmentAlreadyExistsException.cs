namespace StudentManagementAPI.Exceptions
{
    [Serializable]
    public class AssignmentAlreadyExistsException : Exception
    {
        string _message;
        public AssignmentAlreadyExistsException()
        {
            _message = "Uh oh! Assignment already exists!";
        }
        public override string Message => _message;
    }
}