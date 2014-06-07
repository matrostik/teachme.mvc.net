using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeachMe.Models
{
    public enum ResultMessage
    {
        FeedbackSend,
        RegisterStepTwo,
        ConfirmationSuccess,
        ConfirmationFailure,
        ResetPasswordEmail,
        ResetPasswordCompleted,
        ResetPasswordTokenError,
        Error
    }
    public enum ResultType
    {
        info,
        success,
        warning,
        danger
    }

    public class ResultViewModel
    {
        public string Title { get; set; }

        public string Text { get; set; }

        public ResultType Type { get; set; }
    }
}