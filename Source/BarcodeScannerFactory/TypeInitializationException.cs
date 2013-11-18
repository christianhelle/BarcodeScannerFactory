#region License

// The MIT License (MIT)
// 
// Copyright (c) 2013 Christian Resma Helle
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of
// this software and associated documentation files (the "Software"), to deal in
// the Software without restriction, including without limitation the rights to
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
// the Software, and to permit persons to whom the Software is furnished to do so,
// subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
// FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
// IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
// CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

#endregion

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