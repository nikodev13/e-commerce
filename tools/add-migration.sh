#!/bin/sh
dotnet ef migrations add "$1" --startup-project '..\src\ECommerce.API' --project '..\src\ECommerce.Infrastructure' --output-dir 'Persistence\Migrations' 