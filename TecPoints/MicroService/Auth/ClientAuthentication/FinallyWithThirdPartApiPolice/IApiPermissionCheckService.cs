//TODO cache is evil 

using System.Dynamic;
using System.Threading.Tasks;

namespace ClientAuthentication
{
    public interface IApiPermissionCheckService
    {
        //TODO
        Task<bool> HasAccess();
    }
    
    public class CustomApiPermissionCheckService:IApiPermissionCheckService
    {
        public Task<bool> HasAccess()
        {
            throw new System.NotImplementedException();
        }

        // private static IApiPermissionCheckService _instance;
        // public static IApiPermissionCheckService Instance
        // {
        //     get
        //     {
        //         return _instance??_instance=new CustomApiPermissionCheckService()
        //     }
        // }
    }
}