
using AutoFixture; 
using FluentAssertions;
using Moq;
using Application.Common.Interfaces.IServices;
using WebAPI.Controllers;
using Application.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Domain.Constants; 
using Application.Dto;
using Castle.Core.Resource;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Web.Mvc;
using System.Collections.Specialized;
using System.Globalization;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;

namespace PatientCarePlan.Api.Tests.Controller
{
    public class CarePlanControllerTest
    {
        private readonly IFixture _fixture;
        private readonly Mock<IPatientCarePlanService> _serviceMock;
        private readonly CarePlanController _sut;

        public CarePlanControllerTest()
        {
            _fixture= new Fixture();
            _serviceMock = _fixture.Freeze<Mock<IPatientCarePlanService>>();
            _sut = new CarePlanController(_serviceMock.Object);
        }




        #region GetAll


        [Fact]
        public async Task GetAllPatientsCarePlan_ShouldReturnOkResponse_WhenDataFound()
        {
            //Arrange
            var response = new ResponseVM
            {
                Success = true,
                Data =  new List<PatientCarePlanVM>(),
                Message = ResponseConstants.Found
            }; 
             
            _serviceMock.Setup(x => x.GetAllPatientCarePlans())
                .ReturnsAsync(response);

            //Act
            var result = await _sut.GetAllPatientCarePlans().ConfigureAwait(false);

            //Assert

            Assert.NotNull(result);
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<OkObjectResult>();
            result.As<OkObjectResult>().Value
                .Should().NotBeNull()
                .And
                .BeOfType(response.GetType());
            var actualResponse = result.As<OkObjectResult>().Value;


            switch (actualResponse)
            {
                case null:
                    result.Should().NotBeNull();
                    break;

                default:
                    var value = (ResponseVM)actualResponse;
                    value.Success.Should().BeTrue();
                    value.Data.Should().NotBeNull();
                    value.Data.Should().BeAssignableTo<IEnumerable<PatientCarePlanVM>>();
                    value.Message.Should().BeEquivalentTo(ResponseConstants.Found);

                    break;
            }
             
            _serviceMock.Verify(x => x.GetAllPatientCarePlans(), Times.Once());
        }


        [Fact]
        public async Task GetAllPatientsCarePlan_ShouldReturnNotFound_WhenDataNotFound()
        {
            //Arrange
            var response = new ResponseVM
            {
                Success = true,
                Data = null,
                Message = ResponseConstants.NotFound
            };

            //var mc = _fixture.Create<ResponseVM>();
            //mc.Message = ResponseConstants.NotFound;
            //mc.Data = null;
            //mc.Success = true;
              
            _serviceMock.Setup(x => x.GetAllPatientCarePlans())
                .ReturnsAsync(response);

            //Act
            var result = await _sut.GetAllPatientCarePlans().ConfigureAwait(false);

            //Assert

            Assert.NotNull(result);
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<NotFoundObjectResult>();
            result.As<NotFoundObjectResult>().Value
                .Should().NotBeNull()
                .And
                .BeOfType(response.GetType());


            var actualResponse = result.As<NotFoundObjectResult>().Value; 
            switch (actualResponse)
            {
                case null:
                    result.Should().NotBeNull();
                    break;

                default:
                    var value = (ResponseVM)actualResponse;
                    value.Success.Should().BeTrue();
                    value.Data.Should().BeNull(); 
                    value.Message.Should().BeEquivalentTo(ResponseConstants.NotFound); 
                    break;
            }
              
            _serviceMock.Verify(x => x.GetAllPatientCarePlans(), Times.Once());
        }

        [Fact]
        public async Task GetAllPatientsCarePlan_ShouldReturnBadRequest_WhenExceptionThrown()
        {
            //Arrange

            var responseObj = _fixture.Create<ResponseVM>();


            _serviceMock.Setup(x => x.GetAllPatientCarePlans())
                .ReturnsAsync(responseObj);

            //Act
            var result = await _sut.GetAllPatientCarePlans();

            //Assert

            Assert.NotNull(result);
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<BadRequestObjectResult>();
            result.As<BadRequestObjectResult>().Value
                .Should().NotBeNull()
                .And
                .BeOfType(responseObj.GetType());

            result.Should().BeAssignableTo<IEnumerable<PatientCarePlanVM>>();

            _serviceMock.Verify(x => x.GetAllPatientCarePlans(), Times.Once());
        }




        #endregion


        #region GetById



        [Fact]
        public async Task GetPatientsCarePlanById_ShouldReturnOkResponse_WhenValidInput()
        {
            //Arrange

            var responseObj = _fixture.Create<ResponseVM>();

            var id = _fixture.Create<int>();


            _serviceMock.Setup(x => x.GetCarePlanById(id))
                .ReturnsAsync(responseObj);

            //Act
            var result = await _sut.GetCarePlanById(id);

            //Assert

            result.Should().NotBeNull();


            result.Should().BeAssignableTo<OkObjectResult>();
            result.As<OkObjectResult>().Value
                .Should()
                .NotBeNull()
                .And
                .BeOfType(responseObj.GetType());


            _serviceMock.Verify(x => x.GetCarePlanById(id), Times.Once());
        }




        [Fact]
        public async Task GetPatientsCarePlanById_ShouldReturnNotFound_WhenNoDataFound()
        {
            //Arrange

            var responseObj = _fixture.Create<ResponseVM>();
            responseObj = null;

            var id = _fixture.Create<int>();


            _serviceMock.Setup(x => x.GetCarePlanById(id))
                .ReturnsAsync(responseObj);

            //Act
            var result = await _sut.GetCarePlanById(id);

            //Assert

            result.Should().NotBeNull();


            result.Should().BeAssignableTo<NotFoundObjectResult>();
            result.As<NotFoundObjectResult>().Value
                .Should()
                .NotBeNull()
                .And
                .BeOfType(responseObj.GetType());


            _serviceMock.Verify(x => x.GetCarePlanById(id), Times.Once());
        }



        [Theory]
        [InlineData(0)]
        [InlineData(0.01)]
        public async Task GetPatientsCarePlanById_ShouldReturnBadRequest_WhenInputEquaulsOrLessThanZero(int number)
        {
            //Arrange

            var response = new ResponseVM
            {
                Success = false,
                Data = null,
                Message = ResponseConstants.InvalidId
            };
            var id = number;


            _serviceMock.Setup(x => x.GetCarePlanById(id))
                .ReturnsAsync(response);

            //Act
            var result = await _sut.GetCarePlanById(id);

            //Assert

            result.Should().NotBeNull();

            result.Should().BeAssignableTo<BadRequestObjectResult>();
            result.As<BadRequestObjectResult>().Value
                .Should()
                .NotBeNull()
                .And
                .BeOfType(response.GetType());


            _serviceMock.Verify(x => x.GetCarePlanById(id), Times.Once());
        }




        #endregion


        #region Create

         

        [Fact]
        public async Task CreatePatientsCarePlanById_ShouldReturnOkResponse_WhenValidInput()
        {
            //Arrange

            var request = _fixture.Create<PatientCarePlanDto>();
            var response = _fixture.Create<ResponseVM>();


            _serviceMock.Setup(x => x.AddPatientCarePlan(request))
                .ReturnsAsync(response);

            //Act
            var result = await _sut.AddPatientCarePlan(request);

            //Assert

            result.Should().NotBeNull();

            result.Should().BeAssignableTo<CreatedAtRouteResult>();
            result.As<CreatedAtRouteResult>().Value
                .Should()
                .NotBeNull()
                .And
                .BeOfType(response.GetType());


            _serviceMock.Verify(x => x.AddPatientCarePlan(request), Times.Never());
        }


        [Fact]
        public async Task CreatePatientsCarePlanById_ShouldReturnBadRequest_WhenInValidInput()
        {

            //Arrange

            var request = _fixture.Create<PatientCarePlanDto>();
            _sut.ModelState.AddModelError("Username", "The Username field is required");
            var response = new ResponseVM
            {
                Success = false,
                Data = null,
                Message = ResponseConstants.ModelStateInvalid
            };


            _serviceMock.Setup(x => x.AddPatientCarePlan(request))
            .ReturnsAsync(response);

            //Act
            var result = await _sut.AddPatientCarePlan(request);

            //Assert

            result.Should().NotBeNull();


            result.Should().BeAssignableTo<BadRequestObjectResult>();
            result.As<BadRequestObjectResult>().Value
                .Should()
                .NotBeNull()
                .And
                .BeOfType(response.GetType());


            _serviceMock.Verify(x => x.AddPatientCarePlan(request), Times.Never());
        }


        #endregion


        #region Delete


        [Fact]
        public async Task DeletePatientsCarePlanById_ShouldReturnOkResponse_WhenARecordIsDeleted()
        {

            //Arrange

            var id = _fixture.Create<int>();
            var ip = _fixture.Create<string>();
            var response = new ResponseVM
            {
                Success = true,
                Data = null,
                Message = ResponseConstants.Deleted
            };

            _serviceMock.Setup(x => x.DeleteCarePlanById(id, ip))
            .ReturnsAsync(response);

            //Act
            var result = await _sut.DeleteCarePlanById(id, ip);

            //Assert

            result.Should().NotBeNull();


            result.Should().BeAssignableTo<OkObjectResult>();
            result.As<OkObjectResult>().Value
                .Should()
                .NotBeNull()
                .And
                .BeOfType(response.GetType());


        }



        [Fact]
        public async Task DeletePatientsCarePlanById_ShouldReturnNotFound_WhenRecordNotFound()
        {

            //Arrange

            var id = _fixture.Create<int>();
            var ip = _fixture.Create<string>(); 

            var response = new ResponseVM
            {
                Success = true,
                Data = null,
                Message = ResponseConstants.NotDeleted
            };


            _serviceMock.Setup(x => x.DeleteCarePlanById(id, ip))
            .ReturnsAsync(response);

            //Act
            var result = await _sut.DeleteCarePlanById(id, ip);

            //Assert

            result.Should().NotBeNull();


            result.Should().BeAssignableTo<NotFoundObjectResult>();
            result.As<NotFoundObjectResult>().Value
                .Should()
                .NotBeNull()
                .And
                .BeOfType(response.GetType());


        }


         
        [Theory]
        [InlineData(0)]
        [InlineData(0.01)]
        public async Task DeletePatientsCarePlanById_ShouldReturnBadRequest_WhenInputEquaulsOrLessThanZero(int number)
        {
            //Arrange
            var response = new ResponseVM
            {
                Success = false,
                Data = null,
                Message = ResponseConstants.InvalidId
            };
             

            var id = number;
            var ip = _fixture.Create<string>();

            _serviceMock.Setup(x => x.DeleteCarePlanById(id, ip))
                .ReturnsAsync(response);


            //Act
            var result = await _sut.DeleteCarePlanById(id, ip);


            //Assert

            result.Should().NotBeNull();

            result.Should().BeAssignableTo<BadRequestObjectResult>();
            result.As<BadRequestObjectResult>().Value
                .Should()
                .NotBeNull()
                .And
                .BeOfType(response.GetType());


            _serviceMock.Verify(x => x.DeleteCarePlanById(id, ip), Times.Never());
        }



        #endregion


        #region Update

        [Fact]
        public async Task UpdatePatientsCarePlanById_ShouldReturnOkResponse_WhenRecordIsUpdated()
        {

            //Arrange

            var id = _fixture.Create<int>(); 
            var request = _fixture.Create<PatientCarePlanDto>(); 
            var response = new ResponseVM
            {
                Success = true,
                Data = null,
                Message = ResponseConstants.Updated
            };

            _serviceMock.Setup(x => x.UpdateCarePlanById(request, id))
            .ReturnsAsync(response);

            //Act
            var result = await _sut.UpdateCarePlanById(request, id);

            //Assert

            result.Should().NotBeNull(); 
            result.Should().BeAssignableTo<OkObjectResult>();
            result.As<OkObjectResult>().Value
                .Should()
                .NotBeNull()
                .And
                .BeOfType(response.GetType());


        }



        [Fact]
        public async Task UpdatePatientsCarePlanById_ShouldReturnBadRequest_WhenInputIsnvalid()
        {

            //Arrange

            var id = _fixture.Create<int>();
            var request = _fixture.Create<PatientCarePlanDto>();
            var response = new ResponseVM
            {
                Success = false,
                Data = null,
                Message = ResponseConstants.ModelStateInvalid
            };

            _sut.ModelState.AddModelError("Username", "The Username field is required");
            _serviceMock.Setup(x => x.UpdateCarePlanById(request, id))
            .ReturnsAsync(response);

            //Act
            var result = await _sut.UpdateCarePlanById(request, id);

            //Assert

            result.Should().NotBeNull();
            result.Should().BeAssignableTo<BadRequestObjectResult>();
            result.As<BadRequestObjectResult>().Value
                .Should()
                .NotBeNull()
                .And
                .BeOfType(response.GetType());


            _serviceMock.Verify(x => x.UpdateCarePlanById(request, id), Times.Never());
        }


        [Theory]
        [InlineData(0)]
        [InlineData(0.01)]
        public async Task UpdatePatientsCarePlanById_ShouldReturnBadRequest_WhenInputEquaulsOrLessThanZero(int number)
        {

            //Arrange

            var id = number;
            var request = _fixture.Create<PatientCarePlanDto>();
            var response = new ResponseVM
            {
                Success = false,
                Data = null,
                Message = ResponseConstants.InvalidId
            };

            _serviceMock.Setup(x => x.UpdateCarePlanById(request, id))
            .ReturnsAsync(response);

            //Act
            var result = await _sut.UpdateCarePlanById(request, id);

            //Assert

            result.Should().NotBeNull();
            result.Should().BeAssignableTo<BadRequestObjectResult>();
            result.As<BadRequestObjectResult>().Value
                .Should()
                .NotBeNull()
                .And
                .BeOfType(response.GetType());


            _serviceMock.Verify(x => x.UpdateCarePlanById(request, id), Times.Never());
        }



        #endregion

    }
}
