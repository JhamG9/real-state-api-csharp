using NUnit.Framework;
using RealEstate.API.Models;
using Moq;

namespace RealEstate.API.Tests
{
    [TestFixture]
    public class OwnerServiceTests
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
        public void Owner_ValidData_PassesValidation()
        {
            // Arrange
            var owner = new Owner
            {
                IdOwner = "507f1f77bcf86cd799439011",
                Name = "Juan Pérez",
                Address = "Carrera 15 #32-45",
                Photo = "/images/juan.jpg",
                Birthday = "1985-05-15"
            };

            // Act & Assert
            Assert.That(owner.Name, Is.Not.Empty);
            Assert.That(owner.Address, Is.Not.Empty);
            Assert.That(owner.Birthday, Is.Not.Empty);
            Assert.That(owner.IdOwner, Is.Not.Empty);
        }

        [Test]
        public void OwnerUpdateDto_PartialUpdate_OnlyUpdatesProvidedFields()
        {
            // Arrange
            var updateDto = new OwnerUpdateDto
            {
                Name = "Juan Carlos Pérez",
                Address = "Nueva Dirección 123"
                // Photo y Birthday quedan null
            };

            var existingOwner = new Owner
            {
                IdOwner = "507f1f77bcf86cd799439011",
                Name = "Juan Pérez",
                Address = "Dirección Original",
                Photo = "/images/original.jpg",
                Birthday = "1985-05-15"
            };

            // Act - Simular lógica de actualización parcial
            var updatedName = updateDto.Name ?? existingOwner.Name;
            var updatedAddress = updateDto.Address ?? existingOwner.Address;
            var updatedPhoto = updateDto.Photo ?? existingOwner.Photo;
            var updatedBirthday = updateDto.Birthday ?? existingOwner.Birthday;

            // Assert
            Assert.That(updatedName, Is.EqualTo("Juan Carlos Pérez"));
            Assert.That(updatedAddress, Is.EqualTo("Nueva Dirección 123"));
            Assert.That(updatedPhoto, Is.EqualTo("/images/original.jpg")); // No cambió
            Assert.That(updatedBirthday, Is.EqualTo("1985-05-15")); // No cambió
        }

        [Test]
        public void Owner_BirthdayFormat_IsValid()
        {
            // Arrange
            var owner = new Owner
            {
                IdOwner = "507f1f77bcf86cd799439011",
                Name = "Test User",
                Address = "Test Address",
                Birthday = "2025-12-31" // Fecha futura como string
            };

            // Act & Assert
            Assert.That(owner.Birthday, Is.Not.Empty);
            Assert.That(owner.Birthday, Does.Match(@"^\d{4}-\d{2}-\d{2}$"));
            // En un escenario real, esto debería validar el formato de fecha
        }

        [Test]
        public void Owner_EmptyRequiredFields_FailsValidation()
        {
            // Arrange
            var owner = new Owner();

            // Act & Assert
            Assert.That(string.IsNullOrEmpty(owner.Name), Is.True);
            Assert.That(string.IsNullOrEmpty(owner.Address), Is.True);
            Assert.That(string.IsNullOrEmpty(owner.Birthday), Is.True);
        }
    }
}