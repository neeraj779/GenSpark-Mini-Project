namespace StudentManagementAPI.Exceptions
{
    [Serializable]
    public class InvalidRoleException : Exception
    {
        string _message;
        public InvalidRoleException()
        {
            _message = "Invalid role!";
        }

        public override string Message => _message;
    }
}