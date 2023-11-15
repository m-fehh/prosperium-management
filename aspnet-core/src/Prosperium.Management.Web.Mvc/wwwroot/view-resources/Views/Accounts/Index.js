function formatarValor(elemento) {
    var valor = elemento.value.replace(/\D/g, '');
    valor = (parseFloat(valor) / 100).toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' });
    elemento.value = valor;
}

function mostrarCamposAgenciaConta(bankName) {
    var instituicaoSelecionada = document.getElementById('selectedBankId').value;
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
            $('#conta').mask('00000000-0');
            $('#conta').attr('maxlength', '10');
            break;
        case 'Itaú':
            $('#agencia').mask('0000');
            $('#conta').mask('00000-0');
            $('#conta').attr('maxlength', '7');
            break;
        case 'Bradesco':
            $('#agencia').mask('0000');
            $('#conta').mask('00000-0');
            $('#conta').attr('maxlength', '7');
            break;
        case 'Caixa':
            $('#agencia').mask('0000');
            $('#conta').mask('000000000-0');
            $('#conta').attr('maxlength', '11');
            break;
        case 'Santander':
            $('#agencia').mask('0000');
            $('#conta').mask('00000000-0');
            $('#conta').attr('maxlength', '10');
            break;
        case 'Inter':
            $('#agencia').mask('0000');
            $('#conta').mask('00000000-0');
            $('#conta').attr('maxlength', '10');
            break;
        case 'Mercado Pago':
            $('#agencia').mask('0000');
            $('#conta').mask('00000000');
            $('#conta').attr('maxlength', '8');
            break;
        case 'C6':
            $('#agencia').mask('0000');
            $('#conta').mask('000000-0');
            $('#conta').attr('maxlength', '7');
            break;
        case 'Neon':
            $('#agencia').mask('0000');
            $('#conta').mask('00000-0');
            $('#conta').attr('maxlength', '7');
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
            // Adicione as máscaras e limites de caracteres para o PicPay
            break;
        case 'Safra':
            // Adicione as máscaras e limites de caracteres para o Safra
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

function selectBank(bankId, bankName, imagePath) {
    var institutionElement = document.getElementById('instituicao');
    var selectedBankIdElement = document.getElementById('selectedBankId');
    var imageElement = document.getElementById('instituicaoLogo');

    institutionElement.innerText = bankName;
    selectedBankIdElement.value = bankId;

    // Mapeamento de nomes informais para os oficiais
    const nomeOficial = obterNomeOficial(bankName);

    // Inserir o nome oficial no campo de apelido
    document.getElementById('apelido').value = nomeOficial;

    // Obter o nome do arquivo da imagem atual
    var currentImageSrc = imageElement.src;
    var currentImageName = currentImageSrc.substring(currentImageSrc.lastIndexOf("/") + 1);

    var newImageSrc = currentImageSrc.replace(currentImageName, imagePath);

    // Atualizar a imagem na col-2
    imageElement.src = newImageSrc;

    // Tornar a imagem visível
    imageElement.style.display = 'block';

    $('#SelectInstitutionModal').modal('hide');

    mostrarCamposAgenciaConta(bankName);
}



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
        'PicPay': 'PicPay Serviços S.A.',
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

        // Ajustando o campo de BalanceAvailable
        var BalanceAvailableString = account['BalanceAvailable']
            .replace('R$', '')
            .replace(/\./g, '')
            .replace(',', '.');

        account['BalanceAvailable'] = parseFloat(BalanceAvailableString).toFixed(2);

        console.log("Após a mudança", account);

        _accountService.create(account).done(function () {
            abp.notify.info(l('SavedSuccessfully'));
            abp.nav.menus.refresh();
            abp.nav.menus.openActiveMenuItem({ url: '/app/Accounts' });
        }).fail(function (error) {
            console.error('Erro ao criar transação:', error);
        });
    });
})(jQuery);
