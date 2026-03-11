---
applyTo: "**"
---

# Prompt Logging Rule

## Rule: Save Every Prompt as a Separate File

After **every** Copilot interaction that results in a plan, code generation, or significant decision, save the prompt as a new file in:

```
E:\Dev\GodelAI\Projects\ai-pr-review-assistant\prompts\
```

---

## File Naming Convention

```
{IDENTITY_NUMBER}_{SHORT_KEY}.md
```

| Part              | Description                                              | Example       |
|-------------------|----------------------------------------------------------|---------------|
| `IDENTITY_NUMBER` | Sequential integer, no padding                           | `1`, `12`     |
| `SHORT_KEY`       | Hyphen-separated lowercase words describing the prompt   | `setup-auth-backend` |

**Valid examples:**
```
1_initial-project-scaffold.md
2_add-review-entity.md
3_implement-jwt-refresh.md
14_dockerize-frontend.md
```

**Rules:**
- Never reuse numbers.
- Numbers are global across the entire `prompts/` folder (not per-feature).
- Determine the next number by counting existing files in `prompts/` (excluding `_prompt-log-template.md`).
- `SHORT_KEY` must be meaningful and unique enough to identify the prompt at a glance.

---

## File Content

Use the template at `E:\Dev\GodelAI\Projects\ai-pr-review-assistant\prompts\_prompt-log-template.md`.

Copy the template, replace all `{PLACEHOLDER}` values, then fill in:

1. **Metadata** — number, key, date, mode, model, task reference
2. **Context Provided** — files loaded into context
3. **Prompt** — full prompt text (verbatim)
4. **Response Summary** — bullet points of key outputs/decisions
5. **Files Created / Modified** — table of affected files
6. **Outcome** — accepted / edited / rejected
7. **Notes** — any issues or follow-ups

---

## When to Log

Log a prompt when it:
- Requests implementation of a feature, component, or layer
- Produces a plan that was approved
- Results in file creation or modification
- Resolves a bug or architectural question

Do NOT log:
- Clarification questions with no code output
- Read-only lookups ("show me the contents of X")

---

## Enforcement

This rule complements the coding workflow in `project-requarements/4-ai-generation-rules.md`.  
Step 6 of the workflow (Log the prompt) refers to this convention.
