using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Microsoft.AspNetCore.Hosting;
using MobilePhones.Areas.Admin.Controllers;
using Microsoft.EntityFrameworkCore;
using MobilePhones.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading;

namespace MobilePhones.Tests
{
    public class AdminPhonesControllerTests
    {
        [Fact]
        public async Task TestCreate()
        {
            var hostEnv = new Mock<IWebHostEnvironment>();

           

            var options = new DbContextOptionsBuilder<MobileContext>()
                             .UseInMemoryDatabase(databaseName: "schooldb")
                             .Options;
            using (var context = new MobileContext(options))
            {
                var controller = new PhonesController(context, hostEnv.Object);

                var file = new Mock<IFormFile>();
                var sourceImg = File.OpenRead(@"..\..\..\..\MobilePhones.Tests\testfiles\phone_test_image.jpg");
               
                var fileName = "QQ.jpg";

                file.Setup(f => f.FileName).Returns(fileName).Verifiable();
              

                hostEnv.Setup(h => h.WebRootPath).Returns(@"..\..\..\..\MobilePhones.Tests\testfiles\").Verifiable();
                
                var phone = new Phone()
                {
                    Name = "My Phone",
                    Company = "Comapny",
                    Price = 444,
                    ImageFile = file.Object
                };

                var result = await controller.Create(phone);

                Assert.Contains(fileName, phone.ImageName);
            }
        }
    }
}
