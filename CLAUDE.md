# CLAUDE.md

Veteriner Kliniği Randevu ve Hasta Takip Sistemi — university Web Programlama project.

## Stack
- .NET 10 (SDK pinned in `global.json`), ASP.NET Core MVC, xUnit
- Solution: `VetClinic.slnx`
  - `src/VetClinic.Web` — MVC web app
  - `tests/VetClinic.Tests` — xUnit tests

## Commands
- Build: `dotnet build VetClinic.slnx`
- Test: `dotnet test VetClinic.slnx`
- Run: `dotnet run --project src/VetClinic.Web`

## Mandatory requirements (professor)
MVC + REST API (LINQ, DTOs) + MCP server (≥3 tools, one writes data), all using a shared service layer.
Domain includes vaccination records and veterinarian availability. Security and IDOR tests are required.

## Rules
- Never commit or push without explicit user approval. Never fabricate commit dates.
- One meaningful commit per real development day; log each in `docs/PROGRESS.md`.
- Business logic and ownership/authorization checks belong in the service layer, not controllers or MCP tools.
- Never commit secrets; use user-secrets or `*.local.json` (gitignored).
- Do not modify the separate McpPoc project.
- Keep user-facing docs (README, PROGRESS) in Turkish.
