using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrilhaApiDesafio.Context;
using TrilhaApiDesafio.Models;
using System.Linq;

namespace TrilhaApiDesafio.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TarefaController : ControllerBase
    {
        private readonly OrganizadorContext _context;

        public TarefaController(OrganizadorContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public IActionResult ObterPorId(int id)
        {
            // Buscar a tarefa pelo Id
            var tarefa = _context.Tarefas.Find(id);

            // Verificar se a tarefa foi encontrada
            if (tarefa == null)
                return NotFound();  // Se não encontrar, retorna NotFound

            return Ok(tarefa);  // Se encontrar, retorna a tarefa com status OK
        }

        [HttpGet("ObterTodos")]
        public IActionResult ObterTodos()
        {
            // Buscar todas as tarefas do banco
            var tarefas = _context.Tarefas.ToList();
            return Ok(tarefas);  // Retorna a lista de tarefas
        }

        [HttpGet("ObterPorTitulo")]
        public IActionResult ObterPorTitulo(string titulo)
        {
            // Buscar as tarefas que contenham o título recebido
            var tarefas = _context.Tarefas
                .Where(x => x.Titulo.Contains(titulo))
                .ToList();

            return Ok(tarefas);  // Retorna as tarefas encontradas
        }

        [HttpGet("ObterPorData")]
        public IActionResult ObterPorData(DateTime data)
        {
            // Buscar as tarefas que correspondem à data recebida
            var tarefas = _context.Tarefas
                .Where(x => x.Data.Date == data.Date)
                .ToList();

            return Ok(tarefas);  // Retorna as tarefas encontradas
        }

        [HttpGet("ObterPorStatus")]
        public IActionResult ObterPorStatus(EnumStatusTarefa status)
        {
            // Buscar as tarefas que correspondem ao status recebido
            var tarefas = _context.Tarefas
                .Where(x => x.Status == status)
                .ToList();

            return Ok(tarefas);  // Retorna as tarefas encontradas
        }

        [HttpPost]
        public IActionResult Criar(Tarefa tarefa)
        {
            // Validar se a data da tarefa é válida
            if (tarefa.Data == DateTime.MinValue)
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });

            // Adicionar a tarefa no contexto (adiciona no banco de dados)
            _context.Tarefas.Add(tarefa);
            _context.SaveChanges();  // Salvar as alterações no banco

            // Retorna a resposta CreatedAtAction, com o ID da tarefa criada
            return CreatedAtAction(nameof(ObterPorId), new { id = tarefa.Id }, tarefa);
        }

        [HttpPut("{id}")]
        public IActionResult Atualizar(int id, Tarefa tarefa)
        {
            // Buscar a tarefa no banco de dados pelo ID
            var tarefaBanco = _context.Tarefas.Find(id);

            // Verificar se a tarefa foi encontrada
            if (tarefaBanco == null)
                return NotFound();

            // Validar a data da tarefa
            if (tarefa.Data == DateTime.MinValue)
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });

            // Atualizar os dados da tarefa no banco
            tarefaBanco.Titulo = tarefa.Titulo;
            tarefaBanco.Descricao = tarefa.Descricao;
            tarefaBanco.Data = tarefa.Data;
            tarefaBanco.Status = tarefa.Status;

            _context.Tarefas.Update(tarefaBanco);  // Atualiza a tarefa no EF
            _context.SaveChanges();  // Salva as alterações no banco

            return Ok(tarefaBanco);  // Retorna a tarefa atualizada
        }

        [HttpDelete("{id}")]
        public IActionResult Deletar(int id)
        {
            // Buscar a tarefa no banco de dados pelo ID
            var tarefaBanco = _context.Tarefas.Find(id);

            // Verificar se a tarefa foi encontrada
            if (tarefaBanco == null)
                return NotFound();

            // Remover a tarefa do contexto (deleta do banco de dados)
            _context.Tarefas.Remove(tarefaBanco);
            _context.SaveChanges();  // Salva as alterações no banco

            return NoContent();  // Retorna NoContent (sucesso na remoção)
        }
    }
}
