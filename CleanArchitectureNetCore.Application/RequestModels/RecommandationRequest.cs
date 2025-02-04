namespace CleanArchitectureNetCore.Application.RequestModels
{
    public class RecommandationRequest
    {
        public string Description { get; set; }
        public bool? IsCompleted { get; set; }
    }
}
