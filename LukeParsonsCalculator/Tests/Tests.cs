using Xunit;
using LukeParsonsCalculator.Services;
using LukeParsonsCalculator.Models;
using LukeParsonsCalculator.Services.Interfaces;
using LukeParsonsCalculator.Services.Factories;



namespace LukeParsonsCalculator.Tests
{
    public class CalculatorServiceTests
    {
        private readonly ICalculatorService _calculatorService;

        public CalculatorServiceTests()
        {
            _calculatorService = new CalculatorService(new MathOperationFactory());
        }

        [Fact]
        public void Calculate_SimpleAddition_CorrectResult()
        {
            //ARRANGE
            var operation = new Operation
            {
                OperationType = "Add",
                Values = new List<decimal> { 1, 2 }
            };

            //ACT
            var result = _calculatorService.Calculate(operation);
            //ASSERT
            Assert.Equal(3, result.Result);
        }

        [Fact]
        public void Calculate_NestedOperation_CorrectResult()
        {
            //ARRANGE
            var operation = new Operation()
            {
                OperationType = "Add",
                Values = new List<decimal> { 2, 3 },
                NestedOperation = new Operation()
                {
                    OperationType = "Multiply",
                    Values = new List<decimal> { 4, 5 }
                }
            };

            //ACT
            var result = _calculatorService.Calculate(operation);
            //ASSERT
            Assert.Equal(25, result.Result);
        }

        [Fact]
        public void Calculate_DivideByZero_Throws()
        {
            //ARRANGE
            var operation = new Operation
            {
                OperationType = "Divide",
                Values = new List<decimal> { 0, 2 }
            };

            //ACT
            //ASSERT
            Assert.Throws<DivideByZeroException>(() => _calculatorService.Calculate(operation));
        }


        [Fact]
        public void Calculate_UnknownOp_Throws()
        {
            var operation = new Operation
            {
                OperationType = "LukeParsons",
                Values = new List<decimal> { 1, 2 }
            };
            Assert.Throws<Exception>(() => _calculatorService.Calculate(operation));
        }
    }
}
