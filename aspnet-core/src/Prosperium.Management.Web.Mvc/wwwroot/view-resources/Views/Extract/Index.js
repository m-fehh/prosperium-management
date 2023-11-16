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
                var formData = $('#ExtractSearchForm').serializeFormToObject(true);
                formData.filter = filterInput.val();
                formData.monthYear = monthYearInput.val();

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
                data: 'transaction.categories',
                render: function (data, type, row) {
                    if (data && data.name) {
                        return `
                <div class="d-flex" style="display: flex; justify-content: center; align-items: center;">
                    <div>
                        <i class="${data.iconPath}" style="margin-right: 10px; color: #FF8C00;"></i>
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
                targets: 3,
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

    // ... (seu código existente)

})(jQuery);

function UpdateCardValues() {
    var filterValue = filterInput.val();
    var monthYearValue = monthYearInput.val();

    $.ajax({
        url: '/App/Extract/GetValuesTotals',
        type: 'GET',
        data: { filter: filterValue, monthYear: monthYearValue },
        dataType: 'json',
        success: function (data) {
            var gastosString = (data.result.gastos !== undefined) ? "R$ " + data.result.gastos.toFixed(2) : "R$ 0,00";
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
        resumoTitle.text("Resumo de " + formattedDate);
    }
});