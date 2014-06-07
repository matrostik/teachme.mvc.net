using System.Web.Mvc;
using TeachMe.Models;

namespace TeachMe.Controllers
{
    public class ResultController : Controller
    {
        //
        // GET: /Result/
        public ActionResult Index(ResultMessage? message, string userName)
        {
            var model = new ResultViewModel();
            switch (message)
            {
                case ResultMessage.RegisterStepTwo:
                    model.Title = "הוראות הרשמה";
                    model.Text = "להשלמת הרישום, נא כנס לדוא\"ל שלך להמשך הרשמה.";
                    model.Type = ResultType.info;
                    break;
                case ResultMessage.ConfirmationSuccess:
                    model.Title = "ההרשמה הושלמה";
                    model.Text = "השלמת את תהליך הרישום.";
                    model.Type = ResultType.success;
                    break;
                case ResultMessage.ConfirmationFailure:
                    model.Title = "שגיאה בהרשמה";
                    model.Text = "הייתה שגיאה באישור הדוא\"ל שלך, אנא נסה שוב.";
                    model.Type = ResultType.danger;
                    break;
                case ResultMessage.ResetPasswordEmail:
                    model.Title = "בקשת שחזור סיסמא נשלחה לכתובת הדוא\"ל " + userName;
                    model.Text = "אם אינך רואה הודעה זו בתיבת הדואר הנכנס שלך בתוך 15 דקות, חפש אותה בתיקיית דואר זבל שלך. אם אתה מוצא אותו שם, נא סמן אותה כ-לא זבל. ";
                    model.Type = ResultType.info;
                    break;
                case ResultMessage.ResetPasswordCompleted:
                    model.Title = "שחזור סיסמא הושלם";
                    model.Text = "אתה יכול להתחבר לאתר עם הסיסמא החדשה";
                    model.Type = ResultType.success;
                    break;
                case ResultMessage.ResetPasswordTokenError:
                    model.Title = "הלינק שהשתמשת לא נכון.";
                    model.Text = "אנא כנס ל-  <a href=\"/Account/ResetPassword\"> לאפס את הסיסמה של הדף </ a> כדי לשלוח עוד אחד, או פנה למחלקת תמיכת href=\"/Home/Contact\"> </ a>.";
                    model.Type = ResultType.danger;
                    break;
                case ResultMessage.Error:
                    model.Title = "שגיאה!";
                    model.Text = "אופס...משהו השתבש";
                    model.Type = ResultType.danger;
                    break;
                case ResultMessage.FeedbackSend:
                    model.Title = "תודה רבה!";
                    model.Text = "המשוב נשלח.";
                    model.Type = ResultType.success;
                    break;
                default:
                    model.Title = "הי!";
                    model.Text = "לאט לאט...";
                    model.Type = ResultType.danger;
                    break;
            }
            return View(model);
        }
       
	}
   
}