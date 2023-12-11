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
    } else {
        camposAgenciaConta.style.display = 'none';
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


    // Inserir o nome oficial no campo de apelido
    document.getElementById('apelido').value = bankName;

    mostrarCamposAgenciaConta(bankName);
});



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

const itemId = '';

// Pluggy Connection
const initPluggyConnect = async () => {
    try {
        const accessToken = await getPluggyAccessToken();

        if (!accessToken) {
            return;
        }

        const pluggyConnect = new PluggyConnect({
            connectToken: accessToken,
            includeSandbox: true,
            onSuccess: (itemData) => {
                insertAccountPluggy(itemData.item.id);
            },
            onError: (error) => {
                throw error;
            },
        });

        pluggyConnect.init();

    } catch (error) {
        throw error;
    }
};

function insertAccountPluggy(id) {
    $.ajax({
        url: 'Accounts/InsertAccountPluggy',
        type: 'POST',
        data: JSON.stringify(id),
        contentType: 'application/json',
        success: function (response) {
            window.location.href = '/App/Accounts';
        },
        error: function (error) {
            console.error("Erro ao enviar POST request:", error);
        }
    });
}

$(document).on('click', '#bnt-adicionar-conta-pluggy', async function () {
    try {
        await initPluggyConnect();

    } catch (error) {
        console.error(error);
    }
});

const getPluggyAccessToken = async () => {
    try {
        const response = await fetch('/App/Accounts/PluggyGetAccessToken');
        const data = await response.json();

        if (data.result.error) {
            abp.notify.error(data.result.error);
        } 

        return data.result.accessToken;

    } catch (error) {
        console.error('Erro ao obter o Access Token:', error);
        throw error;
    }
};


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

        account["MainAccount"] = account["MainAccount"] === "on";

        account["BankId"] = selectedBankId;

        // Ajustando o campo de BalanceAvailable
        var BalanceAvailableString = account['BalanceAvailable']
            .replace('R$', '')
            .replace(/\./g, '')
            .replace(',', '.');

        account['BalanceAvailable'] = parseFloat(BalanceAvailableString).toFixed(2);

        _accountService.create(account).done(function () {
            abp.notify.info(l('SavedSuccessfully'));
            window.location.href = '/App/Accounts';
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

    $(document).on('click', '#bntExcluirConta', function (e) {
        e.preventDefault;

        var accountId = $(this).data('conta-id');
        var accountName = $(this).data('conta-nome');

        abp.message.confirm(
            abp.utils.formatString(
                l('DeleteAccount'),
                accountName),
            null,
            (isConfirmed) => {
                if (isConfirmed) {
                    _accountService.delete(accountId).done(function () {
                        abp.notify.info(l('SavedSuccessfully'));
                        window.location.reload();
                    });
                }
            }
        );

    });
})(jQuery);

