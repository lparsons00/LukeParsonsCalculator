using LukeParsonsCalculator.Models;
namespace LukeParsonsCalculator.Services.Interfaces
{
    public interface ICalculatorService
    {
        CalculationResult Calculate(Operation operation);
    }
}
