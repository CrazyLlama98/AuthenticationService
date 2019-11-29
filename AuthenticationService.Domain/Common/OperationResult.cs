using System.Collections.Generic;

namespace AuthenticationService.Domain.Common
{
    public class OperationResult
    {
        public bool IsSuccessful { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }

    public class OperationResult<TData> : OperationResult
        where TData : class
    {
        public TData Data { get; set; }
    }
}
