using ClassLibrary.Common;
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
            List<string> messageResults = new();

            foreach (var item in enumerable)
            {
                if (item != null)
                {
                    Type type = item.GetType();
                    Delegate dynamicExpression = Expression.Lambda(expression.Body, expression.Parameters).Compile();
                    bool result = Convert.ToBoolean(dynamicExpression.DynamicInvoke(item));

                    if (result)
                        messageResults.Add($"Entity: {{\"{item.GetType().Name}\"}} Record: {JsonConvert.SerializeObject(item)}");
                }

                yield return item;
            }

            if (Logger != null)
            {
                string message = $"LogRecord Result(s):{AsciiCodes.CRLF}{AsciiCodes.CRLF}";
                foreach (string item in messageResults)
                    message += $"{item}{AsciiCodes.CRLF}";

                Logger.LogDebug(message);
            }
        }

        public static IEnumerable<T> LogAllRecords<T>(this IEnumerable<T> enumerable)
        {
            List<string> messageResults = new();

            foreach (var item in enumerable)
            {
                if (item != null)
                    messageResults.Add($"Entity: {{\"{item.GetType().Name}\"}} Record: {JsonConvert.SerializeObject(item)}");

                yield return item;
            }

            if (Logger != null)
            {
                string message = $"LogAllRecords Result(s):{AsciiCodes.CRLF}{AsciiCodes.CRLF}";
                foreach (string item in messageResults)
                    message += $"{item}{AsciiCodes.CRLF}";

                Logger.LogDebug(message);
            }
        }
    }
}
