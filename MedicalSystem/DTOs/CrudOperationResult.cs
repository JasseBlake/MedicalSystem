namespace MedicalSystem.DTOs
{
    public class CrudOperationResult<TDto>
    {
        public CrudOperationResultStatus Status { get; set; }
        public TDto? Result { get; set; }
        public string? ErrorMessage { get; set; }  
    }

    public enum CrudOperationResultStatus
    {
        Success,
        Failure,
        RecordNotFound
    }
}