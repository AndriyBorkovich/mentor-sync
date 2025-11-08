## General

* Make only high confidence suggestions when reviewing code changes.
* Always use the latest version C# and .NET 9, currently C# 13 features.
* Never change global.json unless explicitly asked to.

## Formatting

* Apply code-formatting style defined in `.editorconfig` file.
* Prefer file-scoped namespace declarations and single-line using directives.
* Insert a newline before the opening curly brace of any code block (e.g., after `if`, `for`, `while`, `foreach`, `using`, `try`, etc.).
* Ensure that the final return statement of a method is on its own line.
* Use pattern matching and switch expressions wherever possible.
* Use `nameof` instead of string literals when referring to member names.
* Ensure that XML doc comments are created for any public APIs. When applicable, include <example> and <code> documentation in the comments.

### Nullable Reference Types

* Declare variables non-nullable, and check for `null` at entry points.
* Always use `is null` or `is not null` instead of `== null` or `!= null`.
* Trust the C# null annotations and don't add null checks when the type system says a value cannot be null.
* Do not use the nullable type operator (`?`) on any reference types because .csproj file do not have <Nullable> attribute (e.g. string?).

### Async/Await
* Use `async` and `await` for all I/O-bound operations.
* Avoid using `async void` methods except for event handlers.
* Use `ConfigureAwait(false)` in library code to avoid deadlocks.
* Prefer `Task.WhenAll` for running multiple asynchronous operations in parallel.
* Avoid blocking calls like `.Result` or `.Wait()` on asynchronous methods.
* Use cancellation tokens to allow for cooperative cancellation of asynchronous operations.
* Handle exceptions in asynchronous methods using try-catch blocks or by observing the returned Task.
* Name asynchronous methods with the "Async" suffix to indicate their asynchronous nature.
