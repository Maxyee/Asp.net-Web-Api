using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Script.Serialization;
using glostars.Models;
using glostars.Models.Api;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace glostars.Controllers.Api
{
    [EnableCors("*", "*", "*")]
    //I'm going in favor of having a separate controller for the API system as the flows are quite different between an API access and conventional MVC access
    [System.Web.Mvc.Authorize]
    [System.Web.Mvc.RoutePrefix("api/account")]
    public class AccountController : ApiController
    {      
        
        [HttpDelete]
        [AllowAnonymous]
        public async Task<HttpResponseMessage> DeleteUser(string email)
        {
            var model1 = _db.Users.FirstOrDefault(x => x.Email == email);
            if (model1 == null)
            {
                var responseMsg = new HttpResponseMessage
                {
                    Content = new StringContent(new JavaScriptSerializer().Serialize(new
                    {
                        res = "Ujjal vai be carefull when you deleting any mail",
                        msg = "No user found to delete"
                    }), Encoding.UTF8, "application/json")
                };
                return responseMsg;
            }
            else
            {
                _db.FollowerNotifications.RemoveRange(
                    _db.FollowerNotifications.Where(x => x.OriginatedById == model1.Id || x.UserId == model1.Id));
                _db.SaveChanges();
                _db.Notifications.RemoveRange(_db.Notifications.Where(x=>x.OriginatedById == model1.Id || x.UserId == model1.Id));
                _db.SaveChanges();
                _db.Activities.RemoveRange(_db.Activities.Where(x=>x.UserId==model1.Id));
                _db.SaveChanges();
                _db.Pictures.RemoveRange(_db.Pictures.Where(x => x.User_Id == model1.Id));
                _db.SaveChanges();
                model1.FollowerList.Clear();
                _db.SaveChanges();
                model1.FollowingList.Clear();
                _db.SaveChanges();
                _db.Users.Remove(model1);
                _db.SaveChanges();

                var responseMsg2 = new HttpResponseMessage
                {
                    Content = new StringContent(new JavaScriptSerializer().Serialize(new
                    {
                        res = "Ujjal vai be carefull when you deleting any mail",
                        msg = "Successfully deleted the user from Glostars BaseUser table"
                    }), Encoding.UTF8, "application/json")
                };
                return responseMsg2;
            }  
        }
    }
}