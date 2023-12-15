(function ($) {
    var myPieChart;
    var _transactionService = abp.services.app.transaction;

    UpdateResumoTitle(new Date());
    UpdateCardValues();

    $(document).ready(function () {
        $("#calendarIcon").datepicker({
            format: "mm/yyyy",
            startView: "months",
            minViewMode: "months",
            autoclose: true,
            language: "pt-BR",
            orientation: "bottom",
            defaultViewDate: { year: new Date().getFullYear(), month: new Date().getMonth() }
        }).on('changeDate', function (e) {
            UpdateResumoTitle(e.date);
            $("#monthYearInput").val(e.format(0, 'mm/yyyy'));
            UpdateCardValues();
        });

    });

    //Atualiza o texto do topo
    function UpdateResumoTitle(date) {
        var monthNames = ["Jan", "Fev", "Mar", "Abr", "Maio", "Jun", "Jul", "Ago", "Set", "Out", "Nov", "Dez"];
        var formattedDate = monthNames[date.getMonth()] + "/" + date.getFullYear();
        $(".titleMonth").text(formattedDate);
    }


    // Function para selecionar "GASTOS" ou "GANHOS"
    window.marcarOpcao = function (elemento, valor) {
        var botoes = document.querySelectorAll('.opcao-btn');
        botoes.forEach(function (botao) {
            botao.classList.remove('opcao-selecionada');
        });

        elemento.classList.add('opcao-selecionada');

        var inputRadio = elemento.querySelector('input[type="radio"]');
        inputRadio.checked = true;

        UpdateCardValues();
    }

    // Formata o valor R$ 
    function formatCurrency(value) {
        const formattedValue = value.toLocaleString('pt-BR', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
        return value >= 0 ? `R$ ${formattedValue}` : `- R$ ${formattedValue.substring(1)}`;
    }

    // Formata a data
    function formatDate(dateString) {
        const options = { year: 'numeric', month: '2-digit', day: '2-digit' };
        return new Date(dateString).toLocaleDateString('pt-BR', options);
    }


    // Atualiza todos os cards
    function UpdateCardValues() {
        $("#latestTransactionsContainer").innerHTML = '';
        $('#progress-bar-container').empty();
        var transactionType = $('input[name="TransactionType"]:checked').val();

        var transactionPromise = CaptureTransactionsPerMonth();

        transactionPromise.then(function (data) {
            // Remove os pagamentos de fatura
            data = data.filter(transaction => transaction.categoryId !== 33);

            if (data.length > 0) {
                $('#chartMessage').hide();
                $('#latestTransactionsContainer').show();
                $('#progress-bar-container').show();
                $('.opcao-btn').show();
                $('.messageNotVisible').hide();


                UpdateSummaryValues(data);
                renderPieChart(data, transactionType);
                UpdateCardLatestTransaction(data);
                UpdateCardCreditCard(data);

            } else {
                $('#chartMessage').show();
                $('#latestTransactionsContainer').hide();
                $('#progress-bar-container').hide();
                $('#myPieChart').hide();
                $('.opcao-btn').hide();
                $('.messageNotVisible').show();

                $('#gastosValue').text(formatCurrency(0));
                $('#ganhosValue').text(formatCurrency(0));
                $('#totalValue').text(formatCurrency(0));

                // Ocultar ou destruir o gráfico quando não houver dados
                var canvas = document.getElementById('myPieChart');
                var ctx = canvas.getContext('2d');
                ctx.clearRect(0, 0, canvas.width, canvas.height);
            }
        });
    }

    // Atualiza card de cartão de crédito
    function UpdateCardCreditCard(transactions) {
        let monthYear = $("#monthYearInput").val();

        if (!monthYear) {
            var currentDate = new Date();

            currentDate.setMonth(currentDate.getMonth() + 1);
            var currentMonthYear = (currentDate.getMonth() + 1).toString().padStart(2, '0') + '/' + currentDate.getFullYear();
            monthYear = currentMonthYear;
        } else {
            var parts = monthYear.split('/');
            if (parts.length === 2) {
                monthYear = new Date(parts[1], parts[0] - 1);
                monthYear.setMonth(monthYear.getMonth() + 1);
                monthYear = (monthYear.getMonth() + 1).toString().padStart(2, '0') + '/' + monthYear.getFullYear();
            }
        }

        var groupedTransactions = filterAndGroupCreditCardTransactions(transactions);
        if (groupedTransactions.length > 0) {
            groupedTransactions.forEach(function (item) {
                if (item.dueDay < 10) {
                    item.dueDay = "0" + item.dueDay;
                }

                var date = item.dueDay + "/" + monthYear;

                var imageFullPath = abp.appPath + 'img/flags/' + item.iconPath;
                var progressBar =
                    `
                            <div class="progress-card">
                                <div class="card" style="height: 120px;">
                                    <div class="card-content">
                                            <div class="card-body d-flex align-items-center" style="width: 100%; flex-direction: column;">
                                            <div class="row" style="width: 100%; display: flex; align-items: center;">
                                                <div class="col-1" style="display: flex; justify-content: center;">
                                                    <img src="${imageFullPath}" width="45" />
                                                </div>
                                                <div class="col-7" style="display: flex; align-items: center; gap: 15px;">
                                                    <p style="font-size: 16px; color: #000; font-weight: bold; margin-left: 10px;">${item.cardName}
                                                        <label style="font-size: 13px; color: #999; margin: 0;">${item.cardNumber}</label>
                                                    </p>
                                                    <p style="font-size: 13px; color: #999; margin: 0;">Usado: ${formatCurrency(item.totalExpenses)}  - Limite: R$ ${formatCurrency(item.limit)}</p>
                                                </div>
                                                 <div class="col-3">
                                                        <p style="font-size: 16px; color: #000; font-weight: bold; margin-left: 10px;">Vencimento: ${date}</p>
                                                </div>
                                            </div>
                                            <div class="progress" role="progressbar" aria-label="${item.cardName}" aria-valuenow="${item.progress}" aria-valuemin="0" aria-valuemax="100">
                                                <div class="progress-bar" style="width: ${item.progress}%">${item.progress}%</div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                                `

                $('#progress-bar-container').append(progressBar);
            });
        } else {
            $('#messageNotVisibleCreditCard').show();
        }
    }

    function filterAndGroupCreditCardTransactions(transactions) {
        var creditCardTransactions = transactions.filter(transaction => transaction.creditCard && transaction.categoryId !== 33);

        var groupedCreditCardTransactions = creditCardTransactions.reduce((acc, transaction) => {
            var creditCardId = transaction.creditCardId;

            if (!acc[creditCardId]) {
                acc[creditCardId] = {
                    creditCardId: creditCardId,
                    totalExpenses: 0,
                    iconPath: transaction.creditCard.flagCard.iconPath,
                    limit: transaction.creditCard.limit,
                    dueDay: transaction.creditCard.dueDay,
                    cardNumber: transaction.creditCard.cardNumber,
                    cardName: transaction.creditCard.cardName,
                    progress: 0,
                };
            }

            acc[creditCardId].totalExpenses += Math.abs(transaction.expenseValue);

            // Adicionando o cálculo do progresso
            var progress = Math.floor((Math.abs(acc[creditCardId].totalExpenses) / Math.abs(transaction.creditCard.limit)) * 100);
            acc[creditCardId].progress = progress;

            return acc;
        }, {});

        var result = Object.values(groupedCreditCardTransactions);

        return result;
    }

    // Atualiza cards de últimas transações
    function UpdateCardLatestTransaction(transactions) {
        $("#latestTransactionsContainer").empty();

        var last4Transactions = transactions.sort(function (a, b) {
            return new Date(b.date) - new Date(a.date);
        }).slice(0, 4);

        last4Transactions.forEach(function (item) {
            $("#latestTransactionsContainer").append(createTransactionCard(item));
        });
    }

    // Atualiza os cards de valores
    function UpdateSummaryValues(transactions) {
        var totalGastos = 0;
        var totalGanhos = 0;

        transactions.forEach(function (item) {
            if (item.transactionType === 1) {
                // Despesa
                totalGastos += Math.abs(item.expenseValue);

            } else if (item.transactionType === 2) {
                // Ganho
                totalGanhos += Math.abs(item.expenseValue);
            }
        });

        const gastos = totalGastos;
        const ganhos = totalGanhos;
        const total = ganhos - gastos;

        // Verifica se gastos é negativo e ajusta o formato
        const gastosFormatted = gastos >= 0 ? `R$ ${formatCurrency(gastos)}` : `- R$ ${formatCurrency(Math.abs(gastos))}`;

        $('#gastosValue').text(gastosFormatted);
        $('#ganhosValue').text(formatCurrency(ganhos));
        $('#totalValue').text(formatCurrency(total));
    }

    function CaptureTransactionsPerMonth() {
        let monthYearValue = $("#monthYearInput").val();
        if (!monthYearValue) {
            var currentDate = new Date();
            var currentMonthYear = (currentDate.getMonth() + 1).toString().padStart(2, '0') + '/' + currentDate.getFullYear();

            monthYearValue = currentMonthYear;
        }

        // Verificar se há filtros selecionados
        var filteredAccounts = $("#selectedAccount").val();
        var filteredCards = $("#selectedCard").val();
        var filteredCategories = $("#selectedCategory").val();

        // Retornar a Promise diretamente
        return _transactionService.getAllListPerMonth(monthYearValue, filteredAccounts, filteredCards, filteredCategories);
    }

    // Gráfico de categória
    function renderPieChart(data, transactionType) {
        var canvas = document.getElementById('myPieChart');

        canvas.width = 500; // por exemplo, 800 pixels
        canvas.height = 400;

        if (!canvas) {
            console.error('Elemento canvas não encontrado.');
            return;
        }

        if (myPieChart) {
            myPieChart.destroy();
        }

        // Filtro para pegar apenas as transações do tipo selecionado
        var filteredData = data.filter(function (item) {
            return item.transactionType == transactionType;
        });

        // Agrupar despesas por categoria
        var groupedData = filteredData.reduce(function (acc, item) {
            var category = item.categories;
            var categoryName = category ? category.name : 'Sem Categoria';

            if (!acc[categoryName]) {
                acc[categoryName] = {
                    categoryName: categoryName,
                    totalExpense: 0
                };
            }

            acc[categoryName].totalExpense += Math.abs(item.expenseValue);

            return acc;
        }, {});

        if (Object.keys(groupedData).length > 0) {
            var labels = Object.keys(groupedData);
            var values = Object.values(groupedData).map(item => item.totalExpense);

            var ctx = canvas.getContext('2d');

            // Cores relacionadas ao laranja
            var orangeColors = ['#FFA07A', '#FF7F50', '#FF6347', '#FF4500', '#FF8C00'];

            myPieChart = new Chart(ctx, {
                type: 'doughnut',
                data: {
                    labels: labels,
                    datasets: [{
                        data: values,
                        backgroundColor: orangeColors,
                    }]
                },
                options: {
                    responsive: false,
                    maintainAspectRatio: false,
                    cutout: '10%', // Ajuste o valor conforme necessário para aumentar ou diminuir o tamanho do gráfico
                    plugins: {
                        legend: {
                            display: true,
                            position: 'right', // Ajuste a posição da legenda (pode ser 'top', 'bottom', 'left', 'right')
                        },
                        tooltip: {
                            callbacks: {
                                label: function (context) {
                                    var label = context.label || '';
                                    var value = Math.abs(context.parsed).toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' });
                                    return label + ': ' + value;
                                },
                            },
                        },
                    },
                },
            });
        } else {
            $('#messageNotVisibleCategories').show();
        }
    }

    // Função para criar o HTML do card de ultimas transações
    function createTransactionCard(transaction) {
        var imageFullPath = abp.appPath + 'img/categories/' + transaction.categories.iconPath;
        var additionalInfo = "";

        if (transaction.creditCard) {
            additionalInfo = `<p style="font-size: 13px; color: #999; margin: 0;">Cart&atilde;o: ${transaction.creditCard.cardNumber}</p>`;
        } else {
            additionalInfo = `<p style="font-size: 13px; color: #999; margin: 0;">Ag&ecirc;ncia: ${transaction.account.agencyNumber} | Conta: ${transaction.account.accountNumber}</p>`;
        }

        var categoryName = transaction.categories.name.length > 20
            ? transaction.categories.name.substring(0, 20) + '...'
            : transaction.categories.name;

        return `
        <div class="card" style="height: 90px;">
            <div class="card-content">
                <div class="card-body d-flex align-items-center" style="width: 100%;">
                    <div class="row" style="width: 100%; display: flex; align-items: center;">
                        <div class="col-2" style="display: flex; justify-content: center;">
                            <img src="${imageFullPath}" width="40" />
                        </div>
                        <div class="col-7" style="display: flex; align-items: center; gap: 5px; flex-direction: column;">
                        <div class="row">
                            <p style="font-size: 16px; color: #000; font-weight: bold; margin-left: 10px;" title="${transaction.categories.name}">${categoryName} ${formatCurrency(transaction.expenseValue)}</p>
                            </div>
                            <div class="row">
                                ${additionalInfo}
                            </div>
                        </div>
                        <div class="col-3">
                            <p  style="font-size: 16px; color: #000; font-weight: bold; margin-left: 10px;">${formatDate(transaction.date)}</p>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    `;
    }

    // Abre a modal do filtro avançado
    $(document).on('click', '#FilterIcon', function (e) {
        e.preventDefault();

        abp.ajax({
            url: 'GetAllFilters',
            type: 'GET',
            dataType: 'html',
            success: function (content) {
                $('#FilterModal div.modal-content').html(content);
            },
            error: function (e) { }
        });
    });

    // Comitta a pesquisa
    abp.event.on('filter.applied', () => {
        $("#btnLimparFiltro").css("visibility", "visible");

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

        $("#btnLimparFiltro").css("visibility", "hidden");
        UpdateCardValues();
    });

})(jQuery);