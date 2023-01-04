using Polly.Retry;
using Polly;

namespace SpecFlowTests.Support
{
    public static class PollyExtensions
    {
        public static AsyncRetryPolicy<TResult> WaitAndRetry5TimesAsync<TResult>(this PolicyBuilder<TResult> policyBuilder)
        {
            return policyBuilder.WaitAndRetryAsync(5, retryAttempt =>
                    TimeSpan.FromMilliseconds(Math.Pow(2, retryAttempt) * 100)
                );
        }
    }
}
