Generate commit messages strictly following the Conventional Commits specification.

Format: <type>: <description>

Allowed types:
- feat: add a new feature
- fix: fix a bug
- docs: documentation-only changes
- refactor: restructure code without changing behavior
- test: add or update tests
- chore: maintenance, configuration, project setup, or tooling changes
- perf: improve performance
- style: formatting-only changes that do not affect behavior
- build: build system or dependency changes
- ci: CI/CD configuration changes

Rules:
1. Analyze the actual code changes before choosing the type.
2. Choose the most accurate type; do not default to chore.
3. Use a short, specific description in English.
4. Start the description with an imperative verb.
5. Use lowercase for the description.
6. Keep the subject line under 72 characters.
7. Do not claim changes that are not present in the diff.
8. Output only the commit message, without explanations or Markdown.
9. Use a scope only when it adds useful clarity, for example: feat(products): add product search.
10. For initial project setup and Clean Architecture structure, use chore: initialize Clean Architecture structure.