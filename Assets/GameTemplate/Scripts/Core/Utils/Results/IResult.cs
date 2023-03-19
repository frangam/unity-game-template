using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IResult
    {
        /// <summary>
        /// Sets the error to result.
        /// </summary>
        ///  /// <param name="error">A predefined result error object.</param>
        void SetError(GTError error);

        /// <summary>
        /// Convert to the result json string.
        /// </summary>
        string ToJson();

        /// <summary>
        /// Gets the result error object. If Error message is empty,
        /// result is succeeded.
        /// </summary>
        GTError Error { get; }

        /// <summary>
        /// Gets a value indicating whether this Result has error.
        /// </summary>
        /// <value><c>true</c> if has error; otherwise, <c>false</c>.</value>
        bool HasError { get; }

        /// <summary>
        /// Gets a value indicating whether this Result is succeeded.
        /// </summary>
        /// <value><c>true</c> if is succeeded; otherwise, <c>false</c>.</value>
        bool IsSucceeded { get; }

        /// <summary>
        /// Gets a value indicating whether this Result is failed.
        /// </summary>
        /// <value><c>true</c> if is failed; otherwise, <c>false</c>.</value>
        bool IsFailed { get; }

        /// <summary>
        /// Returns the id of the player who made the request in order to create correct data references
        /// </summary>
        string RequestId { get; }
    }
