const URL_API = 'https://localhost:7081/api/v1/Documento'

async function enviarDocumento() {
    const codigoCliente = document.getElementById("codigoCliente").value;
    const inputArquivo = document.getElementById("arquivo");
    const arquivo = inputArquivo.files[0];

    if (!codigoCliente || !arquivo) {
        alert("Informe o codigo do cliente e selecione um arquivo")
        return;
    }
    const dadosArquivo = new FormData();
    dadosArquivo.append("arquivo", arquivo);

    const response = await fetch(`${ URL_API }/upload/${ codigoCliente }`, {
        method: "POST",
        body: dadosArquivo
    });

    if (response.ok) {
        alert("Documento enviado com sucesso!");
        document.getElementById("codigoCliente").value = "";
        document.getElementById("arquivo").value = "";

    } else {
        const erro = await response.json();
        alert("Erro: " + (erro.message || "Falha ao enviar o documento"));

    }
}

const API_URL = "https://bancosenai.com"; // Substitua pela URL real da API

async function buscarDocumentos(clientId) {
    if (!clientId) return alert("Informe o código do cliente.");

    try {
        const response = await fetch(`${API_URL}?clienteId=${clientId}`);
        const documentos = await response.json();

        renderizarTabela(documentos);
    } catch (error) {
        console.error("Erro ao buscar documentos:", error);
    }
}

function renderizarTabela(documentos) {
    const tbody = document.getElementById("tabelaDocumentos");
    tbody.innerHTML = "";

    documentos.forEach(doc => {
        tbody.innerHTML += `
      <tr>
        <td>${doc.id}</td>
        <td>${doc.nome}</td>
        <td>${doc.extensao}</td>
        <td>
          <button class="btn-baixar" onclick="baixarArquivo(${doc.id}, '${doc.nome}')" style="background-color: yellow; color: black;">Baixar</button>
          <button class="btn-excluir" onclick="excluirArquivo(${doc.id}, '${doc.clienteId}')" style="background-color: red; color: white;">Excluir</button>
        </td>
      </tr>
    `;
    });
}

document.getElementById("btnBuscar").addEventListener("click", () => {
    const clientId = document.getElementById("searchClientId").value;
    buscarDocumentos(clientId);
});

async function baixarArquivo(documentoId, nomeArquivo) {
    try {
        const response = await fetch(`${API_URL}/download/${documentoId}`);
        const blob = await response.blob();

        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = nomeArquivo;
        document.body.appendChild(a);
        a.click();
        a.remove();
    } catch (error) {
        alert("Erro ao baixar o arquivo.");
    }
}

async function excluirArquivo(documentoId, clientId) {
    if (!confirm("Tem certeza que deseja excluir este documento?")) return;

    try {
        const response = await fetch(`${API_URL}/${documentoId}`, { method: 'DELETE' });

        if (response.ok) {
            alert("Arquivo excluído com sucesso!");
            buscarDocumentos(clientId); // Atualização automática imediata
        }
    } catch (error) {
        alert("Erro ao excluir o arquivo.");
    }
}

document.getElementById("formUpload").addEventListener("submit", async (e) => {
    e.preventDefault();

    const clientId = document.getElementById("clientIdUpload").value;
    const fileInput = document.getElementById("fileInput").files[0];

    const formData = new FormData();
    formData.append("clienteId", clientId);
    formData.append("arquivo", fileInput);

    try {
        const response = await fetch(`${API_URL}/upload`, {
            method: "POST",
            body: formData
        });

        if (response.ok) {
            alert("Documento enviado com sucesso!");

            // Sincroniza o campo de busca e atualiza a tabela automaticamente
            document.getElementById("searchClientId").value = clientId;
            buscarDocumentos(clientId);
        }
    } catch (error) {
        alert("Erro ao enviar arquivo.");
    }
});
