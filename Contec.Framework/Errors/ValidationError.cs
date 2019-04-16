namespace Contec.Framework.Errors
{
    public class ValidationError
    {
        public ValidationError(string itemName, string errorMessage)
        {
            ItemName = itemName;
            ErrorMessage = errorMessage;
        }

        public string ItemName { get; set; }

        public string ErrorMessage { get; set; }
    }
}