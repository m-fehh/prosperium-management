var _$extractTable;
var filterInput;
var monthYearInput;

(function ($) {
    var _transactionService = abp.services.app.transaction,
        l = abp.localization.getSource('Management'),
        _$table = $('#ExtractTable');

    filterInput = $('#filterInput');
    monthYearInput = $('#monthYearInput');

    _$extractTable = _$table.DataTable({
        paging: true,
        serverSide: true,
        language: {
            url: '//cdn.datatables.net/plug-ins/1.10.24/i18n/Portuguese.json'
        },
        listAction: {
            ajaxFunction: function (input) {
                var currentDate = new Date();
                var currentMonthYear = (currentDate.getMonth() + 1).toString().padStart(2, '0') + '/' + currentDate.getFullYear();

                var formData = $('#ExtractSearchForm').serializeFormToObject(true);
                formData.filter = filterInput.val();
                formData.monthYear = monthYearInput.val();

                if (!formData.monthYear || formData.monthYear.trim() === "") {


                    formData.monthYear = currentMonthYear;
                }



                // Filtro avançado:
                formData.filteredAccounts = $("#selectedAccount").val();
                formData.filteredCards = $("#selectedCard").val();
                formData.filteredCategories = $("#selectedCategory").val();
                formData.filteredTags = $("#selectedTag").val();
                formData.filteredTypes = $("#selectedType").val();

                return _transactionService.getAll(formData);
            },
            inputFilter: function () {
                return $('#ExtractSearchForm').serializeFormToObject(true);
            }
        },
        buttons: [
            {
                name: 'refresh',
                text: '<i class="fas fa-redo-alt"></i>',
                class: 'bntRefreshTable',
                action: function () {
                    _$extractTable.draw(false);
                }
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
                data: 'transaction.date',
                render: function (data) {
                    return moment(data).format('DD/MM/YYYY');
                },
                sortable: false
            },
            {
                targets: 1,
                data: 'transaction.description',
                render: function (data) {
                    return data.toUpperCase();
                },
                sortable: false
            },
            {
                targets: 2,
                data: 'transaction',
                render: function (data, type, row) {
                    if (data.account != null)
                    {
                        var accountNickname = data.account.accountNickname;
                        var agencyNumber = data.account.agencyNumber;
                        var accountNumber = data.account.accountNumber;
                        var imageFullPath = abp.appPath + 'img/banks/' + data.account.bank.imagePath;

                        if (data.account && data.account.accountNickname) {
                            return `
                            <div class="d-flex" style="display: flex; justify-content: center; align-items: center;">
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
                        var cardNumber = data.creditCard.cardNumber;
                        var parcelTransaction = data.currentInstallment;
                        var imageFullPath = abp.appPath + 'img/flags/' + data.creditCard.flagCard.iconPath;

                        return `
                            <div class="d-flex" style="display: flex; justify-content: center; align-items: center;">
                                <div>
                                    <img src="${imageFullPath}" style="border-radius: 5px;" width="30" />
                                </div>
                                <div style="margin-left: 10px;">
                                    <span style="font-size: 12px; color: #000; font-weight: 400;">${cardNumber} | Parcela: ${parcelTransaction}</span>
                                </div>
                            </div>
                        `;
                    }
                },
                sortable: false
            },
            {
                targets: 3,
                data: 'transaction.categories',
                render: function (data, type, row) {
                    var imageFullPath = abp.appPath + 'img/categories/' + data.iconPath;

                    if (data && data.name) {
                        return `
                            <div class="d-flex" style="display: flex; justify-content: center; align-items: center;">
                                <div>
                                    <img src="${imageFullPath}" style="padding: 10px;" width="50" />
                                </div>
                                <div>
                                    <h5 class="card-title">${data.name.toUpperCase()}</h5>
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
                targets: 4,
                data: 'transaction.expenseValue',
                render: function (data, type, row) {
                    var formattedValue = Math.abs(data).toLocaleString('pt-BR', { style: 'currency', currency: 'BRL', minimumFractionDigits: 2, maximumFractionDigits: 2 });

                    if (row.transaction.transactionType === 1 /* Gastos */ || row.transaction.transactionType === 3 /* Transferência */) {
                        formattedValue = '-' + formattedValue;
                    }

                    return formattedValue;
                },
                sortable: false
            }
        ]
    });

    _$extractTable.on('draw.dt', function () {
        _$extractTable.rows().every(function () {
            var data = this.data();
            var rowNode = this.node();

            $(rowNode).removeClass('gastos ganhos');

            // Adicione a classe correspondente ao TransactionType
            if (data.transaction.transactionType === 1 /* Gastos */) {
                $(rowNode).addClass('gastos');
            } else if (data.transaction.transactionType === 2 /* Ganhos */) {
                $(rowNode).addClass('ganhos');
            }
        });
    });

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

    abp.event.on('filter.applied', () => {
        $("#btnLimparFiltro").css("visibility", "visible");

        $('#FilterModal').modal('hide');
        _$extractTable.ajax.reload();

        UpdateCardValues();
    });

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


    function UpdateCardValues() {
        // Obtém a data atual
        var currentDate = new Date();
        var currentMonthYear = (currentDate.getMonth() + 1).toString().padStart(2, '0') + '/' + currentDate.getFullYear();


        accountId = $("#selectedAccount").val();
        cardId = $("#selectedCard").val();
        categoryId = $("#selectedCategory").val();
        tagId = $("#selectedTag").val();
        typeId = $("#selectedType").val();

        var filterValue = filterInput.val();
        var monthYearValue = monthYearInput.val();

        if (!monthYearValue || monthYearValue.trim() === "") {
            monthYearValue = currentMonthYear;
        }

        $.ajax({
            url: '/App/Extract/GetValuesTotals',
            type: 'GET',
            data: { filter: filterValue, monthYear: monthYearValue, filteredAccounts: accountId, filteredCards: cardId, filteredTags: tagId, filteredCategories: categoryId, filteredTypes: typeId },
            dataType: 'json',
            success: function (data) {
                var gastosString = (data.result.gastos !== undefined) ? "R$ " + Math.abs(data.result.gastos).toFixed(2) : "R$ 0,00";
                var ganhosString = (data.result.ganhos !== undefined) ? "R$ " + data.result.ganhos.toFixed(2) : "R$ 0,00";

                $('#gastosValue').text(gastosString);
                $('#ganhosValue').text(ganhosString);
            },
            error: function (error) {
                console.error('Erro na requisição:', error);
            }
        });
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

    $(document).ready(function () {
        UpdateCardValues();

        var monthYearInput = $("#monthYearInput");
        var calendarIcon = $("#calendarIcon");
        var resumoTitle = $("#resumoTitle");

        // Obtém a data atual
        var currentDate = new Date();
        var currentMonthYear = (currentDate.getMonth() + 1).toString().padStart(2, '0') + '/' + currentDate.getFullYear();

        // Define o valor padrão do input de mês/ano
        monthYearInput.val(currentMonthYear);
        UpdateResumoTitle(currentDate);

        calendarIcon.datepicker({
            format: "mm/yyyy",
            startView: "months",
            minViewMode: "months",
            autoclose: true,
            language: "pt-BR",
            orientation: "bottom",
            defaultViewDate: { year: currentDate.getFullYear(), month: currentDate.getMonth() }
        }).on('changeDate', function (e) {
            monthYearInput.val(e.format(0, 'mm/yyyy'));
            UpdateResumoTitle(e.date);
            HandleSearchInput();
        });

        monthYearInput.on('change', function () {
            UpdateResumoTitle(new Date(monthYearInput.val()));
            HandleSearchInput();
            UpdateCardValues();
        });

        // Evento de input para o campo de filtro
        filterInput.on('input', function () {
            HandleSearchInput();
        });

        calendarIcon.css({
            "font-size": "28px",
            "color": "#FF8C00",
            "margin-right": "10px",
            "cursor": "pointer"
        });

        // Função para atualizar o texto do título
        function UpdateResumoTitle(date) {
            var monthNames = ["Janeiro", "Fevereiro", "Março", "Abril", "Maio", "Junho", "Julho", "Agosto", "Setembro", "Outubro", "Novembro", "Dezembro"];
            var formattedDate = monthNames[date.getMonth()] + " de " + date.getFullYear();
            resumoTitle.text(formattedDate);
        }
    });

})(jQuery);




