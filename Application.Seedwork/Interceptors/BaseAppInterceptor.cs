using Domain.Seedwork.CoreAttributes;

namespace Application.Seedwork.Interceptors
{
    public abstract class BaseAppInterceptor: InterceptorAttribute
    {
        protected void ReturnFailedOperationResult(string developerMessage, string userMessageKey = null)
        {
            var result = GetFailedOperationResult(developerMessage, userMessageKey);
            ForceReturn(result);
        }

        private object GetFailedOperationResult(string developerMessage, string userMessageKey)
        {
            var returnType = GetInterceptedMethodReturnType();
            var method = returnType.GetMethod("WithError");
            var result = method.Invoke(null, new object[] { developerMessage, userMessageKey });
            return result;
        }        
    }
}