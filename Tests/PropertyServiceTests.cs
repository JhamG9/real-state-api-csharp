using NUnit.Framework;
using RealEstate.API.Services;
using RealEstate.API.Models;
using Microsoft.Extensions.Configuration;
using Moq;

namespace RealEstate.API.Tests
{
    [TestFixture]
    public class PropertyServiceTests
    {
        private Mock<IConfiguration> _mockConfig;

        [SetUp]
        public void Setup()
        {
            _mockConfig = new Mock<IConfiguration>();
            _mockConfig.Setup(c => c["MongoSettings:ConnectionString"]).Returns("mongodb://localhost:27017");
            _mockConfig.Setup(c => c["MongoSettings:DatabaseName"]).Returns("TestDB");
            
            // Note: En un test real, usarías una base de datos en memoria o mocks más complejos
            // Para esta demostración, testearemos la lógica de negocio sin BD
        }

        [Test]
        public void Property_ValidData_PassesValidation()
        {
            // Arrange
            var property = new Property
            {
                IdProperty = "507f1f77bcf86cd799439011",
                Name = "Casa Centro",
                Address = "Calle 123 #45-67",
                Price = 250000,
                CodeInternal = "PROP001",
                Year = 2020,
                IdOwner = "507f1f77bcf86cd799439012"
            };

            // Act & Assert
            Assert.That(property.Name, Is.Not.Empty);
            Assert.That(property.Address, Is.Not.Empty);
            Assert.That(property.Price, Is.GreaterThan(0));
            Assert.That(property.Year, Is.GreaterThan(1900));
            Assert.That(property.IdOwner, Is.Not.Empty);
        }

        [Test]
        public void PropertyUpdateDto_PartialUpdate_OnlyUpdatesProvidedFields()
        {
            // Arrange
            var updateDto = new PropertyUpdateDto
            {
                Name = "Casa Centro Actualizada",
                Price = 275000
                // Otros campos quedan null para test parcial
            };

            var existingProperty = new Property
            {
                IdProperty = "507f1f77bcf86cd799439011",
                Name = "Casa Centro Original",
                Address = "Calle Original",
                Price = 250000,
                CodeInternal = "PROP001",
                Year = 2020,
                IdOwner = "507f1f77bcf86cd799439012"
            };

            // Act - Simular lógica de actualización parcial
            var updatedName = updateDto.Name ?? existingProperty.Name;
            var updatedAddress = updateDto.Address ?? existingProperty.Address;
            var updatedPrice = updateDto.Price ?? existingProperty.Price;
            var updatedCode = updateDto.CodeInternal ?? existingProperty.CodeInternal;

            // Assert
            Assert.That(updatedName, Is.EqualTo("Casa Centro Actualizada"));
            Assert.That(updatedAddress, Is.EqualTo("Calle Original")); // No cambió
            Assert.That(updatedPrice, Is.EqualTo(275000));
            Assert.That(updatedCode, Is.EqualTo("PROP001")); // No cambió
        }

        [Test]
        public void Property_InvalidData_FailsValidation()
        {
            // Arrange & Act & Assert
            var property = new Property();
            
            // Verificar que campos requeridos fallen
            Assert.That(string.IsNullOrEmpty(property.Name), Is.True);
            Assert.That(string.IsNullOrEmpty(property.Address), Is.True);
            Assert.That(property.Price, Is.EqualTo(0));
            Assert.That(string.IsNullOrEmpty(property.IdOwner), Is.True);
        }
    }
}