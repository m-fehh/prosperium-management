var selectedFlagId = null;
var selectedAccountId = null;

function formatarCartao(input) {
    // Remove caracteres não numéricos
    let numeros = input.value.replace(/\D/g, '');

    // Adiciona a máscara se houver pelo menos 8 dígitos
    if (numeros.length >= 8) {
        input.value = numeros.replace(/(\d{4})(\d{4})(\d{0,8})/, '$1 **** **** $2');
    } else if (numeros.length > 4) {
        // Adiciona um espaço após o quarto caractere
        input.value = numeros.replace(/(\d{4})/, '$1 ').trim();
    } else {
        input.value = numeros;
    }
}

$(document).ready(function () {
    const diaVencimentoInput = document.getElementById('diaVencimento');

    diaVencimentoInput.addEventListener('input', function (event) {
        let inputValue = event.target.value;

        inputValue = inputValue.replace(/\D/g, '');

        inputValue = inputValue === '' ? '' : Math.min(parseInt(inputValue, 10), 31).toString();

        event.target.value = inputValue;
    });

    diaVencimentoInput.addEventListener('blur', function (event) {
        if (event.target.value.length === 1) {
            event.target.value = event.target.value.padStart(2, '0');
        }
    });

    // Seleciona a conta:
    $(document).on('click', '#conta', function (e) {
        e.preventDefault();

        abp.ajax({
            url: abp.appPath + 'App/CreditCard/GetAccounts',
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
            <img src="${imageFullPath}" style="margin-left: 10px;" width="40" />
            <span style="font-size: 16px; color: #000; font-weight: bold; margin-left: 5px;">${accountNickname}</span>
            <p style="font-size: 13px; color: #999; margin: 0;">Ag&ecirc;ncia: ${agencyNumber} Conta: ${accountNumber}</p>
        `);

        $('#SelectAccountModal').modal('hide');
    });

    // Seleciona bandeira
    $(document).on('click', '#bandeira', function (e) {
        e.preventDefault();

        abp.ajax({
            url: abp.appPath + 'App/CreditCard/GetFlags',
            type: 'GET',
            dataType: 'html',
            success: function (content) {
                $('#SelectFlagModal div.modal-content').html(content);
            },
            error: function (e) { }
        });
    });

    $(document).on('click', '.flag-modal', function (e) {
        console.log("chegou");
        e.preventDefault();

        var FlagName = $(this).data('bandeira-nome');
        var flagId = $(this).data('bandeira-id');
        var FlagIcon = $(this).data('bandeira-icone');

        selectedFlagId = flagId;

        var imageFullPath = abp.appPath + 'img/flags/' + FlagIcon;

        $('#bandeira').html(`
            <img src="${imageFullPath}" style="margin-left: 10px;" width="40" />
            <span style="font-size: 16px; color: #000; font-weight: bold; margin-left: 5px;">${FlagName}</span>
            <p style="font-size: 13px; color: #999; margin: 0;"></p>
        `);

        $('#SelectFlagModal').modal('hide');
    });
});

function formatarValor(elemento) {
    var valor = elemento.value.replace(/\D/g, '');
    valor = (parseFloat(valor) / 100).toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' });
    elemento.value = valor;
}

// ABP
(function ($) {
    var _cardService = abp.services.app.creditCard,
        l = abp.localization.getSource('Management'),
        _$form = $('#CardCreate').find('form');

    $(document).on('click', '#btnSubmit', function (e) {
        e.preventDefault();

        if (!_$form.valid()) {
            return;
        }

        var cardDto = _$form.serializeFormToObject();

        cardDto['FlagCardId'] = selectedFlagId;
        cardDto['AccountId'] = selectedAccountId;

        console.log(cardDto);

        var limitString = cardDto['Limit']
            .replace('R$', '')
            .replace(/\./g, '')
            .replace(',', '.');

        cardDto['Limit'] = parseFloat(limitString).toFixed(2);

        _cardService.create(cardDto).done(function () {
            abp.notify.info(l('SavedSuccessfully'));
            window.location.href = '/app/CreditCard';
        }).fail(function (error) {
             console.error('Erro ao criar transação:', error);
         });

    })

    $(document).on('click', '#bntChangeStatus', function (e) {
        e.preventDefault

        var cardId = $(this).data('cartao-id');
        var statusChange = $(this).data('status-cartao');

        console.log("a", statusChange);
        console.log("b", !statusChange);

        _cardService.statusChangeCard(cardId, !statusChange).done(function () {
            abp.notify.info(l('SavedSuccessfully'));
            window.location.reload();
        });
    });
})(jQuery);