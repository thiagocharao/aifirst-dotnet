# Roadmap

## Status: ✅ PROJECT APPROVED - EXCELLENT DIRECTION

**Last Updated:** 2025-12-29  
**Review Status:** Comprehensive review completed. Direction is solid. Execute as planned.

---

## Milestone 0: Build Foundation (Week 0 - IMMEDIATE)

**Goal:** Fix build and establish CI/CD infrastructure

**Issues:**
- #1: Add global.json and fix build ✅ COMPLETED
- #2: Add test projects structure
- #3: Add CI/CD workflow
- #4: Add NuGet package metadata

**Deliverables:**
- ✅ Build succeeds with .NET 8.0
- Test infrastructure in place
- GitHub Actions CI running
- NuGet-ready project files

---

## Milestone 1: Hello MCP (Week 1)

**Goal:** Implement MCP connectivity and tool discovery

**Issues:**
- #5: Implement MCP transport abstraction
- #6: Implement MCP tool discovery
- #7: Implement MCP tool invocation
- #8: CLI pull-tools command
- #9: Update HelloMcp sample

**Deliverables:**
- MCP client connects to servers (stdio transport)
- Tool discovery working (`tools/list`)
- Tool invocation working (`tools/call`)
- `aifirst pull-tools` generates manifest
- HelloMcp sample demonstrates connectivity

---

## Milestone 2: Typed Tools (Week 2)

**Goal:** Schema parsing and C# DTO generation

**Issues:**
- #10: Define JSON Schema model
- #11: Implement C# type mapping
- #12: Implement DTO code generator
- #13: CLI gen command
- #14: Update TypedToolsDemo sample

**Deliverables:**
- JSON Schema subset parser
- C# type generation from schemas
- `aifirst gen` command working
- Generated types compile and work
- TypedToolsDemo uses generated types

---

## Milestone 3: C# DSL + Source Generator (Weeks 3-4)

**Goal:** Implement `[Tool]` attribute with source generator and analyzer

**Issues:**
- #15: Implement incremental source generator
- #16: Implement analyzer rule: unknown tool name
- #17: Implement analyzer rule: schema mismatch
- #18: Implement analyzer rule: missing manifest
- #19: Implement analyzer rule: non-allowlisted tool
- #20: Add IDE experience improvements (optional)

**Deliverables:**
- `[Tool]` attribute generates MCP calls
- Analyzer catches tool name typos at build time
- Analyzer validates types match schemas
- Build fails if manifest missing
- TypedToolsDemo compiles with source-generated code

**This is the killer feature milestone.**

---

## Milestone 4: Policies + Tracing (Week 5)

**Goal:** Policy pipeline, tracing, and replay functionality

**Issues:**
- #21: Implement allowlist policy
- #22: Implement redaction policy
- #23: Implement trace sink (file-based)
- #24: Implement CLI replay command
- #25: Implement output safety checks policy
- #26: Add policy pipeline configuration

**Deliverables:**
- Allowlist policy blocks unauthorized tools
- Redaction policy removes secrets from logs
- File-based trace sink captures executions
- `aifirst replay` command for deterministic testing
- Output safety checks detect injection attempts
- Configuration file for policy setup
- PolicyAndTracingDemo shows all features

**This makes the framework production-ready.**

---

## Milestone 5: Stabilize + Docs (Week 6)

**Goal:** Documentation, samples, and release preparation

**Issues:**
- #27: Add Getting Started guide
- #28: Document tool manifest schema
- #29: Document policy authoring guide
- #30: Document trace format specification
- #31: Add analyzer diagnostics documentation
- #32: Improve README with "Why This Exists"
- #33: Add code samples for common scenarios
- #34: Add semantic versioning and release process
- #35: Add Semantic Kernel adapter (optional)
- #36: Performance testing and optimization (optional)

**Deliverables:**
- Comprehensive documentation
- Multiple working samples
- README sells the vision
- Release process documented
- First alpha release (0.1.0-alpha)
- Optional: SK adapter for integration

**This prepares for public release.**

---

## Post-MVP / Future Enhancements

**Not in 6-week plan, but valuable for v0.2+:**

- #37: Add support for $ref in JSON Schema (reusable types)
- #38: Add streaming support for tool calls (IAsyncEnumerable)
- #39: Add caching layer for tool results (performance)
- #40: Add OpenTelemetry integration (enterprise observability)

**Other potential features:**
- HTTP/SSE transports for MCP (in addition to stdio)
- Memory/conversation history primitives
- Prompt template engine with validation
- Eval framework for testing agent behavior
- Visual Studio extension for enhanced IDE experience
- Azure Functions / ASP.NET Core middleware integration

---

## Timeline Summary

| Milestone | Duration | Cumulative |
|-----------|----------|------------|
| Milestone 0 | 2-3 days | Week 0 |
| Milestone 1 | 5-7 days | Week 1 |
| Milestone 2 | 5-7 days | Week 2 |
| Milestone 3 | 10-14 days | Weeks 3-4 |
| Milestone 4 | 5-7 days | Week 5 |
| Milestone 5 | 5-7 days | Week 6 |

**Total MVP Duration:** 6-8 weeks (conservative, single developer)

---

## Success Criteria

### Technical Success
- ✅ Build + tests green on every commit
- ✅ All analyzer rules working
- ✅ Generated code compiles without errors
- ✅ Policy pipeline prevents security issues
- ✅ Traces enable deterministic replay

### Product Success
- ✅ Samples run out-of-box
- ✅ Documentation is clear and complete
- ✅ First external contributor PR merged
- ✅ 10+ GitHub stars within 30 days of release
- ✅ Positive feedback from beta testers

### Business Success
- ✅ Published to NuGet.org
- ✅ Adopted by at least 3 external projects
- ✅ Featured in .NET community blog posts
- ✅ Considered for Microsoft collaboration (stretch goal)

---

## Review Notes

See `docs/project-review.md` for comprehensive analysis.

**Key Findings:**
- ✅ Idea is **excellent** — solves real enterprise problems
- ✅ Architecture is **solid** — clean separations, good abstractions
- ✅ Roadmap is **realistic** — incremental, deliverable milestones
- ⚠️ Build needs fixing (SDK version) — addressed in Milestone 0
- ⚠️ Missing tests — addressed in Milestone 0
- ✅ MCP timing is **perfect** — ecosystem momentum is strong

**Strategic Positioning:**
- Not "another agent framework"
- Instead: "DX + governance layer for MCP + .NET"
- Complements Semantic Kernel (doesn't compete)
- Targets enterprises needing safety + compliance

**Differentiation vs. Semantic Kernel:**
- ✅ Compile-time type safety (SK is stringly-typed)
- ✅ Built-in governance (policies, allowlists)
- ✅ Deterministic replay (SK has logging but not replay)
- ✅ MCP-native (SK's MCP support is emerging)

---

## Next Action

Execute Milestone 0 issues immediately:
1. ✅ Fix build with global.json (DONE)
2. Add test projects
3. Add CI workflow
4. Add NuGet metadata

Then proceed with Milestone 1 in order.
