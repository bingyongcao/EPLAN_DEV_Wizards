---
description: Generates a comprehensive README.md file by analyzing the codebase structure, dependencies, and entry points.
---

1. List the contents of the root directory using `list_dir` to identify project type, frameworks, and key configuration files (e.g., `package.json`, `requirements.txt`, `Cargo.toml`, `pom.xml`).
2. Read the identified configuration files and main entry points (e.g., `main.py`, `index.js`, `src/main.rs`) to understand the project's name, description, dependencies, and build/run scripts.
3. Formulate a professional `README.md` content that includes:
   - **Project Name & Description**: Clear summary of what the project does.
   - **Installation**: Step-by-step instructions (e.g., `npm install`, `pip install`).
   - **Usage**: Commands to run the project (e.g., `npm start`, `python main.py`).
   - **Features**: Key capabilities of the software.
   - **Project Structure**: High-level overview of the main directories.
4. Save the content to `README.md` using the `write_to_file` tool.
