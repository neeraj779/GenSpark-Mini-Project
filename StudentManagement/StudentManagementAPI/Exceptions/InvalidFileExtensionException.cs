namespace StudentManagementAPI.Services
{
    [Serializable]
    public class InvalidFileExtensionException : Exception
    {
        string _message;
        public InvalidFileExtensionException()
        {
            _message = "Invalid file only .pdf files are allowed";
        }
        public override string Message => _message;
    }
}