using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AzureStorageRepository.Helper
{
    public class ReflectionPropertyMatcher
    {
        public static T SetPropertyValues<T>(object updateObject, object retrievedObject, List<string> propertiesToUpdate)
        {
            if (updateObject == null || retrievedObject == null)
            {
                return default(T);
            }
            Type firstType = updateObject.GetType();
            if (retrievedObject.GetType() != firstType)
            {
                return default(T);
            }
            foreach (PropertyInfo propertyInfo in firstType.GetProperties())
            {
                if (propertyInfo.CanRead)
                {
                    object firstValue = propertyInfo.GetValue(updateObject, null);
                    object secondValue = propertyInfo.GetValue(retrievedObject, null);
                    if (!object.Equals(firstValue, secondValue))
                    {
                        if (propertiesToUpdate == null)
                            propertyInfo.SetValue(retrievedObject,firstValue);
                        else if (propertiesToUpdate.Where(p => p.Contains(propertyInfo.Name)).Count() != 0)
                            propertyInfo.SetValue(retrievedObject, firstValue);
                    }
                }
            }
            return (T)retrievedObject;
        }
    }
}
