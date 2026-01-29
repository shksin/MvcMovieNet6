# .NET 10 Upgrade - Execution Tasks

## Progress Dashboard

| Metric | Status |
|--------|--------|
| **Overall Progress** | 50% (4/8 tasks) |
| **Current Phase** | Not Started |
| **Build Status** | Pending |
**Progress**: 8/8 tasks complete (100%) ![100%](https://progress-bar.xyz/100)

## Task Legend

- `[ ]` Not Started
- `[?]` In Progress
- `[?]` Completed
- `[?]` Failed
- `[?]` Skipped

---

## Phase 0: Preparation

### [?] TASK-001: Verify Prerequisites *(Completed: 2026-01-29 15:14)*
**Scope**: Environment validation
**References**: Plan: Prerequisites

**Actions:**
- [?] (1) Verify .NET 10 SDK is installed on the machine
- [?] (2) Check for global.json and update if necessary to allow .NET 10

**Verification:**
- .NET 10 SDK available
- No global.json conflicts

**Commit:** None (verification only)

---

## Phase 1: Atomic Upgrade - Project Files & Packages

### [?] TASK-002: Update All Project Target Frameworks *(Completed: 2026-01-29 15:16)*
**Scope**: All 6 project files
**References**: Plan: Project-by-Project Migration Plans

**Actions:**
- [?] (1) Update `MvcMovie\MvcMovie.csproj`: Change `<TargetFramework>net6.0</TargetFramework>` to `<TargetFramework>net10.0</TargetFramework>`
- [?] (2) Update `RazorMovie\RazorMovie.csproj`: Change `<TargetFramework>net6.0</TargetFramework>` to `<TargetFramework>net10.0</TargetFramework>`
- [?] (3) Update `WpfMovie\WpfMovie.csproj`: Change `<TargetFramework>net6.0-windows</TargetFramework>` to `<TargetFramework>net10.0-windows</TargetFramework>`
- [?] (4) Update `MvcMovie.Tests\MvcMovie.Tests.csproj`: Change `<TargetFramework>net6.0</TargetFramework>` to `<TargetFramework>net10.0</TargetFramework>`
- [?] (5) Update `RazorMovie.Tests\RazorMovie.Tests.csproj`: Change `<TargetFramework>net6.0</TargetFramework>` to `<TargetFramework>net10.0</TargetFramework>`
- [?] (6) Update `WpfMovie.Tests\WpfMovie.Tests.csproj`: Change `<TargetFramework>net6.0-windows</TargetFramework>` to `<TargetFramework>net10.0-windows</TargetFramework>`

**Verification:**
- All 6 project files updated with correct target framework

**Commit:** None (will commit after all changes complete)

---

### [?] TASK-003: Update MvcMovie NuGet Packages *(Completed: 2026-01-29 15:18)*
**Scope**: MvcMovie\MvcMovie.csproj
**References**: Plan: Package Update Reference

**Actions:**
- [?] (1) Update `Microsoft.EntityFrameworkCore.Design` from 6.0.0-rtm.21467.1 to 10.0.2
- [?] (2) Update `Microsoft.EntityFrameworkCore.SqlServer` from 6.0.0-rc.1.21452.10 to 10.0.2
- [?] (3) Update `Microsoft.EntityFrameworkCore.Tools` from 6.0.0-rc.1.21452.10 to 10.0.2
- [?] (4) Update `Microsoft.VisualStudio.Web.CodeGeneration.Design` from 6.0.0-rc.1.21464.1 to 10.0.2
- [?] (5) Review deprecated `Microsoft.Data.SqlClient` package - document status

**Verification:**
- All 4 packages updated to version 10.0.2
- Microsoft.Data.SqlClient status documented

**Commit:** None (will commit after all changes complete)

---

### [?] TASK-004: Update RazorMovie NuGet Packages (Security Fix) *(Completed: 2026-01-29 15:20)*
**Scope**: RazorMovie\RazorMovie.csproj
**References**: Plan: Package Update Reference, Risk Management

**Actions:**
- [?] (1) **SECURITY**: Update `HtmlSanitizer` from 7.1.542 to 9.0.889

**Verification:**
- HtmlSanitizer updated to 9.0.889
- Security vulnerability resolved

**Commit:** None (will commit after all changes complete)

---

### [?] TASK-005: Update WpfMovie NuGet Packages *(Completed: 2026-01-29 15:22)*
**Scope**: WpfMovie\WpfMovie.csproj
**References**: Plan: Package Update Reference

**Actions:**
- [?] (1) Update `Newtonsoft.Json` from 13.0.3 to 13.0.4

**Verification:**
- Newtonsoft.Json updated to 13.0.4

**Commit:** None (will commit after all changes complete)

---

## Phase 2: Build & Fix Breaking Changes

### [?] TASK-006: Build Solution and Fix Compilation Errors *(Completed: 2026-01-30 10:21)*
**Scope**: Entire solution
**References**: Plan: Breaking Changes Catalog

**Actions:**
- [?] (1) Restore all NuGet packages
- [?] (2) Build the solution
- [?] (3) If BinaryFormatter errors in WpfMovie: Replace with System.Text.Json serialization
- [?] (4) Fix any other compilation errors per Breaking Changes Catalog
- [?] (5) Rebuild and verify 0 errors

**Verification:**
- Solution builds with 0 errors
- BinaryFormatter replaced (if applicable)

**Commit:** None (will commit after tests pass)

---

## Phase 3: Test Validation

### [?] TASK-007: Run All Tests *(Completed: 2026-01-30 10:25)*
**Scope**: All 3 test projects
**References**: Plan: Testing & Validation Strategy

**Actions:**
- [?] (1) Run MvcMovie.Tests - verify all tests pass
- [?] (2) Run RazorMovie.Tests - verify all tests pass
- [?] (3) Run WpfMovie.Tests - verify all tests pass

**Verification:**
- All tests pass across all 3 test projects
- 0 test failures

**Commit:** None (will commit after all tests pass)

---

## Phase 4: Finalize

### [?] TASK-008: Commit All Changes *(Completed: 2026-01-30 10:28)*
**Scope**: Source control
**References**: Plan: Source Control Strategy

**Actions:**
- [?] (1) Stage all changes
- [?] (2) Commit with message: "Upgrade solution from .NET 6.0 to .NET 10.0"

**Verification:**
- All changes committed to `upgrade-to-NET10` branch
- Clean working directory

**Commit Message:**
```
Upgrade solution from .NET 6.0 to .NET 10.0

- Updated all 6 projects to target net10.0/net10.0-windows
- Updated 7 NuGet packages including security fix (HtmlSanitizer)
- Replaced BinaryFormatter with System.Text.Json in WpfMovie (if applicable)
- All tests pass
```

---

## Execution Log

*Execution events will be logged here as tasks are completed.*

| Timestamp | Task | Status | Notes |
|-----------|------|--------|-------|
| - | - | - | Awaiting execution start |
