using Dell.Lead.WeApi.Business;
using Dell.Lead.WeApi.Data.VO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Dell.Lead.WeApi.Controllers
{
    [ApiVersion("1")]
    [Authorize("Bearer")]
    [ApiController]
    [Route("api/v{version:ApiVersion}/contributors")]
    public class ContributorsController : ControllerBase
    {
        private readonly IContributorsBusiness _contributorsBusiness;
        public ContributorsController(IContributorsBusiness contributorsBusiness)
        {
            _contributorsBusiness = contributorsBusiness;
        }
        /// <summary>
        /// List all contributors.
        /// </summary>
        /// <remarks>
        /// Exemplo:
        ///
        ///     GET /api/v1/Contributors
        ///   
        ///        {
        ///        "code": 0,
        ///        "name": "string",
        ///        "cpf": 0,
        ///        "date_of_birth": "2022-01-01T16:03:16.910Z",
        ///        "cellfone": 0,
        ///        "gender": "string",
        ///        "address": {
        ///          "code": 0,
        ///          "street": "string",
        ///          "number": 0,
        ///          "district": "string",
        ///          "city": "string",
        ///          "state": "string",
        ///          "cep": "string"
        ///        }
        ///      }
        ///    
        ///
        /// </remarks>
        /// <returns> List contributors</returns>
        /// <response code="200">return list of the contributors</response>
        [HttpGet]
        public ActionResult<List<ContributorsVO>> FindAll()
        {
            var contributors = _contributorsBusiness.FindAll();
            return Ok(contributors);
        }

        /// <summary>
        /// search for contributors.
        /// </summary>
        /// <remarks>
        /// Exemplo:
        ///
        ///     GET /api/v1/contributors/CPF
        ///   
        ///        {
        ///        "id": 0,
        ///        "name": "string",
        ///        "cpf": 0,
        ///        "date_of_birth": "2022-01-01T16:03:16.910Z",
        ///        "cellfone": 0,
        ///        "gender": "string",
        ///        "address": {
        ///          "code": 0,
        ///          "street": "string",
        ///          "number": 0,
        ///          "district": "string",
        ///          "city": "string",
        ///          "state": "string",
        ///          "cep": "string"
        ///        }
        ///      }
        ///    
        ///
        /// </remarks>
        /// <param name="cpf">Isuer the cpf for search Contributors</param>
        /// <returns>Return contributors for cpf</returns>
        /// <response code="200">Return contributors secreat</response>
        /// <response code="400">Retrun invalid cpf</response>
        [HttpGet("{cpf}")]
        public ActionResult<ContributorsVO> FindByCpf(long cpf)
        {
                var contributors = _contributorsBusiness.FindByCpf(cpf);
                if(contributors == null) return BadRequest();
                return Ok(contributors);
        }

        /// <summary>
        /// Registred Contributors.
        /// </summary>
        /// <remarks>
        /// Exemplo:
        ///
        ///     POST /api/v1/Contribuidores/
        ///   
        ///        {
        ///        "id": 0,
        ///        "name": "string",
        ///        "cpf": 0,
        ///        "date_of_birth": "2022-01-01T16:03:16.910Z",
        ///        "cellfone": 0,
        ///        "gender": "string",
        ///        "address": {
        ///          "code": 0,
        ///          "street": "string",
        ///          "number": 0,
        ///          "district": "string",
        ///          "city": "string",
        ///          "state": "string",
        ///          "cep": "string"
        ///        }
        ///      }
        ///    
        ///
        /// </remarks>
        /// <returns>Return the contibutors if registred</returns>
        /// <response code="200">Return new user registered</response>
        /// <response code="400">Return invalid cpf</response>
        [HttpPost]
        public ActionResult Create([FromBody] ContributorsVO contributors)
        {
            if (contributors == null) return BadRequest("Funcionário inválido");
                var newContributors = _contributorsBusiness.Create(contributors);
                return Ok(newContributors);
        }

        /// <summary>
        /// Atualited the contributors registred.
        /// </summary>
        /// <remarks>
        /// Exemplo:
        ///
        ///     PUT /api/v1/Contribuidores/
        ///   
        ///        {
        ///        "id": 0,
        ///        "name": "string",
        ///        "cpf": 0,
        ///        "date_of_birth": "2022-01-01T16:03:16.910Z",
        ///        "cellfone": 0,
        ///        "gender": "string",
        ///        "address": {
        ///          "code": 0,
        ///          "street": "string",
        ///          "number": 0,
        ///          "district": "string",
        ///          "city": "string",
        ///          "state": "string",
        ///          "cep": "string"
        ///        }
        ///      }
        ///    
        ///
        /// </remarks>
        /// <returns>Return contributors if informations atualized</returns>
        /// <response code="200">Return contributors don't atualized</response>
        /// <response code="400">Return invalid cpf</response>
        [HttpPut]
        public ActionResult Put([FromBody] ContributorsVO contributors)
        {
            if (contributors == null) return BadRequest("Funcionário inválido");
                var changerEmployee = _contributorsBusiness.Update(contributors);
                return Ok(changerEmployee);
            
        }

        /// <summary>
        /// Delete contributors for CPF.
        /// </summary>
        /// <remarks>
        /// Exemplo:
        ///
        ///     DELETE /api/v1/Contribuidores/CPF
        ///   
        ///        {
        ///        "id": 0,
        ///        "name": "string",
        ///        "cpf": 0,
        ///        "date_of_birth": "2022-01-01T16:03:16.910Z",
        ///        "cellfone": 0,
        ///        "gender": "string",
        ///        "address": {
        ///          "code": 0,
        ///          "street": "string",
        ///          "number": 0,
        ///          "district": "string",
        ///          "city": "string",
        ///          "state": "string",
        ///          "cep": "string"
        ///        }
        ///      }
        ///    
        ///
        /// </remarks>
        /// <param name="cpf">Cpf and name you want delete registred</param>
        /// <returns>Retrun if contributors were delete</returns>
        /// <response code="201">Return if registred is delete</response>
        /// <response code="400">Return invalid cpf</response>
        [HttpDelete("{cpf}")]
        public ActionResult Delete(long cpf)
        {
                var employee = _contributorsBusiness.FindByCpf(cpf);
                if (employee == null) return BadRequest();
                _contributorsBusiness.Delete(cpf);
                return NoContent();
          
        }
    }
}
