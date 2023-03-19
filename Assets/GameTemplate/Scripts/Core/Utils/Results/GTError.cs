
using System;
using UnityEngine;


/// <inheritdoc />
[Serializable]
public class GTError : IError
{
    [SerializeField]
    int m_code;
    [SerializeField]
    string m_message = string.Empty;

    //--------------------------------------
    // Initialization
    //--------------------------------------

    /// <summary>
    /// Initializes a new instance of the <see cref="GTError"/> class,
    /// with predefined <see cref="Code"/> and <see cref="Message"/> s
    /// </summary>
    /// <param name="code">instance error <see cref="Code"/>.</param>
    /// <param name="message">instance error <see cref="Message"/>.</param>
    public GTError(int code, string message = "")
    {
        m_code = code;
        m_message = message;
    }

    //--------------------------------------
    // Get / Set
    //--------------------------------------

    public int Code => m_code;

    public string Message => m_message;

    public string FullMessage
    {
        get
        {
            if (Message.StartsWith(Code.ToString()))
                return Message;
            else
                return Code + "::" + Message;
        }
    }
}
