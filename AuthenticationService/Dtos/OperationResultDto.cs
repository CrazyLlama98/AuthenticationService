using System.Collections.Generic;

namespace AuthenticationService.Dtos
{
    public class OperationResultDto
    {
        public bool IsSuccessful { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }

    public class OperationResultDto<TData> : OperationResultDto
        where TData : class
    {
        public TData Data { get; set; }
    }
}
