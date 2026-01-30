# MvcMovieSampleNet10

This repository hosts a .NET 10 web app derived from the ASP.NET Core MVC movie sample. It demonstrates fundamental practices for building an MVC application, including models, views, and controllers.

## Purpose

This project explores potential upgrade challenges when moving from .NET 6 to newer versions. It includes modifications to exhibit various learning opportunities and ensure a smoother transition to future .NET releases. **This project has been upgraded to .NET 10.**

## Getting Started

1. Clone the repository.
2. Restore dependencies with `dotnet restore`.
3. Run the app using `dotnet run`.
4. Visit the provided URL in your browser.

## Notes

* This project is based on the original ASP.NET Core MVC movie sample from Microsoft.  
* Minimal changes have been introduced to learn about upgrade issues and best practices.

## Solution structure

1. MvcMovie: an ASP.NET Core 10.0 MVC web app. This app performs CRUD operations on the `Movie` model in SQL Server.
1. MvcMovie.Tests: an nUnit test project for the MVC web app.
1. RazorMovie: an ASP.NET Core 10.0 Razor Pages web app. This app uses HtmlSanitizer.
1. RazorMovie.Tests: an MSTest project for the Razor web app.
1. WpfMovie: a Windows Presentation Framework app that presents a form for editing in-memory `Movie` models.
1. WpfMovie.Tests: an nUnit test project for the WPF project.

## Interesting upgrade scenarios

The following scenarios were addressed during the .NET 10 upgrade:

1. **BinaryFormatter Removal**: The WpfMovie project used BinaryFormatter which was removed from .NET 9. It has been replaced with System.Text.Json for serialization.
1. **Target Framework Monikers**: The upgrade correctly chose net10.0 for standard .NET applications and net10.0-windows for the WPF project. The WpfMovie.Tests project retained the OS-specific TFM (net10.0-windows).
1. **HtmlSanitizer Namespace Change**: The HtmlSanitizer reference in the RazorMovie project was upgraded from 7.1.542 to 9.0.0, which involved a namespace change from `Ganss.XSS` to `Ganss.Xss`.
1. **Entity Framework Core Upgrade**: The MvcMovie project was upgraded from EF Core 6.0 to 10.0.2, including updating Microsoft.Data.SqlClient to address transitive dependencies.
1. **NUnit 4 Breaking Changes**: The upgrade to NUnit 4 required changing assertion syntax from `Assert.AreEqual` to `Assert.That` with constraint-based assertions.
1. **Cross-platform Build Support**: The EnableWindowsTargeting property was added to WPF projects to allow building on non-Windows platforms.