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

    // Se a opção for 'transferencia', esconda a categoria
    if (tipoTransacao === 'transferencia') {
        $(".campo-categoria").css("display", "none")
    } else {
        $(".campo-categoria").css("display", "block")
    }
}


function marcarOpcaoCartao(elemento, tipoCartao) {
    var botoesCartao = document.querySelectorAll('.opcao-cartao-btn');
    botoesCartao.forEach(function (botao) {
        botao.classList.remove('opcao-selecionada');
    });

    elemento.classList.add('opcao-selecionada');
}

function marcarOpcaoCondicao(elemento, tipoCondicao) {
    var botoesCondicao = document.querySelectorAll('.opcao-condicao-btn');
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

            // Limpar variáveis quando o modal é aberto novamente
            categoriaSelecionada = '';
            subcategoriaSelecionada = '';
            modalAtualizado = false; // Reiniciar a flag para permitir atualizações no modal
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

        // Usar categoriaSelecionada e subcategoriaSelecionada conforme necessário
        $('#categoria').html(categoriaSelecionadaTemp + ' > ' + subcategoriaSelecionada);

        // Restaurar as variáveis para futuros usos
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

        for (var i = 0; i < subcategories.length; i++) {
            subcategoriasHtml += `
                        <div class="col-md-12">
                            <div class="card categoria-modal subcategoria" data-subcategoria="${subcategories[i].name}" data-subcategoria-id="${subcategories[i].id}" data-categoria-id="${selectedCategoryId}">
                                <div class="card-body d-flex align-items-center" style="width: 100%;">
                                    <div class="col-1">
                                        <i class="${categoryIcon}" style="margin-right: 10px; float: left; color: #FF8C00"></i>
                                    </div>
                                    <div class="col-10">
                                        <h5 class="card-title">${subcategories[i].name}</h5>
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

// Adicionar evento quando o modal é totalmente fechado para permitir novas aberturas
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

        transactionDto['CategoryId'] = selectedCategoryId;

        // Ajuste para o campo ExpenseValue
        var expenseValueString = transactionDto['ExpenseValue']
            .replace('R$', '') 
            .replace(/\./g, '') 
            .replace(',', '.'); 

        transactionDto['ExpenseValue'] = parseFloat(expenseValueString).toFixed(2);

        // Ajuste para o campo Tags
        var tagsString = transactionDto['Tags'];
        transactionDto['Tags'] = JSON.parse(tagsString).map(tag => ({ Name: tag.value }));

        // Ajuste para o campo Date
        var dateParts = transactionDto['Date'].split('/');
        transactionDto['Date'] = new Date(`${dateParts[1]}/${dateParts[0]}/${dateParts[2]}`);

        console.log(transactionDto);

        _transactionService.create(transactionDto).done(function () {
                abp.notify.info(l('SavedSuccessfully'));
            })
            .fail(function (error) {
                console.error('Erro ao criar transação:', error);
            });

    })

})(jQuery);
