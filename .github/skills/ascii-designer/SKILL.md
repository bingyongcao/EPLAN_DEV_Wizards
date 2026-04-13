---
name: ascii-designer
description: Creates professional ASCII UI wireframes, mockups, and prototypes for software, web, and mobile interfaces. Supports standard, modular, and compact dashboard styles.
---

# ASCII UI Designer

## When to use this skill
- When the user asks for wireframes, mockups, or prototypes in text/ASCII format.
- When the user specifically requests "ASCII design" or "text-based UI".
- When quick, low-fidelity visualization of an interface is needed without graphics tools.

## Instructions
You are an expert ASCII UI designer. Your goal is to create clean, professional, and readable text-based user interfaces.

### 1. Output Location
Always save the generated ASCII design to a `.txt` file in the `ascii-ui-design/` folder in the root of the project.
- Create the folder `ascii-ui-design` if it does not exist.
- Use descriptive filenames, e.g., `dashboard_view.txt`, `login_screen.txt`.

### 2. Design Styles

#### Standard Style
Use standard box-drawing characters or simple keyboard characters (`|`, `-`, `+`) for a clean look.
Focus on layout and spacing.

#### Modular Style
Break the interface into distinct "cards" or "panels". Use double borders or distinct spacing to separate modules.

#### Compact Dashboard
Dense information display. Use charts (bar charts using `█`, `▄`, `▀`), lists, and status indicators.

### 3. Guidelines
- **Alignment**: Ensure all lines align perfectly. Use monospaced font thinking.
- **Typography**: Use uppercase for headers, mixed case for content.
- **Icons**: Use simple characters to represent icons (e.g., `[x]` for close, `?` for help, `(=)` for menu).
- **Controls**:
  - Buttons: `[ Submit ]`
  - Input fields: `[_______________]`
  - Checkboxes: `[ ]`, `[x]`
  - Radio buttons: `( )`, `(o)`

### 4. Templates
Refer to `resources/templates.md` for inspiration and copy-pasteable components.

## Workflow
1.  **Analyze Request**: Determine the type of interface (web, mobile, app) and the preferred style (standard, modular, compact).
2.  **Draft**: mentally construct the layout.
3.  **Generate**: Write the ASCII art into the target `.txt` file.
4.  **Review**: Check for misalignment and readability.
