(function ($) {
    var _tenantService = abp.services.app.tenant,
        l = abp.localization.getSource('Management'),
        _$modal = $('#TenantCreateModal'),
        _$form = _$modal.find('form'),
        _$table = $('#TenantsTable');

    var _$tenantsTable = _$table.DataTable({
        paging: true,
        serverSide: true,
        listAction: {
            ajaxFunction: _tenantService.getAll,
            inputFilter: function () {
                return $('#TenantsSearchForm').serializeFormToObject(true);
            }
        },
        buttons: [
            {
                name: 'refresh',
                text: '<i class="fas fa-redo-alt"></i>',
                action: () => _$tenantsTable.draw(false)
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
                data: 'tenancyName',
                sortable: false
            },
            {
                targets: 2,
                data: 'name',
                sortable: false
            },
            {
                targets: 3,
                data: 'planName',
                render: function (data) {
                    return data || 'Essencial';
                },
                sortable: false
            },
            {
                targets: 4,
                data: 'planExpiration',
                render: function (data) {
                    var formattedDate = data ? moment(data).format('DD/MM/YYYY') : 'Ilimitada';
                    return formattedDate;
                },
                sortable: false
            },
            {
                targets: 5,
                data: 'isActive',
                sortable: false,
                render: data => `<input type="checkbox" disabled ${data ? 'checked' : ''}>`
            },
            {
                targets: 6,
                data: null,
                sortable: false,
                autoWidth: false,
                defaultContent: '',
                render: (data, type, row, meta) => {
                    return [
                        ` 
                             <div class="dropdown">
                                <div id="menuIcon" class="ml-auto dropdown-toggle" data-toggle="dropdown" style="display: flex; align-items: center; justify-content: center;">
                                    <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-list" viewBox="0 0 16 16">
                                        <path d="M2 11h12a1 1 0 0 1 0 2H2a1 1 0 0 1 0-2zm0-5h12a1 1 0 0 1 0 2H2a1 1 0 0 1 0-2zm0-5h12a1 1 0 0 1 0 2H2a1 1 0 0 1 0-2z" />
                                    </svg>
                                </div>
                                <div class="dropdown-menu dropdown-menu-right">
                                    <button class="dropdown-item impersonate-tenant-get-user" data-tenant-id="${row.id}" data-tenancy-name="${row.name}">Impersonar</button>
                                    <button class="dropdown-item changePlanTenant" data-tenant-id="${row.id}" data-tenancy-name="${row.name}" data-toggle="modal" data-target="#TenantPlanModal">Editar plano</button>
                                    <button class="dropdown-item edit-tenant" data-tenant-id="${row.id}" data-toggle="modal" data-target="#TenantEditModal">Editar tenant</button>
                                    <button class="dropdown-item delete-tenant" data-tenant-id="${row.id}" data-tenancy-name="${row.name}">Excluir tenant</button>
                                </div>
                            </div> 
                           `
                    ].join('');
                }
            }
        ]
    });

    _$form.find('.save-button').click(function (e) {
        e.preventDefault();

        if (!_$form.valid()) {
            return;
        }

        var tenant = _$form.serializeFormToObject();

        abp.ui.setBusy(_$modal);

        _tenantService
            .create(tenant)
            .done(function () {
                _$modal.modal('hide');
                _$form[0].reset();
                abp.notify.info(l('SavedSuccessfully'));
                _$tenantsTable.ajax.reload();
            })
            .always(function () {
                abp.ui.clearBusy(_$modal);
            });
    });

    $(document).on('click', '.impersonate-tenant-get-user', function () {
        var tenantId = $(this).data('tenant-id');



        abp.ajax({
            url: abp.appPath + 'Tenants/GetUserByTenantId?tenantId=' + tenantId,
            type: 'POST',
            dataType: 'json',
            success: function (content) {
                ImpersonateTenant(content.tenantId, content.id);
            },
            error: function (e) {
                console.error("ERRO", e);
            }
        });
    });

    function ImpersonateTenant(tenantId, userId) {
        abp.ajax({
            url: abp.appPath + 'Account/Impersonate',
            data: JSON.stringify({
                tenantId: tenantId,
                userId: userId,
            }),
            success: function () {
                if (!app.supportsTenancyNameInUrl) {
                    abp.multiTenancy.setTenantIdCookie(tenantId);
                }
            },
            error: function (e) {
                console.error('Erro ao impersonar usuário: ', e);
            }
        });
    }

    $(document).on('click', '.delete-tenant', function () {
        var tenantId = $(this).attr('data-tenant-id');
        var tenancyName = $(this).attr('data-tenancy-name');

        deleteTenant(tenantId, tenancyName);
    });

    $(document).on('click', '.edit-tenant', function (e) {
        var tenantId = $(this).attr('data-tenant-id');

        abp.ajax({
            url: abp.appPath + 'Tenants/EditModal?tenantId=' + tenantId,
            type: 'POST',
            dataType: 'html',
            success: function (content) {
                $('#TenantEditModal div.modal-content').html(content);
            },
            error: function (e) {
            }
        });
    });

    $(document).on('click', '#btnSubmitPlan', function () {
        var tenantId = $('#tenant').val();
        var selectedPlanId = $('select[name=selectPlan]').val();
        var selectedPlanName = $('select[name=selectPlan] option:selected').text();
        var formattedDate = $('#data').val();

        $.ajax({
            url: abp.appPath + 'Tenants/UpdatePlanByTenant',
            type: 'POST',
            data: { tenantId: tenantId, selectedPlanId: +selectedPlanId, selectedPlanName: selectedPlanName, formattedDate: formattedDate },
            dataType: 'json',
            success: function (content) {
                window.location.reload();
            },
            error: function (e) {
                console.error("ERRO", e);
            }
        });

    });

    $(document).on('click', '.changePlanTenant', function (e) {
        var tenantId = $(this).attr('data-tenant-id');

        abp.ajax({
            url: abp.appPath + 'Tenants/EditPlan?tenantId=' + tenantId,
            type: 'POST',
            dataType: 'html',
            success: function (content) {
                $('#TenantPlanModal div.modal-content').html(content);
            },
            error: function (e) {
            }
        });
    });

    abp.event.on('tenant.edited', (data) => {
        _$tenantsTable.ajax.reload();
    });

    function deleteTenant(tenantId, tenancyName) {
        abp.message.confirm(
            abp.utils.formatString(
                l('AreYouSureWantToDelete'),
                tenancyName
            ),
            null,
            (isConfirmed) => {
                if (isConfirmed) {
                    _tenantService
                        .delete({
                            id: tenantId
                        })
                        .done(() => {
                            abp.notify.info(l('SuccessfullyDeleted'));
                            _$tenantsTable.ajax.reload();
                        });
                }
            }
        );
    }

    _$modal.on('shown.bs.modal', () => {
        _$modal.find('input:not([type=hidden]):first').focus();
    }).on('hidden.bs.modal', () => {
        _$form.clearForm();
    });

    $('.btn-search').on('click', (e) => {
        _$tenantsTable.ajax.reload();
    });

    $('.btn-clear').on('click', (e) => {
        $('input[name=Keyword]').val('');
        $('input[name=IsActive][value=""]').prop('checked', true);
        _$tenantsTable.ajax.reload();
    });

    $('.txt-search').on('keypress', (e) => {
        if (e.which == 13) {
            _$tenantsTable.ajax.reload();
            return false;
        }
    });
})(jQuery);
