using Newtonsoft.Json;
namespace OpenGameListWebApp.ViewModels
{
    [JsonObject(MemberSerialization.OptOut)] // 此為預設，代表除了被標記為JsonIgnoreAttribute的所有成員都將被序列化，可以不需要設定
    public class UserViewModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string PasswordNew { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
    }
}