using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using ShippingCostCalculator.Controllers;
using ShippingCostCalculator.Data.Interface;
using ShippingCostCalculator.Data.Models;

namespace ShippingCostCalculator.Tests.Controllers
{
    [TestFixture]
    public class MasterDataControllerTests
    {
        private Mock<IShippingCostCalculatorRepository> _mockRepository;
        private Mock<ILogger<MasterDataController>> _mockLogger;
        private MasterDataController _controller;

        [SetUp]
        public void Setup()
        {
            _mockRepository = new Mock<IShippingCostCalculatorRepository>();
            _mockLogger = new Mock<ILogger<MasterDataController>>();
            _controller = new MasterDataController(_mockRepository.Object, _mockLogger.Object);
        }

        [Test]
        public async Task GetCountries_ReturnsOkResult_WithListOfCountries()
        {
            // Arrange
            var countries = new List<Country>
            {
                new Country { CountryId = 1, CountryName = "USA" },
                new Country { CountryId = 2, CountryName = "Canada" }
            };

            _mockRepository.Setup(repo => repo.GetCountriesAsync()).ReturnsAsync(countries);

            // Act
            var result = await _controller.GetCountries();

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;

            Assert.Multiple(() =>
            {
                Assert.That(okResult?.StatusCode, Is.EqualTo(200));
                Assert.That(okResult?.Value, Is.Not.Null);
                Assert.That(((IEnumerable<Country>)okResult.Value!).Count(), Is.EqualTo(2));
            });
        }

        [Test]
        public async Task GetCountries_Returns500_WhenExceptionOccurs()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetCountriesAsync()).ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _controller.GetCountries();

            // Assert
            Assert.That(result, Is.InstanceOf<ObjectResult>());
            var objectResult = result as ObjectResult;

            Assert.Multiple(() =>
            {
                Assert.That(objectResult?.StatusCode, Is.EqualTo(500));
                Assert.That(objectResult?.Value, Is.EqualTo("Server Error please try latter"));
            });
        }

        [Test]
        public async Task GetProducts_ReturnsOkResult_WithListOfProducts()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { ProductId = 1, ProductName = "Laptop", BasePrice = 1000 },
                new Product { ProductId = 2, ProductName = "Phone", BasePrice = 500 }
            };

            _mockRepository.Setup(repo => repo.GetProductsAsync(1, 10)).ReturnsAsync(products);

            // Act
            var result = await _controller.GetProducts();

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;

            Assert.Multiple(() =>
            {
                Assert.That(okResult?.StatusCode, Is.EqualTo(200));
                Assert.That(okResult?.Value, Is.Not.Null);
                Assert.That(((IEnumerable<Product>)okResult.Value!).Count(), Is.EqualTo(2));
            });
        }

        [Test]
        public async Task GetProducts_Returns500_WhenExceptionOccurs()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetProductsAsync(1, 10)).ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _controller.GetProducts();

            // Assert
            Assert.That(result, Is.InstanceOf<ObjectResult>());
            var objectResult = result as ObjectResult;

            Assert.Multiple(() =>
            {
                Assert.That(objectResult?.StatusCode, Is.EqualTo(500));
                Assert.That(objectResult?.Value, Is.EqualTo("Server Error please try latter"));
            });
        }
    }
}
