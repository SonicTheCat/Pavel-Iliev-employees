using PairOfEmployeesWeb.Utils;
using System.Globalization;
using System.Reflection;

namespace PairOfEmployeesWeb.Services
{
    /// <summary>
    /// Generic csv file reader.
    /// It maps a line from the file to an instance of the provided generic typeparam.
    /// </summary>
    /// <typeparam name="T">Class of type T</typeparam>
    public class CsvFileReaderService<T> : IFileReaderService<T> where T : class
    {
        private readonly string[] separators = new string[] { ",", "\"", Environment.NewLine };

        public IEnumerable<T> Read(Stream stream)
        {
            var result = new List<T>();

            using (var reader = new StreamReader(stream))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (line == null)
                    {
                        break;
                    }

                    var values = line.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                    var mapped = this.MapValuesToModel(values);
                    result.Add(mapped);
                }
            }

            return result;
        }

        private T MapValuesToModel(string[] values)
        {
            var instance = Activator.CreateInstance(typeof(T));
            if (instance == null)
            {
                throw new CsvReaderException(Constants.InvalidModelTypeMessage);
            }

            foreach (var prop in typeof(T).GetProperties())
            {
                var attr = prop.GetCustomAttribute(typeof(CsvMapperAttribute));
                if (attr == null)
                {
                    continue;
                }

                var csvMapperAttr = (CsvMapperAttribute)attr;
                var order = csvMapperAttr.Order;
                var canBeSendAsNull = csvMapperAttr.CanBeSendAsNull;

                if (canBeSendAsNull && order >= values.Length)
                {
                    continue;
                }

                if (!canBeSendAsNull && order >= values.Length)
                {
                    throw new CsvReaderException(Constants.InvalidOrderMessage);
                }

                this.SetValueToProp((T)instance, prop, values[order]);
            }

            return (T)instance;
        }

        private void SetValueToProp(T instance, PropertyInfo prop, string value)
        {
            value = value.Trim();

            if (prop.PropertyType == typeof(int))
            {
                this.ProcessInteger(instance, prop, value);
            }
            else if (prop.PropertyType == typeof(DateOnly))
            {
                this.ProcessDateOnly(instance, prop, value);
            }
        }

        private void ProcessInteger(T instance, PropertyInfo prop, string value)
        {
            var isInt = int.TryParse(value, out int result);
            if (!isInt)
            {
                throw new CsvReaderException(Constants.InvalidParameterMessage);
            }

            prop.SetValue(instance, result);
        }

        private void ProcessDateOnly(T instance, PropertyInfo prop, string value)
        {
            var isDate = DateOnlyParser.Parse(value);
            if (!isDate.HasValue)
            {
                throw new CsvReaderException(Constants.InvalidParameterMessage);
            }

            prop.SetValue(instance, isDate.Value);
        }
    }
}