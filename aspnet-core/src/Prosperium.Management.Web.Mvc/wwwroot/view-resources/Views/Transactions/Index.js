document.addEventListener('DOMContentLoaded', function () {
    // Calendar
    flatpickr("#data", {
        enableTime: false,
        dateFormat: "d/m/Y",
        locale: "pt",
        maxDate: "today",
    });

    //Tags
    var inputTags = document.getElementById('tags');

    var tagify = new Tagify(inputTags, {
        duplicates: false,
        maxTags: 1,
    });

    inputTags.addEventListener('keydown', function (event) {
        if (event.key === 'Enter') {
            event.preventDefault();

            // Adiciona as tags apenas quando o usuário pressiona Enter
            tagify.addTags(inputTags.value);
        }
    });

    inputTags.style.border = 'none';
    inputTags.style.borderBottom = '2px solid #ccc';
    inputTags.style.fontSize = '18px';
    inputTags.style.color = '#333';
    inputTags.style.outline = 'none';
    inputTags.style.padding = '10px';
    inputTags.style.width = '100%';

    var labelTags = document.getElementById('labelTags');
    labelTags.style.marginBottom = '10px';
    labelTags.style.color = '#333';
    labelTags.style.display = 'block';
    labelTags.style.fontSize = '14px';
    labelTags.style.textAlign = 'left';
    labelTags.style.width = '100%';
});


function mostrarCamposAdicionais(tipoTransacao) {
    var campoCartao = document.querySelector('.campo-cartao');
    var campoCondicoes = document.querySelector('.campo-condicoes');

    if (tipoTransacao === 'gasto') {
        campoCartao.style.display = 'flex';
        campoCondicoes.style.display = 'flex';
    } else {
        campoCartao.style.display = 'none';
        campoCondicoes.style.display = 'none';
    }
}


function marcarOpcaoCartao(elemento, tipoCartao) {
    var botoesCartao = document.querySelectorAll('.opcao-cartao-btn');
    botoesCartao.forEach(function (botao) {
        botao.classList.remove('opcao-selecionada');
    });

    elemento.classList.add('opcao-selecionada');

    // Adicione o código para marcar a opção no seu modelo de dados (se necessário)
}

function marcarOpcaoCondicao(elemento, tipoCondicao) {
    var botoesCondicao = document.querySelectorAll('.opcao-condicao-btn');
    botoesCondicao.forEach(function (botao) {
        botao.classList.remove('opcao-selecionada');
    });

    elemento.classList.add('opcao-selecionada');

    // Adicione o código para marcar a opção no seu modelo de dados (se necessário)
}

function formatarValor(elemento) {
    var valor = elemento.value.replace(/\D/g, ''); // Remove não-dígitos
    valor = (parseFloat(valor) / 100).toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' });
    elemento.value = valor;
}