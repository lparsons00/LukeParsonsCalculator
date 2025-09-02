using LukeParsonsCalculator.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using LukeParsonsCalculator.Services.Interfaces;

namespace LukeParsonsCalculator.Services
{
    public class CalculatorService : ICalculatorService
    {
        private readonly IMathOperationFactory _operationFactory;

        public CalculatorService(IMathOperationFactory operationFactory)
        {
            _operationFactory = operationFactory;
        }

        public CalculationResult Calculate(Operation op)
        {
            if (op == null)
            {
                throw new ArgumentNullException(nameof(op));
            }
            var allValues = GetAllValues(op);
            //create operation requested
            var mathOperation = _operationFactory.CreateOperation(op.OperationType);
            //calc resuylt
            var result = mathOperation.Calculate(allValues.ToArray());


            return new CalculationResult { Result = result };
        }



        private List<decimal> GetAllValues(Operation op)
        {
            var values = new List<decimal>();

            //non nested
            if(op.Values != null && op.Values.Any())
            {
                values.AddRange(op.Values);
            }
            //nested
            if(op.NestedOperation != null)
            {
                var nestedOperation = Calculate(op.NestedOperation);
                values.Add(nestedOperation.Result);
            }

            return values;
        }

    }
}
