using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Linq.Expressions;

namespace System.Collections.Generic
{
    public static class IEnumerableExtentions
    {
        public static ILogger? Logger { get; set; }

        public static IEnumerable<T> LogRecord<T>(this IEnumerable<T> enumerable, Expression<Func<T, bool>> expression)
        {
            foreach (var item in enumerable)
            {
                if (item != null)
                {
                    Type type = item.GetType();
                    Delegate dynamicExpression = Expression.Lambda(expression.Body, expression.Parameters).Compile();
                    bool result = Convert.ToBoolean(dynamicExpression.DynamicInvoke(item));

                    if (result)
                        if (Logger != null)
                            Logger.LogDebug($"LogRecord Result => Entity: {type.Name} Record: {JsonConvert.SerializeObject(item)}");
                }

                yield return item;
            }
        }

        public static IEnumerable<T> LogAllRecords<T>(this IEnumerable<T> enumerable)
        {
            foreach (var item in enumerable)
            {
                if (item != null)
                    if (Logger != null)
                        Logger.LogDebug($"LogRecord Result => Entity: {item.GetType().Name} Record: {JsonConvert.SerializeObject(item)}");

                yield return item;
            }
        }
    }
}
