﻿namespace ECommerce.Domain.Shared.Exceptions;

public class DomainException : Exception
{
    public DomainException(string message) : base(message)
    {
        
    }
}