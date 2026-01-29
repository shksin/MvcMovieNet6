using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfMovie.Models;
using WpfMovie.Services;

namespace WpfMovie.Tests
{
    internal class MovieStateManagerTests
    {
        private MovieStateManager stateManager;

        [SetUp]
        public void SetUp()
        {
            stateManager = new MovieStateManager();
        }

        [Test]
        public void Serialize_ShouldCreateString()
        {
            // Arrange
            string title = "Test Title";
            string description = "Test Description";
            var aMovie = new Movie
            {
                Title = title,
                Description = description
            };

            // Act
            var movieString = stateManager.Serialize(aMovie);

            // Assert
            Assert.NotNull(movieString);
            Assert.IsNotEmpty(movieString);
        }

        [Test]
        public void Deserialize_ShouldCreateMovie()
        {
            // Arrange - Use base64-encoded JSON format (compatible with System.Text.Json)
            var json = "{\"Title\":\"Test Title\",\"Description\":\"Test Description\"}";
            var movieString = Convert.ToBase64String(Encoding.UTF8.GetBytes(json));
            
            // Act
            var newMovie = stateManager.Deserialize(movieString);
            
            // Assert
            Assert.IsNotNull(newMovie);
            Assert.AreEqual("Test Title", newMovie.Title);
            Assert.AreEqual("Test Description", newMovie.Description);
        }

        [Test]
        public void Serialize_IsIdempotent()
        {
            // Arrange
            string title = "Test Title";
            string description = "Test Description";
            var aMovie = new Movie
            {
                Title = title,
                Description = description
            };

            // Act
            var movieString = stateManager.Serialize(aMovie);
            var newMovie = stateManager.Deserialize(movieString);

            // Assert
            Assert.IsNotNull(newMovie);
            Assert.AreEqual(aMovie.Title, newMovie.Title);
            Assert.AreEqual(aMovie.Description, newMovie.Description);
        }
    }
}
