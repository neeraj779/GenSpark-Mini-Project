﻿using System.Runtime.Serialization;

namespace StudentManagementAPI.Exceptions
{
    [Serializable]
    internal class NoSuchUserException : Exception
    {
        string _message;
        public NoSuchUserException()
        {
            _message = "Uh oh! No such user found!";
        }

        public override string Message => _message;
    }
}