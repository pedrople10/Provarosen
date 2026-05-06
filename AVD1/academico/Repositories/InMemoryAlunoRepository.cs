using Academico.Models;

namespace Academico.Repositories
{
    public class InMemoryAlunoRepository : IAlunoRepository
    {
        private readonly List<Aluno> _alunosList = new();
        private int _proximoId = 1;
        private readonly object _sincronizacao = new();

        public InMemoryAlunoRepository()
        {
            _alunosList.Add(new Aluno
            {
                AlunoId = _proximoId++,
                Nome = "Aluno Exemplo",
                Email = "aluno@exemplo.com",
                Telefone = "(11) 99999-9999",
                Endereco = "Rua Exemplo, 123",
                Complemento = "Bloco A",
                Bairro = "Centro",
                Municipio = "Cidade",
                Uf = "SP",
                Cep = "01234-567"
            });
        }

        public Task Create(Aluno aluno, CancellationToken cancellationToken = default)
        {
            lock (_sincronizacao)
            {
                aluno.AlunoId = _proximoId++;
                _alunosList.Add(aluno);
            }

            return Task.CompletedTask;
        }

        public Task Delete(int id, CancellationToken cancellationToken = default)
        {
            lock (_sincronizacao)
            {
                var alunoEncontrado = _alunosList.FirstOrDefault(a => a.AlunoId == id);
                if (alunoEncontrado != null)
                {
                    _alunosList.Remove(alunoEncontrado);
                }
            }

            return Task.CompletedTask;
        }

        public Task<IEnumerable<Aluno>> GetAll(CancellationToken cancellationToken = default)
        {
            IEnumerable<Aluno> listaAlunos;
            lock (_sincronizacao)
            {
                listaAlunos = _alunosList.ToList();
            }

            return Task.FromResult(listaAlunos);
        }

        public Task<Aluno?> GetById(int id, CancellationToken cancellationToken = default)
        {
            Aluno? aluno;
            lock (_sincronizacao)
            {
                aluno = _alunosList.FirstOrDefault(a => a.AlunoId == id);
            }

            return Task.FromResult(aluno);
        }

        public Task Edit(Aluno aluno, CancellationToken cancellationToken = default)
        {
            lock (_sincronizacao)
            {
                var alunoExistente = _alunosList.FirstOrDefault(a => a.AlunoId == aluno.AlunoId);
                if (alunoExistente != null)
                {
                    alunoExistente.Nome = aluno.Nome;
                    alunoExistente.Email = aluno.Email;
                    alunoExistente.Telefone = aluno.Telefone;
                    alunoExistente.Endereco = aluno.Endereco;
                    alunoExistente.Complemento = aluno.Complemento;
                    alunoExistente.Bairro = aluno.Bairro;
                    alunoExistente.Municipio = aluno.Municipio;
                    alunoExistente.Uf = aluno.Uf;
                    alunoExistente.Cep = aluno.Cep;
                }
            }

            return Task.CompletedTask;
        }

        public Task<bool> Exists(int id, CancellationToken cancellationToken = default)
        {
            bool existe;
            lock (_sincronizacao)
            {
                existe = _alunosList.Any(a => a.AlunoId == id);
            }

            return Task.FromResult(existe);
        }
    }
}
