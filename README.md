Task:

Create a simple ASP.NET Core MVC application in C# to manage invoices in any database system. The application should have the following features:
* creating/editing an invoice,
* adding/removing invoice items.

Another part of the application is to create a simple API. Access to API should be restricted by a secret key which is sent as a header value in the request.

Please prepare 3 endpoints which have following functionality:
* getting collection of unpaid invoices,
* paying invoice (changing status to `paid`),
* editing invoice (PATCH request).

Solution:

# InvoiceManager
ASP.NET Core MVC, EF Core, ASP.NET Core Web API

InvoiceManager.Portal -> ASP.NET Core MVC -> creating/editing an invoice + adding/removing invoice items

InvoiceManager.
API Get -> getting collection of unpaid invoices
API Post -> paying invoice (changing status to `paid`)
API Patch -> editing invoice (PATCH request)
API Key -> appsettings.json -> "ApiKey" -> Send in HEADER as "ApiKey"

InvoiceManager.Model -> Model, DbContext, EF Core Migrations
