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
                    break;
                case ResultMessage.ConfirmationSuccess:
                    model.Title = "ההרשמה הושלמה";
                    model.Text = "השלמת את תהליך הרישום.";
                    break;
                case ResultMessage.ConfirmationFailure:
                    model.Title = "שגיאה בהרשמה";
                    model.Text = "הייתה שגיאה באישור הדוא\"ל שלך, אנא נסה שוב.";
                    break;
                case ResultMessage.ResetPasswordEmail:
                    model.Title = "בקשת שחזור סיסמא נשלחה לכתובת הדוא\"ל " + userName;
                    model.Text = "אם אינך רואה הודעה זו בתיבת הדואר הנכנס שלך בתוך 15 דקות, חפש אותו בתיקיית דואר זבל שלך. אם אתה מוצא אותו שם, נא סמן אותה כ-לא זבל. ";
                    break;
                case ResultMessage.ResetPasswordCompleted:
                    model.Title = "שחזור סיסמא הושלם";
                    model.Text = "אתה יכול להתחבר עם הסיסמא החדשה";
                    break;
                case ResultMessage.ResetPasswordTokenError:
                    model.Title = "הלינק שהשתמשת לא נכון.";
                    model.Text = "אנא כנס ל-  <a href=\"/Account/ResetPassword\"> לאפס את הסיסמה של הדף </ a> כדי לשלוח עוד אחד, או פנה למחלקת תמיכת href=\"/Home/Contact\"> </ a>.";
                    break;
                case ResultMessage.Error:
                    model.Title = "שגיאה!";
                    model.Text = "אופס...משהו השתבש";
                    break;
                case ResultMessage.FeedbackSend:
                    model.Title = "תודה רבה!";
                    model.Text = "המשוב נשלח.";
                    break;
                default:
                    model.Title = "הי!";
                    model.Text = "לאט לאט...";
                    break;
            }
            return View(model);
        }
       
	}
   
}