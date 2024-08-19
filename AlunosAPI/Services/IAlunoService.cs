using AlunosAPI.Models;

namespace AlunosAPI.Services
{
    public interface IAlunoService
    {
        Task<IEnumerable<Aluno>> GetStudents();
        Task<Aluno> GetStudent(int id);
        Task<IEnumerable<Aluno>> GetStudentByName(string nome);
        Task CreateStudent(Aluno aluno);
        Task UpdateStudent(Aluno aluno);
        Task DeleteStudent(Aluno aluno);
    }
}
