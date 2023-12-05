using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppMvcFuncional.Data;
using AppMvcFuncional.Models;

namespace AppMvcFuncional.Controllers
{
    [Route("meus-alunos")]
    public class AlunosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AlunosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Alunos
        public async Task<IActionResult> Index() //Fica com a rota padão meus-alunos
        {
            return _context.Aluno != null ?
                        View(await _context.Aluno.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Aluno'  is null.");
        }

        // GET: Alunos/Details/5
        [Route("detalhes/{id:int}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (_context.Aluno == null)
            {
                return NotFound();
            }

            var aluno = await _context.Aluno
                .FirstOrDefaultAsync(m => m.Id == id);
            if (aluno == null)
            {
                return NotFound();
            }

            return View(aluno);
        }

        // GET: Alunos/Create
        [Route("novo")]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost("novo")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,DataNascimento,Email,EmailConfirmacao,Avaliacao,Ativo")] Aluno aluno)
        {
            if (ModelState.IsValid)
            {
                _context.Add(aluno);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(aluno);
        }

        // GET: Alunos/Edit/5
        [Route("editar/{id:int}")]
        public async Task<IActionResult> Edit(int id)
        {
            if (_context.Aluno == null)
            {
                return NotFound();
            }

            var aluno = await _context.Aluno.FindAsync(id);
            if (aluno == null)
            {
                return NotFound();
            }
            return View(aluno);
        }


        [HttpPost(("editar/{id:int}"))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,DataNascimento,Email,Avaliacao,Ativo")] Aluno aluno)
        {
            if (id != aluno.Id)
            {
                return NotFound();
            }

            ModelState.Remove("EmailConfirmacao");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(aluno);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AlunoExists(aluno.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(aluno);
        }

        // GET: Alunos/Delete/5
        [Route("excluir/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (_context.Aluno == null)
            {
                return NotFound();
            }

            var aluno = await _context.Aluno
                .FirstOrDefaultAsync(m => m.Id == id);
            if (aluno == null)
            {
                return NotFound();
            }

            return View(aluno);
        }

        [HttpPost("excluir/{id:int}"), ActionName("Delete")]//action name é Delete, por mais que esteja com outro nome por conta que não posso ter duas actions com msm nome e msm parâ metro
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Aluno == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Aluno'  is null.");
            }
            var aluno = await _context.Aluno.FindAsync(id);
            if (aluno != null)
            {
                _context.Aluno.Remove(aluno);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AlunoExists(int id)
        {
          return (_context.Aluno?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
