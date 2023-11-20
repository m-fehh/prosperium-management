// Pega os meses subsequentes

document.addEventListener('DOMContentLoaded', function () {
    var paymentTypeInput = document.querySelector('input[name="PaymentType"]:checked');
    var paymentTermInput = document.querySelector('input[name="PaymentTerm"]:checked');
    var mesInput = document.getElementById('mes');



    function updateMesOptions() {
        var today = new Date();
        var currentMonth = today.getMonth();
        var currentYear = today.getFullYear();

        if (paymentTypeInput && paymentTermInput && mesInput) {
            // Limpa as op��es existentes
            mesInput.innerHTML = '';

            // Adiciona uma op��o vazia como placeholder
            var placeholderOption = document.createElement('option');
            placeholderOption.value = '';
            placeholderOption.text = 'Selecione a fatura'; 
            placeholderOption.classList.add('custom-placeholder');

            // Adiciona estilos diretamente ao placeholder
            placeholderOption.style.fontSize = '16px';
            placeholderOption.style.color = '#aaa';

            mesInput.appendChild(placeholderOption);

            for (var i = 0; i < 3; i++) {
                var nextMonth = (currentMonth + i) % 12;
                var nextYear = currentYear + Math.floor((currentMonth + i) / 12);

                var option = document.createElement('option');

                // Atribui o valor no formato desejado (m�s/ano)
                var monthValue = (nextMonth + 1).toString().padStart(2, '0'); // adiciona zero � esquerda se necess�rio
                option.value = monthValue + '/' + nextYear;

                // Formata o texto da op��o
                var monthName = new Intl.DateTimeFormat('pt-BR', { month: 'long' }).format(new Date(nextYear, nextMonth, 1));
                option.textContent = monthName.charAt(0).toUpperCase() + monthName.slice(1) + '/' + nextYear;

                mesInput.appendChild(option);
            }
        }
    }


    updateMesOptions();

    document.querySelectorAll('input[name="PaymentType"], input[name="PaymentTerm"]').forEach(function (input) {
        input.addEventListener('change', updateMesOptions);
    });

});


// Faz o calculo da parcela
function formatarMoeda(valor) {
    return parseFloat(valor).toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' });
}

function tratarValor(valorInput) {
    return parseFloat(valorInput
        .replace('R$', '')
        .replace(/\./g, '')
        .replace(',', '.')) || 0;
}

function calcularParcelas() {
    var valorInput = document.getElementById('valor').value;

    var valor = tratarValor(valorInput);

    var parcelas = parseInt(document.getElementById('parcela').value) || 1;

    var valorParcela = valor / parcelas;

    var resultado = parcelas + 'x = ' + formatarMoeda(valorParcela);
    document.getElementById('resultadoParcelas').innerText = resultado;
}

document.getElementById('valor').addEventListener('input', calcularParcelas);
document.getElementById('parcela').addEventListener('input', calcularParcelas);

document.getElementById('valor').addEventListener('change', function () {
    document.getElementById('valor').value = formatarMoeda(tratarValor(document.getElementById('valor').value));
    calcularParcelas();
});

document.getElementById('parcela').addEventListener('change', calcularParcelas);


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
        maxTags: 4,
        dropdown: {
            classname: "tags-look"
        }
    });

    inputTags.classList.add('custom-placeholder');

    inputTags.addEventListener('keydown', function (event) {
        if (event.key === 'Enter') {
            event.preventDefault();
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

    // Limpar o texto da div CATEGORIA
    $('#categoria').html('Selecione uma categoria');

    if (tipoTransacao === 'gasto') {
        campoCartao.style.display = 'flex';
        campoCondicoes.style.display = 'flex';
    } else {
        campoCartao.style.display = 'none';
        campoCondicoes.style.display = 'none';
    }

    // Se a op��o for 'transferencia', esconda a categoria
    if (tipoTransacao === 'transferencia') {
        $(".campo-categoria").css("display", "none")
    } else {
        $(".campo-categoria").css("display", "block")
    }
}



function marcarOpcaoCartao(elemento, tipoCartao) {
    var radioBtn = elemento.querySelector('input[type="radio"]');
    if (radioBtn) {
        radioBtn.checked = true;
    }

    var campoCartao = document.querySelector('.campo-cartao');
    var campoConta = document.querySelector('.campo-conta');
    var botaoParcelado = document.querySelector('.opcao-condicao-btn[onclick="marcarOpcaoCondicao(this, \'parcelado\')"]');
    var botaoParceladoSelecionado = document.querySelector('.opcao-condicao-btn.opcao-selecionada');
    var botaoAVista = document.querySelector('.opcao-condicao-btn[onclick="marcarOpcaoCondicao(this, \'avista\')"]');
    var campoParcelas = document.querySelector('.campo-parcela');
    var campoMes = document.querySelector('.campo-mes');

    if (tipoCartao === 'credito') {
        campoCartao.style.display = 'flex';
        campoConta.style.display = 'none';
        botaoParcelado.style.pointerEvents = 'auto';
        botaoAVista.style.pointerEvents = 'auto';

        $("#info-campo-condicao").css("display", "none");

        campoMes.style.display = 'flex';
    } else if (tipoCartao === 'debito') {
        campoParcelas.style.display = 'none';
        campoMes.style.display = 'none';
        botaoParcelado.style.pointerEvents = 'none';
        campoCartao.style.display = 'none';
        campoConta.style.display = 'flex';

        $("#info-campo-condicao").css("display", "flex");

        // Desmarcar a op��o parcelada se estiver selecionada
        if (botaoParceladoSelecionado) {
            botaoParceladoSelecionado.classList.remove('opcao-selecionada');
        }

        // Selecionar a op��o � Vista
        if (botaoAVista) {
            botaoAVista.classList.add('opcao-selecionada');
        }

        $("#data").val('');
    }

    var botoesCartao = document.querySelectorAll('.opcao-cartao-btn');
    botoesCartao.forEach(function (botao) {
        botao.classList.remove('opcao-selecionada');
    });

    elemento.classList.add('opcao-selecionada');
}


function marcarOpcaoCondicao(elemento, tipoCondicao) {
    var radioBtn = elemento.querySelector('input[type="radio"]');
    if (radioBtn) {
        radioBtn.checked = true;
    }

    var botoesCondicao = document.querySelectorAll('.opcao-condicao-btn');
    var campoParcelas = document.querySelector('.campo-parcela');
    var campoMes = document.querySelector('.campo-mes');

    if (tipoCondicao === 'parcelado') {
        campoParcelas.style.display = 'flex';
        campoMes.style.display = 'flex';
    } else {
        campoParcelas.style.display = 'none';
        campoMes.style.display = 'none';
    }

    botoesCondicao.forEach(function (botao) {
        botao.classList.remove('opcao-selecionada');
    });

    elemento.classList.add('opcao-selecionada');
}

function formatarValor(elemento) {
    var valor = elemento.value.replace(/\D/g, '');
    valor = (parseFloat(valor) / 100).toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' });
    elemento.value = valor;
}

// Seleciona a conta
var selectedAccountId = '';

$(document).on('click', '#conta', function (e) {
    e.preventDefault();

    abp.ajax({
        url: abp.appPath + 'App/Transactions/GetAccounts',
        type: 'GET',
        dataType: 'html',
        success: function (content) {
            $('#SelectAccountModal div.modal-content').html(content);
        },
        error: function (e) { }
    });
});

$(document).on('click', '.institution-modal', function (e) {
    e.preventDefault();

    var accountId = $(this).data('conta-id');
    var agencyNumber = $(this).data('conta-agencia');
    var accountNumber = $(this).data('conta-numero');
    var imagePath = $(this).data('conta-icon');
    var accountNickname = $(this).find('.card-title').text();

    selectedAccountId = accountId;

    var imageFullPath = abp.appPath + 'img/banks/' + imagePath;

    $('#conta').html(`
        <img src="${imageFullPath}" style="padding-bottom: 10px; margin-left: 10px;" width="40" />
        <span style="font-size: 16px; color: #000; font-weight: bold; margin-left: 5px;">${accountNickname}</span>
        <p style="font-size: 13px; color: #999; margin: 0;">Ag&ecirc;ncia: ${agencyNumber} Conta: ${accountNumber}</p>
    `);

    $('#SelectAccountModal').modal('hide');
});

var selectedCardId = '';
$(document).on('click', '#cartao', function (e) {
    e.preventDefault();

    abp.ajax({
        url: abp.appPath + 'App/Transactions/GetCards',
        type: 'GET',
        dataType: 'html',
        success: function (content) {
            $('#SelectCardModal div.modal-content').html(content);
        },
        error: function (e) { }
    });
});

$('#mes').change(function () {
    var valorSelecionado = $(this).val();

    var dataFormatada = dataFaturaCartaoTransacao + '/' + valorSelecionado;

    $("#data").val(dataFormatada);
});


dataFaturaCartaoTransacao = '';

$(document).on('click', '.card-modal', function (e) {
    e.preventDefault();

    var cardId = $(this).data('cartao-id');
    var imagePath = $(this).data('cartao-icon');
    var bankName = $(this).data('banco-name');
    var cardNumber = $(this).data('card-numero');
    var cardName = $(this).find('.card-title').text();
    var vencimentoCartao = $(this).data('vencimento');

    console.log(vencimentoCartao);

    dataFaturaCartaoTransacao = vencimentoCartao;
    selectedCardId = cardId;

    var imageFullPath = abp.appPath + 'img/flags/' + imagePath;

    $('#cartao').html(`
        <img src="${imageFullPath}" style="padding-bottom: 10px; margin-left: 10px;" width="40" />
        <span style="font-size: 16px; color: #000; font-weight: bold; margin-left: 5px;">${cardName}</span>
        <p style="font-size: 13px; color: #999; margin: 0;">${cardNumber}</p>
    `);

    $('#SelectCardModal').modal('hide');
    $('#mes').prop('disabled', false);
});


var categoriaSelecionada = '';
var subcategoriaSelecionada = '';

// Selecionar categoria
$(document).on('click', '#categoria', function (e) {
    e.preventDefault();

    abp.ajax({
        url: abp.appPath + 'App/Transactions/SelectCategory',
        type: 'GET',
        data: { tipoTransacao: tipoTransacaoSelecionado },
        dataType: 'html',
        success: function (content) {
            $('#SelectCategoryModal div.modal-content').html(content);

            // Limpar vari�veis quando o modal � aberto novamente
            categoriaSelecionada = '';
            subcategoriaSelecionada = '';
            modalAtualizado = false; // Reiniciar a flag para permitir atualiza��es no modal
        },
        error: function (e) { }
    });
});

var selectedCategoryId = null;

// Selecionar subcategoria
$(document).on('click', '.categoria-modal', function (e) {
    e.preventDefault();

    // Obter dados da categoria clicada
    var categoriaSelecionadaTemp = $(this).data('categoria-nome');
    var categoryId = $(this).data('categoria-id');
    var categoryIcon = $(this).data('categoria-icone');

    selectedCategoryId = categoryId;

    getSubcategories(categoryId, categoryIcon);

    // Remover o evento 'click' anterior da subcategoria
    $(document).off('click', '.subcategoria');

    $(document).on('click', '.subcategoria', function (e) {
        e.preventDefault();

        // Obter dados da subcategoria clicada
        subcategoriaSelecionada = $(this).data('subcategoria');

        var imageFullPath = abp.appPath + 'img/categories/' + categoryIcon;

        // Usar categoriaSelecionada e subcategoriaSelecionada conforme necess�rio
        $('#categoria').html(`
            <div style="padding: 10px; display: flex; align-items: center; gap: 15px;">
                <img src="${imageFullPath}" style="padding-bottom: 10px;" width="40" />
                <span style="font-size: 16px; color: #000; font-weight: bold; margin-left: 10px;">${categoriaSelecionadaTemp}</span>
                <p style="font-size: 13px; color: #999; margin: 0;">Subcategoria: ${subcategoriaSelecionada}</p>
            </div>
        `);

        // Restaurar as vari�veis para futuros usos
        categoriaSelecionada = '';
        subcategoriaSelecionada = '';

        $('#SelectCategoryModal').modal('hide');
    });
});

function getSubcategories(categoryid, categoryIcon) {
    $.ajax({
        url: abp.appPath + 'App/Transactions/SelectSubcategory',
        type: 'GET',
        data: { categoryId: categoryid },
        dataType: 'json',
        success: function (subcategories) {
            updateModalSubcategories(subcategories.result, categoryIcon);
        },
        error: function (error) {
            console.error('Erro na chamada AJAX:', error);
        }
    });
}

var modalAtualizado = false;

function updateModalSubcategories(subcategories, categoryIcon) {
    if (!modalAtualizado) {
        var subcategoriasHtml = `
            <div class="modal-body">
                <div role="tabpanel" class="tab-pane container" id="select-category">
                    <div class="row">
        `;

        var imageFullPath = abp.appPath + 'img/categories/' + categoryIcon;

        for (var i = 0; i < subcategories.length; i++) {
            subcategoriasHtml += `
                        <div class="col-md-12">
                            <div class="card categoria-modal subcategoria" data-subcategoria="${subcategories[i].name}" data-subcategoria-id="${subcategories[i].id}" data-categoria-id="${selectedCategoryId}">
                                <div class="card-body d-flex align-items-center" style="width: 100%;">
                                    <div class="col-1">
                                        <img src="${imageFullPath}" width="50" />
                                    </div>
                                    <div class="col-10 d-flex align-items-center" style="display: flex; align-items: center; justify-content: space-between;">
                                        <h3 class="card-title" style="color: #000; font-weight: bold; margin-left: 10px;">${subcategories[i].name}</h3>
                                    </div>  
                                    <div class="col-1">
                                        <div class="ml-auto arrow-right">
                                            <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" style="color: #FF8C00;" class="bi bi-arrow-right" viewBox="0 0 16 16">
                                                <path d="M11.354 8.354a.5.5 0 0 0 0-.708L9.172 5.464a.5.5 0 1 1 .707-.708l3 3a.5.5 0 0 1 0 .708l-3 3a.5.5 0 1 1-.707-.708L11.354 8.354z" />
                                            </svg>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
            `;
        }

        subcategoriasHtml += `
                    </div>
                </div>
            </div>
        `;

        $('#SelectCategoryModal div.modal-content').html(subcategoriasHtml);

        modalAtualizado = true;
    }
}

// Adicionar evento quando o modal � totalmente fechado para permitir novas aberturas
$('#SelectCategoryModal').on('hidden.bs.modal', function () {
    modalAtualizado = false;
});


// ABP
(function ($) {
    var _transactionService = abp.services.app.transaction,
        l = abp.localization.getSource('Management'),
        _$form = $('#TransactionCreate').find('form');


    $(document).on('click', '#btnSubmit', function (e) {
        e.preventDefault();


        if (!_$form.valid()) {
            return;
        }


        var transactionDto = _$form.serializeFormToObject();

        var selectedPaymentType = $('input[name="PaymentType"]:checked').val();
        var selectedPaymentTerm = $('input[name="PaymentTerm"]:checked').val();

        // Garante que os valores s�o n�meros inteiros
        transactionDto['PaymentType'] = parseInt(selectedPaymentType);
        transactionDto['PaymentTerm'] = parseInt(selectedPaymentTerm);


        

        console.log(selectedPaymentType);
        console.log(transactionDto);
        console.log(selectedPaymentTerm);

        transactionDto['CategoryId'] = selectedCategoryId;
        transactionDto['AccountId'] = selectedAccountId;
        transactionDto['CreditCardId'] = selectedCardId;

        transactionDto['Installments'] = +transactionDto['Installments'];

        // Ajuste para o campo ExpenseValue
        var expenseValueString = transactionDto['ExpenseValue']
            .replace('R$', '')
            .replace(/\./g, '')
            .replace(',', '.');

        transactionDto['ExpenseValue'] = parseFloat(expenseValueString).toFixed(2);

        // Ajuste para o campo Tags
        var tagsString = transactionDto['Tags'];
        transactionDto['Tags'] = tagsString ? JSON.parse(tagsString).map(tag => ({ Name: tag.value })) : [];

        // Verifique se a propriedade Tags est� presente no objeto e � uma matriz
        if (!transactionDto['Tags'] || !Array.isArray(transactionDto['Tags'])) {
            transactionDto['Tags'] = [];
        }

        // Ajuste para o campo Date
        var dateParts = transactionDto['Date'].split('/');
        transactionDto['Date'] = new Date(`${dateParts[1]}/${dateParts[0]}/${dateParts[2]}`);

        console.log(transactionDto);

        _transactionService.create(transactionDto).done(function () {
            abp.notify.info(l('SavedSuccessfully'));
            window.location.href = '/app/Extract';
        }).fail(function (error) {
            console.error('Erro ao criar transa��o:', error);
        });

    })

})(jQuery);
