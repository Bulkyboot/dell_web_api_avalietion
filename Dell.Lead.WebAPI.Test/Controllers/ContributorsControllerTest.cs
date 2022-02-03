using Dell.Lead.WeApi.Business;
using Dell.Lead.WeApi.Controllers;
using Dell.Lead.WeApi.Data.VO;
using Dell.Lead.WeApi.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace Dell.Lead.WeApi.Test.Controllers
{
    //test dos controllers :)
    public class EmployeeControllerTest
    {

        private readonly Mock<IContributorsBusiness> _mockContributorsBusiness;

        public EmployeeControllerTest()
        {
            _mockContributorsBusiness = new Mock<IContributorsBusiness>();

        }

        private ContributorsController contributorsControllers(Mock<IContributorsBusiness> mockContributorsBusiness)
        {
            return new ContributorsController(mockContributorsBusiness.Object);

        }

        [Fact]
        public void FindAll()
        {
            List<ContributorsVO> contributors = new List<ContributorsVO>()
            {
                new ContributorsVO()
                {
                    Name = "Ruan Bezerra",
                    Cpf = 079510704300,
                    DateOfBirth = Convert.ToDateTime("2003-02-13T00:00:00"),
                    Gender = "Heterossexual",
                    Cellfone = 85985091635,
                    Address = new AddressVO()
                    {
                        Street = "Rua Jaime Vasconcelos",
                        Number = 170,
                        District = "Mucuripe",
                        City = "Fortaleza",
                        State = "Ceara",
                        Cep = 60165260
                    }
                },
                new ContributorsVO()
                {
                    Name = "Lia Abda",
                    Cpf = 08141040306,
                    DateOfBirth = Convert.ToDateTime("2003-02-13T00:00:00"),
                    Gender = "Heterossexual",
                    Cellfone = 85987402451,
                    Address = new AddressVO()
                    {
                        Street = "Rua 15 de Novembro",
                        Number = 400,
                        District = "Montese",
                        City = "Fortaleza",
                        State = "Ceara",
                        Cep = 40415165
                    }
                }
            };

            _mockContributorsBusiness.Setup(x => x.FindAll()).Returns(contributors);

            var contributorsController = contributorsControllers(_mockContributorsBusiness);
            ActionResult<List<ContributorsVO>> response = contributorsController.FindAll();
            OkObjectResult result = (OkObjectResult)response.Result;

            Assert.Equal(200, result.StatusCode);

        }

        [Fact]
        public void FindInvalidCpf()
        {
            var contributorsVO = new ContributorsVO()
            {
                Name = "Ruan Bezerra",
                Cpf = 079510704300,
                DateOfBirth = Convert.ToDateTime("2003-02-13T00:00:00"),
                Gender = "Heterossexual",
                Cellfone = 85985091635,
                Address = new AddressVO()
                {
                    Street = "Rua Jaime Vasconcelos",
                    Number = 170,
                    District = "Mucuripe",
                    City = "Fortaleza",
                    State = "Ceara",
                    Cep = 60165260

                }

            };

            _mockContributorsBusiness.Setup(x => x.FindByCpf(It.Is<long>(item => item.Equals(contributorsVO.Cpf)))).Throws(new CpfInvalidException("CPF Inválido"));

            var contributorsController = contributorsControllers(_mockContributorsBusiness);

            ActionResult<ContributorsVO> response = contributorsController.FindByCpf(contributorsVO.Cpf);
            BadRequestObjectResult result = (BadRequestObjectResult)response.Result;

            Assert.Equal(400, result.StatusCode);
            Assert.Equal("CPF Inválido", result.Value);
        }

        [Fact]
        public void NoExistFindCpf()
        {
            var contributorsVO = new ContributorsVO()
            {
                Name = "Ruan Bezerra",
                Cpf = 079510704300,
                DateOfBirth = Convert.ToDateTime("2003-02-13T00:00:00"),
                Gender = "Heterossexual",
                Cellfone = 85985091635,
                Address = new AddressVO()
                {
                    Street = "Rua Jaime Vasconcelos",
                    Number = 170,
                    District = "Mucuripe",
                    City = "Fortaleza",
                    State = "Ceara",
                    Cep = 60165260

                }

            };

            _mockContributorsBusiness.Setup(x => x.FindByCpf(It.Is<long>(item => !item.Equals(contributorsVO.Cpf)))).Throws(new ExistCpfException("CPF não cadastrado"));

            var contributorsController = contributorsControllers(_mockContributorsBusiness);

            ActionResult<ContributorsVO> response = contributorsController.FindByCpf(1425368);
            BadRequestObjectResult result = (BadRequestObjectResult)response.Result;

            Assert.Equal(400, result.StatusCode);
            Assert.Equal("CPF não cadastrado", result.Value);
        }

        [Fact]
        public void ExistFindByCpf()
        {
            var contributorsVO = new ContributorsVO()
            {
                Name = "Ruan Bezerra",
                Cpf = 079510704300,
                DateOfBirth = Convert.ToDateTime("2003-02-13T00:00:00"),
                Gender = "Heterossexual",
                Cellfone = 85985091635,
                Address = new AddressVO()
                {
                    Street = "Rua Jaime Vasconcelos",
                    Number = 170,
                    District = "Mucuripe",
                    City = "Fortaleza",
                    State = "Ceara",
                    Cep = 60165260

                }

            };

            _mockContributorsBusiness.Setup(x => x.FindByCpf(It.Is<long>(item => item.Equals(contributorsVO.Cpf)))).Throws(new ExistCpfException("CPF já esta cadastrado"));

            var contributorsController = contributorsControllers(_mockContributorsBusiness);

            ActionResult<ContributorsVO> response = contributorsController.FindByCpf(contributorsVO.Cpf);
            BadRequestObjectResult result = (BadRequestObjectResult)response.Result;

            Assert.Equal(400, result.StatusCode);
            Assert.Equal("CPF já esta cadastrado", result.Value);
        }

        [Fact]
        public void Create()
        {
            var contributorsVO = new ContributorsVO()
            {
                Name = "Ruan Bezerra",
                Cpf = 079510704300,
                DateOfBirth = Convert.ToDateTime("2003-02-13T00:00:00"),
                Gender = "Heterossexual",
                Cellfone = 85985091635,
                Address = new AddressVO()
                {
                    Street = "Rua Jaime Vasconcelos",
                    Number = 170,
                    District = "Mucuripe",
                    City = "Fortaleza",
                    State = "Ceara",
                    Cep = 60165260

                }

            };

            _mockContributorsBusiness.Setup(x => x.Create(It.IsAny<ContributorsVO>())).Returns(contributorsVO);

            var contributorsController = contributorsControllers(_mockContributorsBusiness);
            ActionResult<ContributorsVO> response = contributorsController.Create(contributorsVO);
            OkObjectResult result = (OkObjectResult)response.Result;

            Assert.Equal(200, result.StatusCode);
            Assert.Equal(contributorsVO, result.Value);
        }

        [Fact]
        public void SucessPut()
        {
            var contributorsVO = new ContributorsVO()
            {
                Name = "Ruan Bezerra",
                Cpf = 079510704300,
                DateOfBirth = Convert.ToDateTime("2003-02-13T00:00:00"),
                Gender = "Heterossexual",
                Cellfone = 85985091635,
                Address = new AddressVO()
                {
                    Street = "Rua Jaime Vasconcelos",
                    Number = 170,
                    District = "Mucuripe",
                    City = "Fortaleza",
                    State = "Ceara",
                    Cep = 60165260

                }

            };

            _mockContributorsBusiness.Setup(p => p.Update(It.IsAny<ContributorsVO>())).Returns(contributorsVO);

            var contributorsController = contributorsControllers(_mockContributorsBusiness);

            ActionResult<ContributorsVO> response = contributorsController.Put(contributorsVO);
            OkObjectResult result = (OkObjectResult)response.Result;

            Assert.Equal(200, result.StatusCode);
            Assert.Equal(contributorsVO, result.Value);

        }

        [Fact]
        public void SuccessDelete()
        {
            var contributorsVO = new ContributorsVO()
            {
                Name = "Ruan Bezerra",
                Cpf = 079510704300,
                DateOfBirth = Convert.ToDateTime("2003-02-13T00:00:00"),
                Gender = "Heterossexual",
                Cellfone = 85985091635,
                Address = new AddressVO()
                {
                    Street = "Rua Jaime Vasconcelos",
                    Number = 170,
                    District = "Mucuripe",
                    City = "Fortaleza",
                    State = "Ceara",
                    Cep = 60165260

                }

            };

            _mockContributorsBusiness.Setup(x => x.Delete(It.Is<long>(item => item.Equals(contributorsVO.Cpf))));

            var contributorsController = contributorsControllers(_mockContributorsBusiness);

            ActionResult<ContributorsVO> response = contributorsController.Delete(contributorsVO.Cpf);
            NoContentResult result = (NoContentResult)response.Result;

            Assert.Equal(204, result.StatusCode);
        }
    }
}