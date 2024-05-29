﻿using System.Runtime.Serialization;

namespace StudentManagementAPI.Exceptions
{
    [Serializable]
    internal class CourseAlreadyExistsException : Exception
    {
        string _message;
        public CourseAlreadyExistsException()
        {
            _message = "Course already exists.";
        }

        public override string Message => _message;

    }
}