# Agent Coding Config

A centralized configuration repository for the Antigravity agent. This project defines the global rules, specialized skills, and automated workflows that guide the agent's behavior, ensuring consistent code quality and uniform project structures across user workspaces.

## Features

### 🧠 Skills
Specialized capabilities that extend the agent's toolkit:
- **ASCII Designer** (`skills/ascii-designer`): Creates professional ASCII UI wireframes, mockups, and prototypes for software, web, and mobile interfaces.
- **Creating Skills** (`skills/create-skills`): Standards and templates for generating new agent skills, ensuring they follow the "Antigravity" structural requirements.
- **Creating Workflows** (`skills/create-workflows`): Guidelines for defining robust, step-by-step automation workflows.
- **Design Web UI** (`skills/design-webui`): Provides the single source of truth for design guidelines, design tokens, and technology choices.

### ⚡ Workflows
Automated sequences of tasks to speed up development:
- **Generate README** (`workflows/generate-readme.md`): Automatically analyzes a codebase and generates a comprehensive `README.md` file (like this one!).
- **Optimize Code** (`workflows/optimize-code.md`): Review and optimize the open file by analyzing time complexity and identifying redundant logic.

### 📏 Global Rules
- **Code Style**: Enforces naming conventions (PascalCase for classes, camelCase for local vars) and formatting (Allman braces).
- **Architecture**: Promotes patterns like Repository, Dependency Injection, and Single Responsibility.
- **Performance & Quality**: Guidelines for optimization and robust error handling.

### ⚙️ Settings
- **Deny List** (`settings/deny_list_of_commands.txt`): A list of commands that the agent is prohibited from executing.

## Project Structure

```
├── rules/               # Global rule definitions
│   └── global-rules.md
├── settings/            # Agent configuration settings
│   └── deny_list_of_commands.txt
├── skills/              # Skill definitions and resources
│   ├── ascii-designer/
│   ├── create-skills/
│   ├── create-workflows/
│   └── design-webui/
├── workflows/           # Workflow definitions (.md)
│   ├── generate-readme.md
│   └── optimize-code.md
└── README.md            # Project documentation
```

## Usage

This repository is intended to be loaded by the Antigravity agent as a configuration source.
1. **Clone** this repository to your local machine.
2. **Configure** your agent setting to point to this directory (or specific files within it) as the source of truth for rules and skills.

## Contributing

- **New Skills**: Create a new folder in `skills/` defined by `SKILL.md`.
- **New Workflows**: Add `.md` files to `workflows/` with valid YAML frontmatter.
