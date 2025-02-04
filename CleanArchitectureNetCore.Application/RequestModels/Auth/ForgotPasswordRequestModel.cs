namespace CleanArchitectureNetCore.Application.RequestModels.Auth
{
    public class ForgotPasswordRequestModel
    {
        public string Email { get; set; }
        public bool IsPatient { get; set; }

    }
}
