using Microsoft.EntityFrameworkCore;
using StudentManagementAPI.Exceptions;
using StudentManagementAPI.Interfaces;
using StudentManagementAPI.Models.DBModels;
using StudentManagementAPI.Models.DTOs;
using StudentManagementAPI.Repositories;
using StudentManagementAPI.Services;

namespace StudentManagementAPITest.ServiceUnitTest
{
    internal class ClassAttendanceServiceTest
    {
        StudentManagementContext context;
        IRepository<int, ClassAttendance> classAttendanceRepository;
        IRepository<int, Class> classRepository;
        IRepository<int, Student> studentRepository;
        IRepository<int, Enrollment> enrollmentRepository;
        IRepository<int, CourseOffering> courseOfferingRepository;

        ClassAttendanceService classAttendanceService;



        [SetUp]
        public void Setup()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder().UseInMemoryDatabase("StudentManagementDB");
            context = new StudentManagementContext(optionsBuilder.Options);

            classAttendanceRepository = new ClassAttendanceRepository(context);
            classRepository = new ClassRepository(context);
            studentRepository = new StudentRepository(context);
            enrollmentRepository = new EnrollmentRepository(context);
            courseOfferingRepository = new CourseOfferingRepository(context);

            classAttendanceService = new ClassAttendanceService(classAttendanceRepository, classRepository, studentRepository, enrollmentRepository, courseOfferingRepository);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }

        [Test]
        public async Task MarkStudentAttendanceTest()
        {
            //Arrange
            var classAttendanceDTO = new ClassAttendanceDTO
            {
                ClassId = 1,
                StudentId = 4000,
                Status = "Present"
            };

            // Act
            var result = await classAttendanceService.MarkStudentAttendance(classAttendanceDTO);
            var allAttendance = await classAttendanceRepository.Get();

            // Assert
            Assert.NotNull(result);
            Assert.That(result.ClassId, Is.EqualTo(1));
            Assert.That(result.StudentId, Is.EqualTo(4000));
            Assert.That(result.Status, Is.EqualTo("Present"));
            Assert.That(allAttendance.Count(), Is.EqualTo(1));
        }

        [Test]
        public void MarkStudentAttendanceTest_InvalidClassId()
        {
            //Arrange
            var classAttendanceDTO = new ClassAttendanceDTO
            {
                ClassId = 1000,
                StudentId = 4000,
                Status = "Present"
            };

            // Act
            var ex = Assert.ThrowsAsync<NoSuchClassException>(() => classAttendanceService.MarkStudentAttendance(classAttendanceDTO));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("Class with given Id does not exist."));
        }

        [Test]
        public void MarkStudentAttendanceTest_InvalidStudentId()
        {
            //Arrange
            var classAttendanceDTO = new ClassAttendanceDTO
            {
                ClassId = 1,
                StudentId = 1000,
                Status = "Present"
            };

            // Act
            var ex = Assert.ThrowsAsync<NoSuchStudentException>(() => classAttendanceService.MarkStudentAttendance(classAttendanceDTO));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("Uh oh! No such student found!"));
        }

        [Test]
        public void MarkStudentAttendanceTest_InvalidStatus()
        {
            //Arrange
            var classAttendanceDTO = new ClassAttendanceDTO
            {
                ClassId = 1,
                StudentId = 4000,
                Status = "Presentt"
            };

            // Act
            var ex = Assert.ThrowsAsync<InvalidAttendanceStatusException>(() => classAttendanceService.MarkStudentAttendance(classAttendanceDTO));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("Invalid attendance status. Please provide a valid attendance status. It can be Present, Absent, Late, or Excused."));
        }

        [Test]
        public async Task MarkStudentAttendanceTest_AlreadyExists()
        {
            //Arrange
            var classAttendanceDTO = new ClassAttendanceDTO
            {
                ClassId = 1,
                StudentId = 4000,
                Status = "Present"
            };

            await classAttendanceService.MarkStudentAttendance(classAttendanceDTO);

            // Act
            var ex = Assert.ThrowsAsync<ClassAttendanceAlreadyExistsException>(() => classAttendanceService.MarkStudentAttendance(classAttendanceDTO));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("You have already marked attendance for this student in this class. Please Update the attendance status if needed."));
        }

        [Test]
        public void MarkStudentAttendanceTest_StudentNotEnrolledInCourse()
        {
            //Arrange
            context.CourseOfferings.Add(new CourseOffering { CourseOfferingId = 11, CourseCode = "CSE201", TeacherId = 2000 });
            context.Classes.Add(new Class { ClassId = 11, CourseOfferingId = 11, ClassDateAndTime = DateTime.Now });
            context.SaveChanges();



            var classAttendanceDTO = new ClassAttendanceDTO
            {
                ClassId = 11,
                StudentId = 4000,
                Status = "Present"
            };

            // Act
            var ex = Assert.ThrowsAsync<NotEnrolledInCourseException>(() => classAttendanceService.MarkStudentAttendance(classAttendanceDTO));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("Student is not enrolled in the course."));
        }

        [Test]
        public async Task GetAttendanceByClassTest()
        {
            //Arrange
            var classAttendanceDTO = new ClassAttendanceDTO
            {
                ClassId = 1,
                StudentId = 4000,
                Status = "Present"
            };

            await classAttendanceService.MarkStudentAttendance(classAttendanceDTO);

            // Act
            var result = await classAttendanceService.GetAttendanceByClass(1);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Count(), Is.EqualTo(1));
        }

        [Test]
        public void GetAttendanceByClassTest_NoSuchClass()
        {
            // Act
            var ex = Assert.ThrowsAsync<NoSuchClassException>(() => classAttendanceService.GetAttendanceByClass(1000));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("Class with given Id does not exist."));
        }

        [Test]
        public void GetAttendanceByClassTest_NoAttendanceFound()
        {
            // Act
            var ex = Assert.ThrowsAsync<NoClassAttendanceFoundException>(() => classAttendanceService.GetAttendanceByClass(1));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("No class attendance Found"));
        }

        [Test]
        public async Task GetAttendanceByStudentTest()
        {
            //Arrange
            var classAttendanceDTO = new ClassAttendanceDTO
            {
                ClassId = 1,
                StudentId = 4000,
                Status = "Present"
            };

            await classAttendanceService.MarkStudentAttendance(classAttendanceDTO);

            // Act
            var result = await classAttendanceService.GetAttendanceByStudent(4000);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Count(), Is.EqualTo(1));
        }

        [Test]
        public void GetAttendanceByStudentTest_NoSuchStudent()
        {
            // Act
            var ex = Assert.ThrowsAsync<NoSuchStudentException>(() => classAttendanceService.GetAttendanceByStudent(1000));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("Uh oh! No such student found!"));
        }

        [Test]
        public void GetAttendanceByStudentTest_NoAttendanceFound()
        {
            // Act
            var ex = Assert.ThrowsAsync<NoClassAttendanceFoundException>(() => classAttendanceService.GetAttendanceByStudent(4000));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("No class attendance Found"));
        }

        [Test]
        public async Task GetAttendanceByClassAndStudentTest()
        {
            //Arrange
            var classAttendanceDTO = new ClassAttendanceDTO
            {
                ClassId = 1,
                StudentId = 4000,
                Status = "Present"
            };

            await classAttendanceService.MarkStudentAttendance(classAttendanceDTO);

            // Act
            var result = await classAttendanceService.GetAttendanceByClassAndStudent(1, 4000);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.ClassId, Is.EqualTo(1));
            Assert.That(result.StudentId, Is.EqualTo(4000));
            Assert.That(result.Status, Is.EqualTo("Present"));
        }

        [Test]
        public void GetAttendanceByClassAndStudentTest_NoSuchClass()
        {
            // Action
            var ex = Assert.ThrowsAsync<NoSuchClassException>(() => classAttendanceService.GetAttendanceByClassAndStudent(1000, 4000));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("Class with given Id does not exist."));
        }

        [Test]
        public async Task GetAttendanceByClassAndStudentTest_NoSuchStudentAsync()
        {
            //Arrange
            var classAttendanceDTO = new ClassAttendanceDTO
            {
                ClassId = 1,
                StudentId = 4000,
                Status = "Present"
            };

            await classAttendanceService.MarkStudentAttendance(classAttendanceDTO);

            // Act
            var ex = Assert.ThrowsAsync<NoSuchStudentException>(() => classAttendanceService.GetAttendanceByClassAndStudent(1, 1000));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("Uh oh! No such student found!"));
        }


        [Test]
        public void GetAttendanceByClassAndStudentTest_NoAttendanceFoundAsync()
        {
            // Action
            var ex = Assert.ThrowsAsync<NoSuchClassAttendanceException>(() => classAttendanceService.GetAttendanceByClassAndStudent(1, 4000));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("No such class attendance found."));
        }

        [Test]
        public async Task UpdateClassAttendanceTest()
        {
            //Arrange
            var classAttendanceDTO = new ClassAttendanceDTO
            {
                ClassId = 1,
                StudentId = 4000,
                Status = "Present"
            };

            await classAttendanceService.MarkStudentAttendance(classAttendanceDTO);

            classAttendanceDTO.Status = "Absent";

            // Act
            var result = await classAttendanceService.UpdateClassAttendance(classAttendanceDTO);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.ClassId, Is.EqualTo(1));
            Assert.That(result.StudentId, Is.EqualTo(4000));
            Assert.That(result.Status, Is.EqualTo("Absent"));
        }

        [Test]
        public void UpdateClassAttendanceTest_NoSuchClass()
        {
            //Arrange
            var classAttendanceDTO = new ClassAttendanceDTO
            {
                ClassId = 1000,
                StudentId = 4000,
                Status = "Present"
            };

            // Act
            var ex = Assert.ThrowsAsync<NoSuchClassException>(() => classAttendanceService.UpdateClassAttendance(classAttendanceDTO));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("Class with given Id does not exist."));
        }

        [Test]
        public void UpdateClassAttendanceTest_NoSuchStudent()
        {
            //Arrange
            var classAttendanceDTO = new ClassAttendanceDTO
            {
                ClassId = 1,
                StudentId = 1000,
                Status = "Present"
            };

            // Act
            var ex = Assert.ThrowsAsync<NoSuchStudentException>(() => classAttendanceService.UpdateClassAttendance(classAttendanceDTO));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("Uh oh! No such student found!"));
        }

        [Test]

        public void UpdateClassAttendanceTest_NoSuchAttendance()
        {
            //Arrange
            var classAttendanceDTO = new ClassAttendanceDTO
            {
                ClassId = 1,
                StudentId = 4000,
                Status = "Present"
            };

            // Act
            var ex = Assert.ThrowsAsync<NoSuchClassAttendanceException>(() => classAttendanceService.UpdateClassAttendance(classAttendanceDTO));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("No such class attendance found."));
        }

        [Test]
        public async Task UpdateClassAttendanceTest_InvalidStatus()
        {
            //Arrange
            var classAttendanceDTO = new ClassAttendanceDTO
            {
                ClassId = 1,
                StudentId = 4000,
                Status = "Present"
            };

            await classAttendanceService.MarkStudentAttendance(classAttendanceDTO);

            classAttendanceDTO.Status = "Presenttt";


            // Act
            var ex = Assert.ThrowsAsync<InvalidAttendanceStatusException>(() => classAttendanceService.UpdateClassAttendance(classAttendanceDTO));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("Invalid attendance status. Please provide a valid attendance status. It can be Present, Absent, Late, or Excused."));
        }
    }
}
