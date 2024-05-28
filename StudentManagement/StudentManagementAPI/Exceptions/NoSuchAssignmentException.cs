﻿using System.Runtime.Serialization;

namespace StudentManagementAPI.Exceptions
{
    [Serializable]
    internal class NoSuchAssignmentException : Exception
    {
        string _message;
        public NoSuchAssignmentException()
        {
            _message = "No such assignment found!";
        }
        public override string Message => _message;
    }
}