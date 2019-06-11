using MRApiCommon.Infrastructure.Interface;
using System;
using System.Collections.Generic;

namespace MRApiCommon.Exception
{
    /// <summary>
    /// Standart exception type for MR applications
    /// </summary>
    /// <typeparam name="T">Any class to use it in response body</typeparam>
    public class MRException<T> : System.Exception, IMRException
        where T : class, new()
    {
        /// <summary>
        /// Code of error
        /// </summary>
        public int Code { get; set; }
        
        /// <summary>
        /// Code desciprion (addition to message)
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Exception body
        /// </summary>
        public T Body { get; set; }

        /// <summary>
        /// Exception created time
        /// </summary>
        public DateTime CreatedTime { get; set; }

        /// <summary>
        /// Empty constructor
        /// </summary>
        public MRException() { }

        public MRException(int code, string message, string description, T body, System.Exception innerException) : base(message, innerException)
        {
            Code = code;
            Description = description;
            Body = body;
        }

        public MRException(int code, string message, T body, System.Exception innerException) : this(code, message, null, body, innerException) { }
        public MRException(int code, string message, T body) : this(code, message, null, body, null) { }
        public MRException(int code, string message, System.Exception innerException) : this(code, message, null, null, innerException) { }
        public MRException(int code, string message) : this(code, message, null, null, null) { }
        public MRException(int code) : this(code, code.ToString(), null, null, null) { }

        /// <summary>
        /// Transform exception to Api Response
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, dynamic> ToDictionaryShort()
        {
            var result = new Dictionary<string, dynamic>
            {
                { "code", Code.ToString() },
                { "message", Message },
                { "created_time", CreatedTime }
            };

            if (!string.IsNullOrWhiteSpace(Description))
            {
                result["description"] = Description;
            }

            if (Body != null)
            {
                result["body"] = Body;
            }

            return result;
        }

        /// <summary>
        /// For logs
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, dynamic> ToDictionary()
        {
            var result = ToDictionaryShort();

            result["type"] = GetType().Name;
            result["stack_trace"] = StackTrace;

            if (!string.IsNullOrWhiteSpace(HelpLink))
            {
                result["help_link"] = HelpLink;
            }

            // add inner exception
            var exception = InnerException;
            var dictionary = result;
            while (exception != null)
            {
                var innerException = new Dictionary<string, dynamic>
                {
                    { "type", exception.GetType().Name },
                    { "message", exception.Message },
                    { "stack_trace", exception.StackTrace },
                    { "help_link", exception.HelpLink },
                    { "h_result", exception.HResult },
                };

                exception = exception.InnerException;
                dictionary["inner_exception"] = innerException;
                dictionary = innerException;
            }

            return result;
        }
    }
}
