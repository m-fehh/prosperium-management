(function ($) {
    var _tenantService = abp.services.app.tenant,
        l = abp.localization.getSource('Management'),
        _$modal = $('#TenantCreateModal'),
        _$form = _$modal.find('form'),
        _$table = $('#TenantsTable');

    // Destruir a tabela DataTable, se já estiver inicializada
    if ($.fn.DataTable.isDataTable('#TenantsTable')) {
        $('#TenantsTable').DataTable().destroy();
    }

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
                data: 'isActive',
                sortable: false,
                render: data => `<input type="checkbox" disabled ${data ? 'checked' : ''}>`
            },
            {
                targets: 4,
                data: null,
                sortable: false,
                autoWidth: false,
                defaultContent: '',
                render: (data, type, row, meta) => {
                    return [
                        `   <button type="button" style="background: orange; border: 0; color: #fff;" class="btn btn-sm impersonate-tenant-get-user" data-tenant-id="${row.id}" data-tenancy-name="${row.name}" data-toggle="modal" data-target="#GetUserModal">`,
                        `       <i class="fa fa-unlock-alt"></i> `,
                        '   </button>'
                    ].join('');
                }
            },
            {
                targets: 5,
                data: null,
                sortable: false,
                autoWidth: false,
                defaultContent: '',
                render: (data, type, row, meta) => {
                    return [
                        `   <button type="button" style="background: orange; border: 0; color: #fff;"  class="btn btn-sm edit-tenant" data-tenant-id="${row.id}" data-toggle="modal" data-target="#TenantEditModal">`,
                        `       <i class="fas fa-pencil-alt"></i>`,
                        '   </button>',
                        `   <button type="button" style="background: orange; border: 0; color: #fff;"  class="btn btn-sm delete-tenant" data-tenant-id="${row.id}" data-tenancy-name="${row.name}">`,
                        `       <i class="fas fa-trash"></i>`,
                        '   </button>'
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
            dataType: 'html',
            success: function (content) {

                // Atualizar o conteúdo da modal com os dados retornados
                $('#GetUserModal .modal-content').html(content);

                // Exibir a modal
                $('#GetUserModal').modal('show');
            },
            error: function (e) {
                console.error("ERRO", e);
            }
        });
    });

    $(document).on('click', '.impersonate-tenant', function () {
        var tenantId = $(this).data('tenant-id');
        var userId = $(this).data('user-id');

        ImpersonateTenant(tenantId, userId);
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

                console.log('Usuário com ID ' + userId + ' impersonado com sucesso!');
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
