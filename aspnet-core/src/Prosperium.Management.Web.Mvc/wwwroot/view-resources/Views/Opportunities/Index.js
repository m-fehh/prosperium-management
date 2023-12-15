(function ($) {
    var _opportunitiesService = abp.services.app.opportunities,
        l = abp.localization.getSource('Management'),
        _$table = $('#OpportunitiesTable');

    var _$oportunitiesTable = _$table.DataTable({
        paging: true,
        serverSide: true,
        lengthChange: true,
        listAction: {
            ajaxFunction: _opportunitiesService.getAll,
            inputFilter: function () {
                return $('#opportunitySearchForm').serializeFormToObject(true);
            }
        },
        buttons: [
            {
                name: 'refresh',
                class: 'bntRefreshTable',
                text: '<i class="fas fa-redo-alt"></i>',
                action: () => _$oportunitiesTable.draw(false)
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
                data: 'pportunity.date',
                render: function (data) {
                    return moment(data).format('DD/MM/YYYY');
                },
                sortable: false
            },
            {
                targets: 2,
                render: function (data, type, row) {
                    var content = '';
                    if (row.opportunity.account != null) {
                        var agencyNumber = row.opportunity.account.agencyNumber;
                        var accountNumber = row.opportunity.account.accountNumber;
                        var imageFullPath = abp.appPath + 'img/banks/' + row.opportunity.account.bank.imagePath;

                        if (row.opportunity.account && row.opportunity.account.accountNickname) {
                            content = 
                            `
                                <div class="d-flex" style="display: flex; justify-content: center; align-items: center;">
                                    <div>
                                        <img src="${imageFullPath}" style="border-radius: 5px;" width="30" />
                                    </div>
                                    <div style="margin-left: 10px;">
                                        <span style="font-size: 12px; color: #000; font-weight: 400;">Ag&ecirc;ncia: ${agencyNumber} | Conta: ${accountNumber}</span>
                                    </div>
                                </div>
                            `;
                        }
                    } else {
                        var cardName = row.opportunity.creditCard.cardName;
                        var parcelTransaction = row.opportunity.currentInstallment;
                        var imageFullPath = abp.appPath + 'img/flags/' + row.opportunity.creditCard.flagCard.iconPath;

                        content = 
                        `
                            <div class="d-flex" style="display: flex; justify-content: center; align-items: center;">
                                <div>
                                    <img src="${imageFullPath}" style="border-radius: 5px;" width="30" />
                                </div>
                                <div style="margin-left: 20px;">
                                    <div class="row">
                                        <span style="font-size: 12px; color: #000; font-weight: 400;">${row.opportunity.creditCard.cardNumber} | Parcela: ${parcelTransaction}</span>
                                    </div>
                                    <div class="row">
                                        <p class="saldo-label" style="font-size: 12px!important; color: #999;">${cardName}</p>
                                    </div>
                                </div>
                            </div>
                        `;
                    }
                    return content;
                },
                sortable: false
            },

            {
                targets: 3,
                data: 'opportunity.name',
                render: function (data) {
                    var maxLength = 24;
                    var truncatedData = data.length > maxLength ? data.substring(0, maxLength) + '...' : data;
                    return `<span title="${data}">${truncatedData.toUpperCase()}</span>`;
                },
                sortable: false
            },
            {
                targets: 4,
                data: 'opportunity.type',
                render: function (data) {
                    return getOpportunityTypeName(data).toUpperCase() || '';
                },
                sortable: false,
            },
            {
                targets: 5,
                data: 'opportunity.availableLimit',
                render: function (data) {
                    return data !== null ? formatCurrency(data) : '';
                },

                sortable: false
            },
            {
                targets: 6,
                data: 'opportunity.totalQuotas',
                render: function (data) {
                    return data !== null ? data : '';
                },
                sortable: false,
            },
            {
                targets: 7,
                data: 'opportunity.quotasType',
                render: function (data) {
                    return getOpportunitiesDateTypeName(data) || '';
                },
                sortable: false,
            }
        ]
    });

    // Função para formatar valor R$ 
    function formatCurrency(value) {
        return `R$ ${value.toLocaleString('pt-BR', { minimumFractionDigits: 2, maximumFractionDigits: 2 })}`;
    }

    function getOpportunitiesDateTypeName(type) {
        switch (type) {
            case 1:
                return 'Mensal';
            case 2:
                return 'Anual';
            case 3:
                return 'Nenhum';
            default:
                return '';
        }
    }

    function getOpportunityTypeName(type) {
        switch (type) {
            case 1:
                return l("OpportunityTypeNameCard");
            case 2:     
                return l("OpportunityTypeNamePersonalLoan");
            case 3:     
                return l("OpportunityTypeNameComercialLoan");
            case 4:     
                return l("OpportunityTypeNameMortgageLoan");
            case 5:
                return l("OpportunityTypeNameVehicle");
            case 6:
                return l("OpportunityTypeNameOverdraft");
            case 7:
                return l("OpportunityTypeNameOther");
            case 8:
                return l("OpportunityTypeNameNotIncluded");
            default:
                return '';
        }
    }
})(jQuery);