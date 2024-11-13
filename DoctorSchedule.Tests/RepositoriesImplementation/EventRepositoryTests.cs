namespace DoctorSchedule.Tests.RepositoriesImplementation
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using DoctorSchedule.Domain.Entities;
    using DoctorSchedule.Infrastructure.Persistence;
    using DoctorSchedule.Infrastructure.RepositoriesImplementation;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class EventRepositoryTests
    {
        private EventRepository _testClass;
        private AppDbContext _context;
        private ILogger<EventRepository> _logger;

        [SetUp]
        public void SetUp()
        {
            _context = new AppDbContext(new DbContextOptions<AppDbContext>());
            _logger = Substitute.For<ILogger<EventRepository>>();
            _testClass = new EventRepository(_context, _logger);
        }

        [Test]
        public async Task CanCallGetEventByIdAsync()
        {
            // Arrange
            var eventId = new Guid("0a321e3d-ac93-4b06-9440-60dff1fb073a");

            // Act
            var result = await _testClass.GetEventByIdAsync(eventId);

            // Assert
            Assert.Fail("Create or modify test");
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task CanCallGetEventsAsync()
        {
            // Arrange
            var startDate = DateTime.UtcNow;
            var endDate = DateTime.UtcNow;

            // Act
            var result = await _testClass.GetEventsAsync(startDate, endDate);

            // Assert
            Assert.Fail("Create or modify test");
        }

        [Test]
        public async Task CanCallCreateEventAsync()
        {
            // Arrange
            var calendarEvent = new Event
            {
                Id = new Guid("4c9f8cde-4d41-4cc4-973b-6c5a89b31325"),
                Title = "TestValue1662235947",
                Description = "TestValue599447749",
                StartTime = DateTime.UtcNow,
                EndTime = DateTime.UtcNow,
                Attendees = new List<Attendee>()
            };

            // Act
            await _testClass.CreateEventAsync(calendarEvent);

            // Assert
            Assert.Fail("Create or modify test");
        }

        [Test]
        public void CannotCallCreateEventAsyncWithNullCalendarEvent()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => _testClass.CreateEventAsync(default(Event)));
        }

        [Test]
        public async Task CanCallUpdateEventAsync()
        {
            // Arrange
            var calendarEvent = new Event
            {
                Id = new Guid("22914996-3b44-42f5-8579-abcb2816fbff"),
                Title = "TestValue2066954036",
                Description = "TestValue120842752",
                StartTime = DateTime.UtcNow,
                EndTime = DateTime.UtcNow,
                Attendees = new List<Attendee>()
            };

            // Act
            await _testClass.UpdateEventAsync(calendarEvent);

            // Assert
            Assert.Fail("Create or modify test");
        }

        [Test]
        public void CannotCallUpdateEventAsyncWithNullCalendarEvent()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => _testClass.UpdateEventAsync(default(Event)));
        }

        [Test]
        public async Task CanCallDeleteEventAsync()
        {
            // Arrange
            var eventId = new Guid("f442947d-58b6-4c5b-9e15-ab2a84828327");

            // Act
            await _testClass.DeleteEventAsync(eventId);

            // Assert
            Assert.Fail("Create or modify test");
        }

        [Test]
        public async Task CanCallAddAttendeeAsync()
        {
            // Arrange
            var eventId = new Guid("2e3faf7c-e9c3-4af2-b088-5838117d4c5c");
            var attendee = new Attendee
            {
                Id = new Guid("bb356755-8fba-4028-9fd3-6d2083296c8d"),
                Name = "TestValue679700984",
                Email = "TestValue1740547232",
                IsAttending = true,
                EventId = new Guid("dffda24e-8a68-410a-b1ed-0b13cfe81a43"),
                Event = new Event
                {
                    Id = new Guid("b2232f64-3a5b-4bd6-951d-f67fcea2fb2a"),
                    Title = "TestValue1374488865",
                    Description = "TestValue1399434863",
                    StartTime = DateTime.UtcNow,
                    EndTime = DateTime.UtcNow,
                    Attendees = new List<Attendee>()
                }
            };

            // Act
            await _testClass.AddAttendeeAsync(eventId, attendee);

            // Assert
            Assert.Fail("Create or modify test");
        }

        [Test]
        public void CannotCallAddAttendeeAsyncWithNullAttendee()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => _testClass.AddAttendeeAsync(new Guid("265d5e08-c562-4c0a-8e14-077a7c471eef"), default(Attendee)));
        }

        [Test]
        public async Task CanCallUpdateAttendeeAsync()
        {
            // Arrange
            var eventId = new Guid("0b964ba8-4795-48a0-83c6-7ea988ca5bff");
            var updatedAttendee = new Attendee
            {
                Id = new Guid("d2e4c09e-98f3-4231-a447-bbcfbc4b8c76"),
                Name = "TestValue2029853072",
                Email = "TestValue1975387628",
                IsAttending = false,
                EventId = new Guid("07c7e859-d54c-45d2-9d51-de71aa69e404"),
                Event = new Event
                {
                    Id = new Guid("25fdb513-ef06-4554-ae3b-ace92a734f8e"),
                    Title = "TestValue592638129",
                    Description = "TestValue1052824",
                    StartTime = DateTime.UtcNow,
                    EndTime = DateTime.UtcNow,
                    Attendees = new List<Attendee>()
                }
            };

            // Act
            await _testClass.UpdateAttendeeAsync(eventId, updatedAttendee);

            // Assert
            Assert.Fail("Create or modify test");
        }

        [Test]
        public void CannotCallUpdateAttendeeAsyncWithNullUpdatedAttendee()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => _testClass.UpdateAttendeeAsync(new Guid("ed270b13-0f63-4f49-839c-0918e3d36f52"), default(Attendee)));
        }

        [Test]
        public async Task CanCallRemoveAttendeeAsync()
        {
            // Arrange
            var eventId = new Guid("fd527d6c-869b-44ab-ba7a-61ccd20a7e1f");
            var attendeeId = new Guid("7344199a-28e2-471f-899e-8419ad08149b");

            // Act
            await _testClass.RemoveAttendeeAsync(eventId, attendeeId);

            // Assert
            Assert.Fail("Create or modify test");
        }

        [Test]
        public async Task CanCallAcceptEventAsync()
        {
            // Arrange
            var eventId = new Guid("91764361-0ea9-46bf-9a75-feabdd876b35");
            var attendeeId = new Guid("440f7663-abac-4450-a2ce-bfe4e55b5ba4");

            // Act
            await _testClass.AcceptEventAsync(eventId, attendeeId);

            // Assert
            Assert.Fail("Create or modify test");
        }

        [Test]
        public async Task CanCallDeclineEventAsync()
        {
            // Arrange
            var eventId = new Guid("33af41e5-ee97-4d77-9120-0d518bd1e4bb");
            var attendeeId = new Guid("ce2f7155-619b-4699-85f2-5f2eb69ba4f8");

            // Act
            await _testClass.DeclineEventAsync(eventId, attendeeId);

            // Assert
            Assert.Fail("Create or modify test");
        }
    }
}