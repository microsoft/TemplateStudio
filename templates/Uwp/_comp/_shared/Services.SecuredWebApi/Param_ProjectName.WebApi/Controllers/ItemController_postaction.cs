//{[{
using Microsoft.AspNetCore.Authorization;
//}]}

namespace Param_RootNamespace.WebApi.Controllers
{
//^^
//{[{

    // For more info on claims and role-based autorization see
    // https://docs.microsoft.com/aspnet/core/security/authorization/claims?view=aspnetcore-2.2
    // https://docs.microsoft.com/aspnet/core/security/authorization/roles?view=aspnetcore-2.2
//}]}
    [Route("api/[controller]")]
//{[{
    [Authorize(Policy = "SampleClaimPolicy")]
    [Authorize(Roles = "OrderReaders")]
//}]}
    [ApiController]
    public class ItemController : ControllerBase
    {
    }
}