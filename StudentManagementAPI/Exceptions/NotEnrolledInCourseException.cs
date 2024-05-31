namespace StudentManagementAPI.Exceptions
{
    [Serializable]
    public class NotEnrolledInCourseException : Exception
    {
        string _message;
        public NotEnrolledInCourseException()
        {
            _message = "Student is not enrolled in the course.";
        }

        public override string Message => _message;
    }
}
