using System.Runtime.Serialization;

namespace StudentManagementAPI.Exceptions
{
    [Serializable]
    internal class DuplicateAssignmentSubmissionException : Exception
    {
        string _message;
        public DuplicateAssignmentSubmissionException()
        {
            _message = "You have already submitted this assignment.";
        }
        public override string Message => _message;
    }
}