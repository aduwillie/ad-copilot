using Microsoft.SemanticKernel;
using System.ComponentModel;

namespace AdCopilot.Plugins.MathPlugin;

public class MathPlugin
{
    [SKFunction, Description("Add two numbers.")]
    public static double Add(
        [Description("The first number to add")] double number1,
        [Description("The second number to add")] double number2)
    {
        return number1 + number2;
    }

    [SKFunction, Description("Subtract two numbers.")]
    public static double Subtract(
       [Description("The number to subtract from")] double number1,
       [Description("The number to subtract away")] double number2)
    {
        return number1 - number2;
    }

    [SKFunction, Description("Multiply two numbers.")]
    public static double Multiply(
       [Description("The first number to multiply")] double number1,
       [Description("The second number to multiply")] double number2)
    {
        return number1 * number2;
    }

    [SKFunction, Description("Divide two numbers.")]
    public static double Divide(
       [Description("The first number to divide from")] double number1,
       [Description("The second number to divide by. This should not be zero.")] double number2)
    {
        return number1 / number2;
    }
}
