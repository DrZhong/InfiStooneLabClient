using System.Threading.Tasks;
using Guoxu.LabManager.Models.TokenAuth;
using Guoxu.LabManager.Web.Controllers;
using Shouldly;
using Xunit;

namespace Guoxu.LabManager.Web.Tests.Controllers
{
    public class HomeController_Tests: LabManagerWebTestBase
    {
        [Fact]
        public async Task Index_Test()
        {
            await AuthenticateAsync(null, new AuthenticateModel
            {
                UserNameOrEmailAddress = "admin",
                Password = "123qwe"
            });

            //Act
            var response = await GetResponseAsStringAsync(
                GetUrl<HomeController>(nameof(HomeController.Index))
            );

            //Assert
            response.ShouldNotBeNullOrEmpty();
        }
    }
}