namespace RannaTask.Business
{
    public class NoContent
    {
        public NoContent()
        {
            
        }
        public NoContent(string message)
        {
            Message = message;
        }

        public string Message { get; set; }
    }
}