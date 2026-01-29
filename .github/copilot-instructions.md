# Copilot Instructions for MvcMovieNet6

## Project Overview

Multi-project solution containing:
- **MvcMovie** - ASP.NET Core MVC application
- **RazorMovie** - ASP.NET Core Razor Pages application (prioritize Razor Pages patterns)
- **WpfMovie** - WPF Windows desktop application
- **Test Projects** - NUnit (MvcMovie, WpfMovie) and MSTest (RazorMovie)

## Target Framework

- Target: .NET 10.0 (net10.0)
- WPF projects: net10.0-windows

## Code Style

- Use file-scoped namespaces
- Use 4 spaces for indentation (no tabs)
- Use `var` when type is obvious from the right side
- Use nullable reference types
- Prefer primary constructors for dependency injection
- Use collection expressions: `[item1, item2]`

## Naming Conventions

- Private fields: `_camelCase`
- Public properties: `PascalCase`
- Async methods: suffix with `Async`
- Interfaces: prefix with `I`

## ASP.NET Core (Razor Pages - Preferred)

When working with web projects, prioritize Razor Pages patterns:
- Use PageModel for code-behind logic
- Prefer handler methods (`OnGet`, `OnGetAsync`, `OnPost`, `OnPostAsync`)
- Use Tag Helpers over HTML Helpers
- Use `@page` directive for routing

## ASP.NET Core (MVC)

- Use async controller actions
- Return `IActionResult` or specific result types
- Use attribute routing
- Prefer ViewModels over passing entities to views

## Entity Framework Core

- Use async methods (`ToListAsync`, `FirstOrDefaultAsync`, `SaveChangesAsync`)
- Configure entities with Fluent API in `OnModelCreating`
- Use migrations for schema changes
- Prefer `IQueryable` for deferred execution

## WPF

- Use MVVM pattern
- Prefer data binding over code-behind
- Use `ObservableCollection<T>` for bindable collections
- Use modern serialization (`System.Text.Json`) - avoid `BinaryFormatter`

## Error Handling

- Use `ILogger<T>` for logging
- Handle exceptions at appropriate levels
- Use ProblemDetails for API error responses

## Testing

### NUnit (MvcMovie.Tests, WpfMovie.Tests)
```csharp
[Test]
public void MethodName_Scenario_ExpectedResult()
{
    // Arrange
    // Act
    // Assert
}
```

### MSTest (RazorMovie.Tests)
```csharp
[TestMethod]
public void MethodName_Scenario_ExpectedResult()
{
    // Arrange
    // Act
    // Assert
}
```

- Follow Arrange-Act-Assert pattern
- Use Moq for mocking dependencies
- One logical assertion per test

## Security

- Always validate and sanitize user input
- Use HtmlSanitizer for user-generated HTML content
- Keep packages updated for security patches
- Use parameterized queries (EF Core handles this automatically)

## Async/Await

- Prefer `async/await` over `.Result` or `.Wait()`
- Use `ConfigureAwait(false)` in library code
- Suffix async methods with `Async`
