using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IError
    {
        /// <summary>
        /// Error Code
        /// </summary>
        int Code { get; }

        /// <summary>
        /// Error Description Message
        /// </summary>
        string Message { get; }

        /// <summary>
        /// Formatted message that combines <see cref="Code"/> and <see cref="Message"/>
        /// </summary>
        string FullMessage { get; }
    }