using AlunosAPI.Models;
using AlunosAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using AlunosAPI.Context;

namespace AlunosAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class AlunosController : ControllerBase
    {
        private IAlunoService _alunoService;
        private AppDbContext _context;

        public AlunosController(IAlunoService alunoService, AppDbContext context)
        {
            _alunoService = alunoService;
            _context = context;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("/List")]
        public async Task<ActionResult<IAsyncEnumerable<Aluno>>> GetStudents([FromQuery] int skip = 0, [FromQuery] int take = 10)
        {
            try
            {
                var totalCount = await _context.Alunos.CountAsync(); // Total count sem filtro

                var students = await _alunoService.GetStudents(skip, take);

                return Ok(new
                {
                    TotalCount = totalCount,
                    Skip = skip,
                    Take = take,
                    Students = students
                });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao retornar alunos");
            }
        }

        [HttpGet]
        [Produces("application/json")]
        [Route("/StudentsByName")]
        public async Task<ActionResult<IAsyncEnumerable<Aluno>>> GetStudentsbyName([FromQuery] string nameStudents)
        {

            try
            {
                var students = await _alunoService.GetStudentByName(nameStudents);

                if (students.Count() == 0)
                {
                    return NotFound($"Aluno {nameStudents} não encontrado");
                }

                return Ok(students);
            }
            catch
            {
                return BadRequest("Requst inválido");
            }
        }

        [HttpGet]
        [Route("StudentById", Name = "GetStudentById")]
        public async Task<ActionResult<Aluno>> GetStudentById(int id)
        {
            try
            {
                var student = await _alunoService.GetStudent(id);

                if (student == null)
                {
                    return NotFound($"Aluno com o ID: {id} não encontrado");
                }

                return Ok(student);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro interno do servidor");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("/CreateStudent")]
        public async Task<ActionResult> CreateStudent(Aluno student)
        {
            try
            {
                await _alunoService.CreateStudent(student);
                return CreatedAtRoute(nameof(GetStudentById), new { id = student.Id }, student);
            }
            catch (Exception ex)
            {
                return BadRequest("Request inválido" + ex);
            }
        }

        [HttpPut]
        [Route("UpdateStudent{id:int}")]
        public async Task<ActionResult> UpdateStudent(int id, [FromBody] Aluno student)
        {
            try
            {
                if (student.Id == id)
                {
                    await _alunoService.UpdateStudent(student);
                    return Ok($"Aluno {student.Nome} atualizado com sucesso!");
                }
                else
                {
                    return BadRequest("Dados incorretos");
                }
            }
            catch
            {
                return BadRequest("Request inválidos");
            }
        }

        [HttpDelete]
        [Route("DeleteStudent")]
        public async Task<ActionResult> DeleteStudent(int id)
        {
            try
            {
                var student = await _alunoService.GetStudent(id);

                if (student != null)
                {
                    await _alunoService.DeleteStudent(student);
                    return Ok($"Aluno com o ID: {id} foi excluido com sucesso!");
                }
                else
                {
                    return NotFound($"Aluno com o Id {id} não encontrado!");
                }
            }
            catch
            {
                return BadRequest("Request inválidos");
            }
        }
    }
}
