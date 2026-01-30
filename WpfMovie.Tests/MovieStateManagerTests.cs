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
            Assert.That(movieString, Is.Not.Null);
            Assert.That(movieString, Is.Not.Empty);
        }

        [Test]
        public void Deserialize_ShouldCreateMovie()
        {
            // Arrange
            var movieString = "eyJUaXRsZSI6ICJUZXN0IFRpdGxlIiwgIkRlc2NyaXB0aW9uIjogIlRlc3QgRGVzY3JpcHRpb24ifQ==";
            
            // Act
            var newMovie = stateManager.Deserialize(movieString);
            
            // Assert
            Assert.That(newMovie, Is.Not.Null);
            Assert.That(newMovie.Title, Is.EqualTo("Test Title"));
            Assert.That(newMovie.Description, Is.EqualTo("Test Description"));
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
            Assert.That(newMovie, Is.Not.Null);
            Assert.That(newMovie.Title, Is.EqualTo(aMovie.Title));
            Assert.That(newMovie.Description, Is.EqualTo(aMovie.Description));
        }
    }
}
