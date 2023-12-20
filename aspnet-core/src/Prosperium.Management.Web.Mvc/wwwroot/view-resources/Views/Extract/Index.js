(function ($) {
    var _transactionService = abp.services.app.transaction,
        l = abp.localization.getSource('Management'),
        _$table = $('#ExtractTable');

    var hasInterAccount = false;

    var resultData;
    var _$extractTable = _$table.DataTable({
        paging: true,
        serverSide: true,
        lengthChange: true,  // Adicionando esta opção para permitir a alteração do número de itens por página
        listAction: {
            ajaxFunction: function (input) {
                var currentDate = new Date();
                var currentMonthYear = (currentDate.getMonth() + 1).toString().padStart(2, '0') + '/' + currentDate.getFullYear();

                var currentPage = _$table.DataTable().page.info().page;
                var itemsPerPage = _$table.DataTable().page.info().length;

                var formData = $('#ExtractSearchForm').serializeFormToObject(true);

                formData.filter = $('#filterInput').val();
                formData.monthYear = $('#monthYearInput').val();

                if (!formData.monthYear || formData.monthYear.trim() === "") {
                    formData.monthYear = currentMonthYear;
                }

                // Filtro avançado:
                formData.filteredAccounts = $("#selectedAccount").val();
                formData.filteredCards = $("#selectedCard").val();
                formData.filteredCategories = $("#selectedCategory").val();

                formData.skipCount = currentPage * itemsPerPage;
                formData.maxResultCount = itemsPerPage;

                var transactionPromise = _transactionService.getAll(formData);

                transactionPromise.then(function (data) {
                    resultData = data.items;
                    UpdateCardValues();


                    return data.items;
                });

                return transactionPromise;
            },
            inputFilter: function () {
                return $('#ExtractSearchForm').serializeFormToObject(true);
            }
        },
        buttons: [
            {
                name: 'refresh',
                class: 'bntRefreshTable',
                text: '<i class="fas fa-redo-alt"></i>',
                action: () => _$extractTable.draw(false)
            }
        ],
        responsive: {
            details: {
                type: 'column'
            }
        },
        columnDefs: [
            {
                targets: 0,
                className: 'control',
                defaultContent: '',
            },
            {
                targets: 1,
                data: 'transaction.date',
                render: function (data) {
                    return moment(data).format('DD/MM/YYYY');
                },
                sortable: false
            },
            {
                targets: 2,
                data: 'transaction.description',
                render: function (data) {
                    var maxLength = 30;
                    var truncatedData = data.length > maxLength ? data.substring(0, maxLength) + '...' : data;
                    return `<span title="${data}">${truncatedData.toUpperCase()}</span>`;
                },
                sortable: false
            },
            {
                targets: 3,
                data: 'transaction',
                render: function (data, type, row) {
                    if (data.account != null) {
                        var accountNickname = data.account.accountNickname;
                        hasInterAccount = (accountNickname === "BANCO INTER S.A.") ? true : false;


                        var agencyNumber = data.account.agencyNumber;
                        var accountNumber = data.account.accountNumber;
                        var imageFullPath = abp.appPath + 'img/banks/' + data.account.bank.imagePath;

                        if (data.account && data.account.accountNickname) {
                            return `
                            <div class="d-flex" style="display: flex; justify-content: start; margin-left: 20%; align-items: center;">
                                <div>
                                    <img src="${imageFullPath}" style="border-radius: 5px;" width="30" />
                                </div>
                                <div style="margin-left: 10px;">
                                    <span style="font-size: 12px; color: #000; font-weight: 400;">Ag&ecirc;ncia: ${agencyNumber} | Conta: ${accountNumber}</span>
                                </div>
                            </div>
                        `;
                        } else {
                            return '';
                        }
                    } else {
                        var cardName = data.creditCard.cardName;
                        var parcelTransaction = data.currentInstallment;
                        var imageFullPath = abp.appPath + 'img/flags/' + data.creditCard.flagCard.iconPath;

                        return `
                            <div class="d-flex" style="display: flex; justify-content: start; margin-left: 20%; align-items: center;">
                                <div>
                                    <img src="${imageFullPath}" style="border-radius: 5px;" width="30" />
                                </div>
                                <div style="margin-left: 20px;">
                                    <div class="row">
                                        <span style="font-size: 12px; color: #000; font-weight: 400;">${data.creditCard.cardNumber} | Parcela: ${parcelTransaction}</span>
                                    </div>
                                    <div class="row">
                                        <p class="saldo-label" style="font-size: 12px!important; color: #999;">${cardName}</p>
                                    </div>
                                </div>
                            </div>
                        `;
                    }
                },
                sortable: false
            },
            {
                targets: 4,
                data: 'transaction.categories',
                render: function (data, type, row) {
                    var imageFullPath = abp.appPath + 'img/categories/' + data.iconPath;

                    var maxLength = 15;
                    var truncatedData = data.name.length > maxLength ? data.name.substring(0, maxLength) + '...' : data.name;

                    if (data && data.name) {
                        return `
                            <div class="d-flex" style="display: flex; justify-content: start; margin-left: 20%; align-items: center;">
                                <div>
                                    <img src="${imageFullPath}" style="padding: 10px;" width="50" />
                                </div>
                                <div>
                                    <h5 class="card-title" title="${data.name}">${truncatedData.toUpperCase()}</h5>
                                </div>
                            </div>
                        `;
                    } else {
                        return '';
                    }
                },
                sortable: false

            },
            {
                targets: 5,
                data: 'transaction.expenseValue',
                render: function (data) {
                    return formatCurrency(data);
                },
                sortable: false
            }
        ]
    });

    function checkInterAccount() {
        abp.ajax({
            url: abp.appPath + 'App/Extract/ContainsInterAccount',
            type: 'GET',
            success: function (result) {
                if (result) {
                    $(".message-banner").show();
                }
            }
        });
    }

    function formatCurrency(value) {
        const formattedValue = value.toLocaleString('pt-BR', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
        return value >= 0 ? `R$ ${formattedValue}` : `- R$ ${formattedValue.substring(1)}`;
    }

    // Função para atualizar o estilo de cada linha 
    _$extractTable.on('draw.dt', function () {
        _$extractTable.rows().every(function () {
            var data = this.data();
            var rowNode = this.node();

            $(rowNode).removeClass('gastos ganhos transferencia');

            // Adicione a classe correspondente ao TransactionType
            if (data.transaction.transactionType === 1) {
                $(rowNode).addClass('gastos');
            } else if (data.transaction.transactionType === 2) {
                $(rowNode).addClass('ganhos');
            } else if (data.transaction.transactionType === 3) {
                $(rowNode).addClass('transferencia');
            }
        });
    });

    // Função para atualizar o texto do título
    function UpdateResumoTitle(date) {
        var monthNames = ["Jan", "Fev", "Mar", "Abr", "Maio", "Jun", "Jul", "Ago", "Set", "Out", "Nov", "Dez"];
        var formattedDate = monthNames[date.getMonth()] + " de " + date.getFullYear();
        $("#resumoTitle").text(formattedDate);
    }

    // Formata data
    function formatDate(dateString) {
        const options = { year: 'numeric', month: '2-digit', day: '2-digit' };
        return new Date(dateString).toLocaleDateString('pt-BR', options);
    }

    $(document).ready(function () {
        $(".message-banner").hide();
        UpdateResumoTitle(new Date());
        checkInterAccount();

        $("#calendarIcon").datepicker({
            format: "mm/yyyy",
            startView: "months",
            minViewMode: "months",
            autoclose: true,
            language: "pt-BR",
            orientation: "bottom",
            defaultViewDate: { year: new Date().getFullYear(), month: new Date().getMonth() }
        }).on('changeDate', function (e) {
            $("#monthYearInput").val(e.format(0, 'mm/yyyy'));
            UpdateResumoTitle(e.date);
            HandleSearchInput();
        });

        $("#monthYearInput").on('change', function () {
            UpdateResumoTitle(new Date(monthYearInput.val()));
            HandleSearchInput();
        });
    });

    function UpdateCardValues() {
        var totalGastos = 0;
        var totalGanhos = 0;

        if (resultData) {
            resultData.forEach(function (item) {
                if (item.transaction.transactionType === 1) {
                    // Despesa
                    if (item.transaction.categoryId !== 33) {
                        totalGastos += Math.abs(item.transaction.expenseValue);
                    }
                } else if (item.transaction.transactionType === 2) {
                    // Ganho
                    totalGanhos += Math.abs(item.transaction.expenseValue);
                }
            });
        }

        // Atualize os elementos HTML com os novos valores calculados
        $('#gastosValue').text(formatCurrency(totalGastos));
        $('#ganhosValue').text(formatCurrency(totalGanhos));
    }

    // Função para exportar CSV
    function ExportCsv() {
        if (!resultData || resultData.length === 0) {
            abp.notify.warn('Nenhum dado para exportar');
            return;
        }

        // Obtém a data atual
        var currentDate = new Date();
        var day = currentDate.getDate().toString().padStart(2, '0');
        var month = (currentDate.getMonth() + 1).toString().padStart(2, '0');
        var year = currentDate.getFullYear();

        // Formata a data como "dd_mm_yyyy"
        var formattedDate = day + '_' + month + '_' + year;

        // Cria o conteúdo CSV
        var csvContent = "Data;Descricao;Tipo;Conta/Cartao;Categoria;Valor\n";
        resultData.forEach(function (item) {
            var isCard = (item.transaction.account) ? false : true;

            csvContent += (item.transaction.date ? formatDate(item.transaction.date) : "") + ";";
            csvContent += (item.transaction.description ? item.transaction.description : "") + ";";

            csvContent += (isCard) ? "Cartao" + ";" : "Conta" + ";";

            var accountInfo = "";
            if (!isCard) {
                accountInfo = item.transaction.account.agencyNumber + "/" + item.transaction.account.accountNumber;
            } else {
                accountInfo += item.transaction.creditCard.cardName + " (" + item.transaction.creditCard.cardNumber + ")";
            }
            csvContent += accountInfo + ";";

            csvContent += (item.transaction.categories ? removeSpecialChars(item.transaction.categories.name.toUpperCase()) : "") + ";";
            csvContent += (item.transaction.expenseValue ? formatCurrency(item.transaction.expenseValue) : "") + "\n";
        });

        // Converte o conteúdo para Blob e inicia o download
        var blob = new Blob([csvContent], { type: 'text/csv;charset=utf-8;' });
        saveAs(blob, 'Prosperium_' + formattedDate + '.csv');

        abp.notify.success(l("ExportSuccessfully"));
    }

    // Função para remover caracteres especiais
    function removeSpecialChars(str) {
        return str.normalize('NFD').replace(/[\u0300-\u036f]/g, '');
    }


    // Função para lidar com a entrada de pesquisa
    function HandleSearchInput() {
        Debounce(function () {
            _$extractTable.draw();
            UpdateCardValues();
        }, 500)();
    }

    // Função de Debounce para controlar a frequência de chamadas
    function Debounce(func, wait) {
        let timeout;
        return function () {
            const context = this;
            const args = arguments;
            const later = function () {
                timeout = null;
                func.apply(context, args);
            };
            clearTimeout(timeout);
            timeout = setTimeout(later, wait);
        };
    }

    // Abre a modal do filtro avançado
    $(document).on('click', '#FilterIcon', function (e) {
        e.preventDefault();

        abp.ajax({
            url: abp.appPath + 'App/Extract/GetAllFilters',
            type: 'GET',
            dataType: 'html',
            success: function (content) {
                $('#FilterModal div.modal-content').html(content);
            },
            error: function (e) { }
        });
    });


    $(document).on('click', '#exportCsvBtn', function (e) {
        e.preventDefault();

        if (resultData && resultData.length > 0) {
            ExportCsv();
        } else {
            abp.notify.info('Nenhuma transação encontrada.');
        }
    });

    // newTransacionBnt
    $(document).on('click', '#newTransacionBnt', function (e) {
        e.preventDefault();
        window.location.href = 'Transactions';
    });

    // Comitta a pesquisa
    abp.event.on('filter.applied', () => {
        $("#btnLimparFiltro").css("visibility", "visible");

        _$extractTable.ajax.reload();
        $('#FilterModal').modal('hide');
        UpdateCardValues();

    });

    // Limpa os filtro
    $(document).on('click', '#btnLimparFiltro', function () {
        $('#selectedAccount').val('');
        $('#selectedCard').val('');
        $('#selectedCategory').val('');
        $('#selectedTag').val('');
        $('#selectedType').val('');

        _$extractTable.ajax.reload();

        $("#btnLimparFiltro").css("visibility", "hidden");
        UpdateCardValues();
    });

    // Evento de input para o campo de filtro
    $('#filterInput').on('input', function () {
        HandleSearchInput();
    });


})(jQuery);