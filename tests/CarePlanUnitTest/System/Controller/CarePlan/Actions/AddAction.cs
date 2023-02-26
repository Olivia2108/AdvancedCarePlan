using Application.Common.Interfaces.IRepository;
using Application.Common.Interfaces.IServices;
using Application.Dto;
using Application.ViewModels;
using CarePlanUnitTest.Fixtures;
using Domain.Constants;
using Domain.Entities;
using FluentAssertions;
using Infrastructure.Persistence;
using Infrastructure.Repository;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using WebAPI.Controllers;

namespace CarePlanUnitTest.System.Controller.CarePlan.Actions
{
    public class AddAction : DbContextFixture
    { 
        private readonly PatientCarePlanRepository _repository;
        private readonly PatientCarePlanService _carePlanService;
        private readonly PatientCarePlanDto _dto;
        private readonly List<PatientCarePlanDto> _dtoList; 

        public AddAction()
        {
            switch (_repository)
            {
                case null:  
                    _repository = new PatientCarePlanRepository(_context);
                    break;
            }
            switch (_carePlanService)
            {
                case null: 
                    _carePlanService = new PatientCarePlanService(_repository);
                    break;
            }

            switch (_dto)
            {
                case null:
                    var model = _context.Set<PatientCarePlan>().FirstOrDefault();
                    var json = JsonConvert.SerializeObject(model);
                    _dto = JsonConvert.DeserializeObject<PatientCarePlanDto>(json) ?? new PatientCarePlanDto();
                    break;
            }
             
            switch (_dtoList)
            {
                case null: 
                    var modelList = _context.Set<PatientCarePlan>().ToList();
                    var jsonList = JsonConvert.SerializeObject(modelList);
                    _dtoList = JsonConvert.DeserializeObject<List<PatientCarePlanDto>>(jsonList) ?? new List<PatientCarePlanDto>(); 
                    break;
            } 
        }

        string userName = "Tester233";
        string minValue = "0001-01-01 00:00:00.0000000";  

        [Fact]
        public async Task CreatePatientCarePlan_ShouldPass()
        {
            //Arrange  

            var sut = new CarePlanController(_carePlanService);
            _dto.UserName = userName;

            //Act 
            var result = (OkObjectResult)await sut.AddPatientCarePlan(_dto);


            //Assert  
            result.Should().BeOfType<OkObjectResult>();

            result.Value.Should().BeOfType<ResponseVM>();
            switch(result.Value != null)
            {
                case true:
                    var value = (ResponseVM)result.Value;
                    value.Success.Should().BeTrue();
                    value.Data.Should().BeOfType<long>();
                    value.Message.Should().BeEquivalentTo(ResponseConstants.Saved);

                    break;
                case false:
                    result.Value.Should().NotBeNull();
                    break;
            }
              
            result.StatusCode.Should().Be(200);


        }



        //[Fact]
        //public async Task CreateWith_ExistingUsername_ShouldFaiDDl()
        //{

        //    // Arrange 
        //    _mockedRepository.Setup(x => x.GetAllPatientCarePlans())
        //        .Returns(Task.FromResult(new List<PatientCarePlanVM>()
        //        {
        //            new PatientCarePlanVM(),
        //            new PatientCarePlanVM()
        //        }));

        //    _mockedService.Setup(x => x.GetAllPatientCarePlans())
        //        .Returns(Task.FromResult(new ResponseVM()));

        //    var controllerContextMock = new Mock<ControllerContext>();

        //    var controller = new CarePlanController(_mockedService.Object);
        //    controller.ControllerContext = controllerContextMock.Object;

        //    var ty = controller.GetAllPatientCarePlans();
        //    //Arrange  

        //    var sut = new CarePlanController(_carePlanService);

        //    //Act 
        //    var result = (BadRequestObjectResult)await sut.AddPatientCarePlan(_dto);


        //    //Assert 
        //    result.Value.Should().BeOfType<ResponseVM>();

        //    var value = (ResponseVM)result.Value;
        //    value.Success.Should().BeFalse();
        //    value.Data.Should().BeNull();

        //    result.StatusCode.Should().Be(400);


        //}

        #region Username Tests


        [Fact]
        public async Task CreateWith_ExistingUsername_ShouldFail()
        {
            //Arrange  

            var sut = new CarePlanController(_carePlanService);
             
            //Act 
            var result = (BadRequestObjectResult)await sut.AddPatientCarePlan(_dto);


            //Assert 
            result.Value.Should().BeOfType<ResponseVM>();
            switch (result.Value)
            {
                case null:
                    result.Value.Should().NotBeNull();
                    break;

                default:
                    var value = (ResponseVM)result.Value;
                    value.Success.Should().BeFalse();
                    value.Data.Should().BeNull();
                    value.Message.Should().BeEquivalentTo(ResponseConstants.IsExist);

                    break;
            }

            

            result.StatusCode.Should().Be(400);
             
        }


        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public async Task CreateWith_Empty_Null_Username_ShouldFail(string username)
        {
            //Arrange  

            var sut = new CarePlanController(_carePlanService);

            _dto.UserName = username;
            //Act 
            var result = (BadRequestObjectResult)await sut.AddPatientCarePlan(_dto);


            //Assert 
            result.Value.Should().BeOfType<ResponseVM>();

            switch (result.Value)
            {
                case null:
                    result.Value.Should().NotBeNull();
                    break;

                default:
                    var value = (ResponseVM)result.Value;
                    value.Success.Should().BeFalse();
                    value.Data.Should().BeNull();
                    value.Message.Should().BeEquivalentTo("'User Name' must not be empty.");
                     
                    break; 
            }
             
            result.StatusCode.Should().Be(400);
             
        }


        [Theory]
        [InlineData(300)]
        [InlineData(450)]
        [InlineData(451)]
        public async Task CreateWith_Username_WithLessThan450Characters_ShouldPass(int number)
        {
            //Arrange  

            var sut = new CarePlanController(_carePlanService);

            var character = DbInitializer.GenereateRandomAlphaNumeric(number);
            _dto.UserName = character;
            int lenght = character.Length;
             
            //Act 
              
            var results = await sut.AddPatientCarePlan(_dto);

            if (number == 300)
            {
                results = (OkObjectResult)results;
            }
            if(number == 340)
            {
                results = (OkObjectResult)results;
            }
            if(number == 451)
            {
                results = (BadRequestObjectResult)results;

            }



            var result = (OkObjectResult)await sut.AddPatientCarePlan(_dto);
            //Assert 
            result.Value.Should().BeOfType<ResponseVM>();

            switch (result.Value)
            {
                case null:
                    result.Value.Should().NotBeNull();
                    break;

                default:
                    var value = (ResponseVM)result.Value;
                    value.Success.Should().BeTrue();
                    value.Data.Should().BeOfType<long>();
                    value.Message.Should().BeEquivalentTo(ResponseConstants.Saved);

                    break; 
            }
             
            result.StatusCode.Should().Be(200);
             
        }

        
        [Fact]
        public async Task CreateWith_Username_With450Characters_ShouldPass()
        {
            //Arrange  

            var sut = new CarePlanController(_carePlanService);

            var character = DbInitializer.GenereateRandomAlphaNumeric(450);
            _dto.UserName = character;
            int lenght = character.Length;

            //Act 
            var result = (OkObjectResult)await sut.AddPatientCarePlan(_dto);


            //Assert 
            result.Value.Should().BeOfType<ResponseVM>();

            switch (result.Value)
            {
                case null:
                    result.Value.Should().NotBeNull();
                    break;

                default:
                    var value = (ResponseVM)result.Value;
                    value.Success.Should().BeTrue();
                    value.Data.Should().BeOfType<long>();
                    value.Message.Should().BeEquivalentTo(ResponseConstants.Saved);

                    break; 
            }
             
            result.StatusCode.Should().Be(200);
             
        }
        
        [Fact]
        public async Task CreateWith_Username_MoreThan450Characters_ShouldFail()
        {
            //Arrange  

            var sut = new CarePlanController(_carePlanService);

            var character = DbInitializer.GenereateRandomAlphaNumeric(451);
            _dto.UserName = character;
            int lenght = character.Length;
            //Act 
            var result = (BadRequestObjectResult)await sut.AddPatientCarePlan(_dto);


            //Assert 
            result.Value.Should().BeOfType<ResponseVM>();

            switch (result.Value)
            {
                case null:
                    result.Value.Should().NotBeNull();
                    break;

                default:
                    var value = (ResponseVM)result.Value;
                    value.Success.Should().BeFalse();
                    value.Data.Should().BeNull(); 
                    value.Message.Should().NotBeNull(); 
                     
                    break; 
            }
             
            result.StatusCode.Should().Be(400);
             
        }

        #endregion


        #region Patients Tests


        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public async Task CreateWith_Empty_NullPatientName_ShouldFail(string patientName)
        {
            //Arrange  

            var sut = new CarePlanController(_carePlanService);

            _dto.PatientName = patientName;


            //Act 
            var result = (BadRequestObjectResult)await sut.AddPatientCarePlan(_dto);


            //Assert 
            result.Value.Should().BeOfType<ResponseVM>();

            switch (result.Value)
            {
                case null:
                    result.Value.Should().NotBeNull();
                    break;

                default:
                    var value = (ResponseVM)result.Value;
                    value.Success.Should().BeFalse();
                    value.Data.Should().BeNull();
                    value.Message.Should().Be("'Patient Name' must not be empty.");

                    break;

            }

            result.StatusCode.Should().Be(400);

        }

         

        [Fact]
        public async Task CreateWith_PaientName_WithLessThan450Characters_ShouldPass()
        {
            //Arrange  

            var sut = new CarePlanController(_carePlanService);

            var character = DbInitializer.GenereateRandomAlphaNumeric(300);
            _dto.PatientName = character;
            _dto.UserName = userName;
            int lenght = character.Length;

            //Act 
            var result = (OkObjectResult)await sut.AddPatientCarePlan(_dto);


            //Assert 
            result.Value.Should().BeOfType<ResponseVM>();

            switch (result.Value)
            {
                case null:
                    result.Value.Should().NotBeNull();
                    break;

                default:
                    var value = (ResponseVM)result.Value;
                    value.Success.Should().BeTrue();
                    value.Data.Should().BeOfType<long>();
                    value.Message.Should().BeEquivalentTo(ResponseConstants.Saved);

                    break;
            }

            result.StatusCode.Should().Be(200);

        }


        [Fact]
        public async Task CreateWith_PaientName_With450Characters_ShouldPass()
        {
            //Arrange  

            var sut = new CarePlanController(_carePlanService);

            var character = DbInitializer.GenereateRandomAlphaNumeric(450);
            _dto.PatientName = character;
            _dto.UserName = userName;
            int lenght = character.Length;

            //Act 
            var result = (OkObjectResult)await sut.AddPatientCarePlan(_dto);


            //Assert 
            result.Value.Should().BeOfType<ResponseVM>();

            switch (result.Value)
            {
                case null:
                    result.Value.Should().NotBeNull();
                    break;

                default:
                    var value = (ResponseVM)result.Value;
                    value.Success.Should().BeTrue();
                    value.Data.Should().BeOfType<long>();
                    value.Message.Should().BeEquivalentTo(ResponseConstants.Saved);

                    break;
            }

            result.StatusCode.Should().Be(200);

        }

        [Fact]
        public async Task CreateWith_PaientName_MoreThan450Characters_ShouldFail()
        {
            //Arrange  

            var sut = new CarePlanController(_carePlanService);

            var character = DbInitializer.GenereateRandomAlphaNumeric(451);
            _dto.PatientName = character;
            _dto.UserName = userName;
            int lenght = character.Length;
            //Act 
            var result = (BadRequestObjectResult)await sut.AddPatientCarePlan(_dto);


            //Assert 
            result.Value.Should().BeOfType<ResponseVM>();

            switch (result.Value)
            {
                case null:
                    result.Value.Should().NotBeNull();
                    break;

                default:
                    var value = (ResponseVM)result.Value;
                    value.Success.Should().BeFalse();
                    value.Data.Should().BeNull();
                    value.Message.Should().NotBeNull();

                    break;
            }

            result.StatusCode.Should().Be(400);

        }


        #endregion


        #region Title Tests


        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public async Task CreateWith_Empty_NullTitle_ShouldFail(string title)
        {
            //Arrange  

            var sut = new CarePlanController(_carePlanService);

            _dto.Title = title;


            //Act 
            var result = (BadRequestObjectResult)await sut.AddPatientCarePlan(_dto);


            //Assert 
            result.Value.Should().BeOfType<ResponseVM>();

            switch (result.Value)
            {
                case null:
                    result.Value.Should().NotBeNull();
                    break;

                default:
                    var value = (ResponseVM)result.Value;
                    value.Success.Should().BeFalse();
                    value.Data.Should().BeNull();
                    value.Message.Should().Be("'Title' must not be empty.");

                    break;

            }

            result.StatusCode.Should().Be(400);

        }

         
        [Fact]
        public async Task CreateWith_Title_WithLessThan450Characters_ShouldPass()
        {
            //Arrange  

            var sut = new CarePlanController(_carePlanService);

            var character = DbInitializer.GenereateRandomAlphaNumeric(300);
            _dto.Title = character;
            _dto.UserName = userName;
            int lenght = character.Length;

            //Act 
            var result = (OkObjectResult)await sut.AddPatientCarePlan(_dto);


            //Assert 
            result.Value.Should().BeOfType<ResponseVM>();

            switch (result.Value)
            {
                case null:
                    result.Value.Should().NotBeNull();
                    break;

                default:
                    var value = (ResponseVM)result.Value;
                    value.Success.Should().BeTrue();
                    value.Data.Should().BeOfType<long>();
                    value.Message.Should().BeEquivalentTo(ResponseConstants.Saved);

                    break;
            }

            result.StatusCode.Should().Be(200);

        }


        [Fact]
        public async Task CreateWith_Title_With450Characters_ShouldPass()
        {
            //Arrange  

            var sut = new CarePlanController(_carePlanService);

            var character = DbInitializer.GenereateRandomAlphaNumeric(450);
            _dto.Title = character;
            _dto.UserName = userName;
            int lenght = character.Length;

            //Act 
            var result = (OkObjectResult)await sut.AddPatientCarePlan(_dto);


            //Assert 
            result.Value.Should().BeOfType<ResponseVM>();

            switch (result.Value)
            {
                case null:
                    result.Value.Should().NotBeNull();
                    break;

                default:
                    var value = (ResponseVM)result.Value;
                    value.Success.Should().BeTrue();
                    value.Data.Should().BeOfType<long>();
                    value.Message.Should().BeEquivalentTo(ResponseConstants.Saved);

                    break;
            }

            result.StatusCode.Should().Be(200);

        }


        [Fact]
        public async Task CreateWith_Title_MoreThan450Characters_ShouldFail()
        {
            //Arrange  

            var sut = new CarePlanController(_carePlanService);

            var character = DbInitializer.GenereateRandomAlphaNumeric(451);
            _dto.Title = character;
            _dto.UserName = userName;
            int lenght = character.Length;
            //Act 
            var result = (BadRequestObjectResult)await sut.AddPatientCarePlan(_dto);


            //Assert 
            result.Value.Should().BeOfType<ResponseVM>();

            switch (result.Value)
            {
                case null:
                    result.Value.Should().NotBeNull();
                    break;

                default:
                    var value = (ResponseVM)result.Value;
                    value.Success.Should().BeFalse();
                    value.Data.Should().BeNull();
                    value.Message.Should().NotBeNull();

                    break;
            }

            result.StatusCode.Should().Be(400);

        }

        #endregion



        #region Action Tests



        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public async Task CreateWith_Empty_NullAction_ShouldFail(string action)
        {
            //Arrange  

            var sut = new CarePlanController(_carePlanService);

            _dto.Action = action;


            //Act 
            var result = (BadRequestObjectResult)await sut.AddPatientCarePlan(_dto);


            //Assert 
            result.Value.Should().BeOfType<ResponseVM>();

            switch (result.Value)
            {
                case null:
                    result.Value.Should().NotBeNull();
                    break;

                default:
                    var value = (ResponseVM)result.Value;
                    value.Success.Should().BeFalse();
                    value.Data.Should().BeNull();
                    value.Message.Should().Be("'Action' must not be empty.");

                    break;

            }

            result.StatusCode.Should().Be(400);

        }

         

        [Fact]
        public async Task CreateWith_Action_WithLessThan1000Characters_ShouldPass()
        {
            //Arrange  

            var sut = new CarePlanController(_carePlanService);

            var character = DbInitializer.GenereateRandomAlphaNumeric(500);
            _dto.Action = character;
            _dto.UserName = userName;
            int lenght = character.Length;

            //Act 
            var result = (OkObjectResult)await sut.AddPatientCarePlan(_dto);


            //Assert 
            result.Value.Should().BeOfType<ResponseVM>();

            switch (result.Value)
            {
                case null:
                    result.Value.Should().NotBeNull();
                    break;

                default:
                    var value = (ResponseVM)result.Value;
                    value.Success.Should().BeTrue();
                    value.Data.Should().BeOfType<long>();
                    value.Message.Should().BeEquivalentTo(ResponseConstants.Saved);

                    break;
            }

            result.StatusCode.Should().Be(200);

        }


        [Fact]
        public async Task CreateWith_Action_With1000Characters_ShouldPass()
        {
            //Arrange  

            var sut = new CarePlanController(_carePlanService);

            var character = DbInitializer.GenereateRandomAlphaNumeric(1000);
            _dto.Action = character;
            _dto.UserName = userName;
            int lenght = character.Length;

            //Act 
            var result = (OkObjectResult)await sut.AddPatientCarePlan(_dto);


            //Assert 
            result.Value.Should().BeOfType<ResponseVM>();

            switch (result.Value)
            {
                case null:
                    result.Value.Should().NotBeNull();
                    break;

                default:
                    var value = (ResponseVM)result.Value;
                    value.Success.Should().BeTrue();
                    value.Data.Should().BeOfType<long>();
                    value.Message.Should().BeEquivalentTo(ResponseConstants.Saved);

                    break;
            }

            result.StatusCode.Should().Be(200);

        }


        [Fact]
        public async Task CreateWith_Action_MoreThan1000Characters_ShouldFail()
        {
            //Arrange  

            var sut = new CarePlanController(_carePlanService);

            var character = DbInitializer.GenereateRandomAlphaNumeric(1001);
            _dto.Action = character;
            _dto.UserName = userName;
            int lenght = character.Length;
            //Act 
            var result = (BadRequestObjectResult)await sut.AddPatientCarePlan(_dto);


            //Assert 
            result.Value.Should().BeOfType<ResponseVM>();

            switch (result.Value)
            {
                case null:
                    result.Value.Should().NotBeNull();
                    break;

                default:
                    var value = (ResponseVM)result.Value;
                    value.Success.Should().BeFalse();
                    value.Data.Should().BeNull();
                    value.Message.Should().NotBeNull();

                    break;
            }

            result.StatusCode.Should().Be(400);

        }


        #endregion



        #region IpAddress Tests



        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public async Task CreateWith_Empty_NullIpAddress_ShouldFail(string ip)
        {
            //Arrange  

            var sut = new CarePlanController(_carePlanService);

            _dto.IpAddress = ip;


            //Act 
            var result = (BadRequestObjectResult)await sut.AddPatientCarePlan(_dto);


            //Assert 
            result.Value.Should().BeOfType<ResponseVM>();

            switch (result.Value)
            {
                case null:
                    result.Value.Should().NotBeNull();
                    break;

                default:
                    var value = (ResponseVM)result.Value;
                    value.Success.Should().BeFalse();
                    value.Data.Should().BeNull();
                    value.Message.Should().Be("'Ip Address' must not be empty.");

                    break;

            }

            result.StatusCode.Should().Be(400);

        }
         


        #endregion



        #region Target Start Date Tests

         
        [Fact]
        public async Task CreateWith_TargetStartDate_WithRightValue_ShouldPass()
        {
            //Arrange  

            var sut = new CarePlanController(_carePlanService);
            _dto.UserName = userName;
            _dto.ActualStartDate = DateTime.Now;

            //Act 
            var result = (OkObjectResult)await sut.AddPatientCarePlan(_dto);



            //Assert 
            result.Value.Should().BeOfType<ResponseVM>();

            switch (result.Value)
            {
                case null:
                    result.Value.Should().NotBeNull();
                    break;

                default:
                    var value = (ResponseVM)result.Value;
                    value.Success.Should().BeTrue();
                    value.Data.Should().BeOfType<long>();
                    value.Message.Should().BeEquivalentTo(ResponseConstants.Saved);
                    break;

            }

            result.StatusCode.Should().Be(200);

        }

        [Fact]
        public async Task CreateWith_TargetStartDate_WithMinValue_ShouldFail()
        {
            //Arrange  

            var sut = new CarePlanController(_carePlanService);

            _dto.ActualStartDate = Convert.ToDateTime(minValue);

            //Act 
            var result = (BadRequestObjectResult)await sut.AddPatientCarePlan(_dto);



            //Assert 
            result.Value.Should().BeOfType<ResponseVM>();

            switch (result.Value)
            {
                case null:
                    result.Value.Should().NotBeNull();
                    break;

                default:
                    var value = (ResponseVM)result.Value;
                    value.Success.Should().BeFalse();
                    value.Data.Should().BeNull();
                    value.Message.Should().Be("'Actual Start Date' must not be empty.");
                    break;

            }

            result.StatusCode.Should().Be(400);

        }


        #endregion



        #region ActualStart Date Tests

         
        [Fact]
        public async Task CreateWith_ActualStartDate_WithRightValue_ShouldPass()
        {
            //Arrange  

            var sut = new CarePlanController(_carePlanService);
            _dto.UserName = userName;
            _dto.ActualStartDate = DateTime.Now;

            //Act 
            var result = (OkObjectResult)await sut.AddPatientCarePlan(_dto);



            //Assert 
            result.Value.Should().BeOfType<ResponseVM>();

            switch (result.Value)
            {
                case null:
                    result.Value.Should().NotBeNull();
                    break;

                default:
                    var value = (ResponseVM)result.Value;
                    value.Success.Should().BeTrue();
                    value.Data.Should().BeOfType<long>();
                    value.Message.Should().BeEquivalentTo(ResponseConstants.Saved);
                    break;

            }

            result.StatusCode.Should().Be(200);

        }

        [Fact]
        public async Task CreateWith_ActualStartDate_WithMinValue_ShouldFail()
        {
            //Arrange  

            var sut = new CarePlanController(_carePlanService);

            _dto.ActualStartDate = Convert.ToDateTime(minValue);

            //Act 
            var result = (BadRequestObjectResult)await sut.AddPatientCarePlan(_dto);



            //Assert 
            result.Value.Should().BeOfType<ResponseVM>();

            switch (result.Value)
            {
                case null:
                    result.Value.Should().NotBeNull();
                    break;

                default:
                    var value = (ResponseVM)result.Value;
                    value.Success.Should().BeFalse();
                    value.Data.Should().BeNull();
                    value.Message.Should().Be("'Actual Start Date' must not be empty.");
                    break;

            }

            result.StatusCode.Should().Be(400);

        }



        #endregion


        #region Actaul EndDate Tests No

        [Fact]
        public async Task CreateWith_CompletedNo_ShouldPass()
        {
            //Arrange  

            var sut = new CarePlanController(_carePlanService);
            var model = _dtoList.Where(x => x.Completed == false).FirstOrDefault();

            model.UserName = userName;

            //Act 
            var result = (OkObjectResult)await sut.AddPatientCarePlan(model);


            //Assert 
            result.Value.Should().BeOfType<ResponseVM>();

            switch (result.Value)
            {
                case null:
                    result.Value.Should().NotBeNull();
                    break;

                default:
                    var value = (ResponseVM)result.Value;
                    value.Success.Should().BeTrue();
                    value.Data.Should().BeOfType<long>();
                    value.Message.Should().BeEquivalentTo(ResponseConstants.Saved);
                    break;

            }

            result.StatusCode.Should().Be(200);

        }

        [Fact]
        public async Task CreateWith_CompletedNo_NullOutcome_ShouldPass()
        {
            //Arrange  

            var sut = new CarePlanController(_carePlanService);
            var model = _dtoList.Where(x => x.Completed == false).FirstOrDefault();

            model.UserName = userName;
            model.Outcome = null;

            //Act 
            var result = (OkObjectResult)await sut.AddPatientCarePlan(model);


            //Assert 
            result.Value.Should().BeOfType<ResponseVM>();

            switch (result.Value)
            {
                case null:
                    result.Value.Should().NotBeNull();
                    break;

                default:
                    var value = (ResponseVM)result.Value;
                    value.Success.Should().BeTrue();
                    value.Data.Should().BeOfType<long>();
                    value.Message.Should().BeEquivalentTo(ResponseConstants.Saved);
                    break;

            }

            result.StatusCode.Should().Be(200);

        }

        

        [Fact]
        public async Task CreateWith_CompletedNo_MinValueEndDate_ShouldPass()
        {
            //Arrange  

            var sut = new CarePlanController(_carePlanService);
            var model = _dtoList.Where(x => x.Completed == false).FirstOrDefault();

            model.UserName = userName;
            model.ActualEndDate = Convert.ToDateTime(minValue);

            //Act 
            var result = (OkObjectResult)await sut.AddPatientCarePlan(model);


            //Assert 
            result.Value.Should().BeOfType<ResponseVM>();

            switch (result.Value)
            {
                case null:
                    result.Value.Should().NotBeNull();
                    break;

                default:
                    var value = (ResponseVM)result.Value;
                    value.Success.Should().BeTrue();
                    value.Data.Should().BeOfType<long>();
                    value.Message.Should().BeEquivalentTo(ResponseConstants.Saved);
                    break;

            }

            result.StatusCode.Should().Be(200);

        }


        #endregion



        #region Completed Tests Yes


        [Fact]
        public async Task CreateWith_CompletedYes_ShouldPass()
        {
            //Arrange  

            var sut = new CarePlanController(_carePlanService);
            var model = _dtoList.Where(x => x.Completed).FirstOrDefault();

            model.UserName = userName;  

            //Act 
            var result = (OkObjectResult)await sut.AddPatientCarePlan(model);


            //Assert 
            result.Value.Should().BeOfType<ResponseVM>();

            switch (result.Value)
            {
                case null:
                    result.Value.Should().NotBeNull();
                    break;

                default:
                    var value = (ResponseVM)result.Value;
                    value.Success.Should().BeTrue();
                    value.Data.Should().BeOfType<long>();
                    value.Message.Should().BeEquivalentTo(ResponseConstants.Saved);
                    break;

            }
             
            result.StatusCode.Should().Be(200);
             
        }

        [Fact]
        public async Task CreateWith_CompletedYes_NullOutCome_ShouldFail()
        {
            //Arrange  

            var sut = new CarePlanController(_carePlanService);
            var model = _dtoList.Where(x => x.Completed).FirstOrDefault();

            model.UserName = userName;  
            model.Outcome = null;  

            //Act 
            var result = (BadRequestObjectResult)await sut.AddPatientCarePlan(model);


            //Assert 
            result.Value.Should().BeOfType<ResponseVM>();

            switch (result.Value)
            {
                case null:
                    result.Value.Should().NotBeNull();
                    break;

                default:
                    var value = (ResponseVM)result.Value;
                    value.Success.Should().BeFalse();
                    value.Data.Should().BeNull();
                    value.Message.Should().Be("'Outcome' must not be empty.");
                    break;

            }
             
            result.StatusCode.Should().Be(400);
             
        }

        [Fact]
        public async Task CreateWith_CompletedYes_EmptyOutCome_ShouldFail()
        {
            //Arrange  

            var sut = new CarePlanController(_carePlanService);
            var model = _dtoList.Where(x => x.Completed).FirstOrDefault();

            model.UserName = userName;  
            model.Outcome = string.Empty;  

            //Act 
            var result = (BadRequestObjectResult)await sut.AddPatientCarePlan(model);


            //Assert 
            result.Value.Should().BeOfType<ResponseVM>();

            switch (result.Value)
            {
                case null:
                    result.Value.Should().NotBeNull();
                    break;

                default:
                    var value = (ResponseVM)result.Value;
                    value.Success.Should().BeFalse();
                    value.Data.Should().BeNull();
                    break;

            }
             
            result.StatusCode.Should().Be(400);
             
        }




        [Fact]
        public async Task CreateWith_CompletedYes_EndDateMinValue_ShouldFail()
        {
            //Arrange  

            var sut = new CarePlanController(_carePlanService);
            var model = _dtoList.Where(x => x.Completed).FirstOrDefault();

            model.UserName = userName; 
            model.ActualEndDate = Convert.ToDateTime(minValue);

            //Act 
            var result = (BadRequestObjectResult)await sut.AddPatientCarePlan(model);


            //Assert 
            result.Value.Should().BeOfType<ResponseVM>();

            switch (result.Value)
            {
                case null:
                    result.Value.Should().NotBeNull();
                    break;

                default:
                    var value = (ResponseVM)result.Value;
                    value.Success.Should().BeFalse();
                    value.Data.Should().BeNull();
                    value.Message.Should().Be("'Actual End Date' must not be empty.");
                    break;

            }


            result.StatusCode.Should().Be(400);


        }


        [Fact]
        public async Task CreateWith_NoActualEndDate_ShouldFail()
        {
            //Arrange  

            var sut = new CarePlanController(_carePlanService);

            _dto.ActualStartDate = Convert.ToDateTime(minValue);

            //Act 
            var result = (BadRequestObjectResult)await sut.AddPatientCarePlan(_dto);


            //Assert 
            result.Value.Should().BeOfType<ResponseVM>();

            switch (result.Value)
            {
                case null:
                    result.Value.Should().NotBeNull();
                    break;

                default:
                    var value = (ResponseVM)result.Value;
                    value.Success.Should().BeFalse();
                    value.Data.Should().BeNull();
                    break;

            }


            result.StatusCode.Should().Be(400);


        }



        #endregion




    }
}
