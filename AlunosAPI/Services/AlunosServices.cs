using AlunosAPI.Context;
using AlunosAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AlunosAPI.Services
{
    public class AlunosServices : IAlunoService
    {
        private readonly AppDbContext _context;

        public AlunosServices(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Aluno>> GetStudents()
        {
            try
            {
                return await _context.Alunos.ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<Aluno>> GetStudentByName(string nome)
        {
            try
            {
                IEnumerable<Aluno> alunos;

                if(!string.IsNullOrEmpty(nome))
                {
                    alunos = await _context.Alunos.Where(n => n.Nome.Contains(nome)).ToListAsync();
                }
                else
                {
                    alunos = await GetStudents();
                }
                return alunos;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Aluno> GetStudent(int id)
        {
            var aluno = await _context.Alunos.FindAsync(id);
            //FindAync proporciona um desempenho melhor do que o método FistOrDefault, pois busca os dados na memória.
            return aluno;
        }

        public async Task CreateStudent(Aluno aluno)
        {
            _context.Alunos.Add(aluno);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateStudent(Aluno aluno)
        {
            _context.Entry(aluno).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteStudent(Aluno aluno)
        {
            _context.Alunos.Remove(aluno);
            await _context.SaveChangesAsync();
        }
    }
}
