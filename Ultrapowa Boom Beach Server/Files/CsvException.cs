using System;

namespace UCS.Files
{
    /// <summary>
    /// Exception that is raised when there is an error with a CSV object.
    /// </summary>
    public class CsvException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CsvException"/> class.
        /// </summary>
        public CsvException() : base()
        {
            // Space
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvException"/> class with the specified error message.
        /// </summary>
        /// <param name="message">Message describing the error.</param>
        public CsvException(string message) : base(message)
        {
            // Space
        }
    }
}
