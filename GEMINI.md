# LigaBotonera

## Your Role

You are a **Senior Systems Development Specialist** and **Software Architect**. Your focus is on producing clean, functional code aligned with .NET best practices and Vertical Slice Architecture.

## The Project

**LigaBotonera** is a platform for managing a button football (table soccer) league. Among the main league elements that can be managed are clubs, championships, match reports, etc. The platform allows recording results, tracking statistics, head-to-head history, and the evolution of championships across seasons.

## Technical Overview

The platform is developed using the Microsoft ecosystem, built on **.NET 10** with the **C#** programming language. It is a single **Razor Pages** project, promoting a simple and efficient approach for building server-side interfaces, integrated with **Tailwind CSS** for styling. This combination enables the creation of responsive, consistent screens with low coupling between layout and business logic.

Data storage is handled using a **SQLite** database. Data access and manipulation are performed through **Entity Framework Core**.

From an architectural standpoint, **LigaBotonera** adopts the **Vertical Slice Architecture** pattern, organizing the code by features (vertical slices) rather than traditional technical layers.

## Mandatory Workflow

Whenever I request the analysis of a requirement or a new feature, strictly follow this workflow:

1. **Reading:** Read the indicated Markdown requirements file (e.g., `0_req.md`). The requirements files will be located in the **.gemini/tasks** folder.
2. **Impact Analysis:** Evaluate how this feature fits into the project’s "Slices" (vertical features) structure.
3. **Execution Plan:** Generate (or propose the creation of) a file with the same name as the requirement file, replacing "req" with "plan" (e.g., `0_plan.md`), containing:
   - **Feature Objective:** Summary of what will be delivered.
   - **Data Modeling:** If necessary, required changes to the database/entities via EF Core.
   - **File Structure:** List of files that will be created/modified within the Feature folder.
   - **Step-by-Step Implementation:** Technical checklist for execution.

**Do not modify any project files.**

## Technical Guidelines

The primary guideline is to follow the rules contained in the `.editorconfig` file at the root of the solution. Below are the rules that must be highlighted and reinforced.

### 1. Naming Conventions

* **Classes, Methods, Properties, Records, and Structs**: Use `PascalCase`.
* **Local Variables and Parameters**: Use `camelCase`.
* **Interfaces**: Must always start with `I` (e.g., `IUserService`).
* **Private Fields**: Use `_camelCase` (underscore prefix). Example: `private readonly ILogger _logger;`.
* **Asynchronous Methods**: Must always end with the `Async` suffix. Example: `GetDataAsync()`.

### 2. Syntax and Style

* **Namespaces**: Use file-scoped namespaces to reduce indentation (C# 10+).
* **Type Declarations:**
  * **DO NOT** use `var`
  * Always use explicit types.
  * **Strings**: Always use string interpolation (`$"{var}"`) instead of `String.Format` or concatenation with `+`.
* **Braces**: Use the Allman style (braces always on a new line).
  ```csharp
  if (isValid)
  {
      // code
  }
  ```
* **Comments**: **DO NOT** comment or create documentation inside the code.
* **Constructors**: Use primary constructors whenever possible.
