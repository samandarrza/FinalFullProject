using System.ComponentModel.DataAnnotations;

namespace FinalProject.Areas.admin.ViewModels
{
	public class AdminLoginViewModel
	{
        [MaxLength(25)]
        public string UserName { get; set; }
        [MaxLength(20)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool IsPersist { get; set; }
    }
}
