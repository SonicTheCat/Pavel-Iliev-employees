namespace PairOfEmployeesWeb.Services
{
    /// <summary>
    /// Generic file reader.
    /// </summary>
    /// <typeparam name="T">Class of type T</typeparam>
    public interface IFileReaderService<T> where T : class
    {
        IEnumerable<T> Read(Stream stream);
    }
}