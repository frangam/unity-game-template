using System;
using UnityEngine;

/// <inheritdoc />
    [Serializable]
    public class GTResult : IResult
    {
        [SerializeField]
        protected GTError m_error = null;
        [SerializeField]
        protected string m_requestId = string.Empty;

        //--------------------------------------
        // Initialization
        //--------------------------------------

        /// <summary>
        /// Initializes a new instance of the <see cref="GTResult"/> class.
        /// </summary>
        public GTResult() { }

        public GTResult(IResult result)
        {
            m_error = result.Error;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GTResult"/> class with predefined error
        /// </summary>
        /// <param name="error">A predefined result error object.</param>
        public GTResult(GTError error)
        {
            SetError(error);
        }

        //--------------------------------------
        // Public Methods
        //--------------------------------------

        public void SetError(GTError error)
        {
            m_error = error;
        }

        //--------------------------------------
        // Get / Set
        //--------------------------------------

        public GTError Error => m_error;

        public bool HasError
        {
            get
            {
                if (m_error == null || string.IsNullOrEmpty(m_error.Message) && Error.Code == default(int)) return false;

                return true;
            }
        }

        public bool IsSucceeded => !HasError;

        public bool IsFailed => HasError;

        public string RequestId
        {
            get => m_requestId;
            set => m_requestId = value;
        }

        public string ToJson()
        {
            return JsonUtility.ToJson(this);
        }
    }