namespace MongoAdmin.Models
{
    public class GenericResponse
    {
        public int response_code { get; set; }
        public string response_message { get; set; }
        public dynamic data { get; set; }
    }
}
