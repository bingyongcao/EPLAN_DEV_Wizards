# Global Rules

## Code Style

### Naming Conventions

| Element | Convention | Example |
|---------|------------|---------|
| Classes | PascalCase | `UserService`, `DataProcessor` |
| Functions/Methods | PascalCase | `GetUserById`, `ProcessData` |
| Member Variables | `m_` prefix + PascalCase | `m_UserCount`, `m_IsActive` |
| Static Variables | `s_` prefix + PascalCase | `s_Instance`, `s_DefaultConfig` |
| Constants | SCREAMING_SNAKE_CASE | `MAX_RETRY_COUNT`, `DEFAULT_TIMEOUT` |
| Local Variables | camelCase | `userCount`, `isValid` |
| Parameters | camelCase | `userId`, `configOptions` |

### Formatting

- **Braces**: Allman style for function and class definitions
  ```cpp
  void ProcessData()
  {
      // implementation
  }
  ```
- **Indentation**: Use consistent indentation (spaces preferred over tabs)
- **Line Length**: Keep lines under 120 characters when practical
- **Blank Lines**: Use single blank lines to separate logical sections

## Architecture

### Design Patterns

- **Repository Pattern**: Use repositories to abstract data access logic
- **Dependency Injection**: Prefer constructor injection for dependencies
- **Single Responsibility**: Each class/function should have one clear purpose

### Code Organization

- Keep related functionality grouped together
- Separate concerns into appropriate layers (presentation, business logic, data access)
- Use interfaces to define contracts between components

## Requirements

### Performance

- Optimize for performance-critical paths
- Avoid unnecessary allocations in hot loops
- Profile before optimizing—measure, don't guess
- Prefer stack allocation over heap when appropriate

### Quality Standards

- Write self-documenting code with clear intent
- Add comments for complex algorithms or non-obvious decisions
- Handle errors gracefully—never silently swallow exceptions
- Write testable code with clear inputs and outputs

### Best Practices

- Fail fast: Validate inputs early
- Keep functions small and focused
- Prefer composition over inheritance
- Use meaningful names that reveal intent