using ismetertugral.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;

namespace ismetertugral.Identity.TagHelpers
{
    [HtmlTargetElement("getUserInfo")]
    public class GetUserInfo : TagHelper
    {
        private readonly UserManager<AppUser> _userManger;
        public GetUserInfo(UserManager<AppUser> userManger)
        {
            _userManger = userManger;
        }

        public int UserId { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var html = "";

            var user = await _userManger.Users.SingleOrDefaultAsync(x => x.Id == UserId);

            var roles = await _userManger.GetRolesAsync(user);

            foreach (var role in roles)
            {
                html += role + " ";
            }

            output.Content.SetHtmlContent(html);
        }
    }
}
