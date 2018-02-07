namespace Application.Seedwork
{
    public class AppOperationResult
    {
        private AppOperationResult()
        {
            Success = true;
        }

        private AppOperationResult(OperationErrorMessage error)
        {
            Success = false;
            Error = error;
        }

        public bool Success { get; private set; }
        public OperationErrorMessage Error { get; private set; } = new OperationErrorMessage();

        public static AppOperationResult Successful()
        {
            return new AppOperationResult();
        }

        public static AppOperationResult WithError(string developerMessage, string userMessageKey = null)
        {
            return new AppOperationResult(new OperationErrorMessage
            {
                UserMessageKey = userMessageKey,
                DeveloperMessage = developerMessage
            });
        }

        public void SetWithError(string developerMessage, string userMessageKey = null)
        {
            if (Success)
            {
                Success = false;
                Error.UserMessageKey = userMessageKey;
                Error.DeveloperMessage = developerMessage;
            }
        }

        public void SetSuccessful()
        {
            Success = true;
        }
    }

    public class AppOperationResult<TResult>
    {
        private AppOperationResult(TResult result)
        {
            Success = true;
            Result = result;
        }

        private AppOperationResult(OperationErrorMessage error)
        {
            Success = false;
            Error = error;
        }

        public bool Success { get; private set; }
        public OperationErrorMessage Error { get; private set; } = new OperationErrorMessage();
        public TResult Result { get; private set; }

        public static AppOperationResult<TResult> Successful(TResult result)
        {
            return new AppOperationResult<TResult>(result);
        }

        public static AppOperationResult<TResult> WithError(string developerMessage, string userMessageKey = null)
        {
            return new AppOperationResult<TResult>(new OperationErrorMessage
            {
                UserMessageKey = userMessageKey,
                DeveloperMessage = developerMessage
            });
        }

        public void SetWithError(string developerMessage, string userMessageKey = null)
        {
            if (Success)
            {
                Success = false;
                Error.UserMessageKey = userMessageKey;
                Error.DeveloperMessage = developerMessage;
            }
        }

        public void SetSuccessful()
        {
            Success = true;
        }
    }

    public class OperationErrorMessage
    {
        public string UserMessageKey { get; set; }
        public string DeveloperMessage { get; set; }
    }
}