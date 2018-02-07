using System.Collections.Generic;

namespace Application.Seedwork.Interceptors
{
    public delegate AppOperationResult Validator();

    public abstract class BaseAppValidationInterceptor : BaseAppInterceptor
    {
        private AppOperationResult _result;

        protected override void Run()
        {
            Initialize();
            Validate();

            if (_result.Success)
            {
                ContinueExecution();
            }
            else
            {
                ReturnFailedOperationResult(_result.Error.DeveloperMessage, _result.Error.UserMessageKey);
            }
        }

        protected abstract void Initialize();
        protected abstract IEnumerable<Validator> GetValidations();

        private void Validate()
        {
            _result = AppOperationResult.Successful();
            var validationsEnumerator = GetValidations().GetEnumerator();
            RunValidations(validationsEnumerator);
        }

        private void RunValidations(IEnumerator<Validator> validationsEnumerator)
        {
            while (_result.Success && validationsEnumerator.MoveNext())
            {
                var validation = validationsEnumerator.Current;
                if (validation != null)
                {
                    _result = validation();
                }
            }
        }
    }
}