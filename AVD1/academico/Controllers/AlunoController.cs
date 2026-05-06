using Academico.Models;
using Academico.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Academico.Controllers
{
    public class AlunoController : Controller
    {
        private readonly IAlunoRepository _alunoRepository;

        public AlunoController(IAlunoRepository repository)
        {
            ArgumentNullException.ThrowIfNull(repository);
            _alunoRepository = repository;
        }

        public IActionResult Create()
        {
            return View(new Aluno());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Aluno aluno)
        {
            if (!ModelState.IsValid)
            {
                return View(aluno);
            }
            
            await _alunoRepository.Create(aluno);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Index()
        {
            var listaAlunos = await _alunoRepository.GetAll();
            return View(listaAlunos);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var alunoEncontrado = await _alunoRepository.GetById(id);
            if (alunoEncontrado == null)
            {
                return NotFound();
            }
            return View(alunoEncontrado);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Aluno aluno)
        {
            if (id != aluno.AlunoId)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(aluno);
            }

            await _alunoRepository.Edit(aluno);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var alunoEncontrado = await _alunoRepository.GetById(id);
            if (alunoEncontrado == null)
            {
                return NotFound();
            }
            return View(alunoEncontrado);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var alunoEncontrado = await _alunoRepository.GetById(id);
            if (alunoEncontrado == null)
            {
                return NotFound();
            }
            return View(alunoEncontrado);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _alunoRepository.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
