function formatarValor(elemento) {
    var valor = elemento.value.replace(/\D/g, '');
    valor = (parseFloat(valor) / 100).toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' });
    elemento.value = valor;
}

function mostrarCamposAgenciaConta(bankName) {
    var instituicaoSelecionada = selectedBankId;
    var camposAgenciaConta = document.getElementById('camposAgenciaConta');


    if (instituicaoSelecionada) {
        camposAgenciaConta.style.display = 'flex';
        aplicarMascaras(bankName);
    } else {
        camposAgenciaConta.style.display = 'none';
    }
}

function aplicarMascaras(banco) {
    console.log(banco);

    switch (banco) {
        case 'Nubank':
            $('#agencia').mask('0000');
            $('#conta').mask('000000-0');
            $('#conta').attr('maxlength', '8');
            break;
        case 'Itaú':
            $('#agencia').mask('0000');
            $('#conta').mask('00000-0');
            $('#conta').attr('maxlength', '7');
            break;
        case 'Bradesco':
            $('#agencia').mask('0000');
            $('#conta').mask('0000000-0');
            $('#conta').attr('maxlength', '9');
            break;
        case 'Caixa':
            $('#agencia').mask('0000');
            $('#conta').mask('00000000000-0');
            $('#conta').attr('maxlength', '13');
            break;
        case 'Santander':
            $('#agencia').mask('0000');
            $('#conta').mask('00000000-0');
            $('#conta').attr('maxlength', '10');
            break;
        case 'Inter':
            $('#agencia').mask('000');
            $('#conta').mask('0000-0');
            $('#conta').attr('maxlength', '6');
            break;
        case 'Mercado Pago':
            $('#agencia').mask('0000');
            $('#conta').mask('0000000000000-0');
            $('#conta').attr('maxlength', '15');
            break;
        case 'C6':
            $('#agencia').mask('0000');
            $('#conta').mask('000000-0');
            $('#conta').attr('maxlength', '8');
            break;
        case 'Neon':
            $('#agencia').mask('0000');
            $('#conta').mask('000000-0');
            $('#conta').attr('maxlength', '8');
            break;
        case 'BMG':
            $('#agencia').mask('0000');
            $('#conta').mask('00000-0');
            $('#conta').attr('maxlength', '7');
            break;
        case 'Banco do Brasil':
            $('#agencia').mask('0000');
            $('#conta').mask('00000-0');
            $('#conta').attr('maxlength', '7');
            break;
        case 'Banrisul':
            $('#agencia').mask('0000');
            $('#conta').mask('00000-0');
            $('#conta').attr('maxlength', '7');
            break;
        case 'PAN':
            $('#agencia').mask('0000');
            $('#conta').mask('00000000-0');
            $('#conta').attr('maxlength', '10');
            break;
        case 'PagBank':
            $('#agencia').mask('0000');
            $('#conta').mask('00000-0');
            $('#conta').attr('maxlength', '7');
            break;
        case 'PayPal':
            // Adicione as máscaras e limites de caracteres para o PayPal
            break;
        case 'PicPay':
            $('#agencia').mask('0000');
            $('#conta').mask('00000000');
            $('#conta').attr('maxlength', '8');
            break;
        case 'Safra':
            $('#agencia').mask('00000');
            $('#conta').mask('00000000-0');
            $('#conta').attr('maxlength', '10');
            break;
        case 'Sicredi':
            $('#agencia').mask('0000');
            $('#conta').mask('00000-0');
            $('#conta').attr('maxlength', '7');
            break;
        default:
            break;
    }
}

var selectedBankId = '';

$(document).on('click', '.institution-modal', function (e) {
    e.preventDefault();

    var bankName = $(this).data('banco-nome');
    var bankId = $(this).data('banco-id');
    var imagePath = $(this).data('banco-imagem');

    selectedBankId = bankId;

    var imageFullPath = abp.appPath + 'img/banks/' + imagePath;

    $('#instituicao').html(`
        <div style="display: flex; align-items: center; gap: 15px;">
            <img src="${imageFullPath}"  width="40" />
            <span style="font-size: 16px; color: #000; font-weight: bold; margin-left: 5px;">${bankName}</span>
        </div>
    `);

    $('#SelectInstitutionModal').modal('hide');

    // Mapeamento de nomes informais para os oficiais
    const nomeOficial = obterNomeOficial(bankName);

    // Inserir o nome oficial no campo de apelido
    document.getElementById('apelido').value = nomeOficial;

    mostrarCamposAgenciaConta(bankName);
});


function obterNomeOficial(nomeInformal) {
    const mapeamentoNomes = {
        'Nubank': 'Nu Pagamentos S.A.',
        'Ita\u00FA': 'Ita\u00FA Unibanco S.A.',
        'Bradesco': 'Banco Bradesco S.A.',
        'Caixa': 'Caixa Economica Federal',
        'Santander': 'Banco Santander (Brasil) S.A.',
        'Inter': 'Banco Inter S.A.',
        'Mercado Pago': 'MercadoPago',
        'C6': 'Banco C6 S.A.',
        'Neon': 'Neon Pagamentos S.A.',
        'BMG': 'Banco BMG S.A.',
        'Banco do Brasil': 'Banco do Brasil S.A.',
        'Banrisul': 'Banco do Estado do Rio Grande do Sul S.A.',
        'PAN': 'Banco PAN S.A.',
        'PagBank': 'PagSeguro Internet S.A.',
        'PayPal': 'PayPal do Brasil Serviços de Pagamentos Ltda.',
        'PicPay': 'PicPay Servicos S.A.',
        'Safra': 'Banco Safra S.A.',
        'Sicredi': 'Sicredi'
    };


    return mapeamentoNomes[nomeInformal] || nomeInformal;
}


$(document).on('click', '#instituicao', function (e) {
    e.preventDefault();
    abp.ajax({
        url: abp.appPath + 'App/Accounts/GetBanks',
        type: 'GET',
        dataType: 'html',
        success: function (content) {
            $('#SelectInstitutionModal div.modal-content').html(content);
        },
        error: function (e) { }
    });

});

// ABP
(function ($) {
    var _accountService = abp.services.app.account,
        l = abp.localization.getSource('Management'),
        _$form = $('#AccountCreate').find('form');


    $(document).on('click', '#btnSubmit', function (e) {
        e.preventDefault();

        if (!_$form.valid()) {
            return;
        }

        var account = _$form.serializeFormToObject();

        console.log("Antes a mudança", account);

        // Convertendo "on" para true e "off" para false
        account["MainAccount"] = account["MainAccount"] === "on";

        account["BankId"] = selectedBankId;

        // Ajustando o campo de BalanceAvailable
        var BalanceAvailableString = account['BalanceAvailable']
            .replace('R$', '')
            .replace(/\./g, '')
            .replace(',', '.');

        account['BalanceAvailable'] = parseFloat(BalanceAvailableString).toFixed(2);

        console.log("Após a mudança", account);

        _accountService.create(account).done(function () {
            abp.notify.info(l('SavedSuccessfully'));
            window.location.href = '/app/Accounts';
        }).fail(function (error) {
            console.error('Erro ao criar transação:', error);
        });
    });


    // Index.js - Desativa e ativar conta
    $(document).on('click', '#bntChangeStatus', function (e) {
        e.preventDefault

        var accountId = $(this).data('conta-id');
        var statusChange = $(this).data('status-conta');

        _accountService.statusChangeAccount(accountId, !statusChange).done(function () {
            abp.notify.info(l('SavedSuccessfully'));
            window.location.reload();
        });
    });
})(jQuery);

