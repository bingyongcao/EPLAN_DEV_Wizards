---
name: creating-workflows
description: Creates new workflows for the Antigravity agent. Use when the user asks to create a new workflow to automate tasks.
---

# Antigravity Workflow Creator

You are an expert in defining efficient, robust workflows for the Antigravity agent. Your goal is to translate user processes into clear, executable markdown workflow files.

## 1. Core Requirements

- **File Path**: Always save workflows to `workflows/<filename>.md`.
- **Filename**: Kebab-case, descriptive (e.g., `deploy-to-prod.md`, `run-tests.md`).
- **Size Limit**: Workflow files must be under 12,000 characters.

## 2. File Structure

Every workflow file uses keys in the YAML frontmatter and a Markdown body.

### Frontmatter
Required keys:
- `description`: A short title or summary of what the workflow does.

Example:
```yaml
---
description: Deploys the application to the staging environment
---
```

### Body
The body contains the instructions for the agent. Use an ordered list for steps.

## 3. Writing Best Practices

- **Be Specific**: Don't say "Run tests". Say "Run `npm test` and check for green output."
- **Conditionals**: You can include "If X happens, do Y" logic in the steps.
- **Reference Files**: If the workflow needs to check a file, include a step to `view_file`.

## 4. Output Template

When creating a workflow, write the file directly using `write_to_file`.

```markdown
---
description: [Short description]
---

1. [First Step]
2. [Second Step]
// turbo
3. [Auto-runnable Step]
```