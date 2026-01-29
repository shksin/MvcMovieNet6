# .NET 10 Upgrade Plan

## Table of Contents

- [Executive Summary](#executive-summary)
- [Migration Strategy](#migration-strategy)
- [Detailed Dependency Analysis](#detailed-dependency-analysis)
- [Project-by-Project Migration Plans](#project-by-project-migration-plans)
  - [MvcMovie](#mvcmovie)
  - [RazorMovie](#razormovie)
  - [WpfMovie](#wpfmovie)
  - [MvcMovie.Tests](#mvcmovietests)
  - [RazorMovie.Tests](#razormovietests)
  - [WpfMovie.Tests](#wpfmovietests)
- [Package Update Reference](#package-update-reference)
- [Breaking Changes Catalog](#breaking-changes-catalog)
- [Risk Management](#risk-management)
- [Testing & Validation Strategy](#testing--validation-strategy)
- [Complexity & Effort Assessment](#complexity--effort-assessment)
- [Source Control Strategy](#source-control-strategy)
- [Success Criteria](#success-criteria)

---

## Executive Summary

### Upgrade Overview

| Property | Value |
|----------|-------|
| **Scenario** | .NET Version Upgrade |
| **Current Framework** | .NET 6.0 |
| **Target Framework** | .NET 10.0 (LTS) |
| **Total Projects** | 6 |
| **Total Packages Requiring Updates** | 7 |
| **Security Vulnerabilities** | 1 (HtmlSanitizer) |
| **Estimated LOC Impact** | 53+ lines (~2.8% of codebase) |

### Selected Strategy

**All-At-Once Strategy** — All projects upgraded simultaneously in a single atomic operation.

**Rationale:**
- Small solution (6 projects)
- All projects currently on .NET 6.0 (modern SDK-style)
- Simple, clear dependency structure (3 independent app/test pairs)
- No circular dependencies
- All packages have known target versions
- Low overall complexity (5 Low + 1 Medium difficulty projects)

### Solution Structure

| Project Type | Projects | Notes |
|--------------|----------|-------|
| **ASP.NET Core (MVC)** | MvcMovie | 1,068 LOC, Entity Framework |
| **ASP.NET Core (Razor Pages)** | RazorMovie | 377 LOC, Rate limiting |
| **WPF Desktop** | WpfMovie | 271 LOC, Windows-specific |
| **Test Projects** | 3 (NUnit + MSTest) | 198 LOC total |

### Critical Issues to Address

| Priority | Issue | Affected |
|----------|-------|----------|
| ?? **Security** | HtmlSanitizer vulnerability | RazorMovie |
| ?? **Deprecated** | Microsoft.Data.SqlClient deprecated | MvcMovie |
| ?? **Breaking Changes** | BinaryFormatter removal | WpfMovie |
| ?? **Behavioral** | UseExceptionHandler changes | MvcMovie, RazorMovie |

### Complexity Classification

- **Solution Complexity**: Simple
- **Iteration Strategy**: Fast batch (all projects in single atomic operation)
- **Expected Plan Iterations**: Minimal (single upgrade phase + testing phase)

---

## Migration Strategy

### Selected Approach: All-At-Once

**All projects upgraded simultaneously in a single atomic operation** — no intermediate states, no multi-targeting.

### Strategy Rationale

| Factor | Assessment | Implication |
|--------|------------|-------------|
| Solution size | 6 projects | ? Small, manageable |
| Current framework | All .NET 6.0 | ? Uniform starting point |
| Dependency depth | Max 2 levels | ? Simple structure |
| Circular dependencies | None | ? Clean upgrade path |
| Package compatibility | All have target versions | ? No blockers |
| Test coverage | 3 test projects | ? Can validate immediately |

### Execution Phases

#### Phase 0: Preparation (if applicable)
- Verify .NET 10 SDK installation
- Update global.json if present

#### Phase 1: Atomic Upgrade
**All operations performed as single coordinated batch:**
1. Update all 6 project files to target framework
2. Update all 7 package references across projects
3. Restore dependencies
4. Build solution and fix all compilation errors
5. Verify solution builds with 0 errors

**Deliverables:** Solution builds successfully with 0 errors

#### Phase 2: Test Validation
1. Execute all test projects
2. Address any test failures

**Deliverables:** All tests pass

### Why Not Incremental?

Incremental migration would:
- Add unnecessary complexity for this small solution
- Create intermediate states that don't provide value
- Require multi-targeting coordination
- Take longer with no risk reduction benefit

The solution's simplicity and independence of the three app/test pairs makes All-At-Once the optimal choice.

---

## Detailed Dependency Analysis

### Dependency Graph Overview

The solution consists of **3 independent dependency chains** with no cross-dependencies:

```
Chain 1: MvcMovie.Tests ? MvcMovie
Chain 2: RazorMovie.Tests ? RazorMovie  
Chain 3: WpfMovie.Tests ? WpfMovie
```

### Dependency Matrix

| Project | Dependencies | Dependants | Depth |
|---------|--------------|------------|-------|
| MvcMovie | 0 | 1 (Tests) | 0 |
| RazorMovie | 0 | 1 (Tests) | 0 |
| WpfMovie | 0 | 1 (Tests) | 0 |
| MvcMovie.Tests | 1 (MvcMovie) | 0 | 1 |
| RazorMovie.Tests | 1 (RazorMovie) | 0 | 1 |
| WpfMovie.Tests | 1 (WpfMovie) | 0 | 1 |

### Migration Order (All-At-Once)

Since we're using All-At-Once strategy, all projects are upgraded simultaneously. However, the logical order for understanding dependencies is:

**Tier 0 (Leaf Projects - No Dependencies):**
- `MvcMovie.csproj` - ASP.NET Core MVC application
- `RazorMovie.csproj` - ASP.NET Core Razor Pages application
- `WpfMovie.csproj` - WPF Windows application

**Tier 1 (Test Projects - Depend on Tier 0):**
- `MvcMovie.Tests.csproj` ? depends on MvcMovie
- `RazorMovie.Tests.csproj` ? depends on RazorMovie
- `WpfMovie.Tests.csproj` ? depends on WpfMovie

### Critical Path

No critical path concerns — all three application chains are independent and can be upgraded atomically without intermediate states.

---

## Project-by-Project Migration Plans

### MvcMovie

**Project:** `MvcMovie\MvcMovie.csproj`

#### Current State
| Property | Value |
|----------|-------|
| Target Framework | net6.0 |
| Project Kind | ASP.NET Core MVC |
| SDK-style | Yes |
| Lines of Code | 1,068 |
| Package Count | 6 |
| API Issues | 1 (behavioral) |
| Risk Level | ?? Low |

#### Target State
| Property | Value |
|----------|-------|
| Target Framework | net10.0 |
| Packages to Update | 5 |

#### Migration Steps
1. Update `TargetFramework` from `net6.0` to `net10.0`
2. Update package references:
   - `Microsoft.EntityFrameworkCore.Design`: 6.0.0-rtm.21467.1 ? 10.0.2
   - `Microsoft.EntityFrameworkCore.SqlServer`: 6.0.0-rc.1.21452.10 ? 10.0.2
   - `Microsoft.EntityFrameworkCore.Tools`: 6.0.0-rc.1.21452.10 ? 10.0.2
   - `Microsoft.VisualStudio.Web.CodeGeneration.Design`: 6.0.0-rc.1.21464.1 ? 10.0.2
3. Address deprecated `Microsoft.Data.SqlClient` (4.0.5) - evaluate replacement or update
4. Review `UseExceptionHandler` behavioral change in Program.cs

#### Expected Breaking Changes
- `UseExceptionHandler` API behavioral change (verify error handling still works as expected)

#### Validation
- [ ] Project builds without errors
- [ ] Project builds without warnings
- [ ] Unit tests pass (MvcMovie.Tests)

---

### RazorMovie

**Project:** `RazorMovie\RazorMovie.csproj`

#### Current State
| Property | Value |
|----------|-------|
| Target Framework | net6.0 |
| Project Kind | ASP.NET Core Razor Pages |
| SDK-style | Yes |
| Lines of Code | 377 |
| Package Count | 2 |
| API Issues | 1 (behavioral) |
| Risk Level | ?? Low |

#### Target State
| Property | Value |
|----------|-------|
| Target Framework | net10.0 |
| Packages to Update | 1 (security fix) |

#### Migration Steps
1. Update `TargetFramework` from `net6.0` to `net10.0`
2. **SECURITY FIX**: Update `HtmlSanitizer`: 7.1.542 ? 9.0.889
3. Review `UseExceptionHandler` behavioral change in Program.cs (line 43)

#### Expected Breaking Changes
- `UseExceptionHandler` API behavioral change
- HtmlSanitizer API changes (major version bump 7.x ? 9.x)

#### Validation
- [ ] Project builds without errors
- [ ] Project builds without warnings
- [ ] Security vulnerability resolved
- [ ] Unit tests pass (RazorMovie.Tests)

---

### WpfMovie

**Project:** `WpfMovie\WpfMovie.csproj`

#### Current State
| Property | Value |
|----------|-------|
| Target Framework | net6.0-windows |
| Project Kind | WPF Desktop |
| SDK-style | Yes |
| Lines of Code | 271 |
| Package Count | 1 |
| API Issues | 51 (44 binary, 2 source, 5 behavioral) |
| Risk Level | ?? Medium |

#### Target State
| Property | Value |
|----------|-------|
| Target Framework | net10.0-windows |
| Packages to Update | 1 |

#### Migration Steps
1. Update `TargetFramework` from `net6.0-windows` to `net10.0-windows`
2. Update `Newtonsoft.Json`: 13.0.3 ? 13.0.4
3. **CRITICAL**: Replace `BinaryFormatter` usage with modern serialization
4. Verify WPF controls compile correctly (API analysis shows binary incompatibility warnings but these are typically resolved by targeting correct Windows TFM)

#### Expected Breaking Changes
- **BinaryFormatter** removed in .NET 10 - must migrate to `System.Text.Json` or other serialization
- WPF API binary incompatibilities should resolve with `net10.0-windows` target
- `System.Uri` behavioral changes (verify URI handling)

#### Validation
- [ ] Project builds without errors
- [ ] Project builds without warnings
- [ ] BinaryFormatter replaced
- [ ] Unit tests pass (WpfMovie.Tests)

---

### MvcMovie.Tests

**Project:** `MvcMovie.Tests\MvcMovie.Tests.csproj`

#### Current State
| Property | Value |
|----------|-------|
| Target Framework | net6.0 |
| Project Kind | NUnit Test Project |
| SDK-style | Yes |
| Lines of Code | 26 |
| Dependencies | MvcMovie |
| Risk Level | ?? Low |

#### Target State
| Property | Value |
|----------|-------|
| Target Framework | net10.0 |
| Packages to Update | 0 |

#### Migration Steps
1. Update `TargetFramework` from `net6.0` to `net10.0`
2. No package updates required (all compatible)

#### Validation
- [ ] Project builds without errors
- [ ] All tests pass

---

### RazorMovie.Tests

**Project:** `RazorMovie.Tests\RazorMovie.Tests.csproj`

#### Current State
| Property | Value |
|----------|-------|
| Target Framework | net6.0 |
| Project Kind | MSTest Test Project |
| SDK-style | Yes |
| Lines of Code | 30 |
| Dependencies | RazorMovie |
| Risk Level | ?? Low |

#### Target State
| Property | Value |
|----------|-------|
| Target Framework | net10.0 |
| Packages to Update | 0 |

#### Migration Steps
1. Update `TargetFramework` from `net6.0` to `net10.0`
2. No package updates required (all compatible)

#### Validation
- [ ] Project builds without errors
- [ ] All tests pass

---

### WpfMovie.Tests

**Project:** `WpfMovie.Tests\WpfMovie.Tests.csproj`

#### Current State
| Property | Value |
|----------|-------|
| Target Framework | net6.0-windows |
| Project Kind | NUnit Test Project |
| SDK-style | Yes |
| Lines of Code | 142 |
| Dependencies | WpfMovie |
| Risk Level | ?? Low |

#### Target State
| Property | Value |
|----------|-------|
| Target Framework | net10.0-windows |
| Packages to Update | 0 |

#### Migration Steps
1. Update `TargetFramework` from `net6.0-windows` to `net10.0-windows`
2. No package updates required (all compatible)

#### Validation
- [ ] Project builds without errors
- [ ] All tests pass

---

## Package Update Reference

### Packages Requiring Updates (7 total)

| Package | Current | Target | Projects | Reason | Priority |
|---------|---------|--------|----------|--------|----------|
| **HtmlSanitizer** | 7.1.542 | 9.0.889 | RazorMovie | ?? Security vulnerability | Critical |
| **Microsoft.EntityFrameworkCore.Design** | 6.0.0-rtm.21467.1 | 10.0.2 | MvcMovie | Framework compatibility | High |
| **Microsoft.EntityFrameworkCore.SqlServer** | 6.0.0-rc.1.21452.10 | 10.0.2 | MvcMovie | Framework compatibility | High |
| **Microsoft.EntityFrameworkCore.Tools** | 6.0.0-rc.1.21452.10 | 10.0.2 | MvcMovie | Framework compatibility | High |
| **Microsoft.VisualStudio.Web.CodeGeneration.Design** | 6.0.0-rc.1.21464.1 | 10.0.2 | MvcMovie | Framework compatibility | High |
| **Newtonsoft.Json** | 13.0.3 | 13.0.4 | WpfMovie | Recommended update | Medium |
| **Microsoft.Data.SqlClient** | 4.0.5 | — | MvcMovie | ?? Deprecated | Review |

### Compatible Packages (No Update Required)

| Package | Version | Projects |
|---------|---------|----------|
| AspNetCoreRateLimit | 5.0.0 | RazorMovie |
| coverlet.collector | 3.1.2 | MvcMovie.Tests, WpfMovie.Tests |
| DequeNET | 1.0.2 | MvcMovie |
| Microsoft.NET.Test.Sdk | 17.1.0 | MvcMovie.Tests, WpfMovie.Tests |
| Microsoft.NET.Test.Sdk | 17.12.0 | RazorMovie.Tests |
| Moq | 4.18.2 | RazorMovie.Tests |
| MSTest | 3.6.4 | RazorMovie.Tests |
| NUnit | 3.13.3 | MvcMovie.Tests, WpfMovie.Tests |
| NUnit.Analyzers | 3.3.0 | MvcMovie.Tests, WpfMovie.Tests |
| NUnit3TestAdapter | 4.2.1 | MvcMovie.Tests, WpfMovie.Tests |

### Package Updates by Project

**MvcMovie (5 updates):**
```xml
<PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="10.0.2" />
<PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="10.0.2" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="10.0.2" />
<PackageReference Include="Microsoft.VisualStudio.Web.CodeGeneration.Design" Version="10.0.2" />
<!-- Review: Microsoft.Data.SqlClient is deprecated -->
```

**RazorMovie (1 update - SECURITY):**
```xml
<PackageReference Include="HtmlSanitizer" Version="9.0.889" />
```

**WpfMovie (1 update):**
```xml
<PackageReference Include="Newtonsoft.Json" Version="13.0.4" />
```

### Deprecated Package: Microsoft.Data.SqlClient

?? **Action Required**: `Microsoft.Data.SqlClient` 4.0.5 is deprecated.

**Options:**
1. Update to latest supported version if available
2. Evaluate if EF Core's built-in provider is sufficient
3. Document as technical debt if no immediate replacement

---

## Breaking Changes Catalog

### High Priority (Code Changes Required)

#### 1. BinaryFormatter Removal (WpfMovie)

| Property | Details |
|----------|---------|
| **Severity** | ?? Source Incompatible |
| **Affected Project** | WpfMovie |
| **Occurrences** | 2 |
| **Category** | Deprecated Remoting & Serialization |

**Issue:** `System.Runtime.Serialization.Formatters.Binary.BinaryFormatter` is removed in .NET 10 for security reasons.

**Migration Path:**
- Replace with `System.Text.Json` for JSON serialization
- Use `protobuf-net` for binary serialization if size is critical
- Consider `MessagePack` for high-performance scenarios

**Example Fix:**
```csharp
// Before (BinaryFormatter - removed)
var formatter = new BinaryFormatter();
formatter.Serialize(stream, obj);

// After (System.Text.Json)
await JsonSerializer.SerializeAsync(stream, obj);
```

---

### Medium Priority (Behavioral Changes - Testing Required)

#### 2. UseExceptionHandler Behavioral Change (MvcMovie, RazorMovie)

| Property | Details |
|----------|---------|
| **Severity** | ?? Behavioral Change |
| **Affected Projects** | MvcMovie, RazorMovie |
| **Occurrences** | 2 (1 per project) |
| **Locations** | Program.cs |

**Issue:** `UseExceptionHandler(string)` has behavioral changes in .NET 10.

**Action:** Verify error handling behavior after upgrade. Test error scenarios to ensure exceptions are handled correctly.

---

#### 3. System.Uri Behavioral Changes (WpfMovie)

| Property | Details |
|----------|---------|
| **Severity** | ?? Behavioral Change |
| **Affected Project** | WpfMovie |
| **Occurrences** | 3 |

**Issue:** `System.Uri` constructor and parsing behavior may differ.

**Action:** Test URI handling, especially with edge cases (special characters, relative URIs).

---

### Low Priority (Compile-Time Resolution Expected)

#### 4. WPF API Binary Incompatibilities

| Property | Details |
|----------|---------|
| **Severity** | ?? Binary Incompatible (but expected to resolve) |
| **Affected Project** | WpfMovie |
| **Occurrences** | 44 |
| **APIs** | TextBox, ListBox, Window, Application, etc. |

**Issue:** Assessment flags WPF APIs as binary incompatible, but these are standard WPF controls.

**Expected Resolution:** These should compile correctly when targeting `net10.0-windows`. The binary incompatibility warnings are due to assembly version differences, not actual API removal.

**Action:** 
1. Update to `net10.0-windows`
2. Build and verify compilation
3. If errors occur, investigate specific control issues

---

### Breaking Changes Summary by Project

| Project | Breaking Changes | Required Actions |
|---------|------------------|------------------|
| MvcMovie | 1 behavioral | Test exception handling |
| RazorMovie | 1 behavioral + HtmlSanitizer API | Test exception handling, review sanitizer usage |
| WpfMovie | 2 source + 5 behavioral | **Replace BinaryFormatter**, test URI handling |
| Test Projects | 0 | None |

---

## Risk Management

### Risk Assessment Matrix

| Risk | Probability | Impact | Project | Mitigation |
|------|-------------|--------|---------|------------|
| **Security vulnerability exploitation** | Medium | High | RazorMovie | Address HtmlSanitizer update immediately |
| **BinaryFormatter removal breaks functionality** | High | Medium | WpfMovie | Replace with System.Text.Json before upgrade |
| **EF Core migration issues** | Low | Medium | MvcMovie | Test database operations thoroughly |
| **HtmlSanitizer API changes** | Medium | Low | RazorMovie | Review sanitizer usage after update |
| **WPF control compilation failures** | Low | Medium | WpfMovie | Verify Windows TFM targeting |
| **Test failures after upgrade** | Medium | Low | All | Run tests immediately after build success |

### High-Risk Items

#### 1. HtmlSanitizer Security Vulnerability (RazorMovie)

| Property | Details |
|----------|---------|
| **Risk Level** | ?? High |
| **Current Version** | 7.1.542 |
| **Secure Version** | 9.0.889 |
| **Mitigation** | Update as part of atomic upgrade |

**Note:** This is a security fix and should be prioritized. The major version jump (7.x ? 9.x) may include API changes — review sanitization code after upgrade.

---

#### 2. BinaryFormatter Replacement (WpfMovie)

| Property | Details |
|----------|---------|
| **Risk Level** | ?? Medium-High |
| **Impact** | Serialization code will not compile |
| **Mitigation** | Replace with modern serialization during upgrade |

**Action Required:** Identify all BinaryFormatter usages and replace before compilation can succeed.

---

### Contingency Plans

| Scenario | Action |
|----------|--------|
| Build fails after framework update | Review compiler errors, address breaking changes per catalog |
| Package restore fails | Check package compatibility, review deprecated packages |
| Tests fail after upgrade | Isolate failures, check behavioral changes in affected areas |
| WPF controls don't compile | Verify `net10.0-windows` TFM, check UseWPF property |
| HtmlSanitizer API incompatible | Review release notes, adapt sanitization patterns |

### Rollback Strategy

Since this is an All-At-Once upgrade with source control:

1. **Before starting**: Ensure clean commit on `upgrade-to-NET10` branch
2. **If blocked**: Discard all changes with `git checkout .` or `git reset --hard`
3. **If partially complete**: Complete all changes before committing
4. **Final state**: Single commit representing entire upgrade

---

## Testing & Validation Strategy

### Phase 1: Build Validation (During Atomic Upgrade)

After updating all project files and packages:

1. **Restore dependencies**: `dotnet restore`
2. **Build solution**: `dotnet build`
3. **Fix compilation errors** (expected: BinaryFormatter replacement)
4. **Verify**: 0 errors, 0 warnings

### Phase 2: Test Execution

After successful build:

| Test Project | Framework | Test Runner | Expected |
|--------------|-----------|-------------|----------|
| MvcMovie.Tests | NUnit | NUnit3TestAdapter | All pass |
| RazorMovie.Tests | MSTest | MSTest | All pass |
| WpfMovie.Tests | NUnit | NUnit3TestAdapter | All pass |

**Execution:**
```bash
dotnet test MvcMovie.Tests/MvcMovie.Tests.csproj
dotnet test RazorMovie.Tests/RazorMovie.Tests.csproj
dotnet test WpfMovie.Tests/WpfMovie.Tests.csproj
```

### Validation Checklist

#### All Projects
- [ ] Target framework updated to net10.0 (or net10.0-windows for WPF)
- [ ] All packages updated per Package Update Reference
- [ ] Project builds without errors
- [ ] Project builds without warnings

#### Security Validation
- [ ] HtmlSanitizer updated to 9.0.889
- [ ] No security vulnerabilities remain

#### Functionality Validation
- [ ] All unit tests pass
- [ ] Exception handling works correctly (behavioral change areas)
- [ ] Serialization works correctly (BinaryFormatter replacement)

---

## Complexity & Effort Assessment

### Project Complexity Ratings

| Project | Complexity | Rationale |
|---------|------------|-----------|
| MvcMovie | ?? Low | Standard package updates, no code changes expected |
| RazorMovie | ?? Low | Security fix + minor behavioral review |
| WpfMovie | ?? Medium | BinaryFormatter replacement required, highest LOC impact |
| MvcMovie.Tests | ?? Low | Framework update only |
| RazorMovie.Tests | ?? Low | Framework update only |
| WpfMovie.Tests | ?? Low | Framework update only |

### Complexity Factors

| Factor | Assessment |
|--------|------------|
| Total projects | 6 (Small) |
| Dependency depth | 2 (Simple) |
| Security vulnerabilities | 1 (Manageable) |
| Breaking changes requiring code | 1 (BinaryFormatter) |
| Package updates | 7 (Straightforward) |
| API incompatibilities | 53 (Mostly auto-resolved by TFM) |

### Overall Assessment

**Solution Complexity: Low-Medium**

The majority of work is straightforward framework and package version updates. The only significant code change required is replacing `BinaryFormatter` in WpfMovie, which is a well-documented migration with clear alternatives.

---

## Source Control Strategy

### Branch Strategy

| Branch | Purpose |
|--------|---------|
| `main` | Source branch (starting point) |
| `upgrade-to-NET10` | Upgrade branch (all changes here) |

### Commit Strategy

**Single Commit Approach** (Recommended for All-At-Once):

All upgrade changes committed as a single atomic commit:
- All project file updates
- All package reference updates
- All code changes (BinaryFormatter replacement)
- After build success and all tests pass

**Commit Message Format:**
```
Upgrade solution from .NET 6.0 to .NET 10.0

- Updated all 6 projects to target net10.0/net10.0-windows
- Updated 7 NuGet packages including security fix (HtmlSanitizer)
- Replaced BinaryFormatter with System.Text.Json in WpfMovie
- Addressed deprecated Microsoft.Data.SqlClient
- All tests pass
```

### Merge Process

1. Complete all changes on `upgrade-to-NET10` branch
2. Verify build succeeds with 0 errors
3. Verify all tests pass
4. Create Pull Request to `main`
5. Code review focusing on:
   - Security vulnerability resolution
   - BinaryFormatter replacement
   - Package version correctness
6. Merge to `main`

---

## Success Criteria

### Technical Criteria

| Criterion | Requirement |
|-----------|-------------|
| **Target Framework** | All projects target net10.0 or net10.0-windows |
| **Build Status** | Solution builds with 0 errors |
| **Warnings** | Solution builds with 0 warnings (preferred) |
| **Tests** | All tests pass (3 test projects) |
| **Security** | No security vulnerabilities remain |
| **Packages** | All packages updated per specification |

### Project-Specific Criteria

| Project | Success Criteria |
|---------|------------------|
| MvcMovie | net10.0, 5 packages updated, builds, tests pass |
| RazorMovie | net10.0, HtmlSanitizer 9.0.889, builds, tests pass |
| WpfMovie | net10.0-windows, BinaryFormatter replaced, builds, tests pass |
| MvcMovie.Tests | net10.0, builds, all tests pass |
| RazorMovie.Tests | net10.0, builds, all tests pass |
| WpfMovie.Tests | net10.0-windows, builds, all tests pass |

### Quality Criteria

- [ ] Code quality maintained (no degradation)
- [ ] Test coverage maintained
- [ ] Security vulnerabilities addressed
- [ ] Deprecated packages reviewed

### Process Criteria

- [ ] All-At-Once strategy followed (single atomic operation)
- [ ] Single commit for entire upgrade
- [ ] All changes on `upgrade-to-NET10` branch
- [ ] PR created for review before merge

### Definition of Done

The .NET 10 upgrade is **complete** when:

1. ? All 6 projects target .NET 10.0 (or net10.0-windows for WPF projects)
2. ? All 7 package updates applied
3. ? HtmlSanitizer security vulnerability resolved
4. ? BinaryFormatter replaced with modern serialization
5. ? Solution builds with 0 errors
6. ? All tests pass (MvcMovie.Tests, RazorMovie.Tests, WpfMovie.Tests)
7. ? Changes committed to `upgrade-to-NET10` branch
8. ? Ready for PR review and merge to `main`
