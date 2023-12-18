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
        const accessToken = await getPluggyAccessToken(false, null);

        if (!accessToken) {
            return;
        }

        const pluggyConnect = new PluggyConnect({
            connectToken: accessToken,
            includeSandbox: false,
            onSuccess: (itemData) => {
                $('.Connect Prosperium is-modal').modal('hide');
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

// Atualizar conta - Pluggy (Apenas INTER)
$(document).on('click', '#bntAtualizarConta', async function (e) {
    e.preventDefault();

    var accountId = $(this).data('conta-id');
    var itemId = $(this).data('item-id');
    const accessToken = await getPluggyAccessToken(true, itemId);

    if (!accessToken) {
        return;
    }

    try {
        const pluggyConnect = new PluggyConnect({
            connectToken: accessToken,
            updateItem: itemId,
            includeSandbox: false,
            onSuccess: (itemData) => {
                PluggyUpdateAllData(itemId, accountId);
            },
            onError: (error) => {
                throw error;
            },
        });

        pluggyConnect.init();

    } catch (error) {
        throw error;
    }
});

function PluggyUpdateAllData(itemId, accountId) {
    var notification = abp.notify.info("Atualizando item...", '', { autoClose: false });

    var requestData = {
        itemId: itemId,
        accountId: accountId
    };

    $.ajax({
        url: abp.appPath + 'App/Accounts/UpdateAllDataPluggy',
        type: 'POST',
        data: JSON.stringify(requestData),
        contentType: 'application/json',
        success: function (response) {
            abp.notify.success("Item atualizado com sucesso.");
            location.reload();
        },
        error: function (error) {
            console.error("Erro ao enviar GET request:", error);
        },
        complete: function () {
            notification.close();
            abp.ui.clearBusy();
        }
    });
}




function insertAccountPluggy(id) {
    $.ajax({
        url: abp.appPath + 'App/Accounts/InsertAccountPluggy',
        type: 'POST',
        data: JSON.stringify(id),
        contentType: 'application/json',
        success: function (response) {
            console.log(response);

            var hasCreditAccounts = response.result.pluggyAccounts.includes("CREDIT");
            var hasBankAccounts = response.result.pluggyAccounts.includes("BANK");

            console.log("hasCreditAccounts", hasCreditAccounts);
            console.log("hasBankAccounts", hasBankAccounts);

            if (hasCreditAccounts && hasBankAccounts) {
                location.reload();
            } else if (hasCreditAccounts) {
                window.location.href = '/App/CreditCards';
            }
            else {
                location.reload();
            }
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

const getPluggyAccessToken = async (isUpdate, itemId) => {
    try {
        const response = await fetch(`/App/Accounts/PluggyGetAccessToken?IsUpdate=${isUpdate}&ItemId=${itemId}`);
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

