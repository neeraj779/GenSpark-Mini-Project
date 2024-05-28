using System.Runtime.Serialization;

namespace StudentManagementAPI.Services
{
    [Serializable]
    internal class UserNotPartOfInstitutionException : Exception
    {
        string _message;
        public UserNotPartOfInstitutionException()
        {
            _message = "User is not part of the institution!";
        }

        public override string Message => _message;
    }
}