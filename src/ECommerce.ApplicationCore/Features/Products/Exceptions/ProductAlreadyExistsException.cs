﻿using ECommerce.ApplicationCore.Shared.Exceptions;

namespace ECommerce.ApplicationCore.Features.Products.Exceptions;

public class ProductAlreadyExistsException : AlreadyExistsException
{
    public ProductAlreadyExistsException(string productName) : base($"Product with name `{productName}` already exists.") { }
}