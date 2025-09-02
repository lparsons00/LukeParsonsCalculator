using LukeParsonsCalculator.Services.Interfaces;

namespace LukeParsonsCalculator.Services.Operations
{
    public class MultiplyOperation : IMathOperation
    {
        public decimal Calculate(params decimal[] values)
        {
            if (values.Length == 0)
            {
                return 1;
            }
            decimal result = 1;
            foreach (var value in values)
            {
                result *= value;
            }
            return result;
        }

        public string GetExpression(params decimal[] values)
        {
            return string.Join("*", values);
        }
    }
}
