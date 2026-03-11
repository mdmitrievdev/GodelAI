using NUlid;
using PRReviewAssistant.API.Domain.Entities;
using PRReviewAssistant.API.Domain.Enums;
using PRReviewAssistant.API.Domain.Interfaces;

namespace PRReviewAssistant.API.Infrastructure.Services;

public sealed class MockAiAnalysisService : IAiAnalysisService
{
    private const int MinDelayMs = 200;
    private const int MaxDelayMs = 800;

    private record FindingTemplate(
        FindingCategory Category,
        FindingSeverity Severity,
        string Title,
        string Description,
        string Suggestion,
        string? SuggestedFix = null);

    private static readonly IReadOnlyList<FindingTemplate> _genericTemplates =
    [
        new(FindingCategory.Security, FindingSeverity.Critical,
            "Potential SQL Injection Vulnerability",
            "User input is concatenated directly into a query string without sanitization or parameterization.",
            "Use parameterized queries or an ORM to prevent SQL injection attacks.",
            "// Parameterized query example:\nvar result = db.Execute(\"SELECT * FROM users WHERE id = @id\", new { id });"),

        new(FindingCategory.Bug, FindingSeverity.Critical,
            "Null Reference Risk",
            "An object is dereferenced without a prior null check, which may cause a NullReferenceException at runtime.",
            "Add a null guard before accessing the object's members.",
            "if (obj is null) throw new ArgumentNullException(nameof(obj));"),

        new(FindingCategory.Performance, FindingSeverity.Warning,
            "Nested Loop with O(n²) Complexity",
            "A nested loop structure results in quadratic or higher complexity, which degrades performance for large inputs.",
            "Consider a hash-set lookup or sorting-based approach to achieve O(n log n) or O(n) complexity.",
            "// Use a HashSet for O(1) lookups:\nvar set = new HashSet<T>(collection1);\nvar common = collection2.Where(x => set.Contains(x));"),

        new(FindingCategory.CodeStyle, FindingSeverity.Info,
            "Magic Number Without Named Constant",
            "A literal numeric value appears directly in the code without an explanatory constant.",
            "Extract the literal into a named constant to improve readability and maintainability.",
            "private const int SecondsPerDay = 86_400;"),

        new(FindingCategory.Naming, FindingSeverity.Info,
            "Variable Name Lacks Descriptiveness",
            "The variable name is too short or generic (e.g. 'x', 'temp', 'data') and does not convey its purpose.",
            "Rename to a noun or noun phrase that describes the value it holds.",
            null),

        new(FindingCategory.BestPractice, FindingSeverity.Warning,
            "Empty Catch Block Silently Swallows Exceptions",
            "A catch block with no body suppresses exceptions, making failures invisible and hard to debug.",
            "At minimum, log the exception. If intentionally ignored, document the reason explicitly.",
            "catch (Exception ex)\n{\n    _logger.LogWarning(ex, \"Expected recoverable error.\");\n}"),

        new(FindingCategory.Security, FindingSeverity.Warning,
            "Sensitive Data Logged or Exposed",
            "A password, token, or key appears to be included in a log statement or error response.",
            "Remove sensitive values from logs and error messages. Use redacted placeholders for audit trails.",
            null),

        new(FindingCategory.Performance, FindingSeverity.Info,
            "Repeated Property Access in Loop",
            "A property or method is accessed on every iteration of a loop instead of being cached in a local variable.",
            "Hoist the value into a local variable before the loop to avoid repeated evaluations.",
            "var count = items.Count;\nfor (var i = 0; i < count; i++) { ... }"),

        new(FindingCategory.BestPractice, FindingSeverity.Info,
            "Method Exceeds Recommended Length",
            "The method body spans more than 40 lines, making it harder to test and understand in isolation.",
            "Extract logical sub-steps into private helper methods with descriptive names.",
            null),

        new(FindingCategory.Bug, FindingSeverity.Warning,
            "Off-by-One Error in Boundary Condition",
            "A loop bound or array index uses < where <= (or vice versa) is appropriate, leading to missed or extra iterations.",
            "Review the boundary condition against the algorithm intent and add a unit test for the edge value.",
            null),

        new(FindingCategory.CodeStyle, FindingSeverity.Info,
            "Inconsistent Brace / Indentation Style",
            "Some blocks use a different brace placement or indentation level than the surrounding code.",
            "Apply a consistent formatter (e.g. dotnet-format, Prettier) to the entire file.",
            null),

        new(FindingCategory.Naming, FindingSeverity.Warning,
            "Boolean Variable Name Does Not Express a Predicate",
            "The boolean variable name (e.g. 'flag', 'status') does not communicate a true/false question.",
            "Rename to a predicate form: isActive, hasPermission, canRetry.",
            null),
    ];

    private static readonly IReadOnlyDictionary<string, IReadOnlyList<FindingTemplate>> _languageTemplates =
        new Dictionary<string, IReadOnlyList<FindingTemplate>>(StringComparer.OrdinalIgnoreCase)
        {
            ["C#"] =
            [
                new(FindingCategory.Performance, FindingSeverity.Warning,
                    "Synchronous I/O Blocks Thread Pool",
                    "A blocking synchronous call (.Result, .Wait()) is made inside an async method, risking deadlocks under ASP.NET Core.",
                    "Replace with await. Never block an async method with .Result or .Wait().",
                    "var result = await repository.GetAsync(id, ct);"),

                new(FindingCategory.BestPractice, FindingSeverity.Info,
                    "IDisposable Not Disposed",
                    "An object implementing IDisposable is created but not wrapped in a using statement.",
                    "Wrap in a using declaration or using block to ensure deterministic resource disposal.",
                    "await using var stream = new FileStream(path, FileMode.Open);"),

                new(FindingCategory.Security, FindingSeverity.Critical,
                    "Unvalidated Input Passed to Shell Command",
                    "User-supplied input is forwarded to Process.Start or similar without sanitization.",
                    "Validate and whitelist the input. Prefer managed API-based alternatives over shell commands.",
                    null),

                new(FindingCategory.CodeStyle, FindingSeverity.Info,
                    "Use Pattern Matching Instead of Type Cast",
                    "The code uses 'as' followed by a null check instead of the idiomatic 'is' pattern.",
                    "Replace 'obj as T' + null check with 'if (obj is T typed)'.",
                    "if (obj is MyType typed)\n{\n    typed.DoSomething();\n}"),

                new(FindingCategory.BestPractice, FindingSeverity.Warning,
                    "CancellationToken Not Propagated",
                    "An async method accepts a CancellationToken but does not forward it to inner async calls.",
                    "Pass the CancellationToken to all downstream async operations.",
                    null),
            ],

            ["TypeScript"] =
            [
                new(FindingCategory.Security, FindingSeverity.Critical,
                    "XSS Risk via innerHTML Assignment",
                    "User-supplied content is assigned to innerHTML without sanitization, enabling script injection.",
                    "Use textContent for plain text or a sanitization library (e.g. DOMPurify) before assigning HTML.",
                    "element.textContent = userInput; // safe alternative"),

                new(FindingCategory.Bug, FindingSeverity.Warning,
                    "Unhandled Promise Rejection",
                    "An async function is called without await or .catch(), so rejections are silently swallowed.",
                    "Await the call inside a try/catch or attach a .catch() handler.",
                    "try {\n  const result = await fetchData();\n} catch (err) {\n  handleError(err);\n}"),

                new(FindingCategory.Naming, FindingSeverity.Info,
                    "Use of 'any' Type Disables Type Safety",
                    "A variable or parameter is typed as 'any', bypassing TypeScript's type checking entirely.",
                    "Replace 'any' with the specific type, a union, or 'unknown' followed by a type guard.",
                    "function process(data: unknown): void {\n  if (typeof data === 'string') { /* ... */ }\n}"),

                new(FindingCategory.Performance, FindingSeverity.Warning,
                    "useEffect Missing Dependency Array",
                    "A useEffect hook is declared without a dependency array and will re-run on every render.",
                    "Add the appropriate dependency array. Use [] if the effect should run only once.",
                    "useEffect(() => {\n  fetchData();\n}, [id]); // only re-run when id changes"),

                new(FindingCategory.CodeStyle, FindingSeverity.Info,
                    "Prefer Optional Chaining Over Nested Null Checks",
                    "Multiple nested null/undefined checks can be replaced by the optional chaining operator.",
                    "Use optional chaining (?.) and nullish coalescing (??) for cleaner null-safe property access.",
                    "const name = user?.profile?.displayName ?? 'Anonymous';"),
            ],

            ["JavaScript"] =
            [
                new(FindingCategory.Bug, FindingSeverity.Warning,
                    "Loose Equality May Cause Unexpected Coercion",
                    "Loose equality (==) is used where strict equality (===) is likely intended.",
                    "Replace == and != with === and !== to avoid type coercion bugs.",
                    "if (value === null || value === undefined) { /* ... */ }"),

                new(FindingCategory.Security, FindingSeverity.Critical,
                    "eval() Used with External Input",
                    "eval() is called with data that may originate from user input or an external source.",
                    "Eliminate eval(). Use JSON.parse() for JSON data or restructure to avoid dynamic evaluation.",
                    null),

                new(FindingCategory.BestPractice, FindingSeverity.Info,
                    "Missing 'use strict' or Module Context",
                    "The script lacks strict mode, allowing silent errors such as undeclared variable assignments.",
                    "Add 'use strict'; at the top or convert to an ES module with import/export.",
                    "'use strict';\n// or use .mjs extension / type=module"),

                new(FindingCategory.CodeStyle, FindingSeverity.Info,
                    "var Declaration in Modern Code",
                    "var is used instead of const or let, leading to function-scoped hoisting surprises.",
                    "Use const by default; use let only when reassignment is required.",
                    "const maxRetries = 3;\nlet attempt = 0;"),
            ],

            ["Python"] =
            [
                new(FindingCategory.Security, FindingSeverity.Critical,
                    "Command Injection via subprocess shell=True",
                    "subprocess is called with shell=True and a user-supplied string, enabling command injection.",
                    "Use shell=False with a list of arguments, or sanitize the input before use.",
                    "subprocess.run(['ls', '-l', path], shell=False, check=True)"),

                new(FindingCategory.Bug, FindingSeverity.Warning,
                    "Mutable Default Argument",
                    "A mutable object (list, dict) is used as a default argument and is shared across all calls to the function.",
                    "Replace the mutable default with None and initialize inside the function body.",
                    "def process(items=None):\n    if items is None:\n        items = []"),

                new(FindingCategory.Naming, FindingSeverity.Info,
                    "Missing Type Hints",
                    "Function parameters and/or return type lack annotations, reducing IDE support and static analysis coverage.",
                    "Add type hints to all public function signatures.",
                    "def calculate(value: int, multiplier: float = 1.0) -> float:\n    return value * multiplier"),

                new(FindingCategory.Performance, FindingSeverity.Warning,
                    "String Concatenation in Loop Creates O(n²) Allocations",
                    "Strings are concatenated with += inside a loop. Python strings are immutable; each iteration allocates a new object.",
                    "Collect parts in a list and join at the end with ''.join(parts).",
                    "parts = []\nfor item in items:\n    parts.append(str(item))\nresult = ''.join(parts)"),
            ],

            ["Java"] =
            [
                new(FindingCategory.Performance, FindingSeverity.Warning,
                    "String Concatenation in Loop",
                    "The + operator is used to concatenate Strings inside a loop. Each iteration creates a new String object on the heap.",
                    "Use StringBuilder for in-loop string concatenation.",
                    "StringBuilder sb = new StringBuilder();\nfor (String s : items) { sb.append(s); }\nString result = sb.toString();"),

                new(FindingCategory.Bug, FindingSeverity.Critical,
                    "NullPointerException Risk Without Optional",
                    "A method return value or field that may be null is dereferenced without a null check.",
                    "Use Optional<T> or add an explicit null check before dereferencing.",
                    "Optional.ofNullable(value).ifPresent(v -> process(v));"),

                new(FindingCategory.Security, FindingSeverity.Critical,
                    "Deserialization of Untrusted Data",
                    "ObjectInputStream.readObject() is called on data from an external source, enabling remote code execution.",
                    "Avoid deserializing untrusted data. If required, implement a whitelist-based ObjectInputFilter.",
                    null),
            ],

            ["Go"] =
            [
                new(FindingCategory.Bug, FindingSeverity.Critical,
                    "Error Return Value Ignored",
                    "A function returning an error is called but the error is discarded with _ or without any check.",
                    "Always check the error return value and handle or propagate it.",
                    "if err := doSomething(); err != nil {\n    return fmt.Errorf(\"doSomething: %w\", err)\n}"),

                new(FindingCategory.Performance, FindingSeverity.Warning,
                    "Goroutine Leak: Channel Never Closed",
                    "A goroutine sends to a channel that is never closed, causing the goroutine to block indefinitely.",
                    "Ensure the producing goroutine closes the channel when done, or use a context for cancellation.",
                    "defer close(ch)"),

                new(FindingCategory.Security, FindingSeverity.Warning,
                    "Hard-Coded Credential in Source",
                    "A password or API token is hard-coded as a string literal in source code.",
                    "Read secrets from environment variables or a secret manager at runtime.",
                    "token := os.Getenv(\"API_TOKEN\")"),

                new(FindingCategory.CodeStyle, FindingSeverity.Info,
                    "Inconsistent Receiver Name",
                    "Different methods on the same type use different receiver variable names.",
                    "Use a single consistent short name (typically 1–2 letters) for all methods on the same type.",
                    null),
            ],
        };

    public async Task<IReadOnlyList<Finding>> AnalyzeAsync(
        string codeDiff, string language, CancellationToken ct)
    {
        var delayMs = Random.Shared.Next(MinDelayMs, MaxDelayMs + 1);
        await Task.Delay(delayMs, ct);

        var lineCount = codeDiff.Split('\n').Length;
        var maxFindings = Math.Min(8, 3 + lineCount / 20);
        var findingCount = Random.Shared.Next(3, maxFindings + 1);

        var pool = new List<FindingTemplate>(_genericTemplates);
        if (_languageTemplates.TryGetValue(language, out var languageSpecific))
            pool.AddRange(languageSpecific);

        Shuffle(pool);

        var changedLines = ExtractChangedLineNumbers(codeDiff);
        var selected = SelectDiverseFindings(pool, findingCount);

        return selected.Select((template, index) =>
        {
            var lineRef = changedLines.Count > 0
                ? $"Line {changedLines[index % changedLines.Count]}"
                : $"Line {Random.Shared.Next(1, Math.Max(2, lineCount))}";

            return new Finding
            {
                Id = Ulid.NewUlid().ToString(),
                ReviewId = string.Empty,
                Category = template.Category,
                Severity = template.Severity,
                Title = template.Title,
                Description = template.Description,
                LineReference = Random.Shared.Next(0, 3) == 0 ? null : lineRef,
                Suggestion = template.Suggestion,
                Confidence = Random.Shared.Next(40, 99),
                SuggestedFix = template.SuggestedFix,
            };
        }).ToList();
    }

    private static List<FindingTemplate> SelectDiverseFindings(List<FindingTemplate> pool, int count)
    {
        var result = new List<FindingTemplate>(count);

        // Priority pass: guarantee all 3 severity levels are present
        foreach (var severity in (FindingSeverity[])Enum.GetValues(typeof(FindingSeverity)))
        {
            if (result.Count >= count) break;
            var candidate = pool.FirstOrDefault(t => t.Severity == severity && !result.Contains(t));
            if (candidate is not null) result.Add(candidate);
        }

        // Fill remaining slots from the shuffled pool without duplication
        foreach (var template in pool)
        {
            if (result.Count >= count) break;
            if (!result.Contains(template)) result.Add(template);
        }

        return result;
    }

    private static List<int> ExtractChangedLineNumbers(string codeDiff)
    {
        var lines = new List<int>();
        var lineNumber = 0;
        foreach (var line in codeDiff.Split('\n'))
        {
            lineNumber++;
            if (line.StartsWith('+') || line.StartsWith('-'))
                lines.Add(lineNumber);
        }
        return lines;
    }

    private static void Shuffle<T>(List<T> list)
    {
        for (var i = list.Count - 1; i > 0; i--)
        {
            var j = Random.Shared.Next(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
}
