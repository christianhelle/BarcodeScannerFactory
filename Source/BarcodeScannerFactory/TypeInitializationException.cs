#region Imported Namespaces

using System;

#endregion

namespace ChristianHelle.Barcode
{
    /// <summary>
    /// Exception thrown if the specified type could not be loaded
    /// </summary>
    public class TypeInitializationException : TypeLoadException
    {
        /// <summary>
        /// Initializes a new instance of the TypeInitializationException class
        /// </summary>
        public TypeInitializationException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the TypeInitializationException class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error</param>
        public TypeInitializationException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the TypeInitializationException class with a specified
        /// error message and a reference to the inner exception that is the cause of
        /// this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="inner">
        /// The exception that is the cause of the current exception. If the inner parameter
        /// is not a null reference (Nothing in Visual Basic), the current exception
        /// is raised in a catch block that handles the inner exception.
        /// </param>
        public TypeInitializationException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}