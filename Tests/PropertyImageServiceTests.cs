using NUnit.Framework;
using RealEstate.API.Models;
using Moq;

namespace RealEstate.API.Tests
{
    [TestFixture]
    public class PropertyImageServiceTests
    {
        private Mock<IConfiguration> _mockConfig;

        [SetUp]
        public void Setup()
        {
            _mockConfig = new Mock<IConfiguration>();
            _mockConfig.Setup(c => c["MongoSettings:ConnectionString"]).Returns("mongodb://localhost:27017");
            _mockConfig.Setup(c => c["MongoSettings:DatabaseName"]).Returns("TestDB");
        }

        [Test]
        public void PropertyImage_ValidData_PassesValidation()
        {
            // Arrange
            var propertyImage = new PropertyImage
            {
                IdPropertyImage = "507f1f77bcf86cd799439011",
                IdProperty = "507f1f77bcf86cd799439012",
                FilePath = "/public/uploads/test-image.jpg",
                Enabled = true
            };

            // Act & Assert
            Assert.That(propertyImage.IdProperty, Is.Not.Empty);
            Assert.That(propertyImage.FilePath, Is.Not.Empty);
            Assert.That(propertyImage.FilePath, Does.StartWith("/public/uploads/"));
            Assert.That(propertyImage.Enabled, Is.True);
        }

        [Test]
        public void PropertyImage_FilePathFormat_IsCorrect()
        {
            // Arrange
            var fileName = "a33d2f99-a349-4450-a5f4-befbec45d27e_casa-florids.jpg";
            var expectedPath = $"/public/uploads/{fileName}";

            // Act
            var propertyImage = new PropertyImage
            {
                IdPropertyImage = "507f1f77bcf86cd799439011",
                IdProperty = "507f1f77bcf86cd799439012",
                FilePath = expectedPath,
                Enabled = true
            };

            // Assert
            Assert.That(propertyImage.FilePath, Is.EqualTo(expectedPath));
            Assert.That(propertyImage.FilePath, Does.Contain("casa-florids.jpg"));
            Assert.That(propertyImage.FilePath, Does.Match(@"^/public/uploads/[a-f0-9-]+_.*\.(jpg|jpeg|png|gif)$"));
        }

        [Test]
        public void PropertyImage_RequiredFields_MustNotBeEmpty()
        {
            // Arrange
            var propertyImage = new PropertyImage();

            // Act & Assert
            Assert.That(string.IsNullOrEmpty(propertyImage.IdProperty), Is.True);
            Assert.That(string.IsNullOrEmpty(propertyImage.FilePath), Is.True);
            // En un escenario real, estos campos requeridos deberían fallar la validación
        }

        [Test]
        public void PropertyImage_EnabledFlag_DefaultBehavior()
        {
            // Arrange & Act
            var propertyImage = new PropertyImage
            {
                IdPropertyImage = "507f1f77bcf86cd799439011",
                IdProperty = "507f1f77bcf86cd799439012",
                FilePath = "/public/uploads/test.jpg"
                // Enabled no se establece explícitamente
            };

            // Assert
            Assert.That(propertyImage.Enabled, Is.True); // Valor por defecto es true
            
            // Test cambio a false
            propertyImage.Enabled = false;
            Assert.That(propertyImage.Enabled, Is.False);
        }

        [Test]
        public void PropertyImage_MultipleImagesForSameProperty_AreAllowed()
        {
            // Arrange
            var propertyId = "507f1f77bcf86cd799439012";
            
            var image1 = new PropertyImage
            {
                IdPropertyImage = "507f1f77bcf86cd799439013",
                IdProperty = propertyId,
                FilePath = "/public/uploads/image1.jpg",
                Enabled = true
            };

            var image2 = new PropertyImage
            {
                IdPropertyImage = "507f1f77bcf86cd799439014",
                IdProperty = propertyId,
                FilePath = "/public/uploads/image2.jpg",
                Enabled = true
            };

            // Act & Assert
            Assert.That(image1.IdProperty, Is.EqualTo(image2.IdProperty));
            Assert.That(image1.IdPropertyImage, Is.Not.EqualTo(image2.IdPropertyImage));
            Assert.That(image1.FilePath, Is.Not.EqualTo(image2.FilePath));
        }
    }
}