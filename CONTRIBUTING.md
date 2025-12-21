# Contributing to Windows System Manager

First off, thank you for considering contributing to Windows System Manager! 🎉

## Table of Contents

- [Code of Conduct](#code-of-conduct)
- [How Can I Contribute?](#how-can-i-contribute)
  - [Reporting Bugs](#reporting-bugs)
  - [Suggesting Enhancements](#suggesting-enhancements)
  - [Pull Requests](#pull-requests)
- [Development Setup](#development-setup)
- [Style Guidelines](#style-guidelines)
- [Commit Messages](#commit-messages)

## Code of Conduct

This project and everyone participating in it is governed by our commitment to creating a welcoming environment. Please be respectful and constructive in all interactions.

## How Can I Contribute?

### Reporting Bugs

Before creating bug reports, please check existing issues to avoid duplicates.

**When reporting a bug, include:**

- **Clear title** describing the issue
- **Steps to reproduce** the behavior
- **Expected behavior** vs actual behavior
- **Screenshots** if applicable
- **Environment details:**
  - Windows version
  - .NET version
  - Application version

### Suggesting Enhancements

Enhancement suggestions are welcome! Please provide:

- **Clear title** for the feature
- **Detailed description** of the proposed functionality
- **Use case** - why this would be useful
- **Mockups** if applicable (for UI changes)

### Pull Requests

1. **Fork** the repository
2. **Create** a feature branch from `main`:
   ```bash
   git checkout -b feature/your-feature-name
   ```
3. **Make** your changes
4. **Test** your changes thoroughly
5. **Commit** with clear messages
6. **Push** to your fork
7. **Open** a Pull Request

## Development Setup

### Prerequisites

- Windows 10/11
- Visual Studio 2022 (or later)
- .NET 8.0 SDK
- Git

### Getting Started

```bash
# Clone your fork
git clone https://github.com/YOUR_USERNAME/WindowsSystemManager.git
cd WindowsSystemManager

# Add upstream remote
git remote add upstream https://github.com/FurkanKAYAPINAR/WindowsSystemManager.git

# Install dependencies
dotnet restore

# Build
dotnet build

# Run
dotnet run
```

### Project Structure

```
WindowsSystemManager/
├── App.xaml              # Application resources and startup
├── App.xaml.cs           # Application logic
├── MainWindow.xaml       # Main UI definition
├── MainWindow.xaml.cs    # Main window logic
├── app.manifest          # Admin privileges manifest
├── app.ico               # Application icon
└── WindowsSystemManager.csproj
```

## Style Guidelines

### C# Code Style

- Use **PascalCase** for public members
- Use **camelCase** for private fields (prefix with `_`)
- Use **async/await** for asynchronous operations
- Add XML documentation for public methods
- Keep methods focused and under 50 lines when possible

```csharp
// Good example
private readonly ObservableCollection<ServiceItem> _services;

public async Task LoadServicesAsync()
{
    // Implementation
}
```

### XAML Style

- Use consistent indentation (4 spaces)
- Group related properties
- Use meaningful `x:Name` values
- Extract reusable styles to `App.xaml`

### General Guidelines

- Write self-documenting code
- Add comments for complex logic
- Handle exceptions appropriately
- Follow SOLID principles

## Commit Messages

Use clear, descriptive commit messages:

```
<type>: <subject>

<optional body>
```

### Types

- `feat`: New feature
- `fix`: Bug fix
- `docs`: Documentation changes
- `style`: Code style changes (formatting, etc.)
- `refactor`: Code refactoring
- `test`: Adding tests
- `chore`: Maintenance tasks

### Examples

```
feat: Add system tray support with context menu

fix: Resolve service controller timeout issue

docs: Update README with new screenshots

refactor: Extract service operations to separate class
```

---

## Questions?

Feel free to open an issue with your question or reach out to the maintainers.

Thank you for contributing! 🚀
