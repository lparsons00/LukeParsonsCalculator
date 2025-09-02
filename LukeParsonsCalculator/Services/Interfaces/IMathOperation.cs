namespace LukeParsonsCalculator.Services.Interfaces
{
    public interface IMathOperation
    {
        decimal Calculate(params decimal[] values);
        string GetExpression(params decimal[] values);
    }
}
