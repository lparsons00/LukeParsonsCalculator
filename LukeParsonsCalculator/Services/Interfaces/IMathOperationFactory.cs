namespace LukeParsonsCalculator.Services.Interfaces
{
    public interface IMathOperationFactory
    {
        IMathOperation CreateOperation(string operationType);
    }
}
