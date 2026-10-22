using System;
using System.Text;

namespace MealyCommentSimulator;

public enum MachineState
{
    Q0,
    Q1,
    Q2,
    Q3,
    QAccept,
    QTrap
}

public static class CommentMealySimulator
{
    public static (int OutputBit, MachineState NextState) EvaluateStep(MachineState currentState, char inputChar)
    {
        // Map any character other than '*' or '/' to standard placeholder 'a'
        char symbol = (inputChar == '*' || inputChar == '/') ? inputChar : 'a';

        // Mealy output logic: Outputs 1 ONLY when in Q3 reading '/'
        int outputBit = (currentState == MachineState.Q3 && symbol == '/') ? 1 : 0;

        MachineState nextState = (currentState, symbol) switch
        {
            (MachineState.Q0, '/') => MachineState.Q1,
            (MachineState.Q0, _)   => MachineState.QTrap,

            (MachineState.Q1, '*') => MachineState.Q2,
            (MachineState.Q1, _)   => MachineState.QTrap,

            (MachineState.Q2, '*') => MachineState.Q3,
            (MachineState.Q2, _)   => MachineState.Q2,

            (MachineState.Q3, '/') => MachineState.QAccept,
            (MachineState.Q3, '*') => MachineState.Q3,
            (MachineState.Q3, _)   => MachineState.Q2,

            _ => MachineState.QTrap
        };

        return (outputBit, nextState);
    }

    public static void ProcessString(string input)
    {
        var state = MachineState.Q0;
        var bitstream = new StringBuilder();

        Console.WriteLine($"\n--- Execution Trace for Input: \"{input}\" ---");
        Console.WriteLine($"{"Step",-5} | {"Char",-8} | {"Mapped",-8} | {"Next State",-12} | {"Output",-6}");
        Console.WriteLine(new string('-', 55));

        for (int i = 0; i < input.Length; i++)
        {
            char rawChar = input[i];
            char mappedSymbol = (rawChar == '*' || rawChar == '/') ? rawChar : 'a';

            var (bit, nextState) = EvaluateStep(state, rawChar);
            state = nextState;
            bitstream.Append(bit);

            string displayChar = rawChar switch
            {
                ' ' => "' '",
                '\t' => "'\\t'",
                '\n' => "'\\n'",
                _ => $"'{rawChar}'"
            };

            Console.WriteLine($"{i + 1,-5} | {displayChar,-8} | '{mappedSymbol}'      | {state,-12} | {bit,-6}");
        }

        bool isAccepted = state == MachineState.QAccept;

        Console.WriteLine(new string('-', 55));
        Console.WriteLine($"Final State     : {state}");
        Console.WriteLine($"Output Sequence : {bitstream}");
        Console.WriteLine($"Status          : {(isAccepted ? "ACCEPTED" : "REJECTED")}\n");
    }

    public static void Main()
    {
        Console.WriteLine("=========================================================");
        Console.WriteLine("     Mealy Machine Simulator: C-Style Block Comments     ");
        Console.WriteLine("=========================================================");
        Console.WriteLine("Sample Test Cases:");
        Console.WriteLine("  Accepted : /*a*/, /**/, /***/, /*hello world*/");
        Console.WriteLine("  Rejected : /**, /*/*/, //aaaa");
        Console.WriteLine("=========================================================");
        Console.WriteLine("Type 'exit' to quit.\n");

        while (true)
        {
            Console.Write("Enter string to test: ");
            string? input = Console.ReadLine();

            if (input == null || input.Equals("exit", StringComparison.OrdinalIgnoreCase))
            {
                break;
            }

            ProcessString(input);
        }
    }
}