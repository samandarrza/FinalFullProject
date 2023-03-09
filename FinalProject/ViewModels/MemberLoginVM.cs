using System.ComponentModel.DataAnnotations;

namespace FinalProject.ViewModels
{
	public class MemberLoginVM
	{
        [MaxLength(25)]
        public string UserName { get; set; }
        [MaxLength(20)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
