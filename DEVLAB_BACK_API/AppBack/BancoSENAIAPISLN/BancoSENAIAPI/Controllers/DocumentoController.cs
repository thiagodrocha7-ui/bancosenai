using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;

namespace BancoSENAIAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class DocumentoController : Controller
    {
        private readonly string _caminhoRaiz = Path.Combine(
           Directory.GetCurrentDirectory(),
           "ClienteArquivos"
       );

        private static List<Models.DocumentoMetadado> _documentosMetadados = new List<Models.DocumentoMetadado>();

        private static int _nextId = 1;

        [HttpPost("upload/{codigoCliente}")]
        public async Task<IActionResult> AnexarArquivo(int codigoCliente, IFormFile arquivo)
        {

            if (arquivo == null || arquivo.Length == 0)
            {
                return BadRequest("Nenhum Arquivo foi enviado.");
            }
            string pastaCliente = Path.Combine(_caminhoRaiz, codigoCliente.ToString());

            if (!Directory.Exists(pastaCliente))
            {
                Directory.CreateDirectory(pastaCliente);
            }
            string extensao = Path.GetExtension(pastaCliente);
            string nomeOriginal = Path.GetFileNameWithoutExtension(arquivo.FileName);
            string novoNome = $"{codigoCliente}{nomeOriginal}{Guid.NewGuid()}{extensao}";
            string caminhoFinal = Path.Combine(pastaCliente, novoNome);

            using (var stream = new FileStream(caminhoFinal, FileMode.Create))
            {
                await arquivo.CopyToAsync(stream);
            }

            var documentoMetadados = new Models.DocumentoMetadado
            {
                Id = _nextId,
                Name = nomeOriginal,
                Extensao = extensao,
                Caminho = caminhoFinal,
                CodigoCliente = codigoCliente,
            };

            _documentosMetadados.Add(documentoMetadados);

            return Ok(new { mensagem = "Documento anexado com sucesso", arquivoSalvo = novoNome });

        }

        [HttpGet("api/v1/documento/listar/{codigoCliente}")]
        public IActionResult Listar(int codigoCliente)
        {
            
            var documentos = _documentosMetadados
                .Where(d => d.CodigoCliente == codigoCliente)
                .ToList();

            
            if (!documentos.Any())
            {
                return NotFound(); 
            }

          
            return Ok(documentos);
        }

        [HttpGet("api/v1/documento/download/{id}")]
        public IActionResult Download(int id)
        {
            
            var documento = _documentosMetadados
                .FirstOrDefault(d => d.Id == id);

            
            if (documento == null)
            {
                return NotFound();
            }

           
            byte[] fileBytes = System.IO.File.ReadAllBytes(documento.Caminho);

            
            return File(fileBytes, "application/octet-stream", documento.Name);
        }

        [HttpDelete("api/v1/documento/excluir/{id}")]
        public IActionResult Excluir(int id)
        {
            
            var documento = _documentosMetadados
                .FirstOrDefault(d => d.Id == id);

            
            if (documento == null)
            {
                return NotFound();
            }

         
            if (System.IO.File.Exists(documento.Caminho))
            {
                System.IO.File.Delete(documento.Caminho);
            }

           
            _documentosMetadados.Remove(documento);

            
            return Ok(new { mensagem = "Documento removido com sucesso." });
        }


    }
}
