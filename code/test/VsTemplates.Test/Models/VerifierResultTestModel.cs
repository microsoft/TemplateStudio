namespace VsTemplates.Test.Models
{
    public class VerifierResultTestModel
    {
        public VerifierResultTestModel()
        {
        }

        public VerifierResultTestModel(bool success, string message)
            : this()
        {
            Success = success;
            Message = message;
        }

        public bool Success { get; set; }

        public string Message { get; set; }

    }
}
