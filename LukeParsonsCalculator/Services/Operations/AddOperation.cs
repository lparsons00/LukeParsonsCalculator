using LukeParsonsCalculator.Services.Interfaces;

namespace LukeParsonsCalculator.Services.Operations
{
    public class AddOperation : IMathOperation
    {
        public decimal Calculate(params decimal[] values)
        {
            return values.Sum();
        }
        public string GetExpression(params decimal[] values)
        {
            return string.Join("+", values);
        }
    }
}
