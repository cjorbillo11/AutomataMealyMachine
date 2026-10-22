# Automata Mealy Machine

A C# implementation of a Mealy Machine that recognizes valid C-style block comments (`/* ... */`).

## Logic
- Characters other than `*` and `/` are mapped to the generic placeholder symbol `'a'`.
- The output bit is `1` only when transitioning from state `Q3` upon reading `/`.
- The string is accepted if it terminates in state `QAccept`.

## Running the Application

```bash
dotnet run