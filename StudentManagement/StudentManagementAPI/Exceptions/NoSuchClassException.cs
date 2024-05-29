﻿using System.Runtime.Serialization;

namespace StudentManagementAPI.Exceptions
{
    [Serializable]
    internal class NoSuchClassException : Exception
    {
        string _message;
        public NoSuchClassException()
        {
            _message = "Class with given Id does not exist.";
        }

        public override string Message => _message;
    }
}