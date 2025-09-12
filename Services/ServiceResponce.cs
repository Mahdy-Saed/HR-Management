namespace HR_Carrer.Services
{
    public class ServiceResponce<T>
    {
       public  T? Data { get; set; } 

        public string  Message { get; set; } = string.Empty;

        public int StatusCode { get; set; } = 200;

    }
}
