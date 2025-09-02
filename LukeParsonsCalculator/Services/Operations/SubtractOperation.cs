using LukeParsonsCalculator.Services.Interfaces;

namespace LukeParsonsCalculator.Services.Operations
{
    public class SubtractOperation : IMathOperation
    {
        public decimal Calculate(params decimal[] values)
        {
            if (values.Length == 0)
            {
                return 0;
            }
            var result = values[0];
            for (int i = 1; i < values.Length; i++)
            {
                result -= values[i];
            }
            return result;
        }

        public string GetExpression(params decimal[] values)
        {
            return string.Join("-", values);
        }
    }
}
