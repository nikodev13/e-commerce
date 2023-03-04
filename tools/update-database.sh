#!/bin/sh
dotnet ef database update --startup-project '..\src\ECommerce.API' --project '..\src\ECommerce.Infrastructure'