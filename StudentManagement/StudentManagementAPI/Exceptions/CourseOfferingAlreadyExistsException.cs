namespace StudentManagementAPI.Exceptions
{
    [Serializable]
    public class CourseOfferingAlreadyExistsException : Exception
    {
        string _message;
        public CourseOfferingAlreadyExistsException()
        {
            _message = "Course offering already exists!";
        }

        public override string Message => _message;
    }
}