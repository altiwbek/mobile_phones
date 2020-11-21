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
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace MobilePhones.Tests
{
    public class AdminPhonesControllerTests : CleanUpAfterTests
    {
        private PhonesController _controller;
        private MobileContext _context;
        private Mock<IWebHostEnvironment> _hostEnv;
        public AdminPhonesControllerTests()
        {
            var options = new DbContextOptionsBuilder<MobileContext>()
                             .UseInMemoryDatabase(databaseName: "schooldb")
                             .Options;
            _context = new MobileContext(options);
            _hostEnv = new Mock<IWebHostEnvironment>();
            _controller = new PhonesController(_context, _hostEnv.Object);
        }

        [Fact]
        public async Task TestCreate()
        {
            cleanUpDir = @"..\..\..\..\MobilePhones.Tests\testfiles\images\phone_images\";
            cleanUpSearchPattern = "*.jpg";
            int beforeCount = _context.Phones.Count();
            var file = new Mock<IFormFile>();
            var sourceImg = File.OpenRead(@"..\..\..\..\MobilePhones.Tests\testfiles\phone_test_image.jpg");
            var fileName = "test.jpg";
            file.Setup(f => f.FileName).Returns(fileName).Verifiable();
            _hostEnv.Setup(h => h.WebRootPath).Returns(@"..\..\..\..\MobilePhones.Tests\testfiles\").Verifiable();
            var phone = new Phone()
            {
                Name = "My Phone",
                Company = "Company",
                Price = 444,
                ImageFile = file.Object
            };

            var result = await _controller.Create(phone);
            Assert.Contains(fileName, phone.ImageName);
            Assert.Equal(beforeCount + 1, _context.Phones.Count());
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }
    }
}
