using Microsoft.EntityFrameworkCore;
using Repro_Cosmos.Model;

namespace Repro_Cosmos
{
    public class Example
    {
        [Fact]
        public void SetupData()
        {
            var context = Create();
            context.Data.Add(new DataType()
            {
                Id = Guid.NewGuid(),
                Children = new[]
                 {
                     new NestedData() { Name = "child 1" }
                 },
                Status = 1
            });

            Assert.Equal(1, context.SaveChanges());
        }

        [Fact]
        public void NoNested()
        {
            // Mapped property on its own works
            var result = Create()
                .Data
                .Select(x => new
                {
                    Mapped = x.Status == 2 ? "a" : "b"
                });

            Assert.NotEmpty(result);
        }

        [Fact]
        public void WithNestedFails()
        {
            // Nested collection with mapped property fails
            var result = Create()
                .Data
                .Select(x => new
                {
                    Mapped = x.Status == 2 ? "a" : "b",
                    Children = x.Children.Select(x => new { x.Name })
                });

            Assert.NotEmpty(result);
        }

        [Fact]
        public void WithNestedSuccess()
        {
            // Moving he mapped property after the nested collection(s) works
            var result = Create()
                .Data
                .Select(x => new
                {
                    Children = x.Children.Select(x => new { x.Name }),
                    Mapped = x.Status == 2 ? "a" : "b",
                });

            Assert.NotEmpty(result);
        }

        [Fact]
        public void NestedWithoutMapped()
        {
            // Nested collection with simple property works
            var result = Create()
                .Data
                .Select(x => new
                {
                    x.Id,
                    Children = x.Children.Select(x => new { x.Name })
                });

            Assert.NotEmpty(result);
        }

        private static EFContext Create()
        {
            // Used connection is for a the local cosmos emulator
            // Database: Test
            // Container: Test
            return new EFContext(new DbContextOptionsBuilder()
                .UseCosmos("AccountEndpoint=https://localhost:8081/;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==", "Test")
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                .Options);
        }
    }
}