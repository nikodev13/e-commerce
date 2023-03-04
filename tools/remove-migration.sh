#!/bin/sh
dotnet ef migrations remove --startup-project '..\src\ECommerce.API\' --project '..\src\ECommerce.Infrastructure\'