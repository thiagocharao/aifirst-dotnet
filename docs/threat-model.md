# Threat Model

## Prompt injection via tool output

- Treat tool results as untrusted input.
- Apply output safety checks in `OnAfterToolCall`.

## Secrets/PII leakage

- Redact sensitive keys (`password`, `token`, `secret`, etc.) before logging.
- Trace sink is opt-in; defaults to no logging.

## Tool allowlisting

- Only allow approved tool names in production.
- Analyzer should warn/error on non-allowlisted tools.

## SSRF and network-bound tools

- Guard HTTP-capable tools with environment-specific policies.
- Prefer explicit allowlists for endpoints.
