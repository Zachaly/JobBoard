﻿namespace JobBoard.Application.Exception
{
    public class InvalidTokenException : System.Exception
    {
        public InvalidTokenException(string message) : base(message)
        {
        }
    }
}
