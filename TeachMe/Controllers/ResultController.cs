using System.Web.Mvc;
using TeachMe.Models;

namespace TeachMe.Controllers
{
    public class ResultController : Controller
    {
        //
        // GET: /Result/
        public ActionResult Index(ResultMessageId? message)
        {
            var model = new ResultViewModel();
            switch (message)
            {
                case ResultMessageId.Error:
                    model.Title = "Error!";
                    model.Text = "OOPS... Something gone wrong..";
                    break;
                case ResultMessageId.FeedbackSend:
                    model.Title = "Thank you!";
                    model.Text = "You feedback sent.";
                    break;
                default:
                    model.Title = "Hey!";
                    model.Text = "Easy easy its a wrong turn..";
                    break;
            }
            return View(model);
        }
       
	}
    public enum ResultMessageId
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
}