﻿namespace StudentManagementAPI.Exceptions
{
    [Serializable]
    public class UserNotActiveException : Exception
    {
        string _message;
        public UserNotActiveException()
        {
            _message = "Uh oh! User is not active please contact admin!";
        }

        public override string Message => _message;
    }
}