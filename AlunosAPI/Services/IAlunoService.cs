using AlunosAPI.Models;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace AlunosAPI.Services
{
    public interface IAlunoService
    {
        Task<IEnumerable<Aluno>> GetStudents(int skip, int take);
        Task<Aluno> GetStudent(int id);
        Task<IEnumerable<Aluno>> GetStudentByName(string nome);
        Task CreateStudent(Aluno aluno);
        Task UpdateStudent(Aluno aluno);
        Task DeleteStudent(Aluno aluno);
    }
}
