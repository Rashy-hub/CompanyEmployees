using Microsoft.AspNetCore.Mvc.Formatters;
using System.Text;
using System.Reflection;

public class CsvOutputFormatter : TextOutputFormatter
{
    public CsvOutputFormatter()
    {
        SupportedMediaTypes.Add("text/csv");
        SupportedEncodings.Add(Encoding.UTF8);
    }

    protected override bool CanWriteType(Type? type)
    {
        // Le formatter accepte TOUT sauf les types primitifs
        if (type == null)
            return false;

        return true;
    }

    public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
    {
        var response = context.HttpContext.Response;
        var buffer = new StringBuilder();

        if (context.Object == null)
        {
            await response.WriteAsync("");
            return;
        }

        if (context.Object is IEnumerable<object> collection)
        {
            WriteCollection(buffer, collection);
        }
        else
        {
            WriteSingle(buffer, context.Object);
        }

        await response.WriteAsync(buffer.ToString());
    }

    private void WriteCollection(StringBuilder buffer, IEnumerable<object> items)
    {
        var first = items.FirstOrDefault();
        if (first == null) return;

        var props = first.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

        // Header
        buffer.AppendLine(string.Join(",", props.Select(p => Escape(p.Name))));

        foreach (var item in items)
        {
            WriteItem(buffer, item, props);
        }
    }

    private void WriteSingle(StringBuilder buffer, object item)
    {
        var props = item.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

        // Header
        buffer.AppendLine(string.Join(",", props.Select(p => Escape(p.Name))));

        WriteItem(buffer, item, props);
    }

    private void WriteItem(StringBuilder buffer, object item, PropertyInfo[] props)
    {
        var values = props.Select(p =>
        {
            var v = p.GetValue(item);
            return Escape(v?.ToString() ?? "");
        });

        buffer.AppendLine(string.Join(",", values));
    }

    private string Escape(string input)
    {
        // CSV RFC 4180 compliant escaping 
        if (input.Contains("\""))
            input = input.Replace("\"", "\"\"");

        if (input.Contains(",") || input.Contains("\"") || input.Contains("\n"))
            input = $"\"{input}\"";

        return input;
    }
}
