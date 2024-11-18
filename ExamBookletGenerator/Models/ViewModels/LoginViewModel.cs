
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ExamBookletGenerator.Models.ViewModels;

public class LoginViewModel
{
    [Required]
    public string UserName { get; set; }

    [Required]
    [PasswordPropertyText]
    public string Password { get; set; }

    public string captcha { get; set; }
}
