namespace HR_Carrer.Services
{
    public class ServiceResponce<T>
    {
       public  T? Data { get; set; } 

        public string  Message { get; set; } = string.Empty;

        public int StatusCode { get; set; } = 200;




        // .............static  method  helper............

        public static ServiceResponce<T> Fail(string message, int statusCode)
           => new ServiceResponce<T>
           {
               Data = default,
               Message = message,
               StatusCode = statusCode
           };

        public static ServiceResponce<T> success(T data,string message, int statusCode)
         => new ServiceResponce<T>
         {
             Data = data,
             Message = message,
             StatusCode = statusCode
         };

    }
}
