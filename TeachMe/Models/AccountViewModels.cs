using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TeachMe.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required(ErrorMessage = "* יש להכניס שם פרטי")]
        [Display(Name = "שם פרטי")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "* יש להכניס שם משפחה")]
        [Display(Name = "שם משפחה")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "דוא\"ל")]
        [EmailAddress(ErrorMessage = "דוא\"ל שגוי")]
        public string UserName { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [Display(Name = "דוא\"ל")]
        [EmailAddress(ErrorMessage = "דוא\"ל שגוי")]
        public string UserName { get; set; }
    }

    public class ResetPasswordStepTwoViewModel
    {
        public string UserId { get; set; }

        [Required(ErrorMessage = "* יש להכניס סיסמא חדשה")]
        [StringLength(100, ErrorMessage = "ה{0} חייבת להכיל {2} תווים לפחות.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "סיסמא חדשה")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "* יש להכניס אימות סיסמא")]
        [DataType(DataType.Password)]
        [Display(Name = "אימות סיסמא")]
        [Compare("NewPassword", ErrorMessage = "הסיסמא החדשה ואימות הסיסמא החדשה לא תואמים.")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginViewModel
    {
        [Required(ErrorMessage = "* יש להכניס שם משתמש")]
        [Display(Name = "שם משתמש")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "* יש להכניס סיסמא")]
        [DataType(DataType.Password)]
        [Display(Name = "סיסמא")]
        public string Password { get; set; }

        [Display(Name = "זכור אותי")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessage = "* יש להכניס דוא\"ל")]
        [Display(Name = "דוא\"ל")]
        [EmailAddress(ErrorMessage = "דוא\"ל שגוי")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "* יש להכניס שם פרטי")]
        [Display(Name = "שם פרטי")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "* יש להכניס שם משפחה")]
        [Display(Name = "שם משפחה")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "* יש להכניס סיסמא")]
        [StringLength(100, ErrorMessage = "ה{0} חייבת להכיל {2} תווים לפחות.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "סיסמא")]
        public string Password { get; set; }

        [Required(ErrorMessage = "* יש להכניס אימות סיסמא")]
        [DataType(DataType.Password)]
        [Display(Name = "אימות סיסמא")]
        [System.Web.Mvc.Compare("Password", ErrorMessage = "הסיסמא ואימות הסיסמא לא תואמים.")]
        public string ConfirmPassword { get; set; }

        // Return a pre-poulated instance of AppliationUser:
        public ApplicationUser GetUser()
        {
            var user = new ApplicationUser()
            {
                UserName = this.UserName,
                FirstName = this.FirstName,
                LastName = this.LastName,
            };
            return user;
        }
    }

    public class SelectUserRolesViewModel
    {
        public SelectUserRolesViewModel()
        {
            this.Roles = new List<SelectRoleEditorViewModel>();
        }

        // Enable initialization with an instance of ApplicationUser:
        public SelectUserRolesViewModel(ApplicationUser user)
            : this()
        {
            this.UserName = user.UserName;
            this.FirstName = user.FirstName;
            this.LastName = user.LastName;

            var Db = new TeachMeDBContext();

            // Add all available roles to the list of EditorViewModels:
            var allRoles = Db.Roles;
            foreach (var role in allRoles)
            {
                // An EditorViewModel will be used by Editor Template:
                var rvm = new SelectRoleEditorViewModel(role);
                this.Roles.Add(rvm);
            }

            // Set the Selected property to true for those roles for 
            // which the current user is a member:
            foreach (var userRole in user.Roles)
            {
                //var checkUserRole = this.Roles.Find(r => r.RoleName == userRole.Role.Name);
                //checkUserRole.Selected = true;
            }
        }

        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<SelectRoleEditorViewModel> Roles { get; set; }
    }

    // Used to display a single role with a checkbox, within a list structure:
    public class SelectRoleEditorViewModel
    {
        public SelectRoleEditorViewModel() { }
        public SelectRoleEditorViewModel(IdentityRole role)
        {
            this.RoleName = role.Name;
        }

        public bool Selected { get; set; }

        [Required]
        public string RoleName { get; set; }
    }

    public class ManageUserViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "יש להכניס סיסמא נוכחית")]
        [DataType(DataType.Password)]
        [Display(Name = "סיסמא נוכחית")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "יש להכניס סיסמא חדשה")]
        [StringLength(100, ErrorMessage = "ה{0} חייבת להכיל {2} תווים לפחות.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name ="סיסמא חדשה")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "יש להכניס סיסמא חדשה שנית")]
        [DataType(DataType.Password)]
        [Display(Name = "אימות סיסמא")]
        [Compare("NewPassword", ErrorMessage = "הסיסמא החדשה ואימות הסיסמא החדשה לא תואמים.")]
        public string ConfirmPassword { get; set; }

    }

}
