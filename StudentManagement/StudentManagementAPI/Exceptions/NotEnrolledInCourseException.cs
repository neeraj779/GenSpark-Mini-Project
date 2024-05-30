namespace StudentManagementAPI.Exceptions
{
    [Serializable]
    public class NotEnrolledInCourseException : Exception
    {
        string _message;
        public NotEnrolledInCourseException()
        {
            _message = "You are not enrolled in this course.";
        }

        public override string Message => _message;
    }
}
