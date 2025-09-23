using System.Reflection;
using System.Security.Cryptography.X509Certificates;

namespace HR_Carrer.Services.Utility
{
    public static class EntityUpdater
    {
        /// <summary>
        /// this is the reflction method that will check if the property is not null or empty and then will update it to same property in the destination
        /// </summary>
        /// <typeparam name="D"></typeparam>
        /// <typeparam name="S"></typeparam>
        /// <param name="destination"></param>
        /// <param name="source"></param>


        public static void UpdateEntity<D,S>(D destination , S source)
        {
            var SourceProperties = typeof(S).GetProperties(BindingFlags.Public | BindingFlags.Instance);  // get the Properties_Information of the source
            var DestinationProperties = typeof(D).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var property in SourceProperties)
            {

                var value = property.GetValue(source);

                if (value == null) continue;

#pragma warning disable S3626 // Jump statements should not be redundant
                if (value is string str && string.IsNullOrWhiteSpace(str)) continue;

                var dP = DestinationProperties.FirstOrDefault(dp=>dp.Name==property.Name);
                if (dP != null && dP.CanWrite)
                {
                    dP.SetValue(destination, value);
                }
            }

        }


    }
}
