#!/bin/sh
dotnet ef migrations add $1 --startup-project '..\src\API\ECommerce.API\' --project '..\src\Infrastructure\ECommerce.Infrastructure\' --output-dir 'Persistence\Migrations\'