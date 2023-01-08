#!/bin/sh
dotnet ef database update --startup-project '..\src\API\ECommerce.API' --project '..\src\Infrastructure\ECommerce.Infrastructure'