using LukeParsonsCalculator.Services.Operations;
using LukeParsonsCalculator.Services.Interfaces;

namespace LukeParsonsCalculator.Services.Factories
{

    public class MathOperationFactory : IMathOperationFactory
    {
        private readonly Dictionary<string, Func<IMathOperation>> _operation;

        public MathOperationFactory()
        {
            _operation = new Dictionary<string, Func<IMathOperation>>(StringComparer.OrdinalIgnoreCase)
            {
                { "Add", () => new AddOperation()},
                { "Plus", () => new AddOperation()},
                { "Subtract", () => new SubtractOperation()},
                { "Minus", () => new SubtractOperation()},
                { "Multiply", () => new MultiplyOperation()},
                { "Times", () => new MultiplyOperation()},
                { "Multiplication", () => new MultiplyOperation()},
                { "Divide", () => new DivideOperation()},
                { "Division", () => new DivideOperation()}
            };
        }
        public IMathOperation CreateOperation(string operationType)
        {
            if (_operation.TryGetValue(operationType, out var c))
            {
                return c();
            }
            throw new Exception($"Error finding operation for {operationType}");
        }
    }
}
