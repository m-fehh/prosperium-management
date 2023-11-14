function formatarValor(elemento) {
    var valor = elemento.value.replace(/\D/g, '');
    valor = (parseFloat(valor) / 100).toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' });
    elemento.value = valor;
}


function selectBank(bankId, bankName) {
    console.log(bankId);
    console.log(bankName);

    document.getElementById('instituicao').innerText = bankName;
    document.getElementById('selectedBankId').value = bankId;

    // Mapeamento de nomes informais para os oficiais
    const nomeOficial = obterNomeOficial(bankName);

    // Inserir o nome oficial no campo de apelido
    document.getElementById('apelido').value = nomeOficial;

    $('#SelectInstitutionModal').modal('hide');
}

function obterNomeOficial(nomeInformal) {
    const mapeamentoNomes = {
        'Nubank': 'Nubank',
        'Itaú': 'Itaú Unibanco',
        'Bradesco': 'Banco Bradesco',
        'Caixa': 'Caixa Econômica Federal',
        'Santander': 'Banco Santander',
        'Inter': 'Banco Inter',
        'Mercado Pago': 'Mercado Pago',
        'C6': 'C6 Bank',
        'Neon': 'Banco Neon',
        'BMG': 'Banco BMG',
        'Banco do Brasil': 'Banco do Brasil',
        'Banrisul': 'Banrisul',
        'PAN': 'Banco PAN',
        'PagBank': 'PagBank',
        'PayPal': 'PayPal',
        'PicPay': 'PicPay',
        'Safra': 'Banco Safra',
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
