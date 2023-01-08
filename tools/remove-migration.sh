#!/bin/sh
dotnet ef migrations remove --startup-project '..\src\API\ECommerce.API\' --project '..\src\Infrastructure\ECommerce.Infrastructure\'