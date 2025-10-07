using HR_Carrer.CustomValidation;
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



        public static void UpdateEntity<D, S>(D destination, S source)
        {
            var sourceProperties = typeof(S).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var destinationProperties = typeof(D).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in sourceProperties)
            {
                var value = property.GetValue(source);
                if (value == null) continue;

                // تجاهل الفراغات أو "string" الوهمية
                if (value is string str && (string.IsNullOrWhiteSpace(str) || str.Trim().ToLower() == "string"))
                    continue;

                if(value is int && (int)value == 0) { continue; }

                var dP = destinationProperties.FirstOrDefault(dp => dp.Name == property.Name);
                if (dP == null || !dP.CanWrite) continue;

                var destType = dP.PropertyType;
                var sourceType = property.PropertyType;

                if (destType == sourceType)
                {
                    dP.SetValue(destination, value);
                    continue;
                }

                if (destType.IsEnum && value is string stringValue)
                {
                    var enumValue = Enum.Parse(destType, stringValue, true);
                    dP.SetValue(destination, enumValue);
                    continue;
                }

                if (sourceType.IsEnum && destType == typeof(string))
                {
                    dP.SetValue(destination, value.ToString());
                    continue;
                }
                

                try
                {
                    var convertedValue = Convert.ChangeType(value, destType);
                    dP.SetValue(destination, convertedValue);
                }
                catch
                {
                    // تجاهل لو ما نقدر نحول
                }
            }
        }



    }
}
