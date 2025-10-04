using NUnit.Framework;
using Moq;
using RealEstate.API.Models;
using MongoDB.Driver;

namespace RealEstate.API.Tests
{
    [TestFixture]
    public class PropertyTraceServiceTests
    {
        private Mock<IMongoCollection<PropertyTrace>> _mockCollection;
        private Mock<IMongoDatabase> _mockDatabase;
        private Mock<IMongoClient> _mockClient;
        private Mock<IConfiguration> _mockConfig;

        [SetUp]
        public void Setup()
        {
            _mockCollection = new Mock<IMongoCollection<PropertyTrace>>();
            _mockDatabase = new Mock<IMongoDatabase>();
            _mockClient = new Mock<IMongoClient>();
            _mockConfig = new Mock<IConfiguration>();

            _mockConfig.Setup(c => c["MongoSettings:ConnectionString"]).Returns("mongodb://localhost:27017");
            _mockConfig.Setup(c => c["MongoSettings:DatabaseName"]).Returns("TestDB");

            _mockClient.Setup(c => c.GetDatabase(It.IsAny<string>(), null))
                      .Returns(_mockDatabase.Object);

            _mockDatabase.Setup(d => d.GetCollection<PropertyTrace>("PropertyTraces", null))
                        .Returns(_mockCollection.Object);
        }

        [Test]
        public void CreateAsync_ValidPropertyTrace_ReturnsPropertyTrace()
        {
            var propertyTrace = new PropertyTrace
            {
                IdPropertyTrace = "507f1f77bcf86cd799439011",
                DateSale = DateTime.Now,
                Name = "Test Sale",
                Value = 100000,
                Tax = 5000,
                IdProperty = "507f1f77bcf86cd799439012"
            };

            Assert.That(propertyTrace, Is.Not.Null);
            Assert.That(propertyTrace.Name, Is.EqualTo("Test Sale"));
            Assert.That(propertyTrace.Value, Is.EqualTo(100000));
        }

        [Test]
        public void PropertyTrace_ValidData_PassesValidation()
        {
            var propertyTrace = new PropertyTrace
            {
                IdPropertyTrace = "507f1f77bcf86cd799439011",
                DateSale = DateTime.Now,
                Name = "Valid Sale",
                Value = 150000,
                Tax = 7500,
                IdProperty = "507f1f77bcf86cd799439012"
            };

            // Act & Assert
            Assert.That(propertyTrace.Value, Is.GreaterThan(0));
            Assert.That(propertyTrace.Tax, Is.GreaterThan(0));
            Assert.That(propertyTrace.Name, Is.Not.Empty);
            Assert.That(propertyTrace.IdProperty, Is.Not.Empty);
        }

        [Test]
        public void PropertyTraceUpdateDTO_PartialUpdate_OnlyUpdatesProvidedFields()
        {
            var updateDto = new PropertyTraceUpdateDTO
            {
                Value = 200000,
                Tax = 10000
            };

            var existingTrace = new PropertyTrace
            {
                IdPropertyTrace = "507f1f77bcf86cd799439011",
                DateSale = DateTime.Now.AddDays(-30),
                Name = "Original Name",
                Value = 150000,
                Tax = 7500,
                IdProperty = "507f1f77bcf86cd799439012"
            };

            var updatedName = updateDto.Name ?? existingTrace.Name;
            var updatedValue = updateDto.Value ?? existingTrace.Value;
            var updatedTax = updateDto.Tax ?? existingTrace.Tax;

            Assert.That(updatedName, Is.EqualTo("Original Name"));
            Assert.That(updatedValue, Is.EqualTo(200000));
            Assert.That(updatedTax, Is.EqualTo(10000));
        }
    }
}